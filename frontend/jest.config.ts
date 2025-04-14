export default {
    preset: 'ts-jest',
    testEnvironment: 'jsdom',
    setupFilesAfterEnv: ['./src/setupTests.ts'],
    moduleNameMapper: {
        '^src/(.*)$': '<rootDir>/src/$1',
        '\\.(css|less|scss|sass)$': 'identity-obj-proxy',
    },
    transform: {
        '^.+\\.(ts|tsx)$': 'ts-jest',
    },
    testMatch: ['**/__tests__/**/*.[jt]s?(x)', '**/?(*.)+(spec|test).[tj]s?(x)'],
    clearMocks: true,
    resetModules: true,
    restoreMocks: true,
    reporters: [
        'default',
        [
            'jest-junit',
            {
                outputDirectory: 'test-results/jest',
                outputName: 'results.xml',
            },
        ],
    ],
};
