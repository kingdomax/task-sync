export type KanbanItemData = {
    id: number;
    title: string;
    assigneeId: number | null;
    lastModified: Date;
    status: KanbanStatus;
};

export type KanbanStatus = 'BACKLOG' | 'TODO' | 'INPROGRESS' | 'DONE';
