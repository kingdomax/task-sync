import type { DragEndEvent } from '@dnd-kit/core';

import { useMemo, useState } from 'react';
import { DndContext } from '@dnd-kit/core';

import Grid from '@mui/material/Grid';

import { DashboardContent } from 'src/layouts/dashboard';

import { AddItemPanel } from '../add-item-panel';
import { TASK_STATUS } from '../type/kanban-item';
import { DroppableColumn } from '../droppable-column';
import { useKanbanCrud } from '../hooks/useKanbanCrud';
import { useSignalRTaskHub } from '../hooks/useSignalRTaskHub';
import { DraggableKanbanItem } from '../draggable-kanban-item';

import type { TaskDto } from '../type/kanban-item';

export const KanbanBoardView = () => {
    const [kanbanItems, setKanbanItems] = useState<TaskDto[]>([]);

    const { connectionIdRef } = useSignalRTaskHub(setKanbanItems);

    const { handleAddItem, handleStatusChange, handleDeleteItem } = useKanbanCrud(
        setKanbanItems,
        connectionIdRef
    );

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
                    {Object.entries(groupedItems).map(([statusKey, items]) => (
                        <DroppableColumn
                            key={`column-${statusKey}`}
                            status={statusKey as TASK_STATUS}
                        >
                            {items.map((item) => (
                                <DraggableKanbanItem
                                    key={item.id}
                                    item={item}
                                    onStatusChange={handleStatusChange}
                                    onDelete={handleDeleteItem}
                                />
                            ))}
                        </DroppableColumn>
                    ))}
                </Grid>
            </DndContext>
        </DashboardContent>
    );
};
