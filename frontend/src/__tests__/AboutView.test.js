// @vitest-environment jsdom
import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import AboutView from '@/views/AboutView.vue'

describe('AboutView', () => {
  it('renders the about heading', () => {
    const wrapper = mount(AboutView)
    expect(wrapper.text()).toContain('This is an about page')
  })

  it('renders an h1 element', () => {
    const wrapper = mount(AboutView)
    expect(wrapper.find('h1').exists()).toBe(true)
  })
})
