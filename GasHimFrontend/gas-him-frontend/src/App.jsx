
import React, { useState } from 'react';
import StartScreen from './components/StartScreen';
import SubstancesPage from './components/SubstancesPage';
import ProcessesPage from './components/ProcessesPage';
import './App.css';

function App() {
  const [tab, setTab] = useState('chains');

  return (
    <div className="App">
      <nav className="main-tabs">
        <button className={tab === 'chains' ? 'active' : ''} onClick={() => setTab('chains')}>Синтез цепочек</button>
        <button className={tab === 'substances' ? 'active' : ''} onClick={() => setTab('substances')}>Вещества</button>
        <button className={tab === 'processes' ? 'active' : ''} onClick={() => setTab('processes')}>Процессы</button>
      </nav>
      {tab === 'chains' && <StartScreen />}
      {tab === 'substances' && <SubstancesPage />}
      {tab === 'processes' && <ProcessesPage />}
    </div>
  );
}

export default App;
