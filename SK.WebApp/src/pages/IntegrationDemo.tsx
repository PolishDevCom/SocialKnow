import React, { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../hooks/store";
import { loginToApi } from "../services/UserService";
import { sessionSelector, sessionSlice } from "../store/session/sessionSlice";

export const IntegrationDemo: React.FC<unknown> = () => {
    const dispatch = useAppDispatch();
    const sessionData = useAppSelector(sessionSelector);
    
    useEffect(() => {
        loginToApi({ email: "administrator@localhost", password: "Administrator1!" }).then((result) => {
            if (result) {
                dispatch(sessionSlice.actions.saveLoginData(result));
            }
        });
    }, [dispatch]);

    return (
        <div>
            <p>Tutaj dodaj komponent sprawdzający czy zapytanie do API zadziałało prawidłowo.</p>
            <p>1. Login</p>
            <div>
                <p>Login status: {sessionData.token? "Suuccess" : "Error"}</p>
                <p>Username: {sessionData?.username}</p>
                <p>Token: {sessionData?.token}</p>
                <p>Image: {sessionData?.image}</p>
            </div>
        </div>
    );
};
