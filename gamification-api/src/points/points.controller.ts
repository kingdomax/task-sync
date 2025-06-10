import { Body, Controller, Post } from '@nestjs/common';
import { PointsService } from './points.service';
import { CreatePointDto } from './dto/create-point.dto';

@Controller('points')
export class PointsController {
    constructor(private readonly pointsService: PointsService) {}

    @Post()
    create(@Body() dto: CreatePointDto) {
        const data = this.pointsService.create(dto);

        console.log(
            `userId: ${dto.userId} - taskid: ${dto.taskId}, status: ${dto.taskStatus}`
        );
    }
}
