<template>
  <NavBar />
  <main class="schedule-page">

    <div class="page-header">
      <p class="page-eyebrow">Formula 1 · {{ season }}</p>
      <h1 class="page-title">Season Calendar</h1>
      <p class="page-sub" v-if="nextRace">
        Next race in <strong>{{ daysUntilNext }} day{{ daysUntilNext !== 1 ? 's' : '' }}</strong>
        — {{ nextRace.raceName }}
      </p>
    </div>

    <div v-if="loading" class="loading-state">
      <div class="spinner"></div>
    </div>

    <div v-else class="race-list">
      <template v-for="(group, month) in groupedRaces" :key="month">
        <div class="month-header">{{ month }}</div>

        <div
          v-for="race in group"
          :key="race.round"
          class="race-row"
          :class="{
            'is-past': isPast(race),
            'is-next': isNext(race),
            'is-future': isFuture(race),
          }"
        >
          <div class="round-col">
            <span class="round-num">R{{ race.round }}</span>
          </div>

          <div class="date-col">
            <span class="date-day">{{ formatDay(race.date) }}</span>
            <span class="date-month">{{ formatMonth(race.date) }}</span>
          </div>

          <div class="race-info">
            <div class="race-name">{{ race.raceName }}</div>
            <div class="race-meta">
              <span class="circuit-name">{{ race.circuit?.circuitName }}</span>
              <span class="separator">·</span>
              <span class="location">{{ race.circuit?.location?.locality }}, {{ race.circuit?.location?.country }}</span>
            </div>
          </div>

          <div class="race-status">
            <span v-if="isPast(race)" class="badge done">Done</span>
            <span v-else-if="isNext(race)" class="badge next">Next</span>
            <span v-else class="badge upcoming">
              {{ daysUntil(race) }}d
            </span>
          </div>
        </div>
      </template>
    </div>

  </main>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import NavBar from '@/components/NavBar.vue'
import { getRaces } from '@/services/f1ApiService.js'

const season = String(new Date().getFullYear())
const loading = ref(true)
const races = ref([])
const today = new Date()
today.setHours(0, 0, 0, 0)

const nextRace = computed(() =>
  races.value.find(r => new Date(r.date) >= today) ?? null
)

const daysUntilNext = computed(() => {
  if (!nextRace.value) return 0
  return Math.ceil((new Date(nextRace.value.date) - today) / 86400000)
})

function isPast(race)   { return new Date(race.date) < today }
function isNext(race)   { return nextRace.value?.round === race.round }
function isFuture(race) { return !isPast(race) && !isNext(race) }

function daysUntil(race) {
  return Math.ceil((new Date(race.date) - today) / 86400000)
}

function formatDay(dateStr) {
  return new Date(dateStr).toLocaleDateString('en-GB', { day: 'numeric' })
}

function formatMonth(dateStr) {
  return new Date(dateStr).toLocaleDateString('en-GB', { month: 'short' }).toUpperCase()
}

const MONTHS = ['JAN','FEB','MAR','APR','MAY','JUN','JUL','AUG','SEP','OCT','NOV','DEC']

const groupedRaces = computed(() => {
  const groups = {}
  for (const race of races.value) {
    const d = new Date(race.date)
    const key = `${MONTHS[d.getMonth()]} ${d.getFullYear()}`
    if (!groups[key]) groups[key] = []
    groups[key].push(race)
  }
  return groups
})

