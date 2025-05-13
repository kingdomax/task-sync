export enum TASK_STATUS {
    BACKLOG = 'BACKLOG',
    TODO = 'TODO',
    INPROGRESS = 'INPROGRESS',
    DONE = 'DONE',
}

export type KanbanBoardVm = {
    tasks: TaskDto[] | null;
};

export type TaskDto = {
    id: number;
    title: string;
    assigneeId: number | null;
    lastModified: Date | string; // need to new Date(rawItem.lastModified) before
    status: TASK_STATUS;
};

export type AddItemRequest = Pick<TaskDto, 'title' | 'assigneeId' | 'lastModified'> & {
    projectId: number;
};

export enum NOTIFY_STATUS {
    CREATE = 'CREATE',
    UPDATE = 'UPDATE',
    DELETE = 'DELETE',
}

export type NotifyTask = {
    status: NOTIFY_STATUS;
    data: TaskDto;
};
