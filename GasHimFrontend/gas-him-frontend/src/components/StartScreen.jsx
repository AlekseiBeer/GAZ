import React, { useEffect, useState } from 'react';
import { getSubstances, synthesizeChains, synthesizeAllChains } from '../services/api';
import ChainList from '../pages/chains/components/ChainList';
import './StartScreen.css';

const StartScreen = () => {
  const [substances, setSubstances] = useState([]);
  const [startSubstance, setStartSubstance] = useState('');
  const [targetSubstance, setTargetSubstance] = useState('');
  const [chains, setChains] = useState([]);

  useEffect(() => {
    const fetchSubstances = async () => {
      try {
        const data = await getSubstances();
        setSubstances(data);
      } catch (error) {
        console.error("Ошибка при получении веществ:", error);
      }
    };
    fetchSubstances();
  }, []);

  const handleSynthesize = async () => {
    try {
      // Если оба поля заполнены, вызывается поиск цепочки от start к target.
      // Если заполнено только одно поле – используется соответствующий алгоритм.
      let result;
      if (startSubstance && targetSubstance) {
        result = await synthesizeChains(startSubstance, targetSubstance);
      } else if (startSubstance) {
        result = await synthesizeChains(startSubstance, "");
      } else if (targetSubstance) {
        result = await synthesizeChains("", targetSubstance);
      }
      setChains(result);
    } catch (error) {
      console.error("Ошибка синтеза цепочек:", error);
    }
  };

  const handleSynthesizeAll = async () => {
    try {
      const result = await synthesizeAllChains();
      setChains(result);
    } catch (error) {
      console.error("Ошибка комплексного синтеза:", error);
    }
  };

  return (
    <div className="start-screen">
      <h1>Синтез цепочек производственных процессов</h1>
      <div className="input-group">
        <div className="select-group">
          <label>Стартовое вещество:</label>
          <select value={startSubstance} onChange={(e) => setStartSubstance(e.target.value)}>
            <option value="">-- Выберите вещество --</option>
            {substances.map((s) => (
              <option key={s.id} value={s.name}>
                {s.name}
              </option>
            ))}
          </select>
        </div>
        <div className="select-group">
          <label>Конечное вещество:</label>
          <select value={targetSubstance} onChange={(e) => setTargetSubstance(e.target.value)}>
            <option value="">-- Выберите вещество --</option>
            {substances.map((s) => (
              <option key={s.id} value={s.name}>
                {s.name}
              </option>
            ))}
          </select>
        </div>
      </div>
      <div className="buttons">
        <button onClick={handleSynthesize}>Синтез</button>
        <button onClick={handleSynthesizeAll}>Синтезировать все варианты</button>
      </div>
      <ChainList chains={chains} />
    </div>
  );
};

export default StartScreen;