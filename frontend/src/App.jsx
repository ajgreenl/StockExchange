import { useState } from 'react'
import './App.css'

function App() {
  const [symbol, setSymbol] = useState("");
  const [stockData, setStockData] = useState([]);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const searchStock = async () => {
    const formattedSymbol = symbol.trim().toUpperCase();

  // 2. Validate it
  if (!formattedSymbol) {
    setError("Please enter a stock symbol.");
    return;
  }

  setLoading(true);
  setError("");
  setStockData([]);
  
  try {
   const response = await fetch(`http://localhost:5234/api/stocks/${formattedSymbol}`);
      

    if(!response.ok) {
      throw new Error("Error 404 stock symbol could not be found.");
    }
    const data = await response.json();
    setStockData(data);
   } catch(error){
    setError(error.message);
   } finally {
    setLoading(false);
   }
  
  }
  return (
    <>
        <div className="app">
          <h1>Stock Data</h1>

          <div className="search">
            <input 
            type="text"
            placeholder="Enter Stock symbol."
            value={symbol}
            onChange= {(event) => setSymbol(event.target.value)}
          />

          <button onClick={searchStock}>
            Search
          </button>
        </div>
        {loading && <p> Loading stock data...</p>}

        {error && <p className="error">{error}</p>}

        {stockData.length > 0 && (
          <table>

            <thead>

              <tr>
                <th>Day</th>
                <th>Low Average</th>
                <th>High Average</th>
                <th>Volume</th>
              </tr>

            </thead>

          <tbody>
            {stockData.map((stock) => (
              <tr key={stock.day}>
                <td>{stock.day}</td>
                <td>{stock.lowAverage.toFixed(4)}</td>
                <td>{stock.highAverage.toFixed(4)}</td>
                <td>{stock.volume.toLocaleString()}</td>
              </tr>
            ))}
          </tbody>
          </table>
        )}
        </div>
        
    </>
  )
}

export default App
