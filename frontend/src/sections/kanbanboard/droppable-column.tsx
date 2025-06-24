import { useDroppable } from '@dnd-kit/core';

import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';

import { TASK_STATUS } from './type/kanban-item';

type Props = {
    status: TASK_STATUS;
    children: React.ReactNode;
};

const statusLabels: Partial<Record<TASK_STATUS, string>> = {
    [TASK_STATUS.BACKLOG]: 'Backlog',
    [TASK_STATUS.TODO]: 'To Do',
    [TASK_STATUS.INPROGRESS]: 'In Progress',
    [TASK_STATUS.DONE]: 'Done',
};

export const DroppableColumn = ({ status, children }: Props) => {
    const { setNodeRef } = useDroppable({ id: status });

    return (
        <Grid ref={setNodeRef} key={status} size={{ xs: 12, sm: 6, md: 3 }}>
            <Box sx={{ ml: 1, mb: 2, typography: 'h6' }}>{statusLabels[status]}</Box>
            {children}
        </Grid>
    );
};
