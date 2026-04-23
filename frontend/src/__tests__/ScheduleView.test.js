// @vitest-environment jsdom
import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { mount, flushPromises } from '@vue/test-utils'
import ScheduleView from '@/views/ScheduleView.vue'

vi.mock('@/services/f1ApiService.js', () => ({ getRaces: vi.fn() }))
import { getRaces } from '@/services/f1ApiService.js'

const stubs = {
  global: { stubs: { NavBar: true } },
}

const past   = new Date(Date.now() - 30 * 86400000).toISOString().slice(0, 10)
const future = new Date(Date.now() + 30 * 86400000).toISOString().slice(0, 10)

const raceStub = {
  mrData: {
    raceTable: {
      season: '2026',
      races: [
        {
          round: '1', season: '2026', raceName: 'Bahrain Grand Prix', date: past,
          circuit: { circuitName: 'BIC', location: { locality: 'Sakhir', country: 'Bahrain' } },
        },
        {
          round: '2', season: '2026', raceName: 'Australian Grand Prix', date: future,
          circuit: { circuitName: 'Albert Park', location: { locality: 'Melbourne', country: 'Australia' } },
        },
      ],
    },
  },
}

describe('ScheduleView', () => {
  beforeEach(() => {
    vi.mocked(getRaces).mockResolvedValue(raceStub)
  })

  afterEach(() => vi.restoreAllMocks())

  it('shows loading spinner initially', () => {
    const wrapper = mount(ScheduleView, stubs)
    expect(wrapper.find('.spinner').exists()).toBe(true)
  })

  it('hides spinner and shows race rows after data loads', async () => {
    const wrapper = mount(ScheduleView, stubs)
    await flushPromises()
    expect(wrapper.find('.spinner').exists()).toBe(false)
    expect(wrapper.findAll('.race-row')).toHaveLength(2)
  })

  it('displays race names', async () => {
    const wrapper = mount(ScheduleView, stubs)
    await flushPromises()
    expect(wrapper.text()).toContain('Bahrain Grand Prix')
    expect(wrapper.text()).toContain('Australian Grand Prix')
  })

  it('shows Done badge for past races', async () => {
    const wrapper = mount(ScheduleView, stubs)
    await flushPromises()
    expect(wrapper.findAll('.badge.done').length).toBeGreaterThan(0)
    expect(wrapper.find('.badge.done').text()).toBe('Done')
  })

  it('shows Next badge for the upcoming race', async () => {
    const wrapper = mount(ScheduleView, stubs)
    await flushPromises()
    expect(wrapper.find('.badge.next').exists()).toBe(true)
    expect(wrapper.find('.badge.next').text()).toBe('Next')
  })

  it('shows "next race" countdown in the page header', async () => {
    const wrapper = mount(ScheduleView, stubs)
    await flushPromises()
    expect(wrapper.text()).toContain('Australian Grand Prix')
    expect(wrapper.find('.page-sub').exists()).toBe(true)
  })

  it('renders no rows when the API returns an empty race list', async () => {
    vi.mocked(getRaces).mockResolvedValue({ mrData: { raceTable: { season: '2026', races: [] } } })
    const wrapper = mount(ScheduleView, stubs)
    await flushPromises()
    expect(wrapper.findAll('.race-row')).toHaveLength(0)
  })

  it('groups races by month header', async () => {
    const wrapper = mount(ScheduleView, stubs)
    await flushPromises()
    expect(wrapper.findAll('.month-header').length).toBeGreaterThan(0)
  })
})
