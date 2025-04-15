import { CONFIG } from 'src/config-global';

import { KanbanBoardView } from 'src/sections/kanbanboard/view';

// ----------------------------------------------------------------------

export default function Page() {
    return (
        <>
            <title>Board</title>
            <meta
                name="description"
                content="The starting point for your next project with Minimal UI Kit, built on the newest version of Material-UI ©, ready to be customized to your style"
            />
            <meta
                name="keywords"
                content="react,material,kit,application,dashboard,admin,template"
            />

            <KanbanBoardView />
        </>
    );
}
