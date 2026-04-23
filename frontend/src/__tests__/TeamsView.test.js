// @vitest-environment jsdom
import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { mount, flushPromises } from '@vue/test-utils'
import TeamsView from '@/views/TeamsView.vue'

vi.mock('@/services/f1ApiService.js', () => ({
  getConstructors:     vi.fn(),
  getDriverStandings:  vi.fn(),
}))

import { getConstructors, getDriverStandings } from '@/services/f1ApiService.js'

const stubs = {
  global: { stubs: { NavBar: true } },
}

const constructorsStub = {
  mrData: {
    constructorTable: {
      constructors: [
        { constructorId: 'red_bull',  name: 'Red Bull',  nationality: 'Austrian',  url: '' },
        { constructorId: 'mclaren',   name: 'McLaren',   nationality: 'British',   url: '' },
      ],
    },
  },
}

// round: '0' keeps currentRound = 0, avoiding per-round chart API calls
const standingsStub = {
  mrData: {
    standingsTable: {
      standingsLists: [{
        season: '2026', round: '0',
        driverStandings: [
          {
            position: '1', points: '75', wins: '3',
            driver:      { driverId: 'verstappen', givenName: 'Max', familyName: 'Verstappen', url: '' },
            constructors: [{ constructorId: 'red_bull', name: 'Red Bull', url: '' }],
          },
          {
            position: '2', points: '50', wins: '1',
            driver:      { driverId: 'norris', givenName: 'Lando', familyName: 'Norris', url: '' },
            constructors: [{ constructorId: 'mclaren', name: 'McLaren', url: '' }],
          },
        ],
      }],
    },
  },
}

describe('TeamsView', () => {
  beforeEach(() => {
    vi.mocked(getConstructors).mockResolvedValue(constructorsStub)
    vi.mocked(getDriverStandings).mockResolvedValue(standingsStub)
    vi.spyOn(globalThis, 'fetch').mockRejectedValue(new Error('no network'))
  })

  afterEach(() => vi.restoreAllMocks())

  it('shows loading spinner initially', () => {
    const wrapper = mount(TeamsView, stubs)
    expect(wrapper.find('.spinner').exists()).toBe(true)
  })

  it('shows error state when the API fails', async () => {
    vi.mocked(getConstructors).mockRejectedValue(new Error('API down'))
    const wrapper = mount(TeamsView, stubs)
    await flushPromises()
    expect(wrapper.find('.error-state').exists()).toBe(true)
    expect(wrapper.text()).toContain('Failed to load team data')
  })

  it('renders a card for each constructor after data loads', async () => {
    const wrapper = mount(TeamsView, stubs)
    await flushPromises()
    expect(wrapper.findAll('.team-card')).toHaveLength(2)
  })

  it('displays team names in the cards', async () => {
    const wrapper = mount(TeamsView, stubs)
    await flushPromises()
    expect(wrapper.text()).toContain('Red Bull')
    expect(wrapper.text()).toContain('McLaren')
  })

  it('displays driver names inside their team cards', async () => {
    const wrapper = mount(TeamsView, stubs)
    await flushPromises()
    // template renders familyName.toUpperCase()
    expect(wrapper.text()).toContain('VERSTAPPEN')
    expect(wrapper.text()).toContain('NORRIS')
  })

  it('displays constructor points', async () => {
    const wrapper = mount(TeamsView, stubs)
    await flushPromises()
    // Red Bull: 75 pts, McLaren: 50 pts
    expect(wrapper.text()).toContain('75')
    expect(wrapper.text()).toContain('50')
  })
})
