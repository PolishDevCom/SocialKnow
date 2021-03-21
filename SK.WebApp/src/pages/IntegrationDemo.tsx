import React, { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../hooks/store";
import { loginToApi } from "../services/UserService";
import { sessionSlice } from "../store/session/sessionSlice";

export const IntegrationDemo = () => {
    const dispatch = useAppDispatch();
    const sessionData = useAppSelector((state) => state.session);
    
    useEffect(() => {
        const myFn = async () => {
            loginToApi({ email: "administrator@localhost", password: "Administrator1!" }).then((result) => {
                if (result) {
                    dispatch(sessionSlice.actions.setSession(result));
                }
            }).catch(() => {
                console.warn("Login was faild.");
            });
        }
        myFn();
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
