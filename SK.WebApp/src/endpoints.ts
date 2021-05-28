import { anyLangageCodeKeys, anyLanguageCode } from './languages';
export const apiAddress = 'https://localhost:44324';

export enum anyEndpointKeys {
  login = 'LOGIN',
  register = 'REGISTER',
}

export type endpointMap = {
  [key in anyEndpointKeys]: string;
};

export const anyEndpoint: endpointMap = {
  LOGIN: '/{languageCode}/api/User/login',
  REGISTER: '/{languageCode}/api/User/register',
};

interface getEndpointWithLanguageCodeArgs {
  endpointKey: anyEndpointKeys;
  languageCodeKey: anyLangageCodeKeys;
}

export const getEndpointWithLanguageCode = ({
  endpointKey,
  languageCodeKey,
}: getEndpointWithLanguageCodeArgs) => {
  const endpointAddress = anyEndpoint[endpointKey].replace(
    '{languageCode}',
    anyLanguageCode[languageCodeKey]
  );

  return `${apiAddress}${endpointAddress}`;
};
