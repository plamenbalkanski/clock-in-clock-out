import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './TimeClock.css';

interface TimeClockProps {
    employeeId: number;
}

const API_URL = 'https://timeclock-api-wln9.onrender.com';  // Hardcode at the top level

const TimeClock: React.FC<TimeClockProps> = ({ employeeId }) => {
    const [location, setLocation] = useState<string>('');
    const [entries, setEntries] = useState<any[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const handleClockIn = async () => {
        try {
            setLoading(true);
            const response = await axios.post(
                `${API_URL}/api/timeclock/clockin`,
                {
                    employeeId,
                    location: location || 'Unknown'
                }
            );
            setLoading(false);
            setError('Successfully clocked in!');
            fetchEntries();
        } catch (err) {
            setLoading(false);
            setError('Failed to clock in. Please try again.');
            console.error(err);
        }
    };

    // ... rest of the component
};

export default TimeClock; 