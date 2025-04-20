export type KanbanItemResponse = {
    tasks: KanbanItemData[] | null;
};

export type KanbanItemData = {
    id: number;
    title: string;
    assigneeId: number | null;
    lastModified: Date | string; // need to new Date(rawItem.lastModified) before
    status: KanbanStatus;
};

//export type KanbanStatus = 'BACKLOG' | 'TODO' | 'INPROGRESS' | 'DONE';

export enum KanbanStatus {
    BACKLOG = 'BACKLOG',
    TODO = 'TODO',
    INPROGRESS = 'INPROGRESS',
    DONE = 'DONE',
}
