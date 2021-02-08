import React from 'react';
import {
  BrowserRouter as Router,
  Switch,
  Route,
} from "react-router-dom";

import AppBar from '../components/AppBar';
import Home from '../views/Home';
import Login from '../views/Login';

const App: React.FC<unknown> = () => {
  return (
    <div className="App">
      <Router>
        <AppBar />
        <Switch>
          <Route path="/login">
            <Login />
          </Route>
          <Route path="/">
            <Home />
          </Route>
        </Switch>
      </Router>
    </div>
  );
}

export default App;
