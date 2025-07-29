import React, { useState } from 'react';
import { seedSubstances, seedProcesses, clearAll, clearProcesses } from '../services/api';

const TestPage = () => {
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState('');

  const handle = async (fn, label) => {
    setLoading(true);
    setMessage('');
    try {
      await fn();
      setMessage(label + ' — успешно!');
    } catch (e) {
      setMessage('Ошибка: ' + (e.message || e));
    }
    setLoading(false);
  };

  return (
    <div style={{ padding: 24 }}>
      <h2>Тестовые ручки</h2>
      <button disabled={loading} onClick={() => handle(() => seedSubstances(1000), 'Сид веществ')}>
        Seed 1000 веществ
      </button>
      <button disabled={loading} onClick={() => handle(() => seedProcesses(300), 'Сид процессов')} style={{ marginLeft: 8 }}>
        Seed 300 процессов
      </button>
      <button disabled={loading} onClick={() => handle(clearAll, 'Очистка всех данных')} style={{ marginLeft: 8 }}>
        Очистить всё
      </button>
      <button disabled={loading} onClick={() => handle(clearProcesses, 'Очистка процессов')} style={{ marginLeft: 8 }}>
        Очистить процессы
      </button>
      {message && <div style={{ marginTop: 16 }}>{message}</div>}
    </div>
  );
};

export default TestPage;
