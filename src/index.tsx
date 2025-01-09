import React from 'react';
import ReactDOM from 'react-dom/client';
import TimeClock from './components/TimeClock';
import './index.css';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

root.render(
  <React.StrictMode>
    <TimeClock employeeId={1} />
  </React.StrictMode>
);