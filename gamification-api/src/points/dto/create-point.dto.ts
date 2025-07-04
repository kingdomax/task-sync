import { IsInt, IsEnum } from 'class-validator';

export enum TASK_STATUS {
    BACKLOG,
    TODO,
    INPROGRESS,
    DONE,
    CREATE,
    DELETE,
}

export class CreatePointDto {
    @IsInt()
    userId: number;

    @IsInt()
    taskId: number;

    @IsEnum(TASK_STATUS)
    taskStatus: TASK_STATUS;
}
