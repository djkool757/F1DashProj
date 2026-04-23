// @vitest-environment jsdom
import { describe, it, expect, vi, beforeEach } from 'vitest'
import { mount } from '@vue/test-utils'
import { ref } from 'vue'
import NavBar from '@/components/NavBar.vue'

vi.mock('@/composables/useTheme.js', () => ({ useTheme: vi.fn() }))
import { useTheme } from '@/composables/useTheme.js'

const stubs = {
  global: { stubs: { RouterLink: { template: '<a><slot /></a>' } } },
}

function mountNavBar(theme = 'dark') {
  const toggle = vi.fn()
  vi.mocked(useTheme).mockReturnValue({ theme: ref(theme), toggle })
  return { wrapper: mount(NavBar, stubs), toggle }
}

describe('NavBar', () => {
  beforeEach(() => vi.clearAllMocks())

  it('renders all navigation links', () => {
    const { wrapper } = mountNavBar()
    const texts = wrapper.findAll('a').map(l => l.text())
    expect(texts).toContain('Home')
    expect(texts).toContain('Latest Race')
    expect(texts).toContain('Standings')
    expect(texts).toContain('Schedule')
    expect(texts).toContain('Teams')
    expect(texts).toContain('Results')
  })

  it('shows sun icon when theme is dark', () => {
    const { wrapper } = mountNavBar('dark')
    // Desktop theme-toggle (first match) has a <span> with the icon
    expect(wrapper.find('.theme-toggle').find('span').text()).toBe('☀')
  })

  it('shows moon icon when theme is light', () => {
    const { wrapper } = mountNavBar('light')
    expect(wrapper.find('.theme-toggle').find('span').text()).toBe('☾')
  })

  it('calls toggle when the theme button is clicked', async () => {
    const { wrapper, toggle } = mountNavBar()
    await wrapper.find('.theme-toggle').trigger('click')
    expect(toggle).toHaveBeenCalledOnce()
  })

  it('mobile menu is closed initially', () => {
    const { wrapper } = mountNavBar()
    expect(wrapper.find('.mobile-menu').classes()).not.toContain('open')
  })

  it('opens mobile menu when hamburger is clicked', async () => {
    const { wrapper } = mountNavBar()
    await wrapper.find('.hamburger').trigger('click')
    expect(wrapper.find('.mobile-menu').classes()).toContain('open')
  })

  it('closes mobile menu when hamburger is clicked again', async () => {
    const { wrapper } = mountNavBar()
    await wrapper.find('.hamburger').trigger('click')
    await wrapper.find('.hamburger').trigger('click')
    expect(wrapper.find('.mobile-menu').classes()).not.toContain('open')
  })

  it('hamburger has open class while menu is open', async () => {
    const { wrapper } = mountNavBar()
    await wrapper.find('.hamburger').trigger('click')
    expect(wrapper.find('.hamburger').classes()).toContain('open')
  })
})
