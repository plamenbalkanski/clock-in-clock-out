import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { config } from '../config';
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
                `${config.API_URL}/api/timeclock/employee/${employeeId}`
            );
            setEntries(response.data);
        } catch (err) {
            setError('Failed to fetch entries');
            console.error('Fetch error:', err);
        }
    };

    const handleClockIn = async () => {
        try {
            setLoading(true);
            await axios.post(`${config.API_URL}/api/timeclock/clockin`, {
                employeeId,
                location: location || 'Unknown'
            });
            setLoading(false);
            setError('Successfully clocked in!');
            fetchEntries();
        } catch (err) {
            setLoading(false);
            setError('Failed to clock in. Please try again.');
            console.error('Clock in error:', err);
        }
    };

    const handleClockOut = async () => {
        try {
            setLoading(true);
            await axios.post(
                `${config.API_URL}/api/timeclock/clockout/${employeeId}`
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

    return (
        <div className="time-clock-container">
            <h1>Time Clock</h1>
            <div className="button-container">
                <button 
                    className={`button clock-in ${loading ? 'loading' : ''}`}
                    onClick={handleClockIn}
                    disabled={loading}
                >
                    Clock In
                </button>
                <button 
                    className={`button clock-out ${loading ? 'loading' : ''}`}
                    onClick={handleClockOut}
                    disabled={loading}
                >
                    Clock Out
                </button>
            </div>
            {error && (
                <div className={`message ${error.includes('Failed') ? 'error' : 'success'}`}>
                    {error}
                </div>
            )}
            <table className="entries-table">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Clock In</th>
                        <th>Clock Out</th>
                        <th>Location</th>
                    </tr>
                </thead>
                <tbody>
                    {entries.map((entry) => (
                        <tr key={entry.id}>
                            <td>{new Date(entry.clockInTime).toLocaleDateString()}</td>
                            <td>{new Date(entry.clockInTime).toLocaleTimeString()}</td>
                            <td>{entry.clockOutTime ? new Date(entry.clockOutTime).toLocaleTimeString() : '-'}</td>
                            <td>{entry.location}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default TimeClock; 