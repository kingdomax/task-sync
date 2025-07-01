import type { PaletteColorKey } from 'src/theme/core';

import { useState, useCallback } from 'react';
import { varAlpha } from 'minimal-shared/utils';

import Card from '@mui/material/Card';
import Avatar from '@mui/material/Avatar';
import Popover from '@mui/material/Popover';
import MenuList from '@mui/material/MenuList';
import { useTheme } from '@mui/material/styles';
import IconButton from '@mui/material/IconButton';
import CardHeader from '@mui/material/CardHeader';
import Typography from '@mui/material/Typography';
import CardContent from '@mui/material/CardContent';
import MenuItem, { menuItemClasses } from '@mui/material/MenuItem';

import { Iconify } from 'src/components/iconify/iconify';

import { TASK_STATUS } from './type/kanban-item';

import type { TaskDto, Nullable } from './type/kanban-item';

type Props = {
    data: TaskDto;
    onSelect: (itemId: number) => void;
    onStatusChange: (data: TaskDto, newStatus: TASK_STATUS) => void;
    onDelete: (deleteItem: TaskDto) => void;
    color?: PaletteColorKey;
    isDragging?: boolean;
    dragHandleProps?: React.HTMLAttributes<HTMLElement>;
};

export const KanbanItem = ({
    color,
    data,
    onSelect,
    onStatusChange,
    onDelete,
    isDragging,
    dragHandleProps,
}: Props) => {
    const theme = useTheme();

    const [openPopover, setOpenPopover] = useState<HTMLButtonElement | null>(null);

    const handleOpenPopover = useCallback((event: React.MouseEvent<HTMLButtonElement>) => {
        setOpenPopover(event.currentTarget);
    }, []);

    const handleClosePopover = useCallback(() => {
        setOpenPopover(null);
    }, []);

    const handleStatusChange = useCallback(
        (newStatus: TASK_STATUS) => {
            onStatusChange(data, newStatus);
            setOpenPopover(null);
        },
        [data, onStatusChange]
    );

    const handleDeleteItem = useCallback(() => {
        onDelete(data);
        setOpenPopover(null);
    }, [data, onDelete]);

    const handleSelectItem = useCallback(() => {
        onSelect(data.id);
    }, [data, onSelect]);

    return (
        <>
            <Card
                sx={{
                    opacity: isDragging ? 0.5 : 1,
                    transition: 'opacity 0.2s',
                    ...(color && {
                        color: `${color}.darker`,
                        backgroundColor: 'common.white',
                        backgroundImage: `linear-gradient(135deg, ${varAlpha(theme.vars.palette[color].lighterChannel, 0.48)}, ${varAlpha(theme.vars.palette[color].lightChannel, 0.48)})`,
                    }),
                }}
            >
                <CardHeader
                    avatar={
                        <Avatar
                            {...dragHandleProps}
                            src={
                                data.assigneeId
                                    ? `/assets/images/avatar/avatar-${data.assigneeId}.webp`
                                    : ''
                            }
                            sx={{
                                cursor: 'grab', // Adds grab cursor on hover
                                '&:active': {
                                    cursor: 'grabbing', // When actually dragging
                                },
                            }}
                        />
                    }
                    action={
                        <IconButton aria-label="settings" onClick={handleOpenPopover}>
                            <Iconify icon="eva:more-vertical-fill" />
                        </IconButton>
                    }
                    //subheader="#1"
                    //title="Shrimp and Chorizo Paella"
                />
                <CardContent
                    sx={{
                        pl: 3,
                        pr: 3,
                    }}
                >
                    <Typography variant="caption" color="primary.darker">
                        #{data.id}
                    </Typography>
                    <Typography
                        variant="subtitle2"
                        onClick={handleSelectItem}
                        sx={{
                            cursor: 'pointer',
                            '&:hover': {
                                textDecoration: 'underline',
                            },
                        }}
                    >
                        {data.title}
                    </Typography>
                </CardContent>
            </Card>

            <Popover
                open={!!openPopover}
                anchorEl={openPopover}
                onClose={handleClosePopover}
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
                    <MenuItem onClick={() => handleStatusChange(TASK_STATUS.BACKLOG)}>
                        <Iconify icon="material-symbols:pending-actions-rounded" />
                        Backlog
                    </MenuItem>
                    <MenuItem onClick={() => handleStatusChange(TASK_STATUS.TODO)}>
                        <Iconify icon="material-symbols:checklist-rounded" />
                        To Do
                    </MenuItem>
                    <MenuItem onClick={() => handleStatusChange(TASK_STATUS.INPROGRESS)}>
                        <Iconify icon="material-symbols:sync-rounded" />
                        In Progress
                    </MenuItem>
                    <MenuItem onClick={() => handleStatusChange(TASK_STATUS.DONE)}>
                        <Iconify icon="material-symbols:check-circle-rounded" />
                        Done
                    </MenuItem>
                    <MenuItem onClick={() => handleDeleteItem()} sx={{ color: 'error.main' }}>
                        <Iconify icon="solar:trash-bin-trash-bold" />
                        Delete
                    </MenuItem>
                </MenuList>
            </Popover>
        </>
    );
};
