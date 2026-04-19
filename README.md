# Pitwall

A Formula 1 data visualization app built with Vue 3 and ASP.NET Core. Pitwall aggregates live race data — standings, results, schedules, and team/driver profiles — into a clean dashboard.

## Features

- **Latest Race** — live status, winner, podium, and lap data for the most recent race
- **Race Calendar** — full season schedule with circuit and round details
- **Results** — race-by-race results for any season and round
- **Standings** — driver and constructor championship standings with charts
- **Teams** — constructor profiles and season stats

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | Vue 3, Vite, Vue Router, Chart.js |
| Backend | ASP.NET Core (.NET 9), C# |
| Data | Ergast F1 API (via `api.jolpi.ca`) |

## Project Structure

```
Pitwall/
├── backend/          # ASP.NET Core API
│   ├── Controllers/  # F1DataController, HomeController
│   ├── Services/     # F1ApiService (Ergast proxy + caching)
│   └── Models/       # Race, Driver, Constructor, etc.
└── frontend/
    └── src/
        ├── views/    # HomeView, LatestRaceView, ResultsView, ScheduleView, StandingsView, TeamsView
        ├── components/
        ├── router/
        └── services/ # API client
```

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v18+)

### Backend

```bash
cd backend
dotnet run
```

The API runs on `http://localhost:5000`. Swagger UI is available at `/swagger`.

### Frontend

```bash
cd frontend
npm install
npm run dev
```

The dev server runs on `http://localhost:5173`.

## API Endpoints

| Endpoint | Description |
|---|---|
| `GET /api/f1/latest-race` | Latest race info and status |
| `GET /api/f1/races/{season}` | Race schedule for a season |
| `GET /api/f1/results/{season}/{round}` | Race results |
| `GET /api/f1/standings/drivers/{season}` | Driver championship standings |
| `GET /api/f1/standings/constructors/{season}` | Constructor standings |
| `GET /api/f1/drivers/{season}` | Driver list |
| `GET /api/f1/constructors/{season}` | Constructor list |
| `GET /api/f1/laps/{season}/{round}` | Lap data |

## Scripts

```bash
npm run dev        # Start Vite dev server
npm run build      # Production build
npm run lint       # Run ESLint + Oxlint
npm run test:unit  # Run unit tests
```

## Data Source

Race data is sourced from the [Ergast Motor Racing API](https://api.jolpi.ca/ergast/f1). The backend caches responses in-memory to reduce external API calls.
