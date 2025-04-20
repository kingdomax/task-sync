import { useEffect, useMemo, useState } from 'react';

import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';

import { getApiUrl } from 'src/utils/env';

import { DashboardContent } from 'src/layouts/dashboard';

import { Iconify } from 'src/components/iconify';

import { KanbanItem } from '../item-card';
import { KanbanStatus } from '../type/kanban-item';

import type { KanbanItemData, KanbanItemResponse } from '../type/kanban-item';

const statusLabels: Record<KanbanStatus, string> = {
    [KanbanStatus.BACKLOG]: 'Backlog',
    [KanbanStatus.TODO]: 'To Do',
    [KanbanStatus.INPROGRESS]: 'In Progress',
    [KanbanStatus.DONE]: 'Done',
};

const statusColors: Partial<Record<KanbanStatus, 'warning' | 'info' | 'success'>> = {
    [KanbanStatus.TODO]: 'warning',
    [KanbanStatus.INPROGRESS]: 'info',
    [KanbanStatus.DONE]: 'success',
};

export const KanbanBoardView = () => {
    const [kanbanItems, setKanbanItems] = useState<KanbanItemData[]>([]);

    useEffect(() => {
        const fetchTasks = async () => {
            try {
                const response: Response = await fetch(`${getApiUrl()}/task/getTasks/1`);
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

    const handleStatusChange = async (data: KanbanItemData, newStatus: KanbanStatus) => {
        // Optimistically update UI
        setKanbanItems((prev) =>
            prev.map((item) =>
                item.id === data.id
                    ? { ...item, status: newStatus, lastModified: new Date() }
                    : item
            )
        );

        // Then send request to server
        try {
            const res = await fetch(`${getApiUrl()}/task/updateStatus/${data.id}`, {
                method: 'PATCH',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ statusRaw: newStatus }),
            });

            if (!res.ok) {
                throw new Error();
            }

            const updated = await res.json();
            setKanbanItems((prev) => prev.map((item) => (item.id === updated.id ? updated : item)));
        } catch (err) {
            // Revert on failure
            setKanbanItems((prev) =>
                prev.map((item) =>
                    item.id === data.id
                        ? { ...item, status: data.status, lastModified: data.lastModified }
                        : item
                )
            );
            // Optional: toast / error message
            console.log(err);
        }
    };

    // Memoizes the result of a computation - saving performance
    const groupedItems = useMemo(() => {
        const map: Record<KanbanStatus, KanbanItemData[]> = {
            [KanbanStatus.BACKLOG]: [],
            [KanbanStatus.TODO]: [],
            [KanbanStatus.INPROGRESS]: [],
            [KanbanStatus.DONE]: [],
        };

        const sorted = [...kanbanItems].sort(
            (a, b) => new Date(a.lastModified).getTime() - new Date(b.lastModified).getTime()
        );

        for (const item of sorted) {
            const normalized = { ...item, lastModified: new Date(item.lastModified) };
            map[item.status].push(normalized);
        }

        return map;
    }, [kanbanItems]); // only runs when kanbanItems change

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
                {Object.entries(groupedItems).map(([statusKey, items]) => {
                    const status = statusKey as KanbanStatus;
                    return (
                        <Grid key={status} size={{ xs: 12, sm: 6, md: 3 }}>
                            <Box sx={{ ml: 1, mb: 2, typography: 'h6' }}>
                                {statusLabels[status]}
                            </Box>
                            {items.map((item) => (
                                <KanbanItem
                                    key={`kanban-${status}-${item.id}`}
                                    color={statusColors[status]}
                                    data={item}
                                    onStatusChange={handleStatusChange}
                                />
                            ))}
                        </Grid>
                    );
                })}
            </Grid>
        </DashboardContent>
    );
};
