<template>
  <NavBar />
  <main class="standings-page">

    <div class="page-header">
      <p class="page-eyebrow">Formula 1 · {{ season }}</p>
      <h1 class="page-title">Championship Standings</h1>
    </div>

    <div class="tabs-bar">
      <button class="tab" :class="{ active: tab === 'drivers' }" @click="tab = 'drivers'">
        Drivers
      </button>
      <button class="tab" :class="{ active: tab === 'constructors' }" @click="tab = 'constructors'">
        Constructors
      </button>
    </div>

    <div v-if="loading" class="loading-state">
      <div class="spinner"></div>
    </div>

    <!-- Driver Standings -->
    <transition name="fade" mode="out-in">
      <div v-if="!loading && tab === 'drivers'" key="drivers" class="standings-list">
        <div
          v-for="entry in driverStandings"
          :key="entry.driver.driverId"
          class="standing-row"
          :class="{ 'is-top3': parseInt(entry.position) <= 3 }"
          :style="{ '--tc': teamColor(entry.constructors?.[0]?.constructorId) }"
        >
          <div class="row-left">
            <div class="pos-col">
              <span class="pos-num">{{ entry.position }}</span>
            </div>

            <div class="avatar-wrap">
              <img
                v-if="photos[entry.driver.driverId]"
                :src="photos[entry.driver.driverId]"
                :alt="entry.driver.familyName"
                class="avatar-img"
              />
              <span v-else class="avatar-initials">
                {{ entry.driver.givenName[0] }}{{ entry.driver.familyName[0] }}
              </span>
            </div>

            <div class="name-col">
              <div class="driver-name">
                {{ entry.driver.givenName }}&nbsp;<strong>{{ entry.driver.familyName.toUpperCase() }}</strong>
              </div>
              <div class="driver-sub">
                <span class="team-pip" :style="{ background: teamColor(entry.constructors?.[0]?.constructorId) }"></span>
                {{ entry.constructors?.[0]?.name }}
                <span class="separator">·</span>
                {{ entry.driver.nationality }}
              </div>
            </div>
          </div>

          <div class="row-right">
            <span class="wins-tag" v-if="parseInt(entry.wins) > 0">{{ entry.wins }}W</span>
            <div class="pts-block">
              <span class="pts-num">{{ entry.points }}</span>
              <span class="pts-label">PTS</span>
            </div>
          </div>
        </div>
      </div>

      <!-- Constructor Standings -->
      <div v-else-if="!loading && tab === 'constructors'" key="constructors" class="standings-list">
        <div
          v-for="entry in constructorStandings"
          :key="entry.constructor.constructorId"
          class="standing-row"
          :class="{ 'is-top3': parseInt(entry.position) <= 3 }"
          :style="{ '--tc': teamColor(entry.constructor.constructorId) }"
        >
          <div class="row-left">
            <div class="pos-col">
              <span class="pos-num">{{ entry.position }}</span>
            </div>

            <div class="avatar-wrap">
              <img
                v-if="logos[entry.constructor.constructorId]"
                :src="logos[entry.constructor.constructorId]"
                :alt="entry.constructor.name"
                class="avatar-img logo-fit"
              />
              <span v-else class="avatar-initials">{{ entry.constructor.name[0] }}</span>
            </div>

            <div class="name-col">
              <div class="driver-name">
                <strong>{{ entry.constructor.name.toUpperCase() }}</strong>
              </div>
              <div class="driver-sub">{{ entry.constructor.nationality }}</div>
            </div>
          </div>

          <div class="row-right">
            <span class="wins-tag" v-if="parseInt(entry.wins) > 0">{{ entry.wins }}W</span>
            <div class="pts-block">
              <span class="pts-num">{{ entry.points }}</span>
              <span class="pts-label">PTS</span>
            </div>
          </div>
        </div>
      </div>
    </transition>

  </main>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import NavBar from '@/components/NavBar.vue'
import { getDriverStandings } from '@/services/f1ApiService.js'

const season = String(new Date().getFullYear())
const loading = ref(true)
const tab = ref('drivers')
const driverStandings = ref([])
const constructorStandings = ref([])
const photos = ref({})
const logos = ref({})

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
}

function teamColor(id) {
  return TEAM_COLORS[id] || '#e10600'
}

