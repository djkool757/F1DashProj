// @vitest-environment jsdom
import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import HomeView from '@/views/HomeView.vue'

const stubs = {
  global: {
    stubs: {
      NavBar: true,
      RouterLink: { template: '<a><slot /></a>' },
    },
  },
}

describe('HomeView', () => {
  it('renders the hero title', () => {
    const wrapper = mount(HomeView, stubs)
    expect(wrapper.text()).toContain('Every Race.')
    expect(wrapper.text()).toContain('Every Stat.')
    expect(wrapper.text()).toContain('One Dashboard.')
  })

  it('displays the current year in the eyebrow', () => {
    const wrapper = mount(HomeView, stubs)
    expect(wrapper.text()).toContain(String(new Date().getFullYear()))
  })

  it('renders 5 feature cards', () => {
    const wrapper = mount(HomeView, stubs)
    expect(wrapper.findAll('.feature-card')).toHaveLength(5)
  })

  it('feature cards have the correct titles', () => {
    const wrapper = mount(HomeView, stubs)
    const titles = wrapper.findAll('.feature-title').map(el => el.text())
    expect(titles).toContain('Latest Race')
    expect(titles).toContain('Standings')
    expect(titles).toContain('Schedule')
    expect(titles).toContain('Teams')
    expect(titles).toContain('Results')
  })

  it('hero CTA links to /latest-race', () => {
    const wrapper = mount(HomeView, stubs)
    const cta = wrapper.find('.hero-cta')
    expect(cta.exists()).toBe(true)
    expect(cta.text()).toContain('Latest Race')
  })
})
