<template>
  <NavBar />
  <main class="teams-page">

    <div class="page-header">
      <p class="page-eyebrow">Formula 1 · {{ season }}</p>
      <h1 class="page-title">Constructor Profiles</h1>
      <p class="page-sub" v-if="currentRound">{{ currentRound }} rounds completed</p>
    </div>

    <div v-if="loading" class="loading-state">
      <div class="spinner"></div>
      <p>Loading teams…</p>
    </div>

    <div v-else-if="error" class="error-state">
      <p>{{ error }}</p>
    </div>

    <div v-else class="teams-grid">
      <div
        v-for="team in teams"
        :key="team.constructorId"
        class="team-card"
        :style="{ '--tc': teamColor(team.constructorId) }"
      >
        <div class="card-accent"></div>
        <div class="card-glow"></div>

        <div class="card-body">

          <!-- ── Header ── -->
          <div class="team-header">
            <div class="team-identity">
              <div class="logo-wrap">
                <img
                  v-if="teamLogos[team.constructorId]"
                  :src="teamLogos[team.constructorId]"
                  :alt="team.name"
                  class="team-logo"
                />
                <div v-else class="logo-fallback">{{ team.name[0] }}</div>
              </div>
              <div>
                <h2 class="team-name">{{ team.name }}</h2>
                <p class="team-nationality">{{ team.nationality }}</p>
              </div>
            </div>

            <div class="team-standing">
              <div class="pos-badge">P{{ team.position }}</div>
              <div class="pts-total">{{ team.points }}<span> PTS</span></div>
              <div class="wins-tag" v-if="team.wins > 0">
                {{ team.wins }}W
              </div>
            </div>
          </div>

          <div class="divider"></div>

          <!-- ── Drivers ── -->
          <div class="team-drivers">
            <div
              v-for="driver in team.drivers"
              :key="driver.driverId"
              class="driver-chip"
            >
              <div class="driver-avatar">
                <img
                  v-if="driverPhotos[driver.driverId]"
                  :src="driverPhotos[driver.driverId]"
                  :alt="driver.familyName"
                  class="driver-photo"
                />
                <span v-else class="driver-initials">
                  {{ driver.givenName[0] }}{{ driver.familyName[0] }}
                </span>
              </div>
              <div class="driver-info">
                <div class="driver-num">#{{ driver.permanentNumber }}</div>
                <div class="driver-name">
                  {{ driver.givenName }}&nbsp;<strong>{{ driver.familyName.toUpperCase() }}</strong>
                </div>
                <div class="driver-pts">{{ driver.points }} pts</div>
              </div>
            </div>
          </div>

          <!-- ── Points Chart ── -->
          <div class="team-chart" v-if="team.chartData.length > 1">
            <div class="chart-meta">
              <span class="chart-label">SEASON POINTS</span>
              <span class="chart-val">{{ team.chartData[team.chartData.length - 1] }}</span>
            </div>
            <svg
              class="sparkline"
              viewBox="0 0 200 44"
              preserveAspectRatio="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <defs>
                <linearGradient :id="`sg-${team.constructorId}`" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="0%" :stop-color="teamColor(team.constructorId)" stop-opacity="0.25" />
                  <stop offset="100%" :stop-color="teamColor(team.constructorId)" stop-opacity="0" />
                </linearGradient>
              </defs>
              <polygon
                :points="areaPoints(team.chartData)"
                :fill="`url(#sg-${team.constructorId})`"
              />
              <polyline
                :points="linePoints(team.chartData)"
                fill="none"
                :stroke="teamColor(team.constructorId)"
                stroke-width="2"
                stroke-linecap="round"
                stroke-linejoin="round"
              />
              <circle
                v-if="team.chartData.length"
                :cx="endX(team.chartData)"
                :cy="endY(team.chartData)"
                r="3"
                :fill="teamColor(team.constructorId)"
              />
            </svg>
          </div>

        </div>
      </div>
    </div>

  </main>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import NavBar from '@/components/NavBar.vue'
import { getConstructors, getDriverStandings } from '@/services/f1ApiService.js'

const season = String(new Date().getFullYear())
const loading = ref(true)
const error = ref(null)
const teams = ref([])
const currentRound = ref(0)
const teamLogos = ref({})
const driverPhotos = ref({})

const TEAM_COLORS = {
  red_bull:      '#3671C6',
  ferrari:       '#E8002D',
  mclaren:       '#FF8000',
  mercedes:      '#27F4D2',
  aston_martin:  '#358C75',
  alpine:        '#FF87BC',
  williams:      '#64C4FF',
  rb:            '#6692FF',
  kick_sauber:   '#52E252',
  haas:          '#B6BABD',
  sauber:        '#52E252',
  alphatauri:    '#5863B5',
  alfa:          '#B12039',
  racing_point:  '#F596C8',
  renault:       '#FFF500',
}

function teamColor(id) {
  return TEAM_COLORS[id] || '#e10600'
}

const chartMax = computed(() =>
  Math.max(...teams.value.flatMap(t => t.chartData), 1)
)

function linePoints(data) {
  if (data.length < 2) return ''
  return data.map((v, i) => {
    const x = (i / (data.length - 1)) * 196 + 2
    const y = 40 - (v / chartMax.value) * 36 + 2
    return `${x.toFixed(1)},${y.toFixed(1)}`
  }).join(' ')
}

function areaPoints(data) {
  if (data.length < 2) return ''
  const pts = data.map((v, i) => {
    const x = (i / (data.length - 1)) * 196 + 2
    const y = 40 - (v / chartMax.value) * 36 + 2
    return `${x.toFixed(1)},${y.toFixed(1)}`
  })
  const lastX = (196 + 2).toFixed(1)
  return [...pts, `${lastX},42`, `2,42`].join(' ')
}

function endX() {
  return (196 + 2).toFixed(1)
}

function endY(data) {
  const v = data[data.length - 1]
  return (40 - (v / chartMax.value) * 36 + 2).toFixed(1)
}

async function fetchWikiThumb(url, id, store) {
  try {
    const title = decodeURIComponent(url.split('/wiki/')[1])
    const res = await fetch(
      `https://en.wikipedia.org/api/rest_v1/page/summary/${encodeURIComponent(title)}`
    )
    if (!res.ok) return
    const json = await res.json()
    if (json.thumbnail?.source) store.value[id] = json.thumbnail.source
  } catch { /* silent */ }
}

function calcConstructorStats(driverList) {
  const map = {}
  for (const entry of driverList) {
    const cid = entry.constructors?.[0]?.constructorId
    if (!cid) continue
    if (!map[cid]) map[cid] = { points: 0, wins: 0 }
    map[cid].points += parseFloat(entry.points) || 0
    map[cid].wins   += parseInt(entry.wins) || 0
  }
  return map
}

onMounted(async () => {
  try {
    const [constructorListRes, driverRes] = await Promise.all([
      getConstructors(season),
      getDriverStandings(season).catch(() => null),
    ])

    const constructorList =
      constructorListRes?.mrData?.constructorTable?.constructors ?? []

    const standingsListItem =
      driverRes?.mrData?.standingsTable?.standingsLists?.[0]
    const driverList = standingsListItem?.driverStandings ?? []
    currentRound.value = parseInt(standingsListItem?.round ?? '0')

    // Points + wins per constructor from driver standings
    const constructorStats = calcConstructorStats(driverList)

    // Drivers grouped by constructor
    const constructorDrivers = {}
    for (const entry of driverList) {
      const cid = entry.constructors?.[0]?.constructorId
      if (!cid) continue
      if (!constructorDrivers[cid]) constructorDrivers[cid] = []
      constructorDrivers[cid].push({ ...entry.driver, points: entry.points })
    }

    // Chart: per-round driver standings → sum into constructor points
    const rounds = Array.from({ length: currentRound.value }, (_, i) => i + 1)
    const roundResponses = await Promise.all(
      rounds.map(r => getDriverStandings(season, String(r)).catch(() => null))
    )
    const chartData = {}
    for (const res of roundResponses) {
      const entries =
        res?.mrData?.standingsTable?.standingsLists?.[0]?.driverStandings ?? []
      const roundStats = calcConstructorStats(entries)
      for (const [cid, stats] of Object.entries(roundStats)) {
        if (!chartData[cid]) chartData[cid] = []
        chartData[cid].push(stats.points)
      }
    }

    // Build team list, sorted by points desc
    teams.value = constructorList.map(c => {
      const stats = constructorStats[c.constructorId]
      return {
        constructorId: c.constructorId,
        name:          c.name,
        nationality:   c.nationality,
        url:           c.url,
        points:        stats?.points ?? 0,
        wins:          stats?.wins   ?? 0,
        position:      null, // assigned after sort
        drivers:       constructorDrivers[c.constructorId] ?? [],
        chartData:     chartData[c.constructorId] ?? [],
      }
    })

    teams.value.sort((a, b) => b.points - a.points)
    teams.value.forEach((t, i) => { t.position = i + 1 })

    for (const team of teams.value) {
      if (team.url) fetchWikiThumb(team.url, team.constructorId, teamLogos)
      for (const driver of team.drivers) {
        if (driver.url) fetchWikiThumb(driver.url, driver.driverId, driverPhotos)
      }
    }
  } catch (err) {
    console.error(err)
    error.value = 'Failed to load team data.'
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Barlow+Condensed:wght@400;600;700;800;900&display=swap');

.teams-page {
  min-height: 100vh;
  background: var(--bg);
  color: var(--text);
  font-family: 'Barlow Condensed', sans-serif;
  padding-top: 68px;
}

/* ── Page Header ──────────────────────────────── */
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
  margin: 0 0 0.5rem;
}

.page-sub {
  font-size: 0.9rem;
  color: var(--text-3);
  font-family: sans-serif;
  font-weight: 400;
  margin: 0;
}

/* ── Loading / Error ──────────────────────────── */
.loading-state, .error-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  min-height: 40vh;
  color: var(--text-3);
  font-family: sans-serif;
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

/* ── Teams Grid ───────────────────────────────── */
.teams-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(460px, 1fr));
  gap: 1px;
  background: var(--bg-el);
  padding: 1px;
}

/* ── Team Card ────────────────────────────────── */
.team-card {
  position: relative;
  background: var(--bg-card);
  overflow: hidden;
  transition: background 0.2s;
}

.team-card:hover {
  background: var(--bg-hover);
}

.card-accent {
  position: absolute;
  left: 0;
  top: 0;
  bottom: 0;
  width: 4px;
  background: var(--tc);
}

.card-glow {
  position: absolute;
  inset: 0;
  background: radial-gradient(ellipse 80% 60% at 0% 50%, color-mix(in srgb, var(--tc) 8%, transparent), transparent 70%);
  pointer-events: none;
}

.card-body {
  padding: 1.75rem 1.75rem 1.75rem 2.25rem;
  position: relative;
  z-index: 1;
}

/* ── Team Header ──────────────────────────────── */
.team-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 1rem;
}

.team-identity {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.logo-wrap {
  width: 52px;
  height: 52px;
  flex-shrink: 0;
  border-radius: 8px;
  overflow: hidden;
  background: var(--border);
  display: flex;
  align-items: center;
  justify-content: center;
}

.team-logo {
  width: 100%;
  height: 100%;
  object-fit: contain;
  padding: 4px;
}

.logo-fallback {
  width: 52px;
  height: 52px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  font-weight: 900;
  color: var(--text);
  background: var(--tc);
  flex-shrink: 0;
}

.team-name {
  font-size: 1.4rem;
  font-weight: 800;
  text-transform: uppercase;
  letter-spacing: 0.04em;
  margin: 0 0 0.15rem;
  color: var(--text);
}

.team-nationality {
  font-size: 0.78rem;
  color: var(--text-3);
  font-family: sans-serif;
  font-weight: 400;
  margin: 0;
  text-transform: uppercase;
  letter-spacing: 0.08em;
}

.team-standing {
  text-align: right;
  flex-shrink: 0;
}

.pos-badge {
  font-size: 0.75rem;
  font-weight: 700;
  letter-spacing: 0.1em;
  color: var(--tc);
  margin-bottom: 0.2rem;
}

.pts-total {
  font-size: 1.6rem;
  font-weight: 900;
  line-height: 1;
  color: var(--text);
}

.pts-total span {
  font-size: 0.75rem;
  font-weight: 600;
  color: var(--text-3);
  letter-spacing: 0.1em;
}

.wins-tag {
  display: inline-block;
  margin-top: 0.3rem;
  font-size: 0.7rem;
  font-weight: 700;
  letter-spacing: 0.1em;
  padding: 0.15rem 0.5rem;
  border-radius: 2px;
  background: color-mix(in srgb, var(--tc) 20%, transparent);
  color: var(--tc);
  border: 1px solid color-mix(in srgb, var(--tc) 40%, transparent);
}

/* ── Divider ──────────────────────────────────── */
.divider {
  height: 1px;
  background: var(--border);
  margin: 1.25rem 0;
}

/* ── Drivers ──────────────────────────────────── */
.team-drivers {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 0.75rem;
}

.driver-chip {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.6rem 0.75rem;
  background: var(--bg);
  border-radius: 6px;
  border: 1px solid var(--border);
}

.driver-avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  overflow: hidden;
  flex-shrink: 0;
  background: var(--border);
  display: flex;
  align-items: center;
  justify-content: center;
  border: 2px solid color-mix(in srgb, var(--tc) 40%, transparent);
}

.driver-photo {
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: top;
}

.driver-initials {
  font-size: 0.75rem;
  font-weight: 700;
  color: var(--tc);
}

.driver-info {
  min-width: 0;
}

.driver-num {
  font-size: 0.7rem;
  font-weight: 700;
  color: var(--tc);
  letter-spacing: 0.05em;
  line-height: 1;
  margin-bottom: 0.15rem;
}

.driver-name {
  font-size: 0.85rem;
  font-weight: 600;
  color: var(--text-2);
  text-transform: uppercase;
  letter-spacing: 0.03em;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.driver-name strong {
  color: var(--text);
}

.driver-pts {
  font-size: 0.7rem;
  color: var(--text-3);
  font-family: sans-serif;
  font-weight: 400;
  margin-top: 0.1rem;
}

/* ── Chart ────────────────────────────────────── */
.team-chart {
  margin-top: 1.25rem;
}

.chart-meta {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.4rem;
}

.chart-label {
  font-size: 0.7rem;
  font-weight: 700;
  letter-spacing: 0.12em;
  color: var(--text-4);
}

.chart-val {
  font-size: 0.85rem;
  font-weight: 700;
  color: var(--tc);
}

.sparkline {
  width: 100%;
  height: 44px;
  display: block;
  overflow: visible;
}

/* ── Responsive ───────────────────────────────── */
@media (max-width: 900px) {
  .teams-grid {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 520px) {
  .page-header {
    padding: 2.5rem 1.5rem 1.5rem;
  }

  .card-body {
    padding: 1.25rem 1.25rem 1.25rem 1.75rem;
  }

  .team-drivers {
    grid-template-columns: 1fr;
  }
}
</style>