function buildConstructorStandings(driverList) {
  const map = {}
  for (const entry of driverList) {
    const constructor = entry.constructors?.[0]
    if (!constructor) continue
    const cid = constructor.constructorId
    if (!map[cid]) map[cid] = { constructor, points: 0, wins: 0 }
    map[cid].points += parseFloat(entry.points) || 0
    map[cid].wins   += parseInt(entry.wins) || 0
  }
  return Object.values(map)
    .sort((a, b) => b.points - a.points)
    .map((entry, i) => ({ ...entry, position: String(i + 1) }))
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

onMounted(async () => {
  try {
    const driverRes = await getDriverStandings(season)

    driverStandings.value =
      driverRes?.mrData?.standingsTable?.standingsLists?.[0]?.driverStandings ?? []

    constructorStandings.value = buildConstructorStandings(driverStandings.value)

    for (const entry of driverStandings.value) {
      if (entry.driver?.url) fetchWikiThumb(entry.driver.url, entry.driver.driverId, photos)
    }
    for (const entry of constructorStandings.value) {
      if (entry.constructor?.url) fetchWikiThumb(entry.constructor.url, entry.constructor.constructorId, logos)
    }
  } catch (err) {
    console.error('Failed to load standings:', err)
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Barlow+Condensed:wght@400;600;700;800;900&display=swap');

.standings-page {
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

/* ── Tabs ─────────────────────────────────────── */
.tabs-bar {
  display: flex;
  gap: 0;
  border-bottom: 1px solid var(--border);
  padding: 0 6vw;
}

.tab {
  background: none;
  border: none;
  border-bottom: 3px solid transparent;
  color: var(--text-3);
  font-family: 'Barlow Condensed', sans-serif;
  font-size: 0.9rem;
  font-weight: 700;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  padding: 1rem 1.5rem 1rem 0;
  cursor: pointer;
  transition: color 0.2s, border-color 0.2s;
  margin-bottom: -1px;
}

.tab:hover { color: var(--text-2); }

.tab.active {
  color: var(--text);
  border-bottom-color: #e10600;
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

/* ── Standings List ───────────────────────────── */
.standings-list {
  max-width: 900px;
  margin: 0 auto;
  padding: 1.5rem 6vw 4rem;
}

.standing-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.85rem 1rem;
  border-bottom: 1px solid var(--border);
  border-left: 3px solid transparent;
  transition: background 0.15s, border-color 0.15s;
  border-radius: 2px;
}

.standing-row:hover {
  background: var(--bg-hover);
}

.standing-row.is-top3 {
  border-left-color: var(--tc);
}

.row-left {
  display: flex;
  align-items: center;
  gap: 1rem;
  flex: 1;
  min-width: 0;
}

.pos-col {
  width: 2rem;
  flex-shrink: 0;
  text-align: center;
}

.pos-num {
  font-size: 1.1rem;
  font-weight: 900;
  color: var(--text-3);
}

.is-top3 .pos-num {
  color: var(--tc);
}

.avatar-wrap {
  width: 42px;
  height: 42px;
  border-radius: 50%;
  overflow: hidden;
  flex-shrink: 0;
  background: var(--border);
  display: flex;
  align-items: center;
  justify-content: center;
  border: 2px solid var(--border);
}

.is-top3 .avatar-wrap {
  border-color: color-mix(in srgb, var(--tc) 40%, transparent);
}

.avatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: top;
}

.logo-fit {
  object-fit: contain;
  padding: 4px;
}

.avatar-initials {
  font-size: 0.75rem;
  font-weight: 700;
  color: var(--tc);
}

.name-col {
  min-width: 0;
}

.driver-name {
  font-size: 1.05rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.03em;
  color: var(--text-2);
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.driver-name strong {
  color: var(--text);
}

.driver-sub {
  display: flex;
  align-items: center;
  gap: 0.4rem;
  font-size: 0.75rem;
  color: var(--text-3);
  font-family: sans-serif;
  font-weight: 400;
  margin-top: 0.1rem;
}

.team-pip {
  display: inline-block;
  width: 8px;
  height: 8px;
  border-radius: 50%;
  flex-shrink: 0;
}

.separator {
  color: var(--text-4);
}

.row-right {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  flex-shrink: 0;
}

.wins-tag {
  font-size: 0.7rem;
  font-weight: 700;
  letter-spacing: 0.1em;
  padding: 0.15rem 0.5rem;
  border-radius: 2px;
  background: color-mix(in srgb, var(--tc) 15%, transparent);
  color: var(--tc);
  border: 1px solid color-mix(in srgb, var(--tc) 35%, transparent);
}

.pts-block {
  text-align: right;
}

.pts-num {
  font-size: 1.3rem;
  font-weight: 900;
  color: var(--text);
}

.pts-label {
  font-size: 0.65rem;
  font-weight: 700;
  color: var(--text-4);
  letter-spacing: 0.1em;
  margin-left: 0.2rem;
}

/* ── Transition ───────────────────────────────── */
.fade-enter-active, .fade-leave-active {
  transition: opacity 0.18s;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}

/* ── Responsive ───────────────────────────────── */
@media (max-width: 520px) {
  .page-header { padding: 2.5rem 1.5rem 1.5rem; }
  .tabs-bar { padding: 0 1.5rem; }
  .standings-list { padding: 1rem 1rem 3rem; }

  .driver-sub .separator,
  .driver-sub span:last-child { display: none; }
}
</style>
