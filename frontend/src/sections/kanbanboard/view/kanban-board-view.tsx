import type { DragEndEvent } from '@dnd-kit/core';
import type { HubConnection } from '@microsoft/signalr';

import { DndContext } from '@dnd-kit/core';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { useRef, useMemo, useState, useEffect } from 'react';

import Grid from '@mui/material/Grid';

import { getApiUrl, getSeverUrl } from 'src/utils/env';

import { DashboardContent } from 'src/layouts/dashboard';

import { AddItemPanel } from '../add-item-panel';
import { DroppableColumn } from '../droppable-column';
import { DraggableKanbanItem } from '../draggable-kanban-item';
import { TASK_STATUS, NOTIFY_STATUS } from '../type/kanban-item';

import type { TaskDto, NotifyTask, KanbanBoardVm, AddItemRequest } from '../type/kanban-item';

export const KanbanBoardView = () => {
    const [kanbanItems, setKanbanItems] = useState<TaskDto[]>([]);
    const connectionRef = useRef<HubConnection | null>(null); // Persistent data across render without trigger re-render
    const connectionIdRef = useRef<string>('');

    useEffect(() => {
        const fetchTasks = async () => {
            try {
                const response: Response = await fetch(`${getApiUrl()}/task/getTasks/1`);
                if (response.ok) {
                    const data: KanbanBoardVm = await response.json();
                    setKanbanItems(data.tasks ?? []);
                }
            } catch (error) {
                console.log(error);
            }
        };

        fetchTasks();
    }, []);

    useEffect(() => {
        const connectToHub = async () => {
            const connection = new HubConnectionBuilder()
                .withUrl(`${getSeverUrl()}/taskHub`, {
                    withCredentials: true, // Required
                })
                .withAutomaticReconnect()
                .build();

            connectionRef.current = connection;

            connection.on('TaskUpdated', (response: NotifyTask) => {
                if (response.status == NOTIFY_STATUS.CREATE) {
                    setKanbanItems((prev) => [...prev, response.data]);
                } else if (response.status == NOTIFY_STATUS.UPDATE) {
                    setKanbanItems((prev) =>
                        prev.map((item) => (item.id === response.data.id ? response.data : item))
                    );
                } else if (response.status == NOTIFY_STATUS.DELETE) {
                    setKanbanItems((prev) => prev.filter((item) => item.id !== response.data.id));
                }
            });

            await connection.start();
            const connectionId = await connection.invoke('GetConnectionId');
            connectionIdRef.current = connectionId;
        };

        connectToHub();

        return () => {
            connectionRef.current?.stop();
        };
    }, []);

    const handleAddItem = async (
        newItem: AddItemRequest,
        onSuccess: () => void,
        onFailure: (msg: string) => void
    ) => {
        try {
            const res = await fetch(`${getApiUrl()}/task/addTask`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'x-connection-id': connectionIdRef.current,
                },
                body: JSON.stringify(newItem),
            });

            if (!res.ok) {
                throw new Error(`${res.statusText}`);
            }

            const addedItem: TaskDto = await res.json();
            setKanbanItems((prev) => [...prev, addedItem]);
            onSuccess();
        } catch (err: any) {
            if (err instanceof Error) {
                onFailure(err.message);
            } else {
                onFailure(`unknown error: ${err}`);
            }
        }
    };

    const handleStatusChange = async (data: TaskDto, newStatus: TASK_STATUS) => {
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
                headers: {
                    'Content-Type': 'application/json',
                    'x-connection-id': connectionIdRef.current,
                },
                body: JSON.stringify({ statusRaw: newStatus }),
            });

            if (!res.ok) {
                throw new Error();
            }

            const updated = await res.json();
            setKanbanItems((prev) => prev.map((item) => (item.id === updated.id ? updated : item)));
        } catch (err: any) {
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

    const handleDeleteItem = async (deleteItem: TaskDto) => {
        setKanbanItems((prev) => prev.filter((item) => item.id !== deleteItem.id));

        try {
            const res = await fetch(`${getApiUrl()}/task/deleteTask/${deleteItem.id}`, {
                method: 'DELETE',
                headers: { 'x-connection-id': connectionIdRef.current },
            });

            if (!res.ok) {
                throw new Error();
            }
        } catch (err) {
            setKanbanItems([...kanbanItems, deleteItem]); // Revert on failure
            alert(err);
        }
    };

    const handleDragEnd = (event: DragEndEvent) => {
        const { active, over } = event;
        if (!over || active.id === over.id) return;

        const draggedItem = kanbanItems.find((item) => item.id === active.id);
        const newStatus = over.id as TASK_STATUS;

        if (draggedItem && draggedItem.status !== newStatus) {
            handleStatusChange(draggedItem, newStatus);
        }
    };

    // Memoizes the result of a computation - saving performance
    const groupedItems = useMemo(() => {
        const map: Record<TASK_STATUS, TaskDto[]> = {
            [TASK_STATUS.BACKLOG]: [],
            [TASK_STATUS.TODO]: [],
            [TASK_STATUS.INPROGRESS]: [],
            [TASK_STATUS.DONE]: [],
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
            <AddItemPanel onAddItem={handleAddItem} />

            <DndContext onDragEnd={handleDragEnd}>
                <Grid container spacing={3}>
                    {Object.entries(groupedItems).map(([statusKey, items]) => {
                        const status = statusKey as TASK_STATUS;

                        return (
                            <DroppableColumn key={`column-${status}`} status={status}>
                                {items.map((item) => (
                                    <DraggableKanbanItem
                                        key={item.id}
                                        item={item}
                                        onStatusChange={handleStatusChange}
                                        onDelete={handleDeleteItem}
                                    />
                                ))}
                            </DroppableColumn>
                        );
                    })}
                </Grid>
            </DndContext>
        </DashboardContent>
    );
};
