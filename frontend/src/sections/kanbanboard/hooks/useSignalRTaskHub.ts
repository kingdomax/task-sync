import type { HubConnection } from '@microsoft/signalr';

import { useRef, useLayoutEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';

import { getSeverUrl } from 'src/utils/env';

import { NOTIFY_STATUS } from '../type/kanban-item';

import type { TaskDto, NotifyTask } from '../type/kanban-item';

export const useSignalRTaskHub = (
    setKanbanItems: React.Dispatch<React.SetStateAction<TaskDto[]>>
) => {
    const connectionRef = useRef<HubConnection | null>(null); // Persistent data across render without trigger re-render
    const connectionIdRef = useRef<string>('');

    // run after DOM mutation, but before paint
    useLayoutEffect(() => {
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
    }, [setKanbanItems]);

    return { connectionIdRef };
};
