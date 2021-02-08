import React from 'react';
import AppBar from '../components/AppBar';
import Home from '../views/Home';
import './App.css';

const App: React.FC<unknown> = () => {
  return (
    <div className="App">
      <AppBar />
      <header className="App-header">
        <Home />
      </header>
    </div>
  );
}

export default App;
