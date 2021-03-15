import React, { useEffect, useState } from "react";
import { loginToApi } from "../services/UserService";
import { LoginToApiResponse } from "../services/UserService/types";

export const IntegrationDemo = () => {
    const [loginResult, setLoginResult] = useState<LoginToApiResponse | undefined>(undefined);
    useEffect(() => {
        loginToApi({ email: "administrator@localhost", password: "Administrator1!" }).then((result) => {
            if (result) {
                setLoginResult(result);
            }
        });
    }, []);

    return (
        <div>
            <p>Tutaj dodaj komponent sprawdzający czy zapytanie do API zadziałało prawidłowo.</p>
            <p>1. Login</p>
            <div>
                <p>Login status: {loginResult? "Suuccess" : "Error"}</p>
                <p>Username: {loginResult?.username}</p>
                <p>Token: {loginResult?.token}</p>
                <p>Image: {loginResult?.image}</p>
            </div>
        </div>
    );
};
