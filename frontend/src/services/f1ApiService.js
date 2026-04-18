/**
 * F1 API Service - Handles all communication with the backend F1 API
 */

const API_BASE_URL = import.meta.env.VITE_API_URL || '/api/f1';

/**
 * Fetch the latest race (most recent that has occurred, or upcoming)
 * @param {string} season - Optional season year (defaults to current year)
 * @returns {Promise<Object>} Latest race data including name, date, circuit, etc.
 */
export async function getLatestRace() {
  try {

    const response = await fetch(`${API_BASE_URL}/latest-race`);

    if (!response.ok) {
      throw new Error(`API error: ${response.status}`);
    }
    
    return await response.json();
  } catch (error) {
    console.error('Error fetching latest race:', error);
    throw error;
  }
}

export async function getResults(season, round = 1 ) {
  try {
    const response = await fetch(`${API_BASE_URL}/results/${season}/${round}`);
    if (!response.ok) {
      throw new Error(`API error: ${response.status}`);
    }
    
    return await response.json();
  } catch (error) {
    console.error('Error fetching races:', error);
    throw error;
  }
}
// working
export async function getDriverStandings(season, round) {
  try {
    const endpoint = round ? `${API_BASE_URL}/standings/drivers/${season}/${round}` : `${API_BASE_URL}/standings/drivers/${season}`;
    const response = await fetch(endpoint);
    if (!response.ok) {
      throw new Error(`API error: ${response.status}`);
    }
    // mrData.StandingsTable.StandingsLists[0].driverStandings (array of driver standings for the season/round)
    return await response.json();
  } catch (error) {
    console.error('Error fetching driver standings:', error);
    throw error;
  }
}

export async function getConstructorStandings(season, round) {
  try {
    const endpoint = round
      ? `${API_BASE_URL}/standings/constructors/${season}/${round}`
      : `${API_BASE_URL}/standings/constructors/${season}`;
    const response = await fetch(endpoint);
    if (!response.ok) {
      throw new Error(`API error: ${response.status}`);
    }

    return await response.json();
  } catch (error) {
    console.error('Error fetching constructor standings:', error);
    throw error;
  }
}

/**
 * Fetch all races for a specific season
 * @param {string} season - Season year
 * @returns {Promise<Object>} Complete race data
 */
export async function getRaces(season) {
  try {
    const response = await fetch(`${API_BASE_URL}/races/${season}`);
    
    if (!response.ok) {
      throw new Error(`API error: ${response.status}`);
    }
    
    return await response.json();
  } catch (error) {
    console.error('Error fetching races:', error);
    throw error;
  }
}

/**
 * Fetch drivers for a specific season
 * @param {string} season - Season year
 * @returns {Promise<Object>} Complete driver data
 */
export async function getDrivers(season) {
  try {
    const response = await fetch(`${API_BASE_URL}/drivers/${season}`);
    
    if (!response.ok) {
      throw new Error(`API error: ${response.status}`);
    }
    
    return await response.json();
  } catch (error) {
    console.error('Error fetching drivers:', error);
    throw error;
  }
}

export async function getLaps(season, round) {
  try {
    const response = await fetch(`${API_BASE_URL}/laps/${season}/${round}`);
    if (!response.ok) throw new Error(`API error: ${response.status}`);
    return await response.json();
  } catch (error) {
    console.error('Error fetching laps:', error);
    throw error;
  }
}

/**
 * Fetch constructors for a specific season
 * @param {string} season - Season year
 * @returns {Promise<Object>} Complete constructor data
 */
export async function getConstructors(season) {
  try {
    const response = await fetch(`${API_BASE_URL}/constructors/${season}`);
    
    if (!response.ok) {
      throw new Error(`API error: ${response.status}`);
    }
    
    return await response.json();
  } catch (error) {
    console.error('Error fetching constructors:', error);
    throw error;
  }
}
