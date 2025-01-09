import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './TimeClock.css';
import { config } from '../config';

interface TimeClockProps {
    employeeId: number;
}

interface TimeClockEntry {
    id: number;
    employeeId: number;
    clockInTime: string;
    clockOutTime: string | null;
    location: string;
}

const TimeClock: React.FC<TimeClockProps> = ({ employeeId }) => {
    const [location, setLocation] = useState<string>('');
    const [entries, setEntries] = useState<TimeClockEntry[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>('');

    console.log('Using API URL:', config.API_URL);  // Debug log
    
    // Replace process.env.REACT_APP_API_URL with config.API_URL
    const API_URL = config.API_URL;

    // Add more detailed logging
    console.log('Environment variables:', {
        REACT_APP_API_URL: process.env.REACT_APP_API_URL,
        NODE_ENV: process.env.NODE_ENV,
        all: process.env
    });
    
    console.log('Resolved API_URL:', API_URL);

    useEffect(() => {
        if (employeeId) {
            fetchEntries();
        }
    }, [employeeId, fetchEntries]);

    useEffect(() => {
        // Get user's location when component mounts
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                async (position) => {
                    try {
                        const response = await axios.get(
                            `https://api.opencagedata.com/geocode/v1/json?q=${position.coords.latitude}+${position.coords.longitude}&key=YOUR_OPENCAGE_API_KEY`
                        );
                        const city = response.data.results[0]?.components.city || 'Unknown';
                        setLocation(city);
                    } catch (error) {
                        setLocation('Location unavailable');
                    }
                },
                () => {
                    setLocation('Location access denied');
                }
            );
        }
        fetchEntries();
    }, [employeeId]);

    const handleClockIn = async () => {
        try {
            setLoading(true);
            const url = `${API_URL}/api/timeclock/clockin`;
            
            // Add these debug logs
            console.log('Debug - API_URL at clock in:', API_URL);
            console.log('Debug - Constructed URL:', url);
            console.log('Debug - Environment:', {
                REACT_APP_API_URL: process.env.REACT_APP_API_URL,
                resolved: API_URL
            });

            const response = await axios.post(url, { 
                employeeId: employeeId,
                location: location || 'Unknown'
            }, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            console.log('Response:', response);
            setLoading(false);
            setError('Successfully clocked in!');
            fetchEntries();
        } catch (err) {
            setLoading(false);
            if (axios.isAxiosError(err)) {
                const errorMessage = err.response?.data || err.message;
                console.error('Clock in error:', {
                    status: err.response?.status,
                    statusText: err.response?.statusText,
                    data: err.response?.data,
                    config: {
                        url: err.config?.url,
                        method: err.config?.method,
                        headers: err.config?.headers,
                        data: err.config?.data
                    }
                });
                setError(`Failed to clock in: ${errorMessage}`);
            } else {
                console.error('Unknown error:', err);
                setError('Failed to clock in. Please try again.');
            }
        }
    };

    const handleClockOut = async () => {
        try {
            setLoading(true);
            await axios.post(`${API_URL}/api/timeclock/clockout/${employeeId}`);
            setLoading(false);
            setError('Successfully clocked out!');
            fetchEntries();
        } catch (error) {
            setLoading(false);
            setError('Failed to clock out. Please try again.');
        }
    };

    const fetchEntries = async () => {
        try {
            const response = await axios.get<TimeClockEntry[]>(`${API_URL}/api/timeclock/employee/${employeeId}`);
            setEntries(response.data);
        } catch (err) {
            setError('Failed to fetch entries');
            console.error('Fetch entries error:', err);
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
                <div className="error">
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