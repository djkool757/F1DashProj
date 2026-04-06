<template>
  <nav class="navbar">
    <div class="navbar-inner">
      <!-- Left: Logo -->
      <div class="navbar-brand">
        <img
          alt="f1 logo"
          class="logo"
          src="@/assets/F1_logo.png"                  
          width="100"
          height="1000"
        />
      </div>

      <!-- Center: Title -->
      <div class="navbar-title">
        <span class="title-line" aria-hidden="true"></span>
        <h2>F1 Dashboard</h2>
        <span class="title-line" aria-hidden="true"></span>
      </div>

      <!-- Right: Nav Links -->
      <ul class="navbar-links">
        <li><a href="#" class="nav-link active">Standings</a></li>
        <li><a href="#" class="nav-link">Schedule</a></li>
        <li><a href="#" class="nav-link">Teams</a></li>
        <li><a href="#" class="nav-link">Results</a></li>
      </ul>

      <!-- Mobile Hamburger -->
      <button class="hamburger" @click="toggleMenu" :class="{ open: menuOpen }" aria-label="Toggle menu">
        <span></span>
        <span></span>
        <span></span>
      </button>
    </div>

    <!-- Mobile Menu -->
    <div class="mobile-menu" :class="{ open: menuOpen }">
      <ul>
        <li><a href="#" class="nav-link active" @click="menuOpen = false">Standings</a></li>
        <li><a href="#" class="nav-link" @click="menuOpen = false">Schedule</a></li>
        <li><a href="#" class="nav-link" @click="menuOpen = false">Teams</a></li>
        <li><a href="#" class="nav-link" @click="menuOpen = false">Results</a></li>
      </ul>
    </div>

    <!-- Bottom racing stripe -->
    <div class="racing-stripe">
      <div class="stripe red"></div>
      <div class="stripe white"></div>
    </div>
  </nav>
</template>

<script setup>
import { ref } from 'vue'

const menuOpen = ref(false)

function toggleMenu() {
  menuOpen.value = !menuOpen.value
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Barlow+Condensed:wght@400;600;700;800&family=Barlow:wght@400;500&display=swap');

:root {
  --f1-red: #e10600;
  --f1-dark: #111111;
  --f1-carbon: #1a1a1a;
  --f1-light: #f5f5f5;
  --f1-muted: #888888;
  --f1-accent: #e10600;
}

/* ── Navbar Shell ───────────────────────────────── */
.navbar {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  width: 100%;
  z-index: 1000;
  background: var(--f1-dark);
  box-shadow: 0 2px 24px rgba(0, 0, 0, 0.6);
  font-family: 'Barlow Condensed', sans-serif;
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

/* ── Logo ───────────────────────────────────────── */
.navbar-brand {
  display: flex;
  align-items: center;
  flex-shrink: 0;
}

.logo {
  height: 36px;
  top: 0;
  width: auto;
  object-fit: contain;
  display: block;
  /* Invert if logo is dark so it shows on dark bg; remove if logo is already white/red */
  filter: brightness(0) invert(1);
  transition: opacity 0.2s;
}

.logo:hover {
  opacity: 0.85;
}

/* ── Title ──────────────────────────────────────── */
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
  color: var(--f1-light);
  white-space: nowrap;
}

.title-line {
  display: block;
  width: 28px;
  height: 2px;
  background: var(--f1-red);
  flex-shrink: 0;
}

/* ── Nav Links ──────────────────────────────────── */
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
  color: var(--f1-muted);
  text-decoration: none;
  border-bottom: 2px solid transparent;
  transition: color 0.2s, border-color 0.2s;
}

.nav-link:hover,
.nav-link.active {
  color: var(--f1-light);
  border-bottom-color: var(--f1-red);
}

.nav-link.active {
  color: var(--f1-red);
}

/* ── Racing Stripe ──────────────────────────────── */
.racing-stripe {
  display: flex;
  height: 3px;
}

.stripe {
  flex: 1;
}

.stripe.red {
  background: var(--f1-red);
  flex: 8;
}

.stripe.white {
  background: #ffffff;
  flex: 1;
}

/* ── Hamburger ──────────────────────────────────── */
.hamburger {
  display: none;
  flex-direction: column;
  justify-content: center;
  gap: 5px;
  background: none;
  border: none;
  cursor: pointer;
  padding: 6px;
  z-index: 10;
}

.hamburger span {
  display: block;
  width: 24px;
  height: 2px;
  background: var(--f1-light);
  border-radius: 2px;
  transition: transform 0.25s, opacity 0.25s;
  transform-origin: center;
}

.hamburger.open span:nth-child(1) {
  transform: translateY(7px) rotate(45deg);
}
.hamburger.open span:nth-child(2) {
  opacity: 0;
}
.hamburger.open span:nth-child(3) {
  transform: translateY(-7px) rotate(-45deg);
}

/* ── Mobile Menu ────────────────────────────────── */
.mobile-menu {
  display: none;
  background: var(--f1-carbon);
  border-top: 1px solid #2a2a2a;
  overflow: hidden;
  max-height: 0;
  transition: max-height 0.3s ease;
}

.mobile-menu.open {
  max-height: 300px;
}

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
  border-bottom: 1px solid #2a2a2a;
}

/* ── Responsive ─────────────────────────────────── */
@media (max-width: 768px) {
  .navbar-title h2 {
    font-size: 0.9rem;
    letter-spacing: 0.06em;
  }

  .title-line {
    width: 16px;
  }

  .navbar-links {
    display: none;
  }

  .hamburger {
    display: flex;
  }

  .mobile-menu {
    display: block;
  }
}

@media (max-width: 480px) {
  .navbar-title {
    display: none;
  }
}
</style>