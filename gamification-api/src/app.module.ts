import * as dotenv from 'dotenv';

import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';

import { AppService } from './app.service';
import { AppController } from './app.controller';
import { PointsModule } from './points/points.module';

dotenv.config();

@Module({
    imports: [
        TypeOrmModule.forRoot({
            type: 'postgres',
            host: process.env.DB_HOST,
            port: Number(process.env.DB_PORT),
            database: process.env.DB_NAME,
            username: process.env.DB_USER,
            password: process.env.DB_PASS,
            synchronize: false, // It will create, drop, or alter tables based on your current entity state.
            autoLoadEntities: true, // don’t have to manually list every entity in forRoot e.g.  entities: [PointsLogEntity, OtherEntity]
        }),
        PointsModule,
    ],
    controllers: [AppController],
    providers: [AppService],
})
export class AppModule {}
