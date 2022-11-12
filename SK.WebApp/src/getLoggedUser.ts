
export type GetLoggedUserResponse = {
  username: string;
  token: string;
  image: string;
};

export const getLoggedUser = async (): Promise<GetLoggedUserResponse | undefined> => {

  try {
    const response = await fetch(`localhost:5001/${navigator.language}/api/User`, {
      method: 'GET',
      mode: 'cors',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    return response.json();
  } catch (error) {
    console.warn('Login was failed.', error);
  }
};
