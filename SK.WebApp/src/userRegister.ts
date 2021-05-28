
import { anyEndpointKeys, getEndpointWithLanguageCode } from './endpoints';
import { anyLangageCodeKeys } from './languages';

export type RegisterToApiRequest = {
    username: string;
    email: string;
    password: string;
};

export type RegisterToApiResponse = {
  username: string;
  token: string;
  image: string;
};



export const userRegister = async ({
    username,
    email,
    password,
}: RegisterToApiRequest): Promise<RegisterToApiResponse | undefined> => {

    const apiUrl = getEndpointWithLanguageCode({
        endpointKey: anyEndpointKeys.register,
        languageCodeKey: anyLangageCodeKeys.english
    })
    
    try {
        const response = await fetch(apiUrl, {
            method: 'POST',
            mode: 'cors',
            headers: {
                'Conent-Type': 'application/json',
            },
            body: JSON.stringify({username, email, password})
        });

        return response.json();
    } catch (error) {
        console.warn('Register failed.', error);
    }
}
