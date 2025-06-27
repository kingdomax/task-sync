import { IsInt, IsString, IsEnum } from 'class-validator';

export enum TASK_STATUS {
    BACKLOG,
    TODO,
    INPROGRESS,
    DONE,
    CREATE,
    DELETE,
}

export class CreatePointDto {
    @IsString()
    userId: string;

    @IsInt()
    taskId: number;

    @IsEnum(TASK_STATUS)
    taskStatus: TASK_STATUS;
}