onMounted(async () => {
  try {
    const data = await getRaces(season)
    races.value = data?.mrData?.raceTable?.races ?? []
  } catch (err) {
    console.error('Failed to load schedule:', err)
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Barlow+Condensed:wght@400;600;700;800;900&display=swap');

.schedule-page {
  min-height: 100vh;
  background: var(--bg);
  color: var(--text);
  font-family: 'Barlow Condensed', sans-serif;
  padding-top: 68px;
}

/* ── Header ───────────────────────────────────── */
.page-header {
  padding: 3.5rem 6vw 2.5rem;
  border-bottom: 1px solid var(--border);
}

.page-eyebrow {
  font-size: 0.8rem;
  font-weight: 700;
  letter-spacing: 0.2em;
  text-transform: uppercase;
  color: #e10600;
  margin: 0 0 0.6rem;
}

.page-title {
  font-size: clamp(2.2rem, 5vw, 4rem);
  font-weight: 900;
  text-transform: uppercase;
  letter-spacing: -0.01em;
  line-height: 1;
  margin: 0 0 0.6rem;
}

.page-sub {
  font-size: 0.95rem;
  color: var(--text-3);
  font-family: sans-serif;
  font-weight: 400;
  margin: 0;
}

.page-sub strong {
  color: #e10600;
  font-weight: 700;
}

/* ── Loading ──────────────────────────────────── */
.loading-state {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 30vh;
}

.spinner {
  width: 36px;
  height: 36px;
  border: 3px solid var(--border);
  border-top-color: #e10600;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin { to { transform: rotate(360deg); } }

/* ── Race List ────────────────────────────────── */
.race-list {
  max-width: 860px;
  margin: 0 auto;
  padding: 0 6vw 4rem;
}

/* ── Month Label ──────────────────────────────── */
.month-header {
  font-size: 0.75rem;
  font-weight: 700;
  letter-spacing: 0.2em;
  color: var(--text-4);
  text-transform: uppercase;
  padding: 2rem 0 0.6rem;
  border-bottom: 1px solid var(--border);
  margin-bottom: 0;
}

/* ── Race Row ─────────────────────────────────── */
.race-row {
  display: flex;
  align-items: center;
  gap: 1.25rem;
  padding: 1rem 1rem;
  border-bottom: 1px solid var(--border);
  border-left: 3px solid transparent;
  transition: background 0.15s, border-color 0.15s;
  border-radius: 2px;
}

.race-row:hover {
  background: var(--bg-hover);
}

.race-row.is-past {
  opacity: 0.45;
}

.race-row.is-next {
  border-left-color: #e10600;
  background: rgba(225, 6, 0, 0.04);
  opacity: 1;
}

/* ── Round Col ────────────────────────────────── */
.round-col {
  width: 2.5rem;
  flex-shrink: 0;
  text-align: center;
}

.round-num {
  font-size: 0.72rem;
  font-weight: 700;
  letter-spacing: 0.08em;
  color: var(--text-4);
}

.is-next .round-num {
  color: #e10600;
}

/* ── Date Col ─────────────────────────────────── */
.date-col {
  width: 2.8rem;
  flex-shrink: 0;
  text-align: center;
  line-height: 1.1;
}

.date-day {
  display: block;
  font-size: 1.4rem;
  font-weight: 900;
  color: var(--text);
  line-height: 1;
}

.date-month {
  display: block;
  font-size: 0.65rem;
  font-weight: 700;
  letter-spacing: 0.1em;
  color: var(--text-3);
  margin-top: 0.1rem;
}

.is-next .date-day { color: var(--text); }
.is-next .date-month { color: #e10600; }

/* ── Race Info ────────────────────────────────── */
.race-info {
  flex: 1;
  min-width: 0;
}

.race-name {
  font-size: 1.05rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.04em;
  color: var(--text);
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.is-past .race-name { color: var(--text-2); }

.race-meta {
  display: flex;
  align-items: center;
  gap: 0.4rem;
  font-size: 0.75rem;
  color: var(--text-3);
  font-family: sans-serif;
  font-weight: 400;
  margin-top: 0.15rem;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.separator { color: var(--text-4); }

/* ── Status Badge ─────────────────────────────── */
.race-status {
  flex-shrink: 0;
}

.badge {
  display: inline-block;
  font-size: 0.68rem;
  font-weight: 700;
  letter-spacing: 0.1em;
  text-transform: uppercase;
  padding: 0.2rem 0.55rem;
  border-radius: 2px;
}

.badge.done {
  background: var(--bg-el);
  color: var(--text-4);
}

.badge.next {
  background: rgba(225, 6, 0, 0.15);
  color: #e10600;
  border: 1px solid rgba(225, 6, 0, 0.3);
}

.badge.upcoming {
  background: var(--bg-el);
  color: var(--text-3);
}

/* ── Responsive ───────────────────────────────── */
@media (max-width: 520px) {
  .page-header { padding: 2.5rem 1.5rem 1.5rem; }
  .race-list { padding: 0 1rem 3rem; }

  .race-meta .separator,
  .circuit-name { display: none; }
}
</style>
