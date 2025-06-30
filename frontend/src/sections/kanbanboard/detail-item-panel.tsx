import Box from '@mui/material/Box';
import Stack from '@mui/material/Stack';
import Avatar from '@mui/material/Avatar';
import Drawer from '@mui/material/Drawer';
import Divider from '@mui/material/Divider';
import { useTheme } from '@mui/material/styles';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';

import { Label } from 'src/components/label';
import { Iconify } from 'src/components/iconify';
import { Scrollbar } from 'src/components/scrollbar';

import { statusColors, statusLabels } from './viewdata/board-data';

import type { TaskDto, Nullable } from './type/kanban-item';

type Props = {
    item: Nullable<TaskDto>;
    onSelect: (item: Nullable<TaskDto>) => void;
};

export const DetailItemPanel = ({ item, onSelect }: Props) => {
    const theme = useTheme();

    if (item == null) {
        return null;
    }

    return (
        <Drawer
            anchor="right"
            open={item != null}
            onClose={() => onSelect(null)}
            slotProps={{
                paper: {
                    sx: { width: 800, overflow: 'hidden' },
                },
            }}
        >
            <Box
                sx={{
                    py: 2,
                    pl: 2.5,
                    pr: 1.5,
                    display: 'flex',
                    alignItems: 'center',
                }}
            >
                <Typography
                    variant="h6"
                    color={theme.vars.palette.text.secondary}
                    sx={{ marginRight: 1, fontWeight: 300 }}
                >
                    #{item.id}
                </Typography>
                <Typography variant="h6" sx={{ flexGrow: 1 }}>
                    {item.title}
                </Typography>
                <IconButton onClick={() => onSelect(null)}>
                    <Iconify icon="mingcute:close-line" />
                </IconButton>
            </Box>

            <Divider />

            <Scrollbar>
                <Stack spacing={3} sx={{ p: 3 }}>
                    <Stack spacing={1}>
                        <Typography variant="subtitle2">
                            <Iconify
                                height={16}
                                width={16}
                                icon="google-material-icon-outlined:insert-emoticon"
                                sx={{ mr: 0.5 }}
                            />
                            Assignee
                        </Typography>
                        <Avatar
                            src={
                                item.assigneeId
                                    ? `/assets/images/avatar/avatar-${item.assigneeId}.webp`
                                    : ''
                            }
                        />
                    </Stack>
                    <Stack spacing={1}>
                        <Typography variant="subtitle2">
                            <Iconify
                                height={16}
                                width={16}
                                icon="google-material-icon-outlined:timeline"
                                sx={{ mr: 0.5 }}
                            />
                            Status
                        </Typography>
                        <Label color={statusColors[item.status]} sx={{ width: 80 }}>
                            {statusLabels[item.status]}
                        </Label>
                    </Stack>
                    <Stack spacing={1}>
                        <Typography variant="subtitle2">
                            <Iconify
                                height={16}
                                width={16}
                                icon="material-icon-outlined:description"
                                sx={{ mr: 0.5 }}
                            />
                            Description
                        </Typography>
                        <Typography variant="body2">
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod
                            tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim
                            veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea
                            commodo consequat. Duis aute irure dolor in reprehenderit in voluptate
                            velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint
                            occaecat cupidatat non proident, sunt in culpa qui officia deserunt
                            mollit anim id est laborum.
                        </Typography>
                    </Stack>
                    <Stack spacing={1}>
                        <Typography variant="subtitle2">
                            <Iconify
                                height={16}
                                width={16}
                                icon="material-icon-outlined:attach-file"
                                sx={{ mr: 0.5 }}
                            />
                            Attachment
                        </Typography>
                        <Typography variant="body2">link or none</Typography>
                    </Stack>
                    <Stack spacing={1}>
                        <Typography variant="subtitle2">
                            <Iconify
                                height={16}
                                width={16}
                                icon="solar:chat-round-dots-bold"
                                sx={{ mr: 0.5 }}
                            />
                            Comments and activity
                        </Typography>
                        <Typography variant="body2">
                            we can split vertical column to 2 like Trello or stick comment section
                            at the bottom with Scrollbar component.
                        </Typography>
                    </Stack>
                </Stack>
            </Scrollbar>
        </Drawer>
    );
};
