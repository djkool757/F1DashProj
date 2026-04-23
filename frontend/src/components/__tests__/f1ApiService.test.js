import { describe, it, expect, vi, beforeEach } from 'vitest'
import {
  getLatestRace,
  getResults,
  getDriverStandings,
  getConstructorStandings,
  getRaces,
  getDrivers,
  getLaps,
  getConstructors,
} from '../../services/f1ApiService.js'

// ---------------------------------------------------------------------------
// Helpers
// ---------------------------------------------------------------------------

function mockFetch(data, ok = true, status = 200) {
  return vi.spyOn(globalThis, 'fetch').mockResolvedValue({
    ok,
    status,
    json: () => Promise.resolve(data),
  })
}

function mockFetchFailure(message = 'Network error') {
  return vi.spyOn(globalThis, 'fetch').mockRejectedValue(new Error(message))
}

// ---------------------------------------------------------------------------
// Setup / teardown
// ---------------------------------------------------------------------------

beforeEach(() => {
  vi.restoreAllMocks()
})

// ---------------------------------------------------------------------------
// getLatestRace
// ---------------------------------------------------------------------------

describe('getLatestRace', () => {
  it('returns parsed JSON on success', async () => {
    const stub = { RaceName: 'Bahrain GP', Season: '2026', Round: '1' }
    mockFetch(stub)
    const result = await getLatestRace()
    expect(result).toEqual(stub)
  })

  it('throws when the response is not ok', async () => {
    mockFetch({}, false, 500)
    await expect(getLatestRace()).rejects.toThrow('API error: 500')
  })

  it('throws when fetch rejects (network failure)', async () => {
    mockFetchFailure()
    await expect(getLatestRace()).rejects.toThrow('Network error')
  })
})

// ---------------------------------------------------------------------------
// getResults
// ---------------------------------------------------------------------------

describe('getResults', () => {
  it('calls the correct URL for a given season and round', async () => {
    const spy = mockFetch({})
    await getResults('2026', '3')
    expect(spy).toHaveBeenCalledWith('/api/f1/results/2026/3')
  })

  it('defaults round to 1 when not provided', async () => {
    const spy = mockFetch({})
    await getResults('2026')
    expect(spy).toHaveBeenCalledWith('/api/f1/results/2026/1')
  })

  it('returns parsed JSON on success', async () => {
    const stub = { MRData: { RaceTable: { Races: [] } } }
    mockFetch(stub)
    const result = await getResults('2026', '1')
    expect(result).toEqual(stub)
  })

  it('throws when the response is not ok', async () => {
    mockFetch({}, false, 400)
    await expect(getResults('2026', '1')).rejects.toThrow('API error: 400')
  })
})

// ---------------------------------------------------------------------------
// getDriverStandings
// ---------------------------------------------------------------------------

describe('getDriverStandings', () => {
  it('includes round in URL when round is provided', async () => {
    const spy = mockFetch({})
    await getDriverStandings('2026', '5')
    expect(spy).toHaveBeenCalledWith('/api/f1/standings/drivers/2026/5')
  })

  it('omits round from URL when round is not provided', async () => {
    const spy = mockFetch({})
    await getDriverStandings('2026')
    expect(spy).toHaveBeenCalledWith('/api/f1/standings/drivers/2026')
  })

  it('returns parsed JSON on success', async () => {
    const stub = { MRData: { StandingsTable: { StandingsLists: [] } } }
    mockFetch(stub)
    const result = await getDriverStandings('2026', '1')
    expect(result).toEqual(stub)
  })

  it('throws when the response is not ok', async () => {
    mockFetch({}, false, 404)
    await expect(getDriverStandings('2026', '1')).rejects.toThrow('API error: 404')
  })
})

// ---------------------------------------------------------------------------
// getConstructorStandings
// ---------------------------------------------------------------------------

