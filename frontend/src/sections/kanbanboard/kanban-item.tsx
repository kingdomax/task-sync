import type { PaletteColorKey } from 'src/theme/core';

import { useState, useCallback } from 'react';
import { varAlpha } from 'minimal-shared/utils';

import Card from '@mui/material/Card';
import Avatar from '@mui/material/Avatar';
import { useTheme } from '@mui/material/styles';
import IconButton from '@mui/material/IconButton';
import CardHeader from '@mui/material/CardHeader';
import Typography from '@mui/material/Typography';
import CardContent from '@mui/material/CardContent';

import { Iconify } from 'src/components/iconify/iconify';

import { KanbanItemMenu } from './kanban-item-menu';

import type { TaskDto, TASK_STATUS } from './type/kanban-item';

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
            {/* todo-moch: refactor by life <Popover/> component up to <KanbanBoardView />, so it does not bloat the VDOM (i.e. waste more memory) */}
            <KanbanItemMenu
                anchorEl={openPopover}
                onClose={handleClosePopover}
                onStatusChange={handleStatusChange}
                onDelete={handleDeleteItem}
            />

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
        </>
    );
};
