import { ApiAdresses, Culture } from "./consts"

const getCulture = (): string => {
    return Culture.POLISH;
};

const getCultureInURL = (address: string, culture: string): string => {
    return ApiAdresses.BASE + address.replace("{culture}", culture);
};

export { getCulture, getCultureInURL };