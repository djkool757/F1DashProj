<template>
  <NavBar />
  <main class="results-page">

    <div class="page-header">
      <p class="page-eyebrow">Formula 1</p>
      <h1 class="page-title">Race Results</h1>
    </div>

    <!-- ── Filters ── -->
    <div class="filters-bar">
      <div class="filter-group">
        <label class="filter-label">Season</label>
        <select v-model="selectedSeason" class="filter-select" @change="onSeasonChange">
          <option v-for="yr in seasons" :key="yr" :value="yr">{{ yr }}</option>
        </select>
      </div>
      <div class="filter-group">
        <label class="filter-label">Race</label>
        <select
          v-model="selectedRound"
          class="filter-select"
          :disabled="!raceList.length"
          @change="loadRace"
        >
          <option v-for="race in raceList" :key="race.round" :value="race.round">
            R{{ race.round }} — {{ race.raceName }}
          </option>
        </select>
      </div>
    </div>

    <div v-if="loading" class="loading-state">
      <div class="spinner"></div>
    </div>

    <template v-else-if="results.length">

      <!-- ── Race Banner ── -->
      <div class="race-banner" v-if="selectedRaceInfo">
        <div class="banner-left">
          <span class="banner-round">Round {{ selectedRound }}</span>
          <h2 class="banner-name">{{ selectedRaceInfo.raceName }}</h2>
          <p class="banner-meta">
            {{ selectedRaceInfo.circuit?.circuitName }}
            · {{ selectedRaceInfo.circuit?.location?.locality }},
            {{ selectedRaceInfo.circuit?.location?.country }}
            · {{ formatDate(selectedRaceInfo.date) }}
          </p>
        </div>
        <div class="banner-winner" v-if="results[0]">
          <span class="winner-label">Winner</span>
          <span class="winner-name">
            {{ results[0].driver.givenName }}
            <strong>{{ results[0].driver.familyName.toUpperCase() }}</strong>
          </span>
          <span class="winner-team" :style="{ color: teamColor(results[0].constructor.constructorId) }">
            {{ results[0].constructor.name }}
          </span>
        </div>
      </div>

      <!-- ── Results Table ── -->
      <div class="section">
        <div class="table-scroll">
          <table class="results-table">
            <thead>
              <tr>
                <th>POS</th>
                <th>DRIVER</th>
                <th>TEAM</th>
                <th>GRID</th>
                <th>GAP / STATUS</th>
                <th>PTS</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="r in results"
                :key="r.driver.driverId"
                class="result-row"
                :class="{ 'is-dnf': !isFinished(r) }"
                :style="{ '--tc': teamColor(r.constructor.constructorId) }"
              >
                <td class="td-pos">
                  <span class="pos-num" :class="{ 'is-podium': parseInt(r.postion) <= 3 }">
                    {{ r.positionText }}
                  </span>
                </td>

                <td class="td-driver">
                  <div class="driver-cell">
                    <span class="driver-code">{{ r.driver.code }}</span>
                    <span class="driver-full">
                      {{ r.driver.givenName }}
                      <strong>{{ r.driver.familyName.toUpperCase() }}</strong>
                    </span>
                    <span v-if="r.fastestLap?.rank === '1'" class="fl-badge" title="Fastest Lap">FL</span>
                  </div>
                </td>

                <td class="td-team">
                  <span class="team-pip" :style="{ background: teamColor(r.constructor.constructorId) }"></span>
                  {{ r.constructor.name }}
                </td>

                <td class="td-grid">
                  <span v-if="gridGain(r) !== 0" :class="gridGain(r) > 0 ? 'gain' : 'loss'">
                    {{ gridGain(r) > 0 ? '+' : '' }}{{ gridGain(r) }}
                  </span>
                  <span v-else class="neutral">—</span>
                </td>

                <td class="td-time">{{ r.time?.time || r.status }}</td>

                <td class="td-pts">{{ r.points || '—' }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- ── Lap Chart ── -->
      <div class="section chart-section" v-if="lapChartData">
        <div class="section-header">
          <div>
            <h2 class="section-title">Lap Chart</h2>
            <p class="section-sub">Race position by lap</p>
          </div>
          <button class="toggle-btn" @click="showAll = !showAll">
            {{ showAll ? 'Top 10 only' : 'Show all drivers' }}
          </button>
        </div>
        <div class="chart-wrap">
          <Line :data="lapChartData" :options="chartOptions" />
        </div>
      </div>

    </template>

    <div v-else-if="!loading" class="empty-state">
      <p>No results available for this race yet.</p>
    </div>

  </main>
</template>

<script setup>
import { ref, computed, watch, onMounted } from 'vue'
import NavBar from '@/components/NavBar.vue'
import { getRaces, getResults, getLaps } from '@/services/f1ApiService.js'
import { Line } from 'vue-chartjs'
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Tooltip,
  Legend,
} from 'chart.js'

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Tooltip, Legend)

const currentYear = new Date().getFullYear()
const seasons = Array.from({ length: currentYear - 2009 }, (_, i) => String(currentYear - i))

const selectedSeason = ref(String(currentYear))
const selectedRound  = ref(null)
const raceList       = ref([])
const results        = ref([])
const lapsRaw        = ref([])
const loading        = ref(false)
const showAll        = ref(false)

const TEAM_COLORS = {
  red_bull:     '#3671C6',
  ferrari:      '#E8002D',
  mclaren:      '#FF8000',
  mercedes:     '#27F4D2',
  aston_martin: '#358C75',
  alpine:       '#FF87BC',
  williams:     '#64C4FF',
  rb:           '#6692FF',
  kick_sauber:  '#52E252',
  haas:         '#B6BABD',
  sauber:       '#52E252',
  alphatauri:   '#5863B5',
  alfa:         '#B12039',
  racing_point: '#F596C8',
  renault:      '#FFF500',
  toro_rosso:   '#469BFF',
  force_india:  '#F596C8',
}

function teamColor(id) { return TEAM_COLORS[id] || '#e10600' }

const selectedRaceInfo = computed(() =>
  raceList.value.find(r => r.round === selectedRound.value) ?? null
)

function isFinished(r) {
  return r.status === 'Finished' || r.status?.startsWith('+')
}

function gridGain(r) {
  const grid   = parseInt(r.grid)
  const finish = parseInt(r.postion)
  if (isNaN(grid) || isNaN(finish) || grid === 0) return 0
  return grid - finish
}

function formatDate(dateStr) {
  if (!dateStr) return ''
  return new Date(dateStr).toLocaleDateString('en-GB', {
    day: 'numeric', month: 'long', year: 'numeric',
  })
}

// ── Chart ──────────────────────────────────────

const driversForChart = computed(() =>
  showAll.value ? results.value : results.value.slice(0, 10)
)

const lapChartData = computed(() => {
  if (!lapsRaw.value.length) return null

  const sorted = [...lapsRaw.value].sort((a, b) => parseInt(a.number) - parseInt(b.number))
  const labels = sorted.map(l => l.number)

  const datasets = driversForChart.value.map(r => {
    const id    = r.driver.driverId
    const color = teamColor(r.constructor.constructorId)
    const data  = sorted.map(lap => {
      const t = lap.timings?.find(t => t.driverId === id)
      return t ? t.postion : null
    })
    return {
      label:           r.driver.code || r.driver.familyName.slice(0, 3).toUpperCase(),
      data,
      borderColor:     color,
      backgroundColor: color,
      borderWidth:     2,
      pointRadius:     0,
      pointHoverRadius: 4,
      tension:         0.1,
      spanGaps:        false,
    }
  })

  return { labels, datasets }
})

const chartOptions = {
  responsive:          true,
  maintainAspectRatio: false,
  animation:           false,
  scales: {
    y: {
      reverse:  true,
      min:      1,
      ticks: {
        stepSize: 1,
        color:    '#555',
        font:     { family: 'Barlow Condensed, sans-serif', size: 11 },
      },
      grid:   { color: '#1a1a1a' },
      border: { color: '#2a2a2a' },
    },
    x: {
      ticks: {
        color:         '#555',
        font:          { family: 'Barlow Condensed, sans-serif', size: 11 },
        maxTicksLimit: 20,
      },
      grid:   { color: '#1a1a1a' },
      border: { color: '#2a2a2a' },
    },
  },
  plugins: {
    legend: {
      display:  true,
      position: 'top',
      labels: {
        color:    '#888',
        font:     { family: 'Barlow Condensed, sans-serif', size: 11 },
        boxWidth: 14,
        padding:  10,
        usePointStyle: true,
      },
    },
    tooltip: {
      callbacks: {
        label: ctx => ` P${ctx.raw} — ${ctx.dataset.label}`,
      },
    },
  },
  elements: {
    line:  { borderWidth: 2 },
    point: { radius: 0 },
  },
}

// ── Data fetching ──────────────────────────────

async function loadRaces(season) {
  const data = await getRaces(season)
  const races = data?.mrData?.raceTable?.races ?? []
  raceList.value = races

  const today = new Date()
  const latest = [...races].reverse().find(r => new Date(r.date) <= today)
  selectedRound.value = latest?.round ?? races[0]?.round ?? null
}

async function loadRace() {
  if (!selectedRound.value) return
  loading.value = true
  results.value = []
  lapsRaw.value = []

  try {
    const [resultData, lapData] = await Promise.all([
      getResults(selectedSeason.value, selectedRound.value),
      getLaps(selectedSeason.value, selectedRound.value).catch(() => null),
    ])

    results.value  = resultData?.mrData?.raceTable?.races?.[0]?.results ?? []
    lapsRaw.value  = lapData?.mrData?.raceTable?.races?.[0]?.laps ?? []
  } catch (err) {
    console.error('Failed to load race:', err)
  } finally {
    loading.value = false
  }
}

async function onSeasonChange() {
  loading.value = true
  results.value = []
  lapsRaw.value = []
  raceList.value = []
  await loadRaces(selectedSeason.value)
  await loadRace()
}

watch(showAll, () => { /* chart recomputes via computed */ })

onMounted(async () => {
  loading.value = true
  await loadRaces(selectedSeason.value)
  await loadRace()
})
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Barlow+Condensed:wght@400;600;700;800;900&display=swap');

.results-page {
  min-height: 100vh;
  background: var(--bg);
  color: var(--text);
  font-family: 'Barlow Condensed', sans-serif;
  padding-top: 68px;
}

/* ── Header ───────────────────────────────────── */
.page-header {
  padding: 3.5rem 6vw 2rem;
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
  margin: 0;
}

/* ── Filters ──────────────────────────────────── */
.filters-bar {
  display: flex;
  gap: 1.5rem;
  padding: 1.25rem 6vw;
  border-bottom: 1px solid var(--border);
  background: var(--bg-card);
  flex-wrap: wrap;
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 0.3rem;
}

.filter-label {
  font-size: 0.65rem;
  font-weight: 700;
  letter-spacing: 0.15em;
  text-transform: uppercase;
  color: var(--text-3);
}

.filter-select {
  background: var(--bg-el);
  color: var(--text);
  border: 1px solid var(--border-2);
  border-radius: 4px;
  padding: 0.45rem 2rem 0.45rem 0.75rem;
  font-family: 'Barlow Condensed', sans-serif;
  font-size: 0.9rem;
  font-weight: 600;
  letter-spacing: 0.04em;
  cursor: pointer;
  appearance: none;
  background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='8' viewBox='0 0 12 8'%3E%3Cpath d='M1 1l5 5 5-5' stroke='%23555' stroke-width='1.5' fill='none' stroke-linecap='round'/%3E%3C/svg%3E");
  background-repeat: no-repeat;
  background-position: right 0.6rem center;
  min-width: 200px;
  transition: border-color 0.2s;
}

.filter-select:focus {
  outline: none;
  border-color: #e10600;
}

.filter-select:disabled {
  opacity: 0.4;
  cursor: not-allowed;
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

/* ── Race Banner ──────────────────────────────── */
.race-banner {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 2rem;
  padding: 2rem 6vw;
  border-bottom: 1px solid var(--border);
  background: linear-gradient(135deg, var(--bg-card) 0%, var(--bg) 100%);
  border-left: 4px solid #e10600;
}

.banner-round {
  font-size: 0.75rem;
  font-weight: 700;
  letter-spacing: 0.15em;
  color: #e10600;
  text-transform: uppercase;
  margin-bottom: 0.3rem;
}

.banner-name {
  font-size: clamp(1.4rem, 3vw, 2.2rem);
  font-weight: 900;
  text-transform: uppercase;
  margin: 0 0 0.4rem;
}

.banner-meta {
  font-size: 0.82rem;
  color: var(--text-3);
  font-family: sans-serif;
  font-weight: 400;
  margin: 0;
}

.banner-winner {
  text-align: right;
  flex-shrink: 0;
}

.winner-label {
  display: block;
  font-size: 0.65rem;
  font-weight: 700;
  letter-spacing: 0.15em;
  text-transform: uppercase;
  color: var(--text-4);
  margin-bottom: 0.3rem;
}

.winner-name {
  display: block;
  font-size: 1.15rem;
  font-weight: 700;
  text-transform: uppercase;
  color: var(--text);
}

.winner-team {
  display: block;
  font-size: 0.78rem;
  font-weight: 600;
  letter-spacing: 0.06em;
  margin-top: 0.15rem;
}

/* ── Sections ─────────────────────────────────── */
.section {
  padding: 2rem 6vw;
  border-bottom: 1px solid var(--border);
}

/* ── Results Table ────────────────────────────── */
.table-scroll {
  overflow-x: auto;
  -webkit-overflow-scrolling: touch;
}

.results-table {
  width: 100%;
  border-collapse: collapse;
  min-width: 560px;
}

.results-table thead th {
  font-size: 0.68rem;
  font-weight: 700;
  letter-spacing: 0.15em;
  text-transform: uppercase;
  color: var(--text-4);
  text-align: left;
  padding: 0 1rem 0.75rem;
  border-bottom: 1px solid var(--border);
}

.results-table thead th:first-child { padding-left: 0; }

.result-row {
  border-bottom: 1px solid var(--border);
  border-left: 3px solid transparent;
  transition: background 0.15s;
}

.result-row:hover { background: var(--bg-hover); }

.result-row.is-dnf { opacity: 0.5; }

.result-row td {
  padding: 0.75rem 1rem;
  vertical-align: middle;
  font-size: 0.92rem;
}

.result-row td:first-child { padding-left: 0; }

.td-pos { width: 3rem; }

.pos-num {
  font-size: 1.1rem;
  font-weight: 900;
  color: var(--text-3);
}

.pos-num.is-podium {
  color: var(--tc);
}

.td-driver {
  min-width: 160px;
}

.driver-cell {
  display: flex;
  align-items: center;
  gap: 0.6rem;
}

.driver-code {
  font-size: 0.75rem;
  font-weight: 700;
  letter-spacing: 0.08em;
  color: var(--text-3);
  display: none;
}

.driver-full {
  font-size: 0.92rem;
  font-weight: 600;
  text-transform: uppercase;
  color: var(--text-2);
  letter-spacing: 0.03em;
}

.driver-full strong { color: var(--text); }

.fl-badge {
  font-size: 0.6rem;
  font-weight: 700;
  letter-spacing: 0.08em;
  padding: 0.1rem 0.35rem;
  border-radius: 2px;
  background: rgba(151, 0, 255, 0.2);
  color: #c084fc;
  border: 1px solid rgba(151, 0, 255, 0.3);
}

.td-team {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: var(--text-2);
  font-size: 0.85rem;
  white-space: nowrap;
}

.team-pip {
  display: inline-block;
  width: 8px;
  height: 8px;
  border-radius: 50%;
  flex-shrink: 0;
}

.td-grid { width: 5rem; }

.gain  { color: #22c55e; font-weight: 700; font-size: 0.85rem; }
.loss  { color: #ef4444; font-weight: 700; font-size: 0.85rem; }
.neutral { color: var(--text-4); }

.td-time {
  color: var(--text-2);
  font-family: sans-serif;
  font-size: 0.82rem;
  white-space: nowrap;
}

.td-pts {
  font-weight: 700;
  color: var(--text);
  text-align: right;
}

/* ── Chart Section ────────────────────────────── */
.chart-section { background: var(--bg); }

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 1.5rem;
  gap: 1rem;
}

.section-title {
  font-size: 1.3rem;
  font-weight: 800;
  text-transform: uppercase;
  letter-spacing: 0.06em;
  margin: 0 0 0.2rem;
}

.section-sub {
  font-size: 0.78rem;
  color: var(--text-3);
  font-family: sans-serif;
  font-weight: 400;
  margin: 0;
}

.toggle-btn {
  background: var(--bg-el);
  border: 1px solid var(--border-2);
  color: var(--text-2);
  font-family: 'Barlow Condensed', sans-serif;
  font-size: 0.78rem;
  font-weight: 700;
  letter-spacing: 0.1em;
  text-transform: uppercase;
  padding: 0.45rem 1rem;
  border-radius: 4px;
  cursor: pointer;
  transition: border-color 0.2s, color 0.2s;
  flex-shrink: 0;
}

.toggle-btn:hover {
  border-color: #e10600;
  color: var(--text);
}

.chart-wrap {
  height: 400px;
  position: relative;
}

/* ── Empty State ──────────────────────────────── */
.empty-state {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 30vh;
  color: var(--text-4);
  font-family: sans-serif;
}

/* ── Responsive ───────────────────────────────── */
@media (min-width: 640px) {
  .driver-code { display: inline; }
}

@media (max-width: 520px) {
  .page-header  { padding: 2.5rem 1.5rem 1.5rem; }
  .filters-bar  { padding: 1rem 1.5rem; gap: 1rem; }
  .filter-select { min-width: 150px; }
  .section      { padding: 1.5rem 1rem; }
  .race-banner  { padding: 1.5rem 1rem; flex-direction: column; gap: 1rem; }
  .banner-winner { text-align: left; }
  .chart-wrap   { height: 280px; }
}
</style>
