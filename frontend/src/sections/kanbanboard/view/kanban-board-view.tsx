import { useState } from 'react';

import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';

import { DashboardContent } from 'src/layouts/dashboard';

import { Iconify } from 'src/components/iconify';

import { KanbanItem } from '../item-card';
import { KanbanItemData } from '../type/kanban-item';
import { AnalyticsWidgetSummary } from '../../overview/analytics-widget-summary';

export const KanbanBoardView = () => {
    const [kanbanItems, setKanbanItems] = useState<KanbanItemData[]>([]);

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

                    <KanbanItem
                        data={{
                            id: 1,
                            title: 'Implement authentication flow',
                            assigneeId: 1,
                            status: 'backlog',
                            lastModified: new Date(),
                        }}
                    />
                    <KanbanItem
                        data={{
                            id: 1,
                            title: 'Implement authentication flow',
                            assigneeId: 1,
                            status: 'backlog',
                            lastModified: new Date(),
                        }}
                    />
                    <KanbanItem
                        data={{
                            id: 1,
                            title: 'Implement authentication flow',
                            assigneeId: 1,
                            status: 'backlog',
                            lastModified: new Date(),
                        }}
                    />
                    <KanbanItem
                        data={{
                            id: 1,
                            title: 'Implement authentication flow',
                            assigneeId: 1,
                            status: 'backlog',
                            lastModified: new Date(),
                        }}
                    />
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <Box sx={{ ml: 1, mb: 2, typography: 'h6' }}>TODO</Box>

                    <KanbanItem
                        color="warning"
                        data={{
                            id: 1,
                            title: 'Implement authentication flow',
                            assigneeId: 1,
                            status: 'backlog',
                            lastModified: new Date(),
                        }}
                    />
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <Box sx={{ ml: 1, mb: 2, typography: 'h6' }}>IN PROGRESS</Box>

                    <KanbanItem
                        color="info"
                        data={{
                            id: 1,
                            title: 'Implement authentication flow',
                            assigneeId: 1,
                            status: 'backlog',
                            lastModified: new Date(),
                        }}
                    />
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <Box sx={{ ml: 1, mb: 2, typography: 'h6' }}>DONE</Box>

                    <KanbanItem
                        color="success"
                        data={{
                            id: 1,
                            title: 'Implement authentication flow',
                            assigneeId: 1,
                            status: 'backlog',
                            lastModified: new Date(),
                        }}
                    />
                </Grid>
            </Grid>
        </DashboardContent>
    );
};
