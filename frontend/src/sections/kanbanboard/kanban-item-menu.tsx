import Popover from '@mui/material/Popover';
import MenuList from '@mui/material/MenuList';
import MenuItem, { menuItemClasses } from '@mui/material/MenuItem';

import { Iconify } from 'src/components/iconify/iconify';

import { TASK_STATUS } from './type/kanban-item';

type Props = {
    anchorEl: HTMLElement | null;
    onClose: () => void;
    onStatusChange: (newStatus: TASK_STATUS) => void;
    onDelete: () => void;
};

export const KanbanItemMenu = ({ anchorEl, onClose, onStatusChange, onDelete }: Props) => (
    <Popover
        open={!!anchorEl}
        anchorEl={anchorEl}
        onClose={onClose}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'left' }}
    >
        <MenuList
            disablePadding
            sx={{
                p: 0.5,
                gap: 0.5,
                width: 160,
                display: 'flex',
                flexDirection: 'column',
                [`& .${menuItemClasses.root}`]: {
                    px: 1,
                    gap: 2,
                    borderRadius: 0.75,
                    [`&.${menuItemClasses.selected}`]: { bgcolor: 'action.selected' },
                },
            }}
        >
            <MenuItem onClick={() => onStatusChange(TASK_STATUS.BACKLOG)}>
                <Iconify icon="material-symbols:pending-actions-rounded" />
                Backlog
            </MenuItem>
            <MenuItem onClick={() => onStatusChange(TASK_STATUS.TODO)}>
                <Iconify icon="material-symbols:checklist-rounded" />
                To Do
            </MenuItem>
            <MenuItem onClick={() => onStatusChange(TASK_STATUS.INPROGRESS)}>
                <Iconify icon="material-symbols:sync-rounded" />
                In Progress
            </MenuItem>
            <MenuItem onClick={() => onStatusChange(TASK_STATUS.DONE)}>
                <Iconify icon="material-symbols:check-circle-rounded" />
                Done
            </MenuItem>
            <MenuItem onClick={onDelete} sx={{ color: 'error.main' }}>
                <Iconify icon="solar:trash-bin-trash-bold" />
                Delete
            </MenuItem>
        </MenuList>
    </Popover>
);
