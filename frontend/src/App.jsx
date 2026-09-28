import { useState } from 'react'
import heroImg from './assets/hero.png'
import reactLogo from './assets/react.svg'
import viteLogo from './assets/vite.svg'
import './App.css'

async function App() {
  const [symbol, setSymbol] = useState("");
  const [stockData, setStockData] = useState([]);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const searchStock = async () => {
    if(!symbol.trim()){
      setError("Please enter a correct stock symbol.");
      return;
    }
  }

  setLoading(true);
  setError("");
  setStockData([]);
  
  try {
    const response = await fetch(
      'https://localhost:7000/api/stocks/${symbol.trim().toUpperCase()}'
    );

    if(!response.ok) {
      throw new Error("Stock symbol could not be found.");
    }
    const data = await response.json();
    setStockData(data);
   } catch(error){
    setError(error.message);
   } finally {
    setLoading(false);
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
            onChange= {(event) => setSymmbol(event.target.value)}
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
