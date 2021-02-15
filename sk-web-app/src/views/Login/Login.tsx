import React, { useCallback, useState } from "react";
import { loginToApi } from "../../services/UserService";

const Login: React.FC<unknown> = () => {
    const [email, setEmail] = useState<string>("administrator@localhost");
    const [password, setPassword] = useState<string>("Administrator1!");

    const loginCallback = useCallback(
        (): void => {
            loginToApi({
                email,
                password,
            }).then(data => {
                console.log(data.username);
                console.log(data.token);
                console.log(data.image);
            });
        },
        [password, email],
    )

    return (
        <div>
            <p>
                e-mail: 
                <input type="text" value={email} onChange={event => setEmail(event.target.value)} />
            </p>
            <p>
                Password: 
                <input type="password" value={password} onChange={event => setPassword(event.target.value)}/>
            </p>
            <p>
                <button onClick={loginCallback}>
                    Login
                </button>
            </p>
        </div>
    )
};

export default Login;