const apiAddress = "https://localhost:44324";

enum anyLangageCodeKeys {
    polish = 'POLISH',
    english = 'ENGLISH',
};

type languageCodeMap = {
    [key in anyLangageCodeKeys] : string;
}

const anyLanguageCode: languageCodeMap = {
    POLISH: "pl-PL",
    ENGLISH: "en-US",
};

enum anyEndpointKeys {
    login = 'LOGIN',
};

type endpointMap = {
    [key in anyEndpointKeys]: string;
};

const anyEndpoint: endpointMap = {
    LOGIN: "/{languageCode}/api/User/login",
};

export { anyLanguageCode, anyLangageCodeKeys, anyEndpoint, anyEndpointKeys, apiAddress };