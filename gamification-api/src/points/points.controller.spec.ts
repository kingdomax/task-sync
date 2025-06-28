import { getRepositoryToken } from '@nestjs/typeorm';
import { Test, TestingModule } from '@nestjs/testing';
import { PointsService } from './points.service';
import { PointsController } from './points.controller';
import { PointsLogEntity } from './entity/points-log.entity';

describe('PointsController', () => {
    let controller: PointsController;

    beforeEach(async () => {
        const mockRepo = {
            manager: {
                transaction: jest.fn().mockResolvedValue(undefined),
            },
        };

        const module: TestingModule = await Test.createTestingModule({
            controllers: [PointsController],
            providers: [
                PointsService,
                {
                    provide: getRepositoryToken(PointsLogEntity),
                    useValue: mockRepo,
                },
            ],
        }).compile();

        controller = module.get<PointsController>(PointsController);
    });

    it('should be defined', () => {
        expect(controller).toBeDefined();
    });
});
