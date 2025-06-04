export const useSignalRTaskHub = () => {
    //const connectionRef = useRef<HubConnection | null>(null);
    //const connectionIdRef = useRef<string>('');
    //useEffect(() => {
    //    const connect = async () => {
    //        const connection = new HubConnectionBuilder()
    //            .withUrl(`${getSeverUrl()}/taskHub`, { withCredentials: true })
    //            .withAutomaticReconnect()
    //            .build();
    //
    //        connection.on('TaskUpdated', (msg: NotifyTask) => {
    //            setTasks((prev) => {
    //                switch (msg.status) {
    //                    case NOTIFY_STATUS.CREATE:
    //                        return [...prev, msg.data];
    //                    case NOTIFY_STATUS.UPDATE:
    //                        return prev.map((t) => (t.id === msg.data.id ? msg.data : t));
    //                    case NOTIFY_STATUS.DELETE:
    //                        return prev.filter((t) => t.id !== msg.data.id);
    //                    default:
    //                        return prev;
    //                }
    //            });
    //        });
    //
    //        await connection.start();
    //        connectionIdRef.current = await connection.invoke('GetConnectionId');
    //        connectionRef.current = connection;
    //    };
    //
    //    connect();
    //    return () => {
    //        connectionRef.current?.stop();
    //    };
    //}, []);
    //
    //return { connectionId: connectionIdRef.current };
};
