import { Body, Controller, Post } from '@nestjs/common';
import { PointsService } from './points.service';
import { CreatePointDto } from './dto/create-point.dto';

@Controller('points')
export class PointsController {
    constructor(private readonly pointsService: PointsService) {}

    @Post() // auto return 201
    create(@Body() dto: CreatePointDto) {
        this.pointsService.create(dto);
    }
}
