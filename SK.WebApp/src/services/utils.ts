import { apiAddress, anyEndpoint, anyLanguageCode, anyEndpointKeys, anyLangageCodeKeys } from "./consts";

interface getEndpointWithLanguageCodeArgs {
    endpointKey: anyEndpointKeys;
    languageCodeKey: anyLangageCodeKeys;
};

export const getEndpointWithLanguageCode = ({
    endpointKey,
    languageCodeKey,
  }: getEndpointWithLanguageCodeArgs) => {
    const endpointAddress = anyEndpoint[endpointKey].replace('{languageCode}', anyLanguageCode[languageCodeKey]);

    return `${apiAddress}${endpointAddress}`;
};
