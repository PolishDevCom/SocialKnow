import React from 'react';
import Home from '../views/Home';
import './App.css';

const App: React.FC<unknown> = () => {
  return (
    <div className="App">
      <header className="App-header">
        <Home />
      </header>
    </div>
  );
}

export default App;
