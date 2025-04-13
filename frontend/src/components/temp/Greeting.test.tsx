import { render } from '@testing-library/react';

import { Greeting } from './Greeting';

describe('<Greeting />', () => {
    test('greeting renders and match snapshot', () => {
        const { container, getByText } = render(<Greeting name="Pramoch" />);
        expect(container).toMatchSnapshot(); // structure test
        expect(getByText('Hello Pramoch')).toBeInTheDocument(); // behavior test
    });
});
