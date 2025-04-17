import type { WorkspacesPopoverProps } from './components/workspaces-popover';

// ----------------------------------------------------------------------

export const _workspaces: WorkspacesPopoverProps['data'] = [
    {
        id: 'team-1',
        name: 'Project Artemis',
        plan: 'Active',
        logo: '/assets/icons/workspaces/logo-1.webp',
    },
    {
        id: 'team-2',
        name: 'Project Apollo',
        plan: 'Inactive',
        logo: '/assets/icons/workspaces/logo-2.webp',
    },
    {
        id: 'team-3',
        name: 'Project Columbia',
        plan: 'Inactive',
        logo: '/assets/icons/workspaces/logo-3.webp',
    },
];
