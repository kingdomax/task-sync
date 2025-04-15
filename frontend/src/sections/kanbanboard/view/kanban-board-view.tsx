import { useState } from 'react';

import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';

import { DashboardContent } from 'src/layouts/dashboard';
import { _posts, _tasks, _traffic, _timeline } from 'src/_mock';

import { Iconify } from 'src/components/iconify';

import { AnalyticsWidgetSummary } from '../../overview/analytics-widget-summary';

export const KanbanBoardView = () => {
    const [test, useTest] = useState(false);

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

                    {/* Card Items */}
                    <AnalyticsWidgetSummary
                        title="Weekly sales"
                        percent={2.6}
                        total={714000}
                        icon={<img alt="Weekly sales" src="/assets/icons/glass/ic-glass-bag.svg" />}
                        chart={{
                            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug'],
                            series: [22, 8, 35, 50, 82, 84, 77, 12],
                        }}
                    />
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <Box sx={{ ml: 1, mb: 2, typography: 'h6' }}>TODO</Box>

                    <AnalyticsWidgetSummary
                        title="New users"
                        percent={-0.1}
                        total={1352831}
                        color="secondary"
                        icon={<img alt="New users" src="/assets/icons/glass/ic-glass-users.svg" />}
                        chart={{
                            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug'],
                            series: [56, 47, 40, 62, 73, 30, 23, 54],
                        }}
                    />
                    <AnalyticsWidgetSummary
                        title="New users"
                        percent={-0.1}
                        total={1352831}
                        color="secondary"
                        icon={<img alt="New users" src="/assets/icons/glass/ic-glass-users.svg" />}
                        chart={{
                            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug'],
                            series: [56, 47, 40, 62, 73, 30, 23, 54],
                        }}
                    />
                    <AnalyticsWidgetSummary
                        title="New users"
                        percent={-0.1}
                        total={1352831}
                        color="secondary"
                        icon={<img alt="New users" src="/assets/icons/glass/ic-glass-users.svg" />}
                        chart={{
                            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug'],
                            series: [56, 47, 40, 62, 73, 30, 23, 54],
                        }}
                    />
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <Box sx={{ ml: 1, mb: 2, typography: 'h6' }}>IN PROGRESS</Box>

                    <AnalyticsWidgetSummary
                        title="Purchase orders"
                        percent={2.8}
                        total={1723315}
                        color="warning"
                        icon={
                            <img alt="Purchase orders" src="/assets/icons/glass/ic-glass-buy.svg" />
                        }
                        chart={{
                            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug'],
                            series: [40, 70, 50, 28, 70, 75, 7, 64],
                        }}
                    />
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <Box sx={{ ml: 1, mb: 2, typography: 'h6' }}>DONE</Box>

                    <AnalyticsWidgetSummary
                        title="Messages"
                        percent={3.6}
                        total={234}
                        color="error"
                        icon={<img alt="Messages" src="/assets/icons/glass/ic-glass-message.svg" />}
                        chart={{
                            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug'],
                            series: [56, 30, 23, 54, 47, 40, 62, 73],
                        }}
                    />
                </Grid>
            </Grid>
        </DashboardContent>
    );
};
