export type LoginToApiRequest = {
    email: string;
    password: string;
};

export type LoginToApiResponse = {
    username: string;
    token: string;
    image: string;
};
