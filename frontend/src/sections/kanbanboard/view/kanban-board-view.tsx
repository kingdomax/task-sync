import { useEffect, useState } from 'react';

import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';

import { getApiUrl } from 'src/utils/env';

import { DashboardContent } from 'src/layouts/dashboard';

import { Iconify } from 'src/components/iconify';

import { KanbanItem } from '../item-card';
import { KanbanStatus } from '../type/kanban-item';
import { AnalyticsWidgetSummary } from '../../overview/analytics-widget-summary';

import type { KanbanItemData, KanbanItemResponse } from '../type/kanban-item';

export const KanbanBoardView = () => {
    const [kanbanItems, setKanbanItems] = useState<KanbanItemData[]>([]);

    useEffect(() => {
        const fetchTasks = async () => {
            try {
                const response: Response = await fetch(`${getApiUrl()}/tasks/getTasks/1`);

                if (response.ok) {
                    const data: KanbanItemResponse = await response.json();
                    setKanbanItems(data.tasks ?? []);
                }
            } catch (error) {
                console.log(error);
            }
        };

        fetchTasks();
    }, []);

    return (
        <DashboardContent maxWidth="xl">
            <Box
                sx={{
                    mb: 5,
                    display: 'flex',
                    alignItems: 'center',
                }}
            >
                <Button
                    variant="contained"
                    color="inherit"
                    startIcon={<Iconify icon="mingcute:add-line" />}
                >
                    Add item
                </Button>
            </Box>

            <Grid container spacing={3}>
                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <Box sx={{ ml: 1, mb: 2, typography: 'h6' }}>BACKLOG</Box>

                    {kanbanItems.map(
                        (x) =>
                            x.status == KanbanStatus.BACKLOG && (
                                <KanbanItem
                                    data={{
                                        ...x,
                                        lastModified: new Date(x.lastModified),
                                    }}
                                    key={`backlog-item-${x.id}`}
                                />
                            )
                    )}
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <Box sx={{ ml: 1, mb: 2, typography: 'h6' }}>TODO</Box>

                    {kanbanItems.map(
                        (x) =>
                            x.status == KanbanStatus.TODO && (
                                <KanbanItem
                                    color="warning"
                                    data={{
                                        ...x,
                                        lastModified: new Date(x.lastModified),
                                    }}
                                    key={`todo-item-${x.id}`}
                                />
                            )
                    )}
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <Box sx={{ ml: 1, mb: 2, typography: 'h6' }}>IN PROGRESS</Box>

                    {kanbanItems.map(
                        (x) =>
                            x.status == KanbanStatus.INPROGRESS && (
                                <KanbanItem
                                    color="info"
                                    data={{
                                        ...x,
                                        lastModified: new Date(x.lastModified),
                                    }}
                                    key={`inprogress-item-${x.id}`}
                                />
                            )
                    )}
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <Box sx={{ ml: 1, mb: 2, typography: 'h6' }}>DONE</Box>

                    {kanbanItems.map(
                        (x) =>
                            x.status == KanbanStatus.DONE && (
                                <KanbanItem
                                    color="success"
                                    data={{
                                        ...x,
                                        lastModified: new Date(x.lastModified),
                                    }}
                                    key={`done-item-${x.id}`}
                                />
                            )
                    )}
                </Grid>
            </Grid>
        </DashboardContent>
    );
};
