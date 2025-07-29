import React, { useState, useEffect, useMemo } from 'react';
import '../styles/ChainModal.css';
import { getProcessByName } from '../../../services/api';

const ChainModal = ({ chain, onClose }) => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [expandedChain, setExpandedChain] = useState([]);

  const fetchProcessDetails = async (processName) => {
    try {
      setLoading(true);
      const process = await getProcessByName(processName);
      return process;
    } catch (error) {
      setError(`Ошибка при получении процесса: ${error.message}`);
      console.error("Ошибка при получении процесса:", error);
      return null;
    } finally {
      setLoading(false);
    }
  };

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
        processName = processName.slice(1, -1);
      }
      stepsArray.push({
        input: inputSubstance,
        processName,
        output: outputSubstance
      });
    }
    return stepsArray;
  }, [chain]);

  useEffect(() => {
    const fetchExpandedChain = async () => {
      const expandedSteps = [];
      for (const step of steps) {
        const process = await fetchProcessDetails(step.processName);
        if (process) {
          const inputSubstances = process.inputSubstances.split(';').map(s => s.trim());
          const existingInputs = [step.input];
          const additionalInputs = inputSubstances.filter(substance => !existingInputs.includes(substance));
          expandedSteps.push({
            ...step,
            additionalInputs,
            output: process.outputSubstances,
          });
        }
      }
      setExpandedChain(expandedSteps);
    };
    if (steps.length > 0) {
      fetchExpandedChain();
    }
  }, [steps]);

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
                      {step.input}
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
                        <div className="substance-box">{step.output}</div>
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
