import { ApiAdresses } from "./consts"

export const getUrlWithCulture = (address: string, culture: string): string => {
    return ApiAdresses.BASE + address.replace("{culture}", culture);
};
