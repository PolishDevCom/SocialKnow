import { anyEndpointKeys, anyLangageCodeKeys } from '../consts';
import { getEndpointWithLanguageCode } from './../utils';
import { LoginToApiRequest, LoginToApiResponse } from "./types";

export const loginToApi = ({ email, password }: LoginToApiRequest): Promise<LoginToApiResponse> => {
    const apiUrl = getEndpointWithLanguageCode({ endpointKey: anyEndpointKeys.login, languageCodeKey: anyLangageCodeKeys.english});
    return fetch(apiUrl, {
        method: "POST",
        mode: "cors",
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
    }).then(response => response.json());
};
