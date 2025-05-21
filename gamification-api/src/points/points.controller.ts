import {
    Body,
    Controller,
    Post,
    UseGuards,
    UseInterceptors,
} from '@nestjs/common';
import { PointsService } from './points.service';
import { CreatePointDto } from './dto/create-point.dto';
import { TimingInterceptor } from 'src/common/interceptors/timing.interceptor';
import { AuthGuard } from 'src/common/guards/auth.guard';

@Controller('points')
// @UseGuards(AuthGuard) todo-moch: move to global
// @UseInterceptors(TimingInterceptor)
export class PointsController {
    constructor(private readonly pointsService: PointsService) {}

    @Post()
    create(@Body() dto: CreatePointDto) {
        return this.pointsService.create(dto);
    }
}
