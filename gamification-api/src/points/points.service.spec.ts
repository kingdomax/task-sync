import { getRepositoryToken } from '@nestjs/typeorm';
import { Test, TestingModule } from '@nestjs/testing';
import { PointsService } from './points.service';
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
});
