import { ApiAdresses } from "../consts";
import { getCulture, getCultureInURL } from "../utils";
import { LoginToApiRequest, LoginToApiResponse } from "./types";

export const loginToApi = ({ email, password }: LoginToApiRequest): Promise<LoginToApiResponse> => {
    const apiUrl = getCultureInURL(ApiAdresses.LOGIN, getCulture());
    return fetch(apiUrl, {
        method: "POST",
        mode: "cors",
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
    }).then(response => response.json());
};
