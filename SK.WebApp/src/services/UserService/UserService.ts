import { ApiAdresses, Culture } from "../consts";
import { getUrlWithCulture } from './../utils';
import { LoginToApiRequest, LoginToApiResponse } from "./types";

export const loginToApi = ({ email, password }: LoginToApiRequest): Promise<LoginToApiResponse> => {
    const apiUrl = getUrlWithCulture(ApiAdresses.LOGIN, Culture.ENGLISH);
    return fetch(apiUrl, {
        method: "POST",
        mode: "cors",
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
    }).then(response => response.json());
};
