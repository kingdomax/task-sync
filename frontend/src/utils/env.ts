// TODO: Hack it for now, need to refactor to use .env files for cleaner approach later.
//       But need to think carefully and that might involve changed on dockerfile, docker-compose.yml, docker-compose.prod.yml, and ci.yml !!
export const getServerUrl = (): string =>
    window.location.hostname.includes('localhost')
        ? 'http://localhost:5070'
        : 'http://131.189.90.113:5070';

export const getApiUrl = (): string => `${getServerUrl()}/api/v1`;
