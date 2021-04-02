import { anyEndpointKeys, anyLangageCodeKeys } from './consts';
import { getEndpointWithLanguageCode } from './utils';

export type LoginToApiRequest = {
  email: string;
  password: string;
};

export type LoginToApiResponse = {
  username: string;
  token: string;
  image: string;
};

export const loginToApi = async ({
  email,
  password,
}: LoginToApiRequest): Promise<LoginToApiResponse | undefined> => {
  const apiUrl = getEndpointWithLanguageCode({
    endpointKey: anyEndpointKeys.login,
    languageCodeKey: anyLangageCodeKeys.english,
  });

  try {
    const response = await fetch(apiUrl, {
      method: 'POST',
      mode: 'cors',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ email, password }),
    });

    return response.json();
  } catch (error) {
    console.warn('Login was faild.', error);
  }
};
