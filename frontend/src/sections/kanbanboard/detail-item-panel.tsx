import Box from '@mui/material/Box';
import Stack from '@mui/material/Stack';
import Drawer from '@mui/material/Drawer';
import Divider from '@mui/material/Divider';
import { useTheme } from '@mui/material/styles';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';

import { Iconify } from 'src/components/iconify';
import { Scrollbar } from 'src/components/scrollbar';

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
                    sx={{ flexBasis: 30, fontWeight: 300 }}
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
                        <Typography variant="subtitle2">Test Haha</Typography>
                    </Stack>
                </Stack>
            </Scrollbar>
        </Drawer>
    );
};
