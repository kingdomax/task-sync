interface GreetingProps {
    name: string;
}

export const Greeting = ({ name }: GreetingProps) => <h1>Hello {name}</h1>;
