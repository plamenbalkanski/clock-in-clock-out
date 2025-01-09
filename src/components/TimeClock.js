import React, { useState, useEffect } from 'react';
import axios from 'axios';

const TimeClock = () => {
    const [employeeId, setEmployeeId] = useState('');
    const [location, setLocation] = useState('');
    const [entries, setEntries] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const API_URL = 'https://your-api-url/api/timeclock';

    const handleClockIn = async () => {
        try {
            setLoading(true);
            await axios.post(`${API_URL}/clockin`, { employeeId, location });
            fetchEntries();
        } catch (err) {
            setError('Failed to clock in');
        } finally {
            setLoading(false);
        }
    };

    const handleClockOut = async () => {
        try {
            setLoading(true);
            await axios.post(`${API_URL}/clockout`, { employeeId });
            fetchEntries();
        } catch (err) {
            setError('Failed to clock out');
        } finally {
            setLoading(false);
        }
    };

    const fetchEntries = async () => {
        try {
            const response = await axios.get(`${API_URL}/employee/${employeeId}`);
            setEntries(response.data);
        } catch (err) {
            setError('Failed to fetch entries');
        }
    };

    return (
        <div className="time-clock-container">
            <h1>Employee Time Clock</h1>
            
            <div className="input-group">
                <input
                    type="number"
                    value={employeeId}
                    onChange={(e) => setEmployeeId(e.target.value)}
                    placeholder="Employee ID"
                />
                <input
                    type="text"
                    value={location}
                    onChange={(e) => setLocation(e.target.value)}
                    placeholder="Location"
                />
            </div>

            <div className="button-group">
                <button onClick={handleClockIn} disabled={loading}>
                    Clock In
                </button>
                <button onClick={handleClockOut} disabled={loading}>
                    Clock Out
                </button>
            </div>

            {error && <div className="error">{error}</div>}

            <div className="entries-list">
                <h2>Recent Entries</h2>
                {entries.map(entry => (
                    <div key={entry.id} className="entry">
                        <p>Clock In: {new Date(entry.clockInTime).toLocaleString()}</p>
                        {entry.clockOutTime && (
                            <p>Clock Out: {new Date(entry.clockOutTime).toLocaleString()}</p>
                        )}
                        <p>Location: {entry.location}</p>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default TimeClock; 