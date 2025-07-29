
import React, { useState } from 'react';
import ChainsPage from './pages/chains/ChainsPage';
import SubstancesPage from './pages/substances/SubstancesPage';
import ProcessesPage from './pages/processes/ProcessesPage';
import TestPage from './pages/test/TestPage';
import './App.css';

function App() {
  const [tab, setTab] = useState('chains');

  return (
    <div className="App">
      <nav className="main-tabs">
        <button className={tab === 'chains' ? 'active' : ''} onClick={() => setTab('chains')}>Синтез цепочек</button>
        <button className={tab === 'substances' ? 'active' : ''} onClick={() => setTab('substances')}>Вещества</button>
        <button className={tab === 'processes' ? 'active' : ''} onClick={() => setTab('processes')}>Процессы</button>
        <button className={tab === 'test' ? 'active' : ''} onClick={() => setTab('test')}>Тест</button>
      </nav>
      {tab === 'chains' && <ChainsPage />}
      {tab === 'substances' && <SubstancesPage />}
      {tab === 'processes' && <ProcessesPage />}
      {tab === 'test' && <TestPage />}
    </div>
  );
}

export default App;
