import { IsInt, Min } from 'class-validator';

export class CreatePointDto {
    @IsInt()
    userId: number;

    @IsInt()
    @Min(1)
    points: number;
}
