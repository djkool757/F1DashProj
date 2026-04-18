import { ref } from 'vue'

const theme = ref(localStorage.getItem('pitwall-theme') || 'dark')

function apply(t) {
  document.documentElement.setAttribute('data-theme', t)
  localStorage.setItem('pitwall-theme', t)
  theme.value = t
}

apply(theme.value)

export function useTheme() {
  function toggle() { apply(theme.value === 'dark' ? 'light' : 'dark') }
  return { theme, toggle }
}
