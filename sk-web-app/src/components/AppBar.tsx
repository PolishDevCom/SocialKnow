import React from "react";
import { Link } from "react-router-dom";
import { VIEWS_PATH } from "../consts";

const AppBar: React.FC<unknown> = () => {
    return (
        <div>
            <p>
                <Link to={`${VIEWS_PATH.HOME}`}>Home</Link> || 
                <Link to={`${VIEWS_PATH.LOGIN}`}>Login</Link> || 
            </p>
        </div>
    );
};

export default AppBar;