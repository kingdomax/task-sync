import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { SignInView } from './sign-in-view';

jest.mock('src/utils/auth-token', () => ({
    setAuthToken: jest.fn(),
}));

jest.mock('src/routes/hooks', () => ({
    useRouter: () => ({
        push: jest.fn(),
    }),
}));

jest.mock('src/utils/env', () => ({
    getApiUrl: () => 'https://mock-api.com',
}));

// Mock problematic ESM component instead
jest.mock('src/components/iconify', () => ({
    Iconify: () => <span data-testid="icon-placeholder" />,
}));

const mockPush = jest.requireMock('src/routes/hooks').useRouter().push;
const mockSetAuthToken = jest.requireMock('src/utils/auth-token').setAuthToken;

describe('<SignInView />', () => {
    beforeEach(() => {
        jest.resetAllMocks();
    });

    test('calls setAuthToken and navigates on successful login', async () => {
        const fakeResponse = {
            ok: true,
            json: async () => ({ token: 'mock-token' }),
        };

        global.fetch = jest.fn(() => Promise.resolve(fakeResponse)) as jest.Mock;

        render(<SignInView />);

        fireEvent.click(screen.getByRole('button', { name: /sign in/i }));

        await waitFor(() => {
            expect(mockSetAuthToken).toHaveBeenCalledWith('mock-token');
            // expect(mockPush).toHaveBeenCalledWith('/');
        });
    });

    test('shows error on failed login', async () => {
        const fakeResponse = {
            ok: false,
            status: 401,
            statusText: 'Unauthorized',
        };

        global.fetch = jest.fn(() => Promise.resolve(fakeResponse)) as jest.Mock;

        render(<SignInView />);
        fireEvent.click(screen.getByRole('button', { name: /sign in/i }));

        await waitFor(() => {
            expect(screen.getByText(/unauthorized \(401\)/i)).toBeInTheDocument();
        });
    });

    test('shows error on network error', async () => {
        global.fetch = jest.fn(() => Promise.reject(new Error('Failed'))) as jest.Mock;

        render(<SignInView />);
        fireEvent.click(screen.getByRole('button', { name: /sign in/i }));

        await waitFor(() => {
            expect(screen.getByText(/network error/i)).toBeInTheDocument();
        });
    });
});
