import { TASK_STATUS } from '../type/kanban-item';

export const statusLabels: Partial<Record<TASK_STATUS, string>> = {
    [TASK_STATUS.BACKLOG]: 'Backlog',
    [TASK_STATUS.TODO]: 'To Do',
    [TASK_STATUS.INPROGRESS]: 'In Progress',
    [TASK_STATUS.DONE]: 'Done',
};

export const statusColors: Partial<Record<TASK_STATUS, 'warning' | 'info' | 'success'>> = {
    [TASK_STATUS.TODO]: 'warning',
    [TASK_STATUS.INPROGRESS]: 'info',
    [TASK_STATUS.DONE]: 'success',
};
