import type { SelectChangeEvent } from '@mui/material/Select';
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

export const AddItemPanel = () => {
    const [isDialogOpen, setDialogOpen] = useState(false);
    const [assignee, setAssignee] = useState('');
    const [fileName, setFilename] = useState('');

    const handleOpenForm = () => setDialogOpen(true);
    const handleCloseForm = () => setDialogOpen(false);
    const handleAssignee = (event: SelectChangeEvent) => setAssignee(event.target.value);

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
                    onClick={() => handleOpenForm()}
                >
                    Add item
                </Button>
            </Box>

            <Dialog
                open={isDialogOpen}
                onClose={(_, reason) => {
                    if (reason !== 'backdropClick' && reason !== 'escapeKeyDown') {
                        handleCloseForm();
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
                            const formData = new FormData(event.currentTarget);
                            const formJson = Object.fromEntries((formData as any).entries());
                            const email = formJson.email;
                            console.log(email);
                            handleCloseForm();
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
                    />
                    <FormControl variant="standard" sx={{ minWidth: 200, mt: 1, mb: 0.5 }}>
                        <InputLabel required id="assignee-label">
                            Assignee
                        </InputLabel>
                        <Select
                            required
                            labelId="assignee-label"
                            id="assignee-select"
                            value={assignee}
                            onChange={handleAssignee}
                            label="Age"
                        >
                            <MenuItem value="">None</MenuItem>
                            <MenuItem value="a">Mr. A</MenuItem>
                            <MenuItem value="b">Mrs. B</MenuItem>
                            <MenuItem value="c">Miss C</MenuItem>
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
                    <Button color="inherit" onClick={handleCloseForm}>
                        Cancel
                    </Button>
                    <Button color="inherit" type="submit">
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
