import React from 'react';
import './Card.css';

const Card = ({ children, onClick, style }) => (
  <div className="custom-card" onClick={onClick} style={style}>
    {children}
  </div>
);

export default Card;
