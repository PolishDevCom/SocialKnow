import { apiAddress, anyEndpointKeys, anyEndpoint } from './endpoints';
import { anyLanguageCode, anyLangageCodeKeys } from './languages';

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
