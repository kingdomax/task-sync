import { Navigate, Outlet } from 'react-router-dom';

import { getAuthToken } from 'src/utils/auth-token';

export const ProtectedRoute = () => {
    const token = getAuthToken();

    if (!token) {
        return <Navigate to="/sign-in" replace />;
    }

    return <Outlet />;
};
