export const TOKEN_KEY: string = 'authToken';
export const EXPIRATION_KEY: string = 'authTokenExpiration';

export const setAuthToken = (token: string): void => {
    const expiresIn = 3 * 60 * 60 * 1000; // 3 hour in milliseconds
    const expirationTime = new Date().getTime() + expiresIn;

    sessionStorage.setItem(TOKEN_KEY, token); // todo-moch: this vanulabel for CSRF, change to store in HTTP only cookie
    sessionStorage.setItem(EXPIRATION_KEY, expirationTime.toString());
};

export const getAuthToken = (): string | null => {
    const token = sessionStorage.getItem(TOKEN_KEY);
    const expiry = Number(sessionStorage.getItem(EXPIRATION_KEY));

    if (!token || Date.now() > expiry) {
        clearAuthToken();
        return null;
    }

    return token;
};

export const clearAuthToken = (): void => {
    sessionStorage.removeItem(TOKEN_KEY);
    sessionStorage.removeItem(EXPIRATION_KEY);
};
