import {
    setAuthToken,
    getAuthToken,
    clearAuthToken,
    TOKEN_KEY,
    EXPIRATION_KEY,
} from './auth-token';

describe('auth-token.ts', () => {
    beforeEach(() => {
        localStorage.clear();
        jest.useFakeTimers(); // to control time
    });

    afterEach(() => {
        jest.useRealTimers();
    });

    test('setAuthToken should store token and expiration', () => {
        setAuthToken('dummy-token-1');

        const token = localStorage.getItem(TOKEN_KEY);
        expect(token).toBe('dummy-token-1');

        const expiry = Number(localStorage.getItem(EXPIRATION_KEY));
        expect(typeof expiry).toBe('number');
        expect(expiry).toBeGreaterThan(Date.now());
    });

    test('getAuthToken should return token if not expired', () => {
        setAuthToken('dummy-token-2');
        expect(getAuthToken()).toBe('dummy-token-2');
    });

    test('getAuthToken should return null and clear storage if expired', () => {
        setAuthToken('dummy-token-3');

        jest.advanceTimersByTime(4 * 60 * 60 * 1000);

        expect(getAuthToken()).toBeNull();
        expect(localStorage.getItem(TOKEN_KEY)).toBeNull();
        expect(localStorage.getItem(EXPIRATION_KEY)).toBeNull();
    });

    test('clearAuthToken should remove token and expiration in storage', () => {
        setAuthToken('dummy-token-4');
        clearAuthToken();

        expect(localStorage.getItem(TOKEN_KEY)).toBeNull();
        expect(localStorage.getItem(EXPIRATION_KEY)).toBeNull();
    });
});
