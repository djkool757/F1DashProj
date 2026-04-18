<template>
  <nav class="navbar">
    <div class="navbar-inner">
      <!-- Left: Logo -->
      <div class="navbar-brand">
        <img alt="f1 logo" class="logo" src="../assets/F1_logo.png" width="100" height="1000" />
      </div>

      <!-- Center: Title -->
      <div class="navbar-title">
        <span class="title-line" aria-hidden="true"></span>
        <h2>F1 Dashboard</h2>
        <span class="title-line" aria-hidden="true"></span>
      </div>

      <!-- Right: Nav Links + Theme Toggle -->
      <div class="navbar-right">
        <ul class="navbar-links">
          <li><RouterLink to="/" class="nav-link" exact-active-class="active">Home</RouterLink></li>
          <li><RouterLink to="/latest-race" class="nav-link" exact-active-class="active">Latest Race</RouterLink></li>
          <li><RouterLink to="/standings" class="nav-link" exact-active-class="active">Standings</RouterLink></li>
          <li><RouterLink to="/schedule" class="nav-link" exact-active-class="active">Schedule</RouterLink></li>
          <li><RouterLink to="/teams" class="nav-link" exact-active-class="active">Teams</RouterLink></li>
          <li><RouterLink to="/results" class="nav-link" exact-active-class="active">Results</RouterLink></li>
        </ul>

        <button class="theme-toggle" @click="toggle" :aria-label="theme === 'dark' ? 'Switch to light mode' : 'Switch to dark mode'">
          <span v-if="theme === 'dark'">☀</span>
          <span v-else>☾</span>
        </button>
      </div>

      <!-- Mobile Hamburger -->
      <button class="hamburger" @click="toggleMenu" :class="{ open: menuOpen }" aria-label="Toggle menu">
        <span></span><span></span><span></span>
      </button>
    </div>

    <!-- Mobile Menu -->
    <div class="mobile-menu" :class="{ open: menuOpen }">
      <ul>
        <li><RouterLink to="/" class="nav-link" exact-active-class="active" @click="menuOpen = false">Home</RouterLink></li>
        <li><RouterLink to="/latest-race" class="nav-link" exact-active-class="active" @click="menuOpen = false">Latest Race</RouterLink></li>
        <li><RouterLink to="/standings" class="nav-link" exact-active-class="active" @click="menuOpen = false">Standings</RouterLink></li>
        <li><RouterLink to="/schedule" class="nav-link" exact-active-class="active" @click="menuOpen = false">Schedule</RouterLink></li>
        <li><RouterLink to="/teams" class="nav-link" exact-active-class="active" @click="menuOpen = false">Teams</RouterLink></li>
        <li><RouterLink to="/results" class="nav-link" exact-active-class="active" @click="menuOpen = false">Results</RouterLink></li>
        <li>
          <button class="theme-toggle mobile-theme" @click="toggle">
            {{ theme === 'dark' ? '☀ Light mode' : '☾ Dark mode' }}
          </button>
        </li>
      </ul>
    </div>

    <div class="racing-stripe">
      <div class="stripe red"></div>
      <div class="stripe white"></div>
    </div>
  </nav>
</template>

<script setup>
import { ref } from 'vue'
import { useTheme } from '@/composables/useTheme.js'

const menuOpen = ref(false)
const { theme, toggle } = useTheme()

function toggleMenu() { menuOpen.value = !menuOpen.value }
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Barlow+Condensed:wght@400;600;700;800&display=swap');

.navbar {
  position: fixed;
  top: 0; left: 0; right: 0;
  width: 100%;
  z-index: 1000;
  background: var(--bg-card);
  box-shadow: 0 2px 24px rgba(0,0,0,0.3);
  font-family: 'Barlow Condensed', sans-serif;
  transition: background 0.2s;
}

.navbar-inner {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 2rem;
  height: 68px;
  max-width: 1400px;
  margin: 0 auto;
}

.navbar-brand { display: flex; align-items: center; flex-shrink: 0; }

.logo {
  height: 36px;
  width: auto;
  object-fit: contain;
  display: block;
  filter: brightness(0) invert(1);
  transition: opacity 0.2s, filter 0.2s;
}

[data-theme="light"] .logo {
  filter: brightness(0);
}

.logo:hover { opacity: 0.85; }

.navbar-title {
  display: flex;
  align-items: center;
  gap: 0.9rem;
  position: absolute;
  left: 25%;
  transform: translateX(-50%);
}

.navbar-title h2 {
  margin: 0;
  font-size: 1.15rem;
  font-weight: 800;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  color: #e10600;
  white-space: nowrap;
}

.title-line {
  display: block;
  width: 28px;
  height: 2px;
  background: #e10600;
  flex-shrink: 0;
}

.navbar-right {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.navbar-links {
  display: flex;
  align-items: center;
  gap: 0.25rem;
  list-style: none;
  margin: 0;
  padding: 0;
}

.nav-link {
  display: block;
  padding: 0.45rem 0.85rem;
  font-size: 0.88rem;
  font-weight: 700;
  letter-spacing: 0.1em;
  text-transform: uppercase;
  color: #e10600;
  text-decoration: none;
  border-bottom: 2px solid transparent;
  transition: color 0.2s, border-color 0.2s;
}

.nav-link:hover, .nav-link.active {
  color: #e10600;
  border-bottom-color: #e10600;
}

/* ── Theme Toggle ──────────────────────────────── */
.theme-toggle {
  background: none;
  border: 1px solid var(--border-2);
  color: var(--text-2);
  width: 34px;
  height: 34px;
  border-radius: 6px;
  cursor: pointer;
  font-size: 1rem;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: border-color 0.2s, color 0.2s, background 0.2s;
  flex-shrink: 0;
}

.theme-toggle:hover {
  border-color: #e10600;
  color: var(--text);
}

/* ── Racing Stripe ────────────────────────────── */
.racing-stripe { display: flex; height: 3px; }
.stripe { flex: 1; }
.stripe.red  { background: #e10600; flex: 8; }
.stripe.white { background: #ffffff; flex: 1; }

/* ── Hamburger ────────────────────────────────── */
.hamburger {
  display: none;
  flex-direction: column;
  justify-content: center;
  gap: 5px;
  background: none;
  border: none;
  cursor: pointer;
  padding: 6px;
}

.hamburger span {
  display: block;
  width: 24px;
  height: 2px;
  background: #e10600;
  border-radius: 2px;
  transition: transform 0.25s, opacity 0.25s;
  transform-origin: center;
}

.hamburger.open span:nth-child(1) { transform: translateY(7px) rotate(45deg); }
.hamburger.open span:nth-child(2) { opacity: 0; }
.hamburger.open span:nth-child(3) { transform: translateY(-7px) rotate(-45deg); }

/* ── Mobile Menu ──────────────────────────────── */
.mobile-menu {
  display: none;
  background: var(--bg-el);
  border-top: 1px solid var(--border);
  overflow: hidden;
  max-height: 0;
  transition: max-height 0.3s ease;
}

.mobile-menu.open { max-height: 360px; }

.mobile-menu ul {
  list-style: none;
  margin: 0;
  padding: 0.5rem 2rem 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.mobile-menu .nav-link {
  font-size: 1rem;
  padding: 0.6rem 0;
  border-bottom: 1px solid var(--border);
}

.mobile-theme {
  background: none;
  border: none;
  color: var(--text-2);
  font-family: 'Barlow Condensed', sans-serif;
  font-size: 1rem;
  font-weight: 700;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  padding: 0.6rem 0;
  cursor: pointer;
  text-align: left;
  width: auto;
  height: auto;
  border-radius: 0;
  display: block;
}

/* ── Responsive ───────────────────────────────── */
@media (max-width: 768px) {
  .navbar-title h2 { font-size: 0.9rem; letter-spacing: 0.06em; }
  .title-line { width: 16px; }
  .navbar-links { display: none; }
  .theme-toggle:not(.mobile-theme) { display: none; }
  .hamburger { display: flex; }
  .mobile-menu { display: block; }
}

@media (max-width: 480px) {
  .navbar-title { display: none; }
}
</style>
