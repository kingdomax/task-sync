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
        icon: icon('ic-lock'),
    },
    {
        title: 'Dashboard (TODO)',
        path: '/dashboard',
        icon: icon('ic-analytics'),
    },
    {
        title: 'User (TODO)',
        path: '/user',
        icon: icon('ic-user'),
    },
    {
        title: 'Product (TODO)',
        path: '/products',
        icon: icon('ic-cart'),
        info: (
            <Label color="error" variant="inverted">
                +3
            </Label>
        ),
    },
    {
        title: 'Blog (TODO)',
        path: '/blog',
        icon: icon('ic-blog'),
    },
    /*{
        title: 'Sign in',
        path: '/sign-in',
        icon: icon('ic-lock'),
    },
    {
        title: 'Not found',
        path: '/404',
        icon: icon('ic-disabled'),
    },*/
];
