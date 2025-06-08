import { useEffect } from 'react';

import { getApiUrl } from 'src/utils/env';
import { getAuthToken } from 'src/utils/auth-token';

import type { TaskDto, TASK_STATUS, KanbanBoardVm, AddItemRequest } from '../type/kanban-item';

export const useKanbanCrud = (
    setKanbanItems: React.Dispatch<React.SetStateAction<TaskDto[]>>,
    connectionIdRef: React.RefObject<string>
) => {
    // keep in mind useEffect() run after DOM painted
    useEffect(() => {
        const fetchTasks = async () => {
            try {
                const response: Response = await fetch(`${getApiUrl()}/projects/1/tasks`); // todo-moch: hardcode for now
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

    const handleAddItem = async (
        newItem: AddItemRequest,
        onSuccess: () => void,
        onFailure: (msg: string) => void
    ) => {
        try {
            const res = await fetch(`${getApiUrl()}/projects/1/tasks`, {
                method: 'POST',
                body: JSON.stringify(newItem),
                headers: createHeader(connectionIdRef.current),
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
            const res = await fetch(`${getApiUrl()}/tasks/${data.id}`, {
                method: 'PATCH',
                body: JSON.stringify({ statusRaw: newStatus }),
                headers: createHeader(connectionIdRef.current),
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
            const res = await fetch(`${getApiUrl()}/tasks/${deleteItem.id}`, {
                method: 'DELETE',
                headers: createHeader(connectionIdRef.current),
            });

            if (!res.ok) {
                throw new Error();
            }
        } catch (err) {
            setKanbanItems((prev) => [...prev, deleteItem]); // Revert on failure
            alert(err);
        }
    };

    return { handleAddItem, handleStatusChange, handleDeleteItem };
};

const createHeader = (connectionId: string): Record<string, string> => ({
    'Content-Type': 'application/json',
    'x-connection-id': connectionId,
    Authorization: `Bearer ${getAuthToken()}`,
});
