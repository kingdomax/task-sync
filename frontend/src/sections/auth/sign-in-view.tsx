import { useState, useCallback } from 'react';

import Box from '@mui/material/Box';
import Link from '@mui/material/Link';
import Button from '@mui/material/Button';
import Divider from '@mui/material/Divider';
import TextField from '@mui/material/TextField';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import InputAdornment from '@mui/material/InputAdornment';

import { useRouter } from 'src/routes/hooks';

import { getApiUrl } from 'src/utils/env';

import { Iconify } from 'src/components/iconify';

interface TestResponse {
    username: string;
}

export function SignInView() {
    const router = useRouter();

    const [showPassword, setShowPassword] = useState(false);

    //const handleSignIn = useCallback(() => {
    //    router.push('/');
    //}, [router]);

    const handleSignIn = async () => {
        const response: any = await fetch(`${getApiUrl()}/Authentication/Test`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ id: 888, name: 'Hiiiii' }),
        });

        const data: TestResponse = await response.json();

        alert(data.username);
    };

    const renderForm = (
        <Box
            sx={{
                display: 'flex',
                alignItems: 'flex-end',
                flexDirection: 'column',
            }}
        >
            <TextField
                fullWidth
                name="email"
                label="Email address"
                defaultValue="admin@tasksync.com"
                sx={{ mb: 3 }}
                slotProps={{
                    inputLabel: { shrink: true },
                }}
            />

            {/*
            <Link variant="body2" color="inherit" sx={{ mb: 1.5 }}>
                Forgot password?
            </Link>
            */}

            <TextField
                fullWidth
                name="password"
                label="Password"
                defaultValue="admin"
                type={showPassword ? 'text' : 'password'}
                slotProps={{
                    inputLabel: { shrink: true },
                    input: {
                        endAdornment: (
                            <InputAdornment position="end">
                                <IconButton
                                    onClick={() => setShowPassword(!showPassword)}
                                    edge="end"
                                >
                                    <Iconify
                                        icon={
                                            showPassword
                                                ? 'solar:eye-bold'
                                                : 'solar:eye-closed-bold'
                                        }
                                    />
                                </IconButton>
                            </InputAdornment>
                        ),
                    },
                }}
                sx={{ mb: 3 }}
            />

            <Button
                fullWidth
                size="large"
                type="submit"
                color="inherit"
                variant="contained"
                onClick={handleSignIn}
            >
                Sign in
            </Button>
        </Box>
    );

    return (
        <>
            <Box
                sx={{
                    gap: 1.5,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    mb: 5,
                }}
            >
                <Typography variant="h5">Sign in</Typography>
                <Typography
                    variant="body2"
                    sx={{
                        color: 'text.secondary',
                    }}
                >
                    Don’t have an account?
                    <Link variant="subtitle2" sx={{ ml: 0.5 }}>
                        Get started
                    </Link>
                </Typography>
            </Box>
            {renderForm}
            <Divider sx={{ my: 3, '&::before, &::after': { borderTopStyle: 'dashed' } }}>
                <Typography
                    variant="overline"
                    sx={{ color: 'text.secondary', fontWeight: 'fontWeightMedium' }}
                >
                    OR
                </Typography>
            </Divider>
            <Box
                sx={{
                    gap: 1,
                    display: 'flex',
                    justifyContent: 'center',
                }}
            >
                <IconButton color="inherit">
                    <Iconify width={22} icon="socials:google" />
                </IconButton>
                <IconButton color="inherit">
                    <Iconify width={22} icon="socials:github" />
                </IconButton>
                <IconButton color="inherit">
                    <Iconify width={22} icon="socials:twitter" />
                </IconButton>
            </Box>
        </>
    );
}
