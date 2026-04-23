// @vitest-environment jsdom
import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { mount, flushPromises } from '@vue/test-utils'
import LatestRaceView from '@/views/LatestRaceView.vue'

vi.mock('@/services/f1ApiService.js', () => ({
  getLatestRace:      vi.fn(),
  getDriverStandings: vi.fn(),
  getResults:         vi.fn(),
  getRaces:           vi.fn(),
}))

import { getLatestRace, getDriverStandings, getResults, getRaces } from '@/services/f1ApiService.js'

const stubs = {
  global: { stubs: { NavBar: true } },
}

const latestRaceStub = {
  season: '2026', round: '3',
  raceName: 'Bahrain Grand Prix',
  date: '2026-03-01T15:00:00Z',
  url: 'https://en.wikipedia.org/wiki/2026_Bahrain_Grand_Prix',
  circuit: {
    circuitName: 'Bahrain International Circuit',
    location: { locality: 'Sakhir', country: 'Bahrain' },
  },
}

const standingsStub = {
  mrData: {
    standingsTable: {
      standingsLists: [{
        season: '2026', round: '3',
        driverStandings: [
          {
            position: '1', points: '75', wins: '3',
            driver:       { driverId: 'verstappen', givenName: 'Max', familyName: 'Verstappen', url: '' },
            constructors: [{ constructorId: 'red_bull', name: 'Red Bull', url: '' }],
          },
        ],
      }],
    },
  },
}

describe('LatestRaceView', () => {
  beforeEach(() => {
    vi.mocked(getLatestRace).mockResolvedValue(latestRaceStub)
    vi.mocked(getDriverStandings).mockResolvedValue(standingsStub)
    vi.mocked(getResults).mockResolvedValue({ mrData: { raceTable: { races: [{ results: [] }] } } })
    vi.mocked(getRaces).mockResolvedValue({ mrData: { raceTable: { races: [] } } })
    // Silence Wikipedia / track layout fetches
    vi.spyOn(globalThis, 'fetch').mockRejectedValue(new Error('no network'))
  })

  afterEach(() => vi.restoreAllMocks())

  it('shows loading text initially', () => {
    const wrapper = mount(LatestRaceView, stubs)
    expect(wrapper.find('.page-loading').exists()).toBe(true)
    expect(wrapper.text()).toContain('Loading')
  })

  it('shows error message when getLatestRace fails', async () => {
    vi.mocked(getLatestRace).mockRejectedValue(new Error('API error'))
    const wrapper = mount(LatestRaceView, stubs)
    await flushPromises()
    expect(wrapper.find('.error').exists()).toBe(true)
    expect(wrapper.text()).toContain('API error')
  })

  it('shows the race name after data loads', async () => {
    const wrapper = mount(LatestRaceView, stubs)
    await flushPromises()
    expect(wrapper.text()).toContain('Bahrain Grand Prix')
  })

  it('shows the race badge with round and season', async () => {
    const wrapper = mount(LatestRaceView, stubs)
    await flushPromises()
    expect(wrapper.find('.race-badge').text()).toContain('Round 3')
    expect(wrapper.find('.race-badge').text()).toContain('2026')
  })

  it('renders the standings widget', async () => {
    const wrapper = mount(LatestRaceView, stubs)
    await flushPromises()
    expect(wrapper.find('.standings-widget').exists()).toBe(true)
  })

  it('Drivers tab is active by default in the standings widget', async () => {
    const wrapper = mount(LatestRaceView, stubs)
    await flushPromises()
    const activeTabs = wrapper.findAll('.standings-tab.active')
    expect(activeTabs.length).toBeGreaterThan(0)
    expect(activeTabs[0].text()).toBe('Drivers')
  })

  it('switching to Constructors tab in the standings widget makes it active', async () => {
    const wrapper = mount(LatestRaceView, stubs)
    await flushPromises()
    const tabs = wrapper.findAll('.standings-tab')
    await tabs[1].trigger('click')
    expect(tabs[1].classes()).toContain('active')
    expect(tabs[0].classes()).not.toContain('active')
  })

  it('shows driver standings after data loads', async () => {
    const wrapper = mount(LatestRaceView, stubs)
    await flushPromises()
    // template renders familyName.toUpperCase()
    expect(wrapper.text()).toContain('VERSTAPPEN')
  })
})
