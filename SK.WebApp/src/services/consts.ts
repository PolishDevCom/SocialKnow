export const apiAddress = 'https://localhost:44324';

export enum anyLangageCodeKeys {
  polish = 'POLISH',
  english = 'ENGLISH',
}

export type languageCodeMap = {
  [key in anyLangageCodeKeys]: string;
};

export const anyLanguageCode: languageCodeMap = {
  POLISH: 'pl-PL',
  ENGLISH: 'en-US',
};

export enum anyEndpointKeys {
  login = 'LOGIN',
}

export type endpointMap = {
  [key in anyEndpointKeys]: string;
};

export const anyEndpoint: endpointMap = {
  LOGIN: '/{languageCode}/api/User/login',
};
