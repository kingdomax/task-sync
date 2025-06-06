export enum TASK_STATUS { // todo-moch: change to int increase performance since when we do compare it take O(n) time complexity https://romgrk.com/posts/optimizing-javascript/?fbclid=IwY2xjawKei9BleHRuA2FlbQIxMQBicmlkETE2dzNMOWxrMjY3blF3RTNTAR48AW-NDx1_fh8xcfCXDgvi5TrsrjiDKTeY60krrUlF13bbKo2l-FnCrPjSDA_aem_y8APfc7ZEOz6QNIzQUpOug
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
