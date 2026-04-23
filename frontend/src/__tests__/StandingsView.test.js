// @vitest-environment jsdom
import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { mount, flushPromises } from '@vue/test-utils'
import StandingsView from '@/views/StandingsView.vue'

vi.mock('@/services/f1ApiService.js', () => ({ getDriverStandings: vi.fn() }))
import { getDriverStandings } from '@/services/f1ApiService.js'

const stubs = {
  global: { stubs: { NavBar: true } },
}

const standingsStub = {
  mrData: {
    standingsTable: {
      standingsLists: [{
        season: '2026', round: '3',
        driverStandings: [
          {
            position: '1', points: '75', wins: '3',
            driver: { driverId: 'verstappen', givenName: 'Max', familyName: 'Verstappen', nationality: 'Dutch', url: '' },
            constructors: [{ constructorId: 'red_bull', name: 'Red Bull', url: '' }],
          },
          {
            position: '2', points: '50', wins: '0',
            driver: { driverId: 'hamilton', givenName: 'Lewis', familyName: 'Hamilton', nationality: 'British', url: '' },
            constructors: [{ constructorId: 'mercedes', name: 'Mercedes', url: '' }],
          },
        ],
      }],
    },
  },
}

describe('StandingsView', () => {
  beforeEach(() => {
    vi.mocked(getDriverStandings).mockResolvedValue(standingsStub)
    vi.spyOn(globalThis, 'fetch').mockRejectedValue(new Error('no network'))
  })

  afterEach(() => vi.restoreAllMocks())

  it('shows loading spinner initially', () => {
    const wrapper = mount(StandingsView, stubs)
    expect(wrapper.find('.spinner').exists()).toBe(true)
  })

  it('renders Drivers and Constructors tab buttons', () => {
    const wrapper = mount(StandingsView, stubs)
    const tabs = wrapper.findAll('.tab').map(t => t.text())
    expect(tabs).toContain('Drivers')
    expect(tabs).toContain('Constructors')
  })

  it('Drivers tab is active by default', () => {
    const wrapper = mount(StandingsView, stubs)
    expect(wrapper.find('.tab.active').text()).toBe('Drivers')
  })

  it('shows driver names after data loads', async () => {
    const wrapper = mount(StandingsView, stubs)
    await flushPromises()
    expect(wrapper.text()).toContain('VERSTAPPEN')
    expect(wrapper.text()).toContain('HAMILTON')
  })

  it('shows driver points', async () => {
    const wrapper = mount(StandingsView, stubs)
    await flushPromises()
    expect(wrapper.text()).toContain('75')
    expect(wrapper.text()).toContain('50')
  })

  it('shows position numbers', async () => {
    const wrapper = mount(StandingsView, stubs)
    await flushPromises()
    const positions = wrapper.findAll('.pos-num').map(el => el.text())
    expect(positions).toContain('1')
    expect(positions).toContain('2')
  })

  it('switching to Constructors tab marks it as active', async () => {
    const wrapper = mount(StandingsView, stubs)
    await flushPromises()
    const tabs = wrapper.findAll('.tab')
    await tabs[1].trigger('click')
    expect(tabs[1].classes()).toContain('active')
    expect(tabs[0].classes()).not.toContain('active')
  })

  it('shows constructor names after switching tab', async () => {
    const wrapper = mount(StandingsView, stubs)
    await flushPromises()
    await wrapper.findAll('.tab')[1].trigger('click')
    // template renders constructor.name.toUpperCase()
    expect(wrapper.text()).toContain('RED BULL')
    expect(wrapper.text()).toContain('MERCEDES')
  })
})
