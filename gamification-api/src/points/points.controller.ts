import { Body, Controller, Post } from '@nestjs/common';
import { PointsService } from './points.service';
import { CreatePointDto } from './dto/create-point.dto';

@Controller('points')
export class PointsController {
    constructor(private readonly pointsService: PointsService) {}

    @Post()
    create(@Body() dto: CreatePointDto) {
        return this.pointsService.create(dto);
    }
}
