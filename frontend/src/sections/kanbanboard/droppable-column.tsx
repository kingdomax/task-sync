import { useDroppable } from '@dnd-kit/core';

import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';

import { statusLabels } from './viewdata/board-data';

import type { TASK_STATUS } from './type/kanban-item';

type Props = {
    status: TASK_STATUS;
    children: React.ReactNode;
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
