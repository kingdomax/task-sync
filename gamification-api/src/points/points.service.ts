import { Repository } from 'typeorm';
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

        if (awardedPoint > 0) {
            // Commit as 1 transaction for atomicity
            await this.pointsRepo.manager.transaction(
                async (transactionalEntityManager) => {
                    // Insert log, 1st db call
                    const log = transactionalEntityManager.create(
                        PointsLogEntity,
                        {
                            user_id: dto.userId,
                            task_id: dto.taskId,
                            points_awarded: awardedPoint,
                            reason: awardedReason,
                        }
                    );
                    await transactionalEntityManager.save(log);

                    // Update user's point balance, 2nd db call
                    await transactionalEntityManager.increment(
                        UserEntity,
                        { id: dto.userId },
                        'points',
                        awardedPoint
                    );
                }
            );

            console.log(
                `[PointsService] Award Point = points_awarded:${awardedPoint}, reason:${awardedReason}`
            );
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
