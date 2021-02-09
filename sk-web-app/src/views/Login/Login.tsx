import React, { useState } from "react";

const Login: React.FC<unknown> = () => {
    const [userName, setUserName] = useState<String>("");
    const [password, setPassword] = useState<String>("");
    return (
        <div>
            <p>
                Username: 
                <input type="text" onChange={event => setUserName(event.target.value)} />
            </p>
            <p>
                Password: 
                <input type="password" onChange={event => setPassword(event.target.value)}/>
            </p>
            <p>
                <button onClick={() => {
                    console.log(userName + " " + password);
                }}>
                    Login
                </button>
            </p>
        </div>
    )
};

export default Login;