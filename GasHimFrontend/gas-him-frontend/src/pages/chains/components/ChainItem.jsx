import React, { useState } from 'react';
import ChainModal from './ChainModal';
import '../styles/ChainItem.css';

const ChainItem = ({ chain, index }) => {
  const [showModal, setShowModal] = useState(false);

  const openModal = () => setShowModal(true);
  const closeModal = () => setShowModal(false);

  // Превью цепочки (короткий вид)
  // Выделим процессы красным (если item в []), вещества - чёрным
  const chainPreview = chain.map((item, idx) => {
    const isProcess = item.startsWith('[') && item.endsWith(']');
    return (
      <span key={idx} className={isProcess ? 'process' : 'substance'}>
        {item}
      </span>
    );
  });

  // Вставляем стрелку "->" между элементами
  const previewWithArrows = chainPreview.reduce((prev, curr) => [prev, ' -> ', curr]);

  return (
    <>
      <div className="chain-container" onClick={openModal}>
        <div className="chain-index">
          {index + 1}.
        </div>
        <div className="chain-summary">
          {previewWithArrows}
        </div>
      </div>
      {showModal && (
        <ChainModal chain={chain} onClose={closeModal} />
      )}
    </>
  );
};

export default ChainItem;
