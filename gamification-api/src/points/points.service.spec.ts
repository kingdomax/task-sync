import { getRepositoryToken } from '@nestjs/typeorm';
import { Test, TestingModule } from '@nestjs/testing';

import { PointsService } from './points.service';
import { TASK_STATUS } from './dto/create-point.dto';
import { PointsLogEntity } from './entity/points-log.entity';

describe('PointsService', () => {
    let service: PointsService;

    beforeEach(async () => {
        const mockRepo = {
            manager: {
                transaction: jest.fn().mockResolvedValue(undefined),
            },
        };

        const module: TestingModule = await Test.createTestingModule({
            providers: [
                PointsService,
                {
                    provide: getRepositoryToken(PointsLogEntity),
                    useValue: mockRepo,
                },
            ],
        }).compile();

        service = module.get<PointsService>(PointsService);
    });

    it('should be defined', () => {
        expect(service).toBeDefined();
    });

    describe('calculatePoint', () => {
        it('should award 1 point for TASK_STATUS.CREATE', () => {
            const result = service.calculatePoint(TASK_STATUS.CREATE);
            expect(result).toEqual({
                awardedPoint: 1,
                awardedReason: 'created_task',
            });
        });

        it('should award 3 points for TASK_STATUS.DONE', () => {
            const result = service.calculatePoint(TASK_STATUS.DONE);
            expect(result).toEqual({
                awardedPoint: 3,
                awardedReason: 'completed_task',
            });
        });

        it('should award 0 point for other statuses', () => {
            const result = service.calculatePoint(TASK_STATUS.BACKLOG);
            expect(result).toEqual({
                awardedPoint: 0,
                awardedReason: 'BACKLOG',
            });
        });
    });
});
