import React from "react";

const Login: React.FC<unknown> = () => {
    return (
        <div>
            <p>
                Username: <input type="text"/>
            </p>
            <p>
                Password: <input type="password"/>
            </p>
            <p>
                <button>Login</button>
            </p>
        </div>
    )
};

export default Login;