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
    const url = `${API_BASE_URL}/latest-race`;
    
    const response = await fetch(url);
    
    if (!response.ok) {
      throw new Error(`API error: ${response.status}`);
    }
    
    return await response.json();
  } catch (error) {
    console.error('Error fetching latest race:', error);
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
