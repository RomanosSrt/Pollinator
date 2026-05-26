import React, { useState, useEffect } from 'react';

interface PollenReading {
  timestamp: string;
  alder: number;
  birch: number;
  grass: number;
  mugwort: number;
  olive: number;
  ragweed: number;
}

interface ApiResponse {
  city: string;
  cached: boolean;
  data: PollenReading[]; // This MUST be an array
}

const PollenForecast: React.FC<{
  latitude: number;
  longitude: number;
  cityName: string;
}> = ({ latitude, longitude, cityName }) => {
  const [data, setData] = useState<PollenReading[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [cached, setCached] = useState(false);
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const [rawResponse, setRawResponse] = useState<any>(null); // For debugging

  useEffect(() => {
    const fetchPollen = async () => {
      try {
        setLoading(true);
        setError(null);

        const response = await fetch(
          `/api/pollen/forecast?latitude=${latitude}&longitude=${longitude}&cityName=${cityName}`
        );

        if (!response.ok) {
          throw new Error(`API error: ${response.status}`);
        }

        const json = (await response.json()) as ApiResponse;

        // DEBUG: Log the full response
        console.log('API Response:', json);
        setRawResponse(json);

        // VALIDATE: Check if data exists and is an array
        if (!json || !Array.isArray(json.data)) {
          console.error('Invalid response structure. Expected { data: PollenReading[] }, got:', json);
          throw new Error(
            `Invalid API response: data is ${typeof json?.data}, not an array`
          );
        }

        setData(json.data);
        setCached(json.cached);
      } catch (err) {
        const errorMsg = err instanceof Error ? err.message : 'Failed to fetch pollen data';
        setError(errorMsg);
        console.error('Fetch error:', errorMsg);
      } finally {
        setLoading(false);
      }
    };

    fetchPollen();
  }, [latitude, longitude, cityName]);

  if (loading) return <div>Loading pollen data...</div>;
  if (error) {
    return (
      <div style={{ padding: '20px', color: 'red', fontFamily: 'monospace' }}>
        <h3>❌ Error</h3>
        <p>{error}</p>
        {rawResponse && (
          <details style={{ marginTop: '10px', padding: '10px', backgroundColor: '#f5f5f5' }}>
            <summary>Raw API Response (for debugging)</summary>
            <pre style={{ whiteSpace: 'pre-wrap', wordBreak: 'break-word', fontSize: '12px' }}>
              {JSON.stringify(rawResponse, null, 2)}
            </pre>
          </details>
        )}
      </div>
    );
  }

  // Safe check: if data is empty or invalid, show a message
  if (!Array.isArray(data) || data.length === 0) {
    return (
      <div style={{ padding: '20px' }}>
        <h2>Pollen Forecast for {cityName}</h2>
        <p>No pollen data available.</p>
      </div>
    );
  }

  const today = data.slice(0, 24);

  return (
    <div style={{ padding: '20px', fontFamily: 'sans-serif' }}>
      <h2>Pollen Forecast for {cityName}</h2>
      <p style={{ fontSize: '12px', color: '#666' }}>
        {cached ? '(Cached)' : '(Fresh)'} Data from Open-Meteo (Copernicus CAMS).
      </p>

      <h3>Olive Pollen (Today)</h3>
      <table style={{ width: '100%', borderCollapse: 'collapse', marginTop: '10px' }}>
        <thead>
          <tr style={{ borderBottom: '2px solid #ccc' }}>
            <th style={{ textAlign: 'left', padding: '8px' }}>Hour</th>
            <th style={{ padding: '8px' }}>Olive</th>
            <th style={{ padding: '8px' }}>Grass</th>
            <th style={{ padding: '8px' }}>Ragweed</th>
          </tr>
        </thead>
        <tbody>
          {today.slice(0, 8).map((reading, idx) => (
            <tr key={idx} style={{ borderBottom: '1px solid #eee' }}>
              <td style={{ padding: '8px' }}>{reading.timestamp.slice(11, 16)}</td>
              <td style={{ padding: '8px', textAlign: 'center' }}>
                {reading.olive?.toFixed(0) ?? '—'}
              </td>
              <td style={{ padding: '8px', textAlign: 'center' }}>
                {reading.grass?.toFixed(0) ?? '—'}
              </td>
              <td style={{ padding: '8px', textAlign: 'center' }}>
                {reading.ragweed?.toFixed(0) ?? '—'}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default PollenForecast;