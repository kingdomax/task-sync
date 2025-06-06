import { Injectable } from '@nestjs/common';
import { CreatePointDto } from './dto/create-point.dto';

@Injectable()
export class PointsService {
    private records: CreatePointDto[] = []; // For demo. Use DB later!

    create(dto: CreatePointDto) {
        const record = {
            ...dto,
            id: this.records.length + 1,
            createdAt: new Date(),
        };
        this.records.push(dto);
        return record;
    }

    findAll() {
        return this.records;
    }
}
