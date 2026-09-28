# Stock Exchange Application

A full-stack web application for fetching and analyzing intraday stock exchange data. Built with a **.NET 10 Minimal API** backend and a **React + Vite** frontend.

---

## 📋 Prerequisites

Before running the application, ensure you have the following installed:

* [.NET 10 SDK](https://dotnet.microsoft.com/download) (Required for running the backend)
* [Node.js (v18+ or LTS)](https://nodejs.org/) (Includes `npm`, required for Vite & React)
* [VS Code](https://code.visualstudio.com/) *(Recommended editor)*

Run Instructions:

Back-End:

1. Install .NET

Download and install the .NET 10 SDK if it is not already installed.

You can verify your installation by opening a terminal and running:

dotnet --version

Make sure the version matches the version required by this project.

2. Navigate to the Backend

Open a terminal and navigate to the backend folder:

cd backend
3. Run the Backend

Start the application with:

dotnet run

The terminal will display a local URL similar to:

https://localhost:7000

The exact port will be different on your computer.

4. Test the Stock API

To retrieve data for a stock, add the following endpoint to the backend URL:

/api/stocks/{symbol}

Replace {symbol} with the stock symbol you want to search for.

For example:

https://localhost:7000/api/stocks/TSLA

Open this URL in a browser to see the JSON response from the backend.

Front-End:

1. Install Node.js

The React frontend uses Vite, which runs through Node.js and npm.

Install Node.js if it is not already installed.

Verify that Node.js and npm are installed:

node --version
npm --version
2. Navigate to the Frontend

Open a second terminal and navigate to the frontend folder:

cd frontend
3. Install Dependencies

Before running the frontend for the first time, install the project's dependencies:

npm install

This installs Vite, React, and the other packages required by the project.

4. Start the Frontend

Run:

npm run dev

Vite will provide a local URL:

http://localhost:5173

Open this URL in your browser.

5. Search for a Stock

Enter a stock symbol into the search field on the frontend.

For example:

TSLA

The frontend will send a request to the backend API:

/api/stocks/TSLA

The backend retrieves the stock data and returns it to the frontend for display.

Running Both Applications:

TO run together open up two seperate terminals

have the first be your back end and paste the dotnet run from earlier

Have the second be your front end. Paste the npm run dev from the front end portion

Copy the local host link and past it in a browser tab.

Now yyou can start testing the api