<template>
  <NavBar />

  <main class="home">
    <!-- Hero -->
    <section class="hero">
      <div class="hero-content">
        <p class="hero-eyebrow">Formula 1 · {{ currentYear }}</p>
        <h1 class="hero-title">Every Race.<br />Every Stat.<br />One Dashboard.</h1>
        <p class="hero-sub">Live standings, race results, schedules and more — all in one place.</p>
        <RouterLink to="/latest-race" class="hero-cta">View Latest Race</RouterLink>
      </div>
      <div class="hero-number" aria-hidden="true">F1</div>
    </section>

    <!-- Feature cards -->
    <section class="features">
      <RouterLink
        v-for="card in cards"
        :key="card.to"
        :to="card.to"
        class="feature-card"
      >
        <span class="feature-icon">{{ card.icon }}</span>
        <div>
          <div class="feature-title">{{ card.title }}</div>
          <div class="feature-desc">{{ card.desc }}</div>
        </div>
        <span class="feature-arrow">→</span>
      </RouterLink>
    </section>
  </main>
</template>

<script setup>
import NavBar from '@/components/NavBar.vue'

const currentYear = new Date().getFullYear()

const cards = [
  { to: '/latest-race', icon: '🏁', title: 'Latest Race',  desc: 'Podium, results & highlights' },
  { to: '/standings',   icon: '🏆', title: 'Standings',    desc: 'Driver & constructor championship' },
  { to: '/schedule',    icon: '📅', title: 'Schedule',     desc: 'Full season calendar' },
  { to: '/teams',       icon: '🔧', title: 'Teams',        desc: 'Constructor profiles' },
  { to: '/results',     icon: '📊', title: 'Results',      desc: 'All race results by round' },
]
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Barlow+Condensed:wght@400;700;800;900&display=swap');

.home {
  height: 100vh;
  background: var(--bg);
  width: 100vw;
  color: var(--text);
  font-family: 'Barlow Condensed', sans-serif;
}

/* ── Hero ─────────────────────────────────────── */
.hero {
  position: relative;
  display: flex;
  align-items: center;
  width: 100%;
  min-height: calc(100vh - 68px);
  padding: 4rem 2rem 4rem 8vw;
  overflow: hidden;
}

.hero::before {
  content: '';
  position: absolute;
  inset: 0;
  background:
    radial-gradient(ellipse 60% 80% at 20% 50%, rgba(225, 6, 0, 0.12) 0%, transparent 70%),
    linear-gradient(135deg, var(--bg) 0%, var(--bg-el) 100%);
}

.hero-content {
  position: relative;
  z-index: 2;
  max-width: 640px;
}

.hero-eyebrow {
  font-size: 0.9rem;
  font-weight: 700;
  letter-spacing: 0.2em;
  text-transform: uppercase;
  color: #e10600;
  margin: 0 0 1rem;
}

.hero-title {
  font-size: clamp(3rem, 8vw, 6.5rem);
  font-weight: 900;
  line-height: 1;
  letter-spacing: -0.01em;
  text-transform: uppercase;
  margin: 0 0 1.5rem;
  color: var(--text);
}

.hero-sub {
  font-size: clamp(1rem, 2vw, 1.25rem);
  color: var(--text-2);
  font-family: sans-serif;
  font-weight: 400;
  margin: 0 0 2.5rem;
  max-width: 420px;
  line-height: 1.6;
}

.hero-cta {
  display: inline-block;
  background: #e10600;
  color: var(--text);
  font-family: 'Barlow Condensed', sans-serif;
  font-size: 1rem;
  font-weight: 700;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  text-decoration: none;
  padding: 0.85rem 2rem;
  border-radius: 4px;
  transition: background 0.18s, transform 0.18s;
}

.hero-cta:hover {
  background: #c00500;
  transform: translateY(-2px);
}

.hero-number {
  position: absolute;
  right: 4vw;
  top: 50%;
  transform: translateY(-50%);
  font-size: clamp(12rem, 28vw, 24rem);
  font-weight: 900;
  letter-spacing: -0.05em;
  color: rgba(255, 255, 255, 0.03);
  user-select: none;
  pointer-events: none;
  line-height: 1;
}

/* ── Feature cards ───────────────────────────── */
.features {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1px;
  background: var(--border);
  border-top: 1px solid var(--border);
}

.feature-card {
  display: flex;
  align-items: center;
  gap: 1.25rem;
  padding: 1.75rem 2rem;
  background: var(--bg-card);
  text-decoration: none;
  color: inherit;
  transition: background 0.15s;
}

.feature-card:hover {
  background: var(--bg-hover);
}

.feature-card:hover .feature-arrow {
  transform: translateX(4px);
}

.feature-icon {
  font-size: 1.75rem;
  flex-shrink: 0;
  width: 2.5rem;
  text-align: center;
}

.feature-title {
  font-size: 1.1rem;
  font-weight: 700;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  color: var(--text);
  margin-bottom: 0.2rem;
}

.feature-desc {
  font-size: 0.85rem;
  color: var(--text-3);
  font-family: sans-serif;
  font-weight: 400;
}

.feature-arrow {
  margin-left: auto;
  color: #e10600;
  font-size: 1.2rem;
  transition: transform 0.18s;
  flex-shrink: 0;
}

/* ── Responsive ──────────────────────────────── */
@media (max-width: 600px) {
  .hero {
    padding: 3rem 1.5rem;
    min-height: 70vh;
  }

  .hero-number {
    display: none;
  }
}
</style>
