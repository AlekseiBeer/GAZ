import React, { useState, useEffect, useMemo } from 'react';
import './ChainModal.css';
import { getProcessByName } from '../services/api'; // Импортируем функцию для получения процесса по имени

const ChainModal = ({ chain, processes, onClose }) => {
  const [loading, setLoading] = useState(false); // Состояние для загрузки
  const [error, setError] = useState(null); // Состояние для ошибок
  const [expandedChain, setExpandedChain] = useState([]); // Состояние для расширенной цепочки

  // Функция для получения процесса по имени
  const fetchProcessDetails = async (processName) => {
    try {
      setLoading(true);
      const process = await getProcessByName(processName); // Получаем процесс по имени
      return process; // Возвращаем данные процесса
    } catch (error) {
      setError(`Ошибка при получении процесса: ${error.message}`);
      console.error("Ошибка при получении процесса:", error);
      return null;
    } finally {
      setLoading(false);
    }
  };

  // Формируем шаги цепочки
  const steps = useMemo(() => {
    if (!chain || chain.length === 0) {
      setError('Цепочка не найдена или пустая');
      return [];
    }

    const stepsArray = [];
    for (let i = 0; i < chain.length - 1; i += 2) {
      const inputSubstance = chain[i];
      const processNameRaw = chain[i + 1];
      const outputSubstance = chain[i + 2] || null;

      let processName = processNameRaw;
      if (processName && processName.startsWith('[') && processName.endsWith(']')) {
        processName = processName.slice(1, -1); // Убираем квадратные скобки
      }

      stepsArray.push({
        input: inputSubstance,
        processName,
        output: outputSubstance
      });
    }

    return stepsArray;
  }, [chain]);

  // Загружаем расширенную цепочку с дополнительными веществами
  useEffect(() => {
    const fetchExpandedChain = async () => {
      const expandedSteps = [];
      for (const step of steps) {
        const process = await fetchProcessDetails(step.processName);

        if (process) {
          const inputSubstances = process.inputSubstances.split(';').map(s => s.trim());
          const existingInputs = [step.input];

          // Проверяем на дополнительные вещества
          const additionalInputs = inputSubstances.filter(substance => !existingInputs.includes(substance));

          // Формируем расширенную цепочку, добавляем вещества на вход
          expandedSteps.push({
            ...step,
            additionalInputs, // Добавляем дополнительные вещества на вход
            output: process.outputSubstances, // Выходные вещества
          });
        }
      }
      setExpandedChain(expandedSteps);
    };

    if (steps.length > 0) {
      fetchExpandedChain();
    }
  }, [steps]);

  // Функция для обработки клика по фону модалки
  const handleBackdropClick = (e) => {
    if (e.target.classList.contains('modal-backdrop')) {
      onClose();
    }
  };

  return (
    <div className="modal-backdrop" onClick={handleBackdropClick}>
      <div className="modal-content">
        <button className="modal-close" onClick={onClose}>×</button>
        <h2>Детали цепочки</h2>

        {error && <p className="error-message">{error}</p>}

        {loading ? (
          <p>Загрузка...</p>
        ) : (
          <div className="chain-steps">
            {expandedChain.length === 0 ? (
              <p>Цепочка не найдена или пустая</p>
            ) : (
              expandedChain.map((step, idx) => {
                return (
                  <div key={idx} className="chain-step">
                    <div className="substance-box">
                      {step.input} {/* Вещество на входе */}
                      {step.additionalInputs && step.additionalInputs.length > 0 && (
                        <div className="additional-inputs">
                          <p>Доп. вещества на вход:</p>
                          <ul>
                            {step.additionalInputs.map((inp, i) => (
                              <li key={i} className="additional-substance">{inp}</li>
                            ))}
                          </ul>
                        </div>
                      )}
                    </div>
                    <div className="arrow-box">↓</div>
                    <div className="process-box">
                      {step.processName}
                    </div>
                    {step.output && (
                      <>
                        <div className="arrow-box">↓</div>
                        <div className="substance-box">{step.output}</div> {/* Вещество на выходе */}
                      </>
                    )}
                  </div>
                );
              })
            )}
          </div>
        )}
      </div>
    </div>
  );
};

export default ChainModal;
