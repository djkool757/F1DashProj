# Latest Race Feature - Implementation Guide

## What's Been Added

### Backend (C#)
1. **New Service Method**: `GetLatestRace()` in `F1ApiService`
   - Returns the most recent race that has occurred
   - Falls back to upcoming race if none have occurred yet
   - Automatically uses current year if no season specified

2. **New API Endpoint**: `GET /api/f1/latest-race`
   - Parameters: `season` (optional)
   - Returns: Race object with all race details

### Frontend (Vue.js)
1. **API Service**: `frontend/src/services/f1ApiService.js`
   - `getLatestRace(season)` function to call the backend endpoint
   - Handles all API communication

2. **Updated App.vue**
   - Fetches latest race on app load (onMounted hook)
   - Displays race name and date in header
   - Shows loading/error states

## Usage Examples

### From the Frontend

```javascript
import { getLatestRace } from './services/f1ApiService'

// Get latest race for current year
const race = await getLatestRace()
console.log(race.raceName, race.date)

// Get latest race for specific year
const race2025 = await getLatestRace('2025')
```

### Calling the API Directly

```bash
# Get latest race (current year)
curl http://localhost:5000/api/f1/latest-race

# Get latest race for specific year
curl http://localhost:5000/api/f1/latest-race?season=2024
```

## Response Example

```json
{
  "season": "2025",
  "round": 1,
  "url": "https://en.wikipedia.org/wiki/2025_Bahrain_Grand_Prix",
  "raceName": "Bahrain Grand Prix",
  "circuit": {
    "circuitId": "bahrain",
    "circuitName": "Bahrain International Circuit",
    "url": "https://en.wikipedia.org/wiki/Bahrain_International_Circuit",
    "location": {
      "latitude": "26.0345",
      "longitude": "50.5106",
      "locality": "Sakhir",
      "country": "Bahrain"
    }
  },
  "date": "2025-03-16T00:00:00",
  "time": "15:00:00Z",
  "results": null,
  "qualifyingResults": null,
  "pitStops": null,
  "laps": null,
  "sprintResults": null
}
```

## How It Works

1. User opens the app
2. `App.vue` `onMounted` hook fires
3. `getLatestRace()` is called
4. Frontend calls `GET /api/f1/latest-race` on the backend
5. Backend fetches all races for current year
6. Backend filters: finds most recent race that has already occurred
7. Backend returns the race data
8. Frontend displays: **"Latest Race: [Race Name] ([Date])‌"**

## Configuration

If your backend is on a different port, update the API call in `f1ApiService.js`:

```javascript
const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api/f1'
```

Or set the `VITE_API_URL` environment variable in your `.env` file.
