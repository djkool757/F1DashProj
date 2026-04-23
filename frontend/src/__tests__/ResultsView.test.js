// @vitest-environment jsdom
import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { mount, flushPromises } from '@vue/test-utils'
import { nextTick } from 'vue'
import ResultsView from '@/views/ResultsView.vue'

vi.mock('@/services/f1ApiService.js', () => ({
  getRaces:   vi.fn(),
  getResults: vi.fn(),
  getLaps:    vi.fn(),
}))

vi.mock('vue-chartjs', () => ({
  Line: { template: '<div class="chart-stub" />' },
}))

vi.mock('chart.js', () => ({
  Chart:         { register: vi.fn() },
  CategoryScale: class {},
  LinearScale:   class {},
  PointElement:  class {},
  LineElement:   class {},
  Tooltip:       class {},
  Legend:        class {},
}))

import { getRaces, getResults, getLaps } from '@/services/f1ApiService.js'

const stubs = {
  global: { stubs: { NavBar: true } },
}

const pastDate = new Date(Date.now() - 7 * 86400000).toISOString().slice(0, 10)

const raceListStub = {
  mrData: {
    raceTable: {
      races: [{
        round: '1', raceName: 'Bahrain Grand Prix', date: pastDate,
        circuit: { circuitName: 'BIC', location: { locality: 'Sakhir', country: 'Bahrain' } },
      }],
    },
  },
}

const resultsStub = {
  mrData: {
    raceTable: {
      races: [{
        results: [{
          positionText: '1', points: '25', grid: '1',
          driver:      { driverId: 'verstappen', givenName: 'Max', familyName: 'Verstappen', code: 'VER' },
          constructor: { constructorId: 'red_bull', name: 'Red Bull' },
          status:      'Finished',
          time:        { time: '1:30:00.000' },
        }],
      }],
    },
  },
}

describe('ResultsView', () => {
  beforeEach(() => {
    vi.mocked(getRaces).mockResolvedValue(raceListStub)
    vi.mocked(getResults).mockResolvedValue(resultsStub)
    vi.mocked(getLaps).mockResolvedValue({ mrData: { raceTable: { races: [] } } })
  })

  afterEach(() => vi.restoreAllMocks())

  it('renders the Race Results heading', () => {
    const wrapper = mount(ResultsView, stubs)
    expect(wrapper.text()).toContain('Race Results')
  })

  it('season select contains the current year as first option', () => {
    const wrapper = mount(ResultsView, stubs)
    const selects = wrapper.findAll('.filter-select')
    expect(selects[0].find('option').text()).toBe(String(new Date().getFullYear()))
  })

  it('race select is disabled before races are loaded', () => {
    // getRaces never resolves so raceList stays empty
    vi.mocked(getRaces).mockReturnValue(new Promise(() => {}))
    const wrapper = mount(ResultsView, stubs)
    const raceSelect = wrapper.findAll('.filter-select')[1]
    expect(raceSelect.attributes('disabled')).toBeDefined()
  })

  it('shows loading spinner while data is fetching', async () => {
    vi.mocked(getRaces).mockReturnValue(new Promise(() => {}))
    const wrapper = mount(ResultsView, stubs)
    await nextTick()
    expect(wrapper.find('.spinner').exists()).toBe(true)
  })

  it('shows the results table after data loads', async () => {
    const wrapper = mount(ResultsView, stubs)
    await flushPromises()
    expect(wrapper.find('.results-table').exists()).toBe(true)
    // template renders familyName.toUpperCase()
    expect(wrapper.text()).toContain('VERSTAPPEN')
  })

  it('shows the winner in the race banner', async () => {
    const wrapper = mount(ResultsView, stubs)
    await flushPromises()
    expect(wrapper.find('.banner-winner').exists()).toBe(true)
    expect(wrapper.text()).toContain('VERSTAPPEN')
  })

  it('shows empty state when the race has no results', async () => {
    vi.mocked(getResults).mockResolvedValue({ mrData: { raceTable: { races: [] } } })
    const wrapper = mount(ResultsView, stubs)
    await flushPromises()
    expect(wrapper.find('.empty-state').exists()).toBe(true)
    expect(wrapper.text()).toContain('No results available')
  })

  it('race select is enabled after races load', async () => {
    const wrapper = mount(ResultsView, stubs)
    await flushPromises()
    const raceSelect = wrapper.findAll('.filter-select')[1]
    expect(raceSelect.attributes('disabled')).toBeUndefined()
  })
})
