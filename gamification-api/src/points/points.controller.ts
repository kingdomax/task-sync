import { Body, Controller, Post } from '@nestjs/common';
import { PointsService } from './points.service';
import { CreatePointDto, TASK_STATUS } from './dto/create-point.dto';

@Controller('points')
export class PointsController {
    constructor(private readonly pointsService: PointsService) {}

    @Post() // auto return 201
    async create(@Body() dto: CreatePointDto) {
        console.log(
            `[PointsController] Request = userId:${dto.userId}, taskid:${dto.taskId}, status:${TASK_STATUS[dto.taskStatus]}`
        );
        await this.pointsService.create(dto);
    }
}
