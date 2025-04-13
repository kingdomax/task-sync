interface GreetingProps {
    name: string;
}

export const Greeting = ({ name }: GreetingProps) => {
    return <h1>Hello {name}</h1>;
};
