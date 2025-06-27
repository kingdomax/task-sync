import { Injectable } from '@nestjs/common';
import { CreatePointDto, TASK_STATUS } from './dto/create-point.dto';

@Injectable()
export class PointsService {
    create(dto: CreatePointDto) {
        console.log(
            `[PointsService] userId:${dto.userId}, taskid:${dto.taskId}, status:${TASK_STATUS[dto.taskStatus]}`
        );
    }
}
