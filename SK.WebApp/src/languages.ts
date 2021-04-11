export enum anyLangageCodeKeys {
  polish = 'POLISH',
  english = 'ENGLISH',
}

export type languageCodeMap = {
  [key in anyLangageCodeKeys]: string;
};

export const anyLanguageCode: languageCodeMap = {
  POLISH: 'pl-PL',
  ENGLISH: 'en-US',
};
