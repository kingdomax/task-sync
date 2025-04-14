export const TOKEN_KEY: string = 'authToken';
export const EXPIRATION_KEY: string = 'authTokenExpiration';

export const setAuthToken = (token: string): void => {
    const expiresIn = 3 * 60 * 60 * 1000; // 3 hour in milliseconds
    const expirationTime = new Date().getTime() + expiresIn;

    localStorage.setItem(TOKEN_KEY, token);
    localStorage.setItem(EXPIRATION_KEY, expirationTime.toString());
};

export const getAuthToken = (): string | null => {
    const token = localStorage.getItem(TOKEN_KEY);
    const expiry = Number(localStorage.getItem(EXPIRATION_KEY));

    if (!token || Date.now() > expiry) {
        clearAuthToken();
        return null;
    }

    return token;
};

export const clearAuthToken = (): void => {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(EXPIRATION_KEY);
};
