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
    EventId: string;
    UserId: number;
    TaskId: number;
    TaskStatus: number;
    OccurredAtUtc: string;
    Source: string;
};

@Injectable()
export class PointsRmqConsumer implements OnModuleInit, OnModuleDestroy {
    private conn!: AmqpConnectionManager;
    private channel!: ChannelWrapper;

    constructor(private readonly pointsService: PointsService) {}

    async onModuleInit() {
        const url =
            process.env.RABBITMQ_URL || 'amqp://user:password@localhost:5672';
        const mainQueue = process.env.RABBITMQ_QUEUE || 'points.award.q';

        this.conn = connect([url]);

        this.channel = this.conn.createChannel({
            setup: async (ch) => {
                await ch.consume(
                    mainQueue,
                    async (msg: ConsumeMessage | null) => {
                        if (!msg) return;

                        try {
                            const raw = msg.content.toString('utf8');
                            const evt: PointAwardedEvent = JSON.parse(raw);

                            console.log(
                                `[PointsRmqConsumer] Received: userId:${evt.UserId}, taskId:${evt.TaskId}, status:${TASK_STATUS[evt.TaskStatus]}`
                            );
                            await this.pointsService.create({
                                userId: evt.UserId,
                                taskId: evt.TaskId,
                                taskStatus: evt.TaskStatus,
                            });

                            ch.ack(msg);
                        } catch (err) {
                            console.error('[PointsRmqConsumer] Failed:', err);

                            // reject without requeue -> RabbitMQ dead-letters to DLQ (because main queue is configured by .NET)
                            ch.nack(msg, false, false);
                        }
                    },
                    { noAck: false }
                );
            },
        });

        console.log(`[PointsRmqConsumer] Listening queue: ${mainQueue}`);
    }

    async onModuleDestroy() {
        await this.channel?.close().catch(() => {});
        await this.conn?.close().catch(() => {});
    }
}
