import { Repository } from 'typeorm';
import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
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
            const record = this.pointsRepo.create({
                user_id: Number(dto.userId),
                task_id: dto.taskId,
                points_awarded: awardedPoint,
                reason: awardedReason,
            });

            await this.pointsRepo.save(record);

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
