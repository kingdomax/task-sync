import { Label } from 'src/components/label';
import { SvgColor } from 'src/components/svg-color';

// ----------------------------------------------------------------------

const icon = (name: string) => <SvgColor src={`/assets/icons/navbar/${name}.svg`} />;

export type NavItem = {
    title: string;
    path: string;
    icon: React.ReactNode;
    info?: React.ReactNode;
};

export const navData = [
    {
        title: 'Board',
        path: '/',
        icon: icon('ic-analytics'),
        info: (
            <Label color="error" variant="inverted">
                +3
            </Label>
        ),
    },
    {
        title: 'Not found',
        path: '/404',
        icon: icon('ic-disabled'),
    },
    {
        title: 'Sign in',
        path: '/sign-in',
        icon: icon('ic-lock'),
    },
    {
        title: '[TODO] User',
        path: '/user',
        icon: icon('ic-user'),
    },
    {
        title: '[TODO] Product',
        path: '/products',
        icon: icon('ic-cart'),
    },
    {
        title: '[TODO] Blog',
        path: '/blog',
        icon: icon('ic-blog'),
    },
    {
        title: '[TODO] Analytic',
        path: '/dashboard',
        icon: icon(''),
    },
];
