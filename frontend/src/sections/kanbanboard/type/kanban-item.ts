export type KanbanItemData = {
    id: number;
    title: string;
    assigneeId: number;
    lastModified: Date;
    status: KanbanStatus;
};

export type KanbanStatus = 'backlog' | 'todo' | 'inprogress' | 'done';
