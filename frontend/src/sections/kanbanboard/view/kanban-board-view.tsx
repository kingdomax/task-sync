import { useCallback, useState } from 'react';

import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';
import Card from '@mui/material/Card';
import Button from '@mui/material/Button';
import Avatar from '@mui/material/Avatar';
import Popover from '@mui/material/Popover';
import MenuList from '@mui/material/MenuList';
import CardHeader from '@mui/material/CardHeader';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import CardContent from '@mui/material/CardContent';
import MenuItem, { menuItemClasses } from '@mui/material/MenuItem';

import { DashboardContent } from 'src/layouts/dashboard';

import { Iconify } from 'src/components/iconify';

import { AnalyticsWidgetSummary } from '../../overview/analytics-widget-summary';

export const KanbanBoardView = () => {
    const [openPopover, setOpenPopover] = useState<HTMLButtonElement | null>(null);

    const handleOpenPopover = useCallback((event: React.MouseEvent<HTMLButtonElement>) => {
        setOpenPopover(event.currentTarget);
    }, []);

    const handleClosePopover = useCallback(() => {
        setOpenPopover(null);
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

            <Popover
                open={!!openPopover}
                anchorEl={openPopover}
                onClose={handleClosePopover}
                anchorOrigin={{ vertical: 'bottom', horizontal: 'left' }}
                //transformOrigin={{ vertical: 'top', horizontal: 'right' }}
            >
                <MenuList
                    disablePadding
                    sx={{
                        p: 0.5,
                        gap: 0.5,
                        width: 160,
                        display: 'flex',
                        flexDirection: 'column',
                        [`& .${menuItemClasses.root}`]: {
                            px: 1,
                            gap: 2,
                            borderRadius: 0.75,
                            [`&.${menuItemClasses.selected}`]: { bgcolor: 'action.selected' },
                        },
                    }}
                >
                    <MenuItem onClick={handleClosePopover}>
                        <Iconify icon="material-symbols:pending-actions-rounded" />
                        BACKLOG
                    </MenuItem>
                    <MenuItem onClick={handleClosePopover}>
                        <Iconify icon="material-symbols:checklist-rounded" />
                        TODO
                    </MenuItem>
                    <MenuItem onClick={handleClosePopover}>
                        <Iconify icon="material-symbols:sync-rounded" />
                        IN PROGRESS
                    </MenuItem>
                    <MenuItem onClick={handleClosePopover}>
                        <Iconify icon="material-symbols:check-circle-rounded" />
                        DONE
                    </MenuItem>
                    <MenuItem onClick={handleClosePopover} sx={{ color: 'error.main' }}>
                        <Iconify icon="solar:trash-bin-trash-bold" />
                        Delete
                    </MenuItem>
                </MenuList>
            </Popover>

            <Grid container spacing={3}>
                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <Box sx={{ ml: 1, mb: 2, typography: 'h6' }}>BACKLOG</Box>

                    {/* Use Card Items */}
                    <Card>
                        <CardHeader
                            avatar={<Avatar />}
                            action={
                                <IconButton aria-label="settings" onClick={handleOpenPopover}>
                                    <Iconify icon="eva:more-vertical-fill" />
                                </IconButton>
                            }
                            // see user-table-row.tsx for open menu
                            //subheader="Un assigned"
                            //title="Shrimp and Chorizo Paella"
                        />
                        <CardContent sx={{ pl: 3, pr: 3 }}>
                            <Typography variant="subtitle2">
                                Implement authentication flow
                            </Typography>
                        </CardContent>
                    </Card>
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
