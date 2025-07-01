import { useDraggable } from '@dnd-kit/core';

import { KanbanItem } from './kanban-item';
import { statusColors } from './viewdata/board-data';

import type { TaskDto, Nullable, TASK_STATUS } from './type/kanban-item';

type Props = {
    item: TaskDto;
    onSelect: (itemId: number) => void;
    onStatusChange: (data: TaskDto, newStatus: TASK_STATUS) => void;
    onDelete: (deleteItem: TaskDto) => void;
};

export const DraggableKanbanItem = ({ item, onSelect, onStatusChange, onDelete }: Props) => {
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
                onSelect={onSelect}
                onStatusChange={onStatusChange}
                onDelete={onDelete}
                dragHandleProps={{ ...listeners, ...attributes }}
                isDragging={isDragging}
            />
        </div>
    );
};
