import { IsInt, IsString, Min, Max } from 'class-validator';

export class CreatePointDto {
    @IsString()
    userId: string;

    @IsInt()
    taskId: number;

    @IsInt()
    @Min(0)
    @Max(5)
    taskStatus: number;
}
