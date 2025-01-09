import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './TimeClock.css';

interface TimeClockProps {
    employeeId: number;
}

const TimeClock: React.FC<TimeClockProps> = ({ employeeId }) => {
    const [location, setLocation] = useState<string>('');
    const [entries, setEntries] = useState<any[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    useEffect(() => {
        if (employeeId) {
            fetchEntries();
        }
    }, [employeeId]);

    const fetchEntries = async () => {
        try {
            const response = await axios.get(
                `https://timeclock-api-wln9.onrender.com/api/timeclock/employee/${employeeId}`
            );
            setEntries(response.data);
        } catch (err) {
            setError('Failed to fetch entries');
            console.error('Fetch error:', err);
        }
    };

    const handleClockOut = async () => {
        try {
            setLoading(true);
            await axios.post(
                `https://timeclock-api-wln9.onrender.com/api/timeclock/clockout/${employeeId}`
            );
            setLoading(false);
            setError('Successfully clocked out!');
            fetchEntries();
        } catch (err) {
            setLoading(false);
            setError('Failed to clock out. Please try again.');
            console.error('Clock out error:', err);
        }
    };

    // ... rest of the component
};

export default TimeClock; 