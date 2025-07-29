import React from 'react';
import ChainItem from './ChainItem';
import '../styles/ChainList.css';

const ChainList = ({ chains }) => {
  if (!chains || !Array.isArray(chains)) {
    return (
      <div className="chain-list">
        <p>Цепочки не найдены (данные не являются массивом)</p>
      </div>
    );
  }

  return (
    <div className="chain-list">
      {chains.length === 0 ? (
        <p>Цепочки не найдены</p>
      ) : (
        chains.map((chain, index) => (
          <ChainItem key={index} chain={chain} index={index} />
        ))
      )}
    </div>
  );
};

export default ChainList;
