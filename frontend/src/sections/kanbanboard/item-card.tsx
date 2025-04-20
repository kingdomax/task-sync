import type { PaletteColorKey } from 'src/theme/core';

import { useCallback, useState } from 'react';
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

import type { KanbanItemData } from './type/kanban-item';

type Props = {
    color?: PaletteColorKey;
    data: KanbanItemData;
};

export const KanbanItem = ({ color, data }: Props) => {
    const theme = useTheme();

    const [openPopover, setOpenPopover] = useState<HTMLButtonElement | null>(null);

    const handleOpenPopover = useCallback((event: React.MouseEvent<HTMLButtonElement>) => {
        setOpenPopover(event.currentTarget);
    }, []);

    const handleClosePopover = useCallback(() => {
        setOpenPopover(null);
    }, []);

    return (
        <>
            <Card
                sx={
                    color && {
                        color: `${color}.darker`,
                        backgroundColor: 'common.white',
                        backgroundImage: `linear-gradient(135deg, ${varAlpha(theme.vars.palette[color].lighterChannel, 0.48)}, ${varAlpha(theme.vars.palette[color].lightChannel, 0.48)})`,
                    }
                }
            >
                <CardHeader
                    avatar={
                        <Avatar
                            src={
                                data.assigneeId
                                    ? `/assets/images/avatar/avatar-${data.assigneeId}.webp`
                                    : ''
                            }
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
                <CardContent sx={{ pl: 3, pr: 3 }}>
                    <Typography variant="caption" color="primary.darker">
                        #{data.id}
                    </Typography>
                    <Typography variant="subtitle2">{data.title}</Typography>
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
                    <MenuItem onClick={handleClosePopover}>
                        <Iconify icon="material-symbols:pending-actions-rounded" />
                        BACKLOG
                    </MenuItem>
                    <MenuItem onClick={handleClosePopover}>
                        <Iconify icon="material-symbols:checklist-rounded" />
                        TODO
                    </MenuItem>
                    <MenuItem onClick={handleClosePopover}>
                        <Iconify icon="material-symbols:sync-rounded" />
                        IN PROGRESS
                    </MenuItem>
                    <MenuItem onClick={handleClosePopover}>
                        <Iconify icon="material-symbols:check-circle-rounded" />
                        DONE
                    </MenuItem>
                    <MenuItem onClick={handleClosePopover} sx={{ color: 'error.main' }}>
                        <Iconify icon="solar:trash-bin-trash-bold" />
                        Delete
                    </MenuItem>
                </MenuList>
            </Popover>
        </>
    );
};
