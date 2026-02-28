/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import { QueryFailedError, Repository } from 'typeorm';
import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';

import { UserEntity } from './entity/user.entity';
import { PointsLogEntity } from './entity/points-log.entity';

import { CreatePointDto, TASK_STATUS } from './dto/create-point.dto';

@Injectable()
export class PointsService {
    constructor(
        @InjectRepository(PointsLogEntity)
        private readonly pointsRepo: Repository<PointsLogEntity>
    ) {}

    async create(dto: CreatePointDto): Promise<void> {
        const { awardedPoint, awardedReason } = this.calculatePoint(
            dto.taskStatus
        );

        if (awardedPoint <= 0) return;

        try {
            await this.pointsRepo.manager.transaction(async (em) => {
                // 1) Insert log first (unique event_id)
                const log = em.create(PointsLogEntity, {
                    event_id: dto.eventId,
                    user_id: dto.userId,
                    task_id: dto.taskId,
                    points_awarded: awardedPoint,
                    reason: awardedReason,
                });

                await em.save(log);

                // 2) Only increment if insert succeeded
                await em.increment(
                    UserEntity,
                    { id: dto.userId },
                    'points',
                    awardedPoint
                );
            });

            console.log(
                `[PointsService] Awarded ${awardedPoint} (${awardedReason})`
            );
        } catch (err) {
            // PostgreSQL unique violation
            if (err instanceof QueryFailedError) {
                const pgErr = err as any;

                if (pgErr?.code === '23505') {
                    console.log(
                        `[PointsService] Duplicate event ignored: ${dto.eventId}`
                    );
                    return; // idempotent success
                }
            }

            throw err; // real error → consumer will nack → DLQ
        }
    }

    calculatePoint(action: TASK_STATUS): {
        awardedPoint: number;
        awardedReason: string;
    } {
        switch (action) {
            case TASK_STATUS.CREATE:
                return {
                    awardedPoint: 1,
                    awardedReason: 'created_task',
                };
            case TASK_STATUS.DONE:
                return {
                    awardedPoint: 3,
                    awardedReason: 'completed_task',
                };
            default:
                return {
                    awardedPoint: 0,
                    awardedReason: `${TASK_STATUS[action]}`,
                };
        }
    }
}
