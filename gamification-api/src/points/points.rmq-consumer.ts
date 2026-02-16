/* eslint-disable @typescript-eslint/require-await */
/* eslint-disable @typescript-eslint/no-redundant-type-constituents */
/* eslint-disable @typescript-eslint/no-unsafe-call */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import { Injectable, OnModuleDestroy, OnModuleInit } from '@nestjs/common';
import {
    connect,
    AmqpConnectionManager,
    ChannelWrapper,
} from 'amqp-connection-manager';
import type { ConsumeMessage } from 'amqplib';
import { PointsService } from './points.service';
import { CreatePointDto, TASK_STATUS } from './dto/create-point.dto';

type PointAwardedEvent = {
    eventId: string;
    userId: number;
    taskId: number;
    taskStatus: number; // enum int
    occurredAtUtc: string;
    source: string;
};

@Injectable()
export class PointsRmqConsumer implements OnModuleInit, OnModuleDestroy {
    private conn!: AmqpConnectionManager;
    private channel!: ChannelWrapper;

    constructor(private readonly pointsService: PointsService) {}

    async onModuleInit() {
        const url =
            process.env.RABBITMQ_URL || 'amqp://user:password@localhost:5672';
        const queue = process.env.RABBITMQ_QUEUE || 'points.award.queue';

        if (!url) throw new Error('RABBITMQ_URL is missing');

        this.conn = connect([url]);

        this.channel = this.conn.createChannel({
            setup: async (ch) => {
                await ch.assertQueue(queue, { durable: true });

                await ch.consume(
                    queue,
                    async (msg: ConsumeMessage | null) => {
                        if (!msg) return;

                        try {
                            // MassTransit sends JSON; body is Buffer
                            const raw = msg.content.toString('utf8');

                            // ⚠️ MassTransit envelope can wrap the message. We'll handle both cases:
                            // Case 1: body is the event directly
                            // Case 2: body is MassTransit envelope with "message" field
                            const parsed = JSON.parse(raw);
                            const evt: PointAwardedEvent =
                                parsed.message ?? parsed;

                            console.log(
                                `[PointsRmqConsumer] Received: userId:${evt.userId}, taskId:${evt.taskId}, status:${TASK_STATUS[evt.taskStatus]}`
                            );

                            const dto: CreatePointDto = {
                                userId: evt.userId,
                                taskId: evt.taskId,
                                taskStatus: evt.taskStatus,
                            };

                            await this.pointsService.create(dto);

                            ch.ack(msg);
                        } catch (err) {
                            console.error(
                                '[PointsRmqConsumer] Failed to process message:',
                                err
                            );
                            ch.nack(msg, false, false); // todo-moch: Send to DLQ later; don't requeue to avoid infinite loop
                        }
                    },
                    { noAck: false }
                );
            },
        });

        console.log(`[PointsRmqConsumer] Listening queue: ${queue}`);
    }

    async onModuleDestroy() {
        await this.channel?.close().catch(() => {});
        await this.conn?.close().catch(() => {});
    }
}
