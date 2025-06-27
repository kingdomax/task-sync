import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { PointsService } from './points.service';
import { PointsController } from './points.controller';
import { UserEntity } from './entity/user.entity';
import { PointsLogEntity } from './entity/points-log.entity';

@Module({
    imports: [TypeOrmModule.forFeature([PointsLogEntity, UserEntity])],
    controllers: [PointsController],
    providers: [PointsService],
})
export class PointsModule {}