describe('getConstructorStandings', () => {
  it('includes round in URL when round is provided', async () => {
    const spy = mockFetch({})
    await getConstructorStandings('2026', '5')
    expect(spy).toHaveBeenCalledWith('/api/f1/standings/constructors/2026/5')
  })

  it('omits round from URL when round is not provided', async () => {
    const spy = mockFetch({})
    await getConstructorStandings('2026')
    expect(spy).toHaveBeenCalledWith('/api/f1/standings/constructors/2026')
  })

  it('returns parsed JSON on success', async () => {
    const stub = { MRData: { StandingsTable: { ConstructorStandingsLists: [] } } }
    mockFetch(stub)
    const result = await getConstructorStandings('2026', '1')
    expect(result).toEqual(stub)
  })

  it('throws when the response is not ok', async () => {
    mockFetch({}, false, 404)
    await expect(getConstructorStandings('2026', '1')).rejects.toThrow('API error: 404')
  })
})

// ---------------------------------------------------------------------------
// getRaces
// ---------------------------------------------------------------------------

describe('getRaces', () => {
  it('calls the correct URL for a given season', async () => {
    const spy = mockFetch({})
    await getRaces('2026')
    expect(spy).toHaveBeenCalledWith('/api/f1/races/2026')
  })

  it('returns parsed JSON on success', async () => {
    const stub = { MRData: { RaceTable: { Season: '2026', Races: [] } } }
    mockFetch(stub)
    const result = await getRaces('2026')
    expect(result).toEqual(stub)
  })

  it('throws when the response is not ok', async () => {
    mockFetch({}, false, 500)
    await expect(getRaces('2026')).rejects.toThrow('API error: 500')
  })
})

// ---------------------------------------------------------------------------
// getDrivers
// ---------------------------------------------------------------------------

describe('getDrivers', () => {
  it('calls the correct URL for a given season', async () => {
    const spy = mockFetch({})
    await getDrivers('2026')
    expect(spy).toHaveBeenCalledWith('/api/f1/drivers/2026')
  })

  it('returns parsed JSON on success', async () => {
    const stub = { MRData: { DriverTable: { Season: '2026', Drivers: [] } } }
    mockFetch(stub)
    const result = await getDrivers('2026')
    expect(result).toEqual(stub)
  })

  it('throws when the response is not ok', async () => {
    mockFetch({}, false, 500)
    await expect(getDrivers('2026')).rejects.toThrow('API error: 500')
  })
})

// ---------------------------------------------------------------------------
// getLaps
// ---------------------------------------------------------------------------

describe('getLaps', () => {
  it('calls the correct URL for a given season and round', async () => {
    const spy = mockFetch({})
    await getLaps('2026', '3')
    expect(spy).toHaveBeenCalledWith('/api/f1/laps/2026/3')
  })

  it('returns parsed JSON on success', async () => {
    const stub = { MRData: { RaceTable: { Races: [] } } }
    mockFetch(stub)
    const result = await getLaps('2026', '3')
    expect(result).toEqual(stub)
  })

  it('throws when the response is not ok', async () => {
    mockFetch({}, false, 500)
    await expect(getLaps('2026', '3')).rejects.toThrow('API error: 500')
  })
})

// ---------------------------------------------------------------------------
// getConstructors
// ---------------------------------------------------------------------------

describe('getConstructors', () => {
  it('calls the correct URL for a given season', async () => {
    const spy = mockFetch({})
    await getConstructors('2026')
    expect(spy).toHaveBeenCalledWith('/api/f1/constructors/2026')
  })

  it('returns parsed JSON on success', async () => {
    const stub = { MRData: { ConstructorTable: { Season: '2026', Constructors: [] } } }
    mockFetch(stub)
    const result = await getConstructors('2026')
    expect(result).toEqual(stub)
  })

  it('throws when the response is not ok', async () => {
    mockFetch({}, false, 500)
    await expect(getConstructors('2026')).rejects.toThrow('API error: 500')
  })
})
