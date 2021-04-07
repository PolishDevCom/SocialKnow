export const apiAddress = 'https://localhost:44324';

export enum anyEndpointKeys {
  login = 'LOGIN',
}

export type endpointMap = {
  [key in anyEndpointKeys]: string;
};

export const anyEndpoint: endpointMap = {
  LOGIN: '/{languageCode}/api/User/login',
};
