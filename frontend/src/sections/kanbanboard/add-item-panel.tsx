import type { TransitionProps } from '@mui/material/transitions';

import { useState, forwardRef } from 'react';

import Box from '@mui/material/Box';
import Slide from '@mui/material/Slide';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import Select from '@mui/material/Select';
import { styled } from '@mui/material/styles';
import MenuItem from '@mui/material/MenuItem';
import TextField from '@mui/material/TextField';
import InputLabel from '@mui/material/InputLabel';
import FormControl from '@mui/material/FormControl';
import DialogTitle from '@mui/material/DialogTitle';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';

import { Iconify } from 'src/components/iconify';

import type { AddItemRequest } from './type/kanban-item';

type Props = {
    onAddItem: (
        newItem: AddItemRequest,
        onSuccess: () => void,
        onFailure: (msg: string) => void
    ) => void;
};

export const AddItemPanel = ({ onAddItem }: Props) => {
    const [isDialogOpen, setDialogOpen] = useState(false);
    const [isLoading, setLoading] = useState(false);
    const [title, setTitle] = useState('');
    const [assigneeId, setAssigneeId] = useState(0);
    const [fileName, setFilename] = useState('');

    const handleSuccess = () => {
        setLoading(false);
        setTitle('');
        setAssigneeId(0);
        setFilename('');
        setDialogOpen(false);
    };

    const handleError = (msg: string) => {
        setLoading(false);
        alert(`Failed to add item: ${msg}`);
    };

    return (
        <>
            <Box
                sx={{
                    mb: 5,
                    display: 'flex',
                    alignItems: 'center',
                }}
            >
                <Button
                    variant="contained"
                    color="inherit"
                    startIcon={<Iconify icon="mingcute:add-line" />}
                    onClick={() => setDialogOpen(true)}
                >
                    Add item
                </Button>
            </Box>

            <Dialog
                open={isDialogOpen}
                onClose={(_, reason) => {
                    if (reason !== 'backdropClick' && reason !== 'escapeKeyDown') {
                        setDialogOpen(false);
                    }
                }}
                disableEscapeKeyDown
                slots={{
                    transition: Transition,
                }}
                slotProps={{
                    paper: {
                        component: 'form',
                        onSubmit: (event: React.FormEvent<HTMLFormElement>) => {
                            event.preventDefault();
                            setLoading(true);

                            const newItem: AddItemRequest = {
                                title,
                                assigneeId: assigneeId <= 0 ? null : assigneeId,
                                lastModified: new Date(),
                                projectId: 1, // todo-moch: hardcode for now
                            };

                            onAddItem(newItem, handleSuccess, handleError);
                        },
                    },
                }}
            >
                <DialogTitle>Add item</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        To create a task, please fill all necessary information below.
                    </DialogContentText>
                    <TextField
                        required
                        margin="dense"
                        id="title"
                        name="title"
                        label="Title"
                        type="text"
                        fullWidth
                        variant="standard"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                    />
                    <FormControl variant="standard" sx={{ minWidth: 200, mt: 1, mb: 0.5 }}>
                        <InputLabel required id="assignee-label">
                            Assignee
                        </InputLabel>
                        <Select
                            required
                            labelId="assignee-label"
                            id="assignee-select"
                            value={assigneeId.toString()}
                            onChange={(e) => setAssigneeId(Number(e.target.value))}
                            label="Age"
                        >
                            {/* todo-moch: fix hardcode later */}
                            <MenuItem value="0">None</MenuItem>
                            <MenuItem value="1">admin</MenuItem>
                            <MenuItem value="2">kingdomax</MenuItem>
                            <MenuItem value="3">jaydonk</MenuItem>
                        </Select>
                    </FormControl>
                    <Box sx={{ mt: 2.5, mb: 0.5 }}>
                        <Button
                            component="label"
                            role={undefined}
                            variant="contained"
                            color="inherit"
                            tabIndex={-1}
                            startIcon={<Iconify icon="material-symbols:upload" />}
                        >
                            {fileName || 'Upload files'}
                            <VisuallyHiddenInput
                                type="file"
                                onChange={(event) => console.log(event.target.files)}
                                multiple
                            />
                        </Button>
                    </Box>
                </DialogContent>
                <DialogActions>
                    <Button
                        color="inherit"
                        onClick={() => setDialogOpen(false)}
                        disabled={isLoading}
                    >
                        Cancel
                    </Button>
                    <Button color="inherit" type="submit" loading={isLoading}>
                        Create
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    );
};

const Transition = forwardRef(function Transition(
    props: TransitionProps & {
        children: React.ReactElement<any, any>;
    },
    ref: React.Ref<unknown>
) {
    return <Slide direction="up" ref={ref} {...props} />;
});

const VisuallyHiddenInput = styled('input')({
    clip: 'rect(0 0 0 0)',
    clipPath: 'inset(50%)',
    height: 1,
    overflow: 'hidden',
    position: 'absolute',
    bottom: 0,
    left: 0,
    whiteSpace: 'nowrap',
    width: 1,
});
