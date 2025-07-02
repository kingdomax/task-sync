import { render, screen, fireEvent } from '@testing-library/react';

import { KanbanItem } from './kanban-item';
import { TASK_STATUS } from './type/kanban-item';

// mock module
jest.mock('minimal-shared/utils', () => ({
    varAlpha: () => 'rgba(0,0,0,0.1)',
}));

jest.mock('src/components/iconify/iconify', () => ({
    Iconify: () => <span data-testid="mock-icon" />,
}));

const mockData = {
    id: 101,
    title: 'Fix login bug',
    assigneeId: 1,
    status: TASK_STATUS.TODO,
    lastModified: new Date().toISOString(),
};

describe('<KanbanItem />', () => {
    test('renders task title and id', () => {
        render(
            <KanbanItem
                data={mockData}
                onSelect={jest.fn()}
                onStatusChange={jest.fn()}
                onDelete={jest.fn()}
            />
        );

        expect(screen.getByText('#101')).toBeInTheDocument();
        expect(screen.getByText('Fix login bug')).toBeInTheDocument();
    });

    test('opens popover on settings icon click', () => {
        render(
            <KanbanItem
                data={mockData}
                onSelect={jest.fn()}
                onStatusChange={jest.fn()}
                onDelete={jest.fn()}
            />
        );

        const settingButton = screen.getByRole('button', { name: /settings/i }); // because we use <IconButton /> component, normall we can screen.GetByText()
        fireEvent.click(settingButton);

        expect(screen.getByText('Backlog')).toBeInTheDocument();
        expect(screen.getByText('To Do')).toBeInTheDocument();
        expect(screen.getByText('In Progress')).toBeInTheDocument();
        expect(screen.getByText('Done')).toBeInTheDocument();
        expect(screen.getByText('Delete')).toBeInTheDocument();
    });

    test.each([
        [TASK_STATUS.BACKLOG, 'Backlog'],
        [TASK_STATUS.TODO, 'To Do'],
        [TASK_STATUS.INPROGRESS, 'In Progress'],
        [TASK_STATUS.DONE, 'Done'],
    ])('calls onStatusChange when "%s" is clicked', (status: TASK_STATUS, buttonName: string) => {
        const mockStatusChange = jest.fn(); // mock function
        render(
            <KanbanItem
                data={mockData}
                onSelect={jest.fn()}
                onStatusChange={mockStatusChange}
                onDelete={jest.fn()}
            />
        );

        // click setting to open pop over first
        const settingButton = screen.getByRole('button', { name: /settings/i });
        fireEvent.click(settingButton);

        // click done button in popover
        const doneButton = screen.getByText(buttonName);
        fireEvent.click(doneButton);

        expect(mockStatusChange).toHaveBeenCalledWith(mockData, status);
    });

    test('calls onDelete when delete is clicked', () => {
        const mockDelete = jest.fn();
        render(
            <KanbanItem
                data={mockData}
                onSelect={jest.fn()}
                onStatusChange={jest.fn()}
                onDelete={mockDelete}
            />
        );

        // click setting to open pop over first
        const settingButton = screen.getByRole('button', { name: /settings/i });
        fireEvent.click(settingButton);

        // click done button in popover
        const deleteButton = screen.getByText('Delete');
        fireEvent.click(deleteButton);

        expect(mockDelete).toHaveBeenCalledWith(mockData);
    });
});
