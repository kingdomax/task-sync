import { render, screen } from '@testing-library/react';

import { Greeting } from './Greeting';

describe('<Greeting />', () => {
    test('greeting renders and match snapshot', () => {
        const { container } = render(<Greeting name="Pramoch" />);
        expect(container).toMatchSnapshot(); // structure test
        expect(screen.getByText('Hello Pramoch')).toBeInTheDocument(); // behavior test
    });
});
