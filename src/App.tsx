import React from 'react';
import TimeClock from './components/TimeClock';

const App: React.FC = () => {
    // This employeeId would typically come from your authentication/user context
    const employeeId = 12345; 

    return (
        <div>
            <TimeClock employeeId={employeeId} />
        </div>
    );
};

export default App; 