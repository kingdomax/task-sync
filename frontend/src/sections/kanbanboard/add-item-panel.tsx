import type { TransitionProps } from '@mui/material/transitions';

import { useReducer, forwardRef } from 'react';

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

type FormState = {
    isDialogOpen: boolean;
    isLoading: boolean;
    title: string;
    assigneeId: number;
    fileName: string;
};

type Action =
    | { type: 'openDialog' }
    | { type: 'closeDialog' }
    | { type: 'startSubmit' }
    | { type: 'submitSuccess' }
    | { type: 'submitError' }
    | { type: 'setTitle'; payload: string }
    | { type: 'setAssignee'; payload: number }
    | { type: 'setFilename'; payload: string };

const initialState: FormState = {
    isDialogOpen: false,
    isLoading: false,
    title: '',
    assigneeId: 0,
    fileName: '',
};

function reducer(state: FormState, action: Action): FormState {
    switch (action.type) {
        case 'openDialog':
            return { ...state, isDialogOpen: true };
        case 'closeDialog':
            return { ...initialState };
        case 'startSubmit':
            return { ...state, isLoading: true };
        case 'submitSuccess':
            return { ...initialState };
        case 'submitError':
            return { ...state, isLoading: false };
        case 'setTitle':
            return { ...state, title: action.payload };
        case 'setAssignee':
            return { ...state, assigneeId: action.payload };
        case 'setFilename':
            return { ...state, fileName: action.payload };
        default:
            return state;
    }
}

export const AddItemPanel = ({ onAddItem }: Props) => {
    const [state, dispatch] = useReducer(reducer, initialState);

    const handleSuccess = () => dispatch({ type: 'submitSuccess' });
    const handleError = (msg: string) => {
        dispatch({ type: 'submitError' });
        alert(`Failed to add item: ${msg}`);
    };

    return (
        <>
            <Box sx={{ mb: 5, display: 'flex', alignItems: 'center' }}>
                <Button
                    variant="contained"
                    color="inherit"
                    startIcon={<Iconify icon="mingcute:add-line" />}
                    onClick={() => dispatch({ type: 'openDialog' })}
                >
                    Add item
                </Button>
            </Box>

            <Dialog
                open={state.isDialogOpen}
                onClose={(_, reason) => {
                    if (reason !== 'backdropClick' && reason !== 'escapeKeyDown') {
                        dispatch({ type: 'closeDialog' });
                    }
                }}
                disableEscapeKeyDown
                slots={{ transition: Transition }}
                slotProps={{
                    paper: {
                        component: 'form',
                        onSubmit: (event: React.FormEvent<HTMLFormElement>) => {
                            event.preventDefault();
                            dispatch({ type: 'startSubmit' });

                            const newItem: AddItemRequest = {
                                title: state.title,
                                assigneeId: state.assigneeId <= 0 ? null : state.assigneeId,
                                lastModified: new Date(),
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
                        type="text"
                        value={state.title}
                        onChange={(e) => dispatch({ type: 'setTitle', payload: e.target.value })}
                        margin="dense"
                        id="title"
                        name="title"
                        label="Title"
                        fullWidth
                        variant="standard"
                    />
                    <FormControl variant="standard" sx={{ minWidth: 200, mt: 1, mb: 0.5 }}>
                        <InputLabel required id="assignee-label">
                            Assignee
                        </InputLabel>
                        <Select
                            required
                            value={state.assigneeId.toString()}
                            onChange={(e) =>
                                dispatch({ type: 'setAssignee', payload: Number(e.target.value) })
                            }
                            labelId="assignee-label"
                            id="assignee-select"
                            label="Age"
                        >
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
                            {state.fileName || 'Upload files'}
                            <VisuallyHiddenInput
                                type="file"
                                onChange={(event) => {
                                    const file = event.target.files?.[0];
                                    if (file) {
                                        dispatch({ type: 'setFilename', payload: file.name });
                                    }
                                }}
                                multiple
                            />
                        </Button>
                    </Box>
                </DialogContent>
                <DialogActions>
                    <Button
                        color="inherit"
                        onClick={() => dispatch({ type: 'closeDialog' })}
                        disabled={state.isLoading}
                    >
                        Cancel
                    </Button>
                    <Button color="inherit" type="submit" disabled={state.isLoading}>
                        Create
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    );
};

const Transition = forwardRef(function Transition(
    props: TransitionProps & { children: React.ReactElement<any, any> },
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
