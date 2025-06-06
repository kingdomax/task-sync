import { useDraggable } from '@dnd-kit/core';

import { KanbanItem } from './kanban-item';
import { TASK_STATUS } from './type/kanban-item';

import type { TaskDto } from './type/kanban-item';

type Props = {
    item: TaskDto;
    onStatusChange: (data: TaskDto, newStatus: TASK_STATUS) => void;
    onDelete: (deleteItem: TaskDto) => void;
};

const statusColors: Partial<Record<TASK_STATUS, 'warning' | 'info' | 'success'>> = {
    [TASK_STATUS.TODO]: 'warning',
    [TASK_STATUS.INPROGRESS]: 'info',
    [TASK_STATUS.DONE]: 'success',
};

export const DraggableKanbanItem = ({ item, onStatusChange, onDelete }: Props) => {
    const { attributes, listeners, setNodeRef, transform, isDragging } = useDraggable({
        id: item.id,
    });

    const style = {
        transform: transform ? `translate(${transform.x}px, ${transform.y}px)` : undefined,
        touchAction: 'none',
    };

    return (
        <div ref={setNodeRef} style={style}>
            <KanbanItem
                data={item}
                color={statusColors[item.status]}
                onStatusChange={onStatusChange}
                onDelete={onDelete}
                dragHandleProps={{ ...listeners, ...attributes }}
                isDragging={isDragging}
            />
        </div>
    );
};
