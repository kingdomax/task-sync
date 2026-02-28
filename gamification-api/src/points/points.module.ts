import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { PointsService } from './points.service';
import { PointsController } from './points.controller';
import { UserEntity } from './entity/user.entity';
import { PointsLogEntity } from './entity/points-log.entity';
import { PointsRmqConsumer } from './points.rmq-consumer';

@Module({
    imports: [TypeOrmModule.forFeature([PointsLogEntity, UserEntity])],
    controllers: [PointsController],
    providers: [PointsService, PointsRmqConsumer],
})
export class PointsModule {}
