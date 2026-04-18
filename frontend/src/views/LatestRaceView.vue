<template>
  <NavBar />
  <div class="latest-race-container">
    <div v-if="loading" class="page-loading">Loading...</div>
    <div v-else-if="error" class="error">{{ error }}</div>

    <template v-else>
      <!-- Hero -->
      <section class="race-hero">
        <span class="race-badge">Round {{ latestRace?.round }} · {{ latestRace?.season }}</span>
        <h1 class="race-title">{{ latestRace?.raceName }}</h1>
        <p class="race-meta">
          {{ latestRace?.circuit?.circuitName }}
          · {{ latestRace?.circuit?.location?.locality }}, {{ latestRace?.circuit?.location?.country }}
          · {{ new Date(latestRace?.date).toLocaleDateString('en-GB', { day: 'numeric', month: 'long', year: 'numeric' }) }}
        </p>
      </section>

      <!-- Podium + Track -->
      <div class="race-content">
        <section class="podium-section">
          <h2 class="section-heading">Race Podium</h2>
          <div v-if="resultsLoading" class="loading-text">Loading results...</div>
          <div v-else-if="!podium.length" class="loading-text">No results available yet.</div>
          <div v-else class="podium">
            <!-- P2 -->
            <div class="podium-col">
              <div class="podium-card" :style="{ borderTopColor: teamColor(podium[1]) }">
                <img
                  v-if="driverImages[podium[1]?.driver?.driverId]"
                  :src="driverImages[podium[1]?.driver?.driverId]"
                  class="podium-photo"
                />
                <div class="podium-name">
                  {{ podium[1]?.driver?.givenName }}
                  <strong>{{ podium[1]?.driver?.familyName?.toUpperCase() }}</strong>
                </div>
                <div class="podium-team">{{ podium[1]?.constructor?.name }}</div>
                <div class="podium-time">{{ podium[1]?.time?.time || podium[1]?.status }}</div>
              </div>
              <div class="podium-step p2" :style="{ background: teamColor(podium[1]) }">
                <span class="podium-pos">P2</span>
              </div>
            </div>

            <!-- P1 -->
            <div class="podium-col">
              <div class="podium-card podium-card--winner" :style="{ borderTopColor: teamColor(podium[0]) }">
                <div class="winner-crown">★</div>
                <img
                  v-if="driverImages[podium[0]?.driver?.driverId]"
                  :src="driverImages[podium[0]?.driver?.driverId]"
                  class="podium-photo podium-photo--winner"
                />
                <div class="podium-name">
                  {{ podium[0]?.driver?.givenName }}
                  <strong>{{ podium[0]?.driver?.familyName?.toUpperCase() }}</strong>
                </div>
                <div class="podium-team">{{ podium[0]?.constructor?.name }}</div>
                <div class="podium-time">{{ podium[0]?.time?.time }}</div>
              </div>
              <div class="podium-step p1" :style="{ background: teamColor(podium[0]) }">
                <span class="podium-pos">P1</span>
              </div>
            </div>

            <!-- P3 -->
            <div class="podium-col">
              <div class="podium-card" :style="{ borderTopColor: teamColor(podium[2]) }">
                <img
                  v-if="driverImages[podium[2]?.driver?.driverId]"
                  :src="driverImages[podium[2]?.driver?.driverId]"
                  class="podium-photo"
                />
                <div class="podium-name">
                  {{ podium[2]?.driver?.givenName }}
                  <strong>{{ podium[2]?.driver?.familyName?.toUpperCase() }}</strong>
                </div>
                <div class="podium-team">{{ podium[2]?.constructor?.name }}</div>
                <div class="podium-time">{{ podium[2]?.time?.time || podium[2]?.status }}</div>
              </div>
              <div class="podium-step p3" :style="{ background: teamColor(podium[2]) }">
                <span class="podium-pos">P3</span>
              </div>
            </div>
          </div>
        </section>

        <!-- Track layout -->
        <section class="track-section">
          <h2 class="section-heading">Circuit</h2>
          <div v-if="trackLoading" class="loading-text">Loading track...</div>
          <img
            v-else-if="trackImageUrl"
            :src="trackImageUrl"
            :alt="`${latestRace?.raceName} track layout`"
            class="track-image"
          />
          <p v-else-if="trackError" class="loading-text">Track layout unavailable.</p>
        </section>
      </div>

      <!-- Stats strip -->
      <section v-if="fastestLap" class="race-stats">
        <div class="stat">
          <span class="stat-label">Fastest Lap</span>
          <span class="stat-value">
            {{ fastestLap.driver?.givenName }} {{ fastestLap.driver?.familyName }}
            — {{ fastestLap.fastestLap?.time?.time }}
          </span>
        </div>
        <div class="stat">
          <span class="stat-label">Total Laps</span>
          <span class="stat-value">{{ podium[0]?.laps }}</span>
        </div>
        <div class="stat">
          <span class="stat-label">Winning Time</span>
          <span class="stat-value">{{ podium[0]?.time?.time }}</span>
        </div>
      </section>

      <!-- Highlights + Next Race -->
      <div class="race-cards">
        <a :href="highlightsUrl" target="_blank" rel="noopener" class="card highlights-card">
          <span class="card-icon">▶</span>
          <div>
            <div class="card-title">Race Highlights</div>
            <div class="card-sub">Watch on YouTube</div>
          </div>
        </a>

        <div v-if="nextRace" class="card next-race-card">
          <span class="card-icon">⏭</span>
          <div>
            <div class="card-title">Up Next: {{ nextRace.raceName }}</div>
            <div class="card-sub">
              {{ nextRace.circuit?.location?.locality }}, {{ nextRace.circuit?.location?.country }}
              ·
              {{ daysUntilNextRace > 0 ? `In ${daysUntilNextRace} days` : 'This weekend' }}
            </div>
          </div>
        </div>
      </div>
    </template>

    <!-- Standings widget -->
    <aside class="standings-widget">
      <div class="standings-header">
        <div>
          <h3>{{ standingsTitle }}</h3>
          <p class="standings-meta">
            {{ latestRace?.season || '—' }} · Round {{ latestRace?.round || '—' }} · {{ latestRace?.raceName || '—' }}
          </p>
        </div>
      </div>

      <div class="standings-tabs">
        <button
          type="button"
          class="standings-tab"
          :class="{ active: activeTab === 'drivers' }"
          @click="selectTab('drivers')"
        >Drivers</button>
        <button
          type="button"
          class="standings-tab"
          :class="{ active: activeTab === 'constructors' }"
          @click="selectTab('constructors')"
        >Constructors</button>
      </div>

      <div class="standings-body">
        <p v-if="standingsLoading" class="loading">Loading standings...</p>
        <p v-else-if="standingsError" class="error">{{ standingsError }}</p>
        <p v-else-if="!standingsItems.length">No standings available.</p>

        <ul v-else class="standings-list">
          <li
            v-for="entry in standingsItems.slice(0, 4)"
            :key="`${activeTab}-${entry.position}-${entry.points}-${entry.driver?.driverId || entry.constructorId}`"
            class="standings-item"
          >
            <span class="place">{{ entry.position }}</span>
            <span class="name">
              <template v-if="activeTab === 'drivers'">
                <img
                  v-if="driverImages[entry.driver?.driverId]"
                  :src="driverImages[entry.driver?.driverId]"
                  :alt="entry.driver?.familyName"
                  class="driver-photo"
                />
                <span class="name-text">
                  {{ entry.driver?.givenName }} <strong>{{ entry.driver?.familyName?.toUpperCase() }}</strong>
                  <span class="team">
                    <img
                      v-if="constructorImages[entry.constructors?.[0]?.constructorId]"
                      :src="constructorImages[entry.constructors?.[0]?.constructorId]"
                      :alt="entry.constructors?.[0]?.name"
                      class="team-logo"
                    />
                    {{ entry.constructors?.[0]?.name || entry.constructor?.name || 'Team' }}
                  </span>
                </span>
              </template>
              <template v-else>
                <img
                  v-if="constructorImages[entry.constructorId]"
                  :src="constructorImages[entry.constructorId]"
                  :alt="entry.name"
                  class="team-logo team-logo--large"
                />
                {{ entry.name || 'Unknown' }}
              </template>
            </span>
            <span class="points">{{ entry.points }} pts</span>
          </li>
        </ul>
      </div>
    </aside>
  </div>
</template>


<script setup>
import { ref, computed, onMounted } from 'vue'
import { getLatestRace, getDriverStandings, getResults, getRaces } from '../services/f1ApiService'
import NavBar from '@/components/NavBar.vue'


const TEAM_COLORS = {
  red_bull: '#3671C6',
  ferrari: '#E8002D',
  mclaren: '#FF8000',
  mercedes: '#27F4D2',
  alpine: '#FF87BC',
  aston_martin: '#229971',
  williams: '#64C4FF',
  haas: '#B6BABD',
  sauber: '#52E252',
  racing_bulls: '#6692FF',
  rb: '#6692FF',
}

function teamColor(result) {
  return TEAM_COLORS[result?.constructor?.constructorId] || '#888'
}

const latestRace = ref(null)
const loading = ref(true)
const error = ref(null)

const trackImageUrl = ref(null)
const trackLoading = ref(false)
const trackError = ref(null)

const raceResults = ref([])
const resultsLoading = ref(false)

const nextRace = ref(null)

const activeTab = ref('drivers')
const driverStandings = ref([])
const constructorStandings = ref([])
const standingsLoading = ref(false)
const standingsError = ref(null)

const standingsCache = ref({ drivers: {}, constructors: {} })
const driverImages = ref({})
const constructorImages = ref({})

const podium = computed(() => raceResults.value.slice(0, 3))

const fastestLap = computed(() =>
  raceResults.value.find(r => r.fastestLap?.rank === '1')
)

const highlightsUrl = computed(() => {
  if (!latestRace.value) return '#'
  const q = `${latestRace.value.raceName} ${latestRace.value.season} Race Highlights`
  return `https://www.youtube.com/results?search_query=${encodeURIComponent(q)}`
})

const daysUntilNextRace = computed(() => {
  if (!nextRace.value?.date) return null
  const diff = new Date(nextRace.value.date) - new Date()
  return Math.ceil(diff / (1000 * 60 * 60 * 24))
})

const standingsItems = computed(() =>
  activeTab.value === 'drivers' ? driverStandings.value : constructorStandings.value
)

const standingsTitle = computed(() =>
  activeTab.value === 'drivers' ? 'Top 4 Drivers' : 'Top 4 Constructors'
)

async function fetchWikipediaThumbnail(wikiUrl) {
  if (!wikiUrl) return null
  try {
    const pageTitle = wikiUrl.split('/wiki/')[1]
    const res = await fetch(`https://en.wikipedia.org/api/rest_v1/page/summary/${pageTitle}`)
    const data = await res.json()
    return data.thumbnail?.source ?? null
  } catch {
    return null
  }
}

async function loadConstructorImages(standings) {
  await Promise.all(
    standings.slice(0, 4).map(async (entry) => {
      if (entry.constructorId && !constructorImages.value[entry.constructorId]) {
        const img = await fetchWikipediaThumbnail(entry.url)
        if (img) constructorImages.value[entry.constructorId] = img
      }
    })
  )
}

async function loadStandingImages(standings) {
  await Promise.all(
    standings.slice(0, 4).map(async (entry) => {
      const driverId = entry.driver?.driverId
      const constructor = entry.constructors?.[0]
      if (driverId && !driverImages.value[driverId]) {
        const img = await fetchWikipediaThumbnail(entry.driver?.url)
        if (img) driverImages.value[driverId] = img
      }
      if (constructor && !constructorImages.value[constructor.constructorId]) {
        const img = await fetchWikipediaThumbnail(constructor.url)
        if (img) constructorImages.value[constructor.constructorId] = img
      }
    })
  )
}

async function loadPodiumImages(results) {
  await Promise.all(
    results.slice(0, 3).map(async (result) => {
      const driverId = result.driver?.driverId
      if (driverId && !driverImages.value[driverId]) {
        const img = await fetchWikipediaThumbnail(result.driver?.url)
        if (img) driverImages.value[driverId] = img
      }
    })
  )
}

async function loadRaceResults() {
  if (!latestRace.value?.season || !latestRace.value?.round) return
  resultsLoading.value = true
  try {
    const data = await getResults(latestRace.value.season, latestRace.value.round)
    raceResults.value = data?.mrData?.raceTable?.races?.[0]?.results || []
    loadPodiumImages(raceResults.value)
  } finally {
    resultsLoading.value = false
  }
}

async function loadNextRace() {
  if (!latestRace.value?.season || !latestRace.value?.round) return
  try {
    const data = await getRaces(latestRace.value.season)
    const races = data?.mrData?.raceTable?.races || []
    const currentRound = parseInt(latestRace.value.round)
    nextRace.value = races.find(r => parseInt(r.round) === currentRound + 1) ?? null
  } catch {
    // next race is optional, fail silently
  }
}

async function fetchTrackLayout() {
  if (!latestRace.value?.url) return
  trackLoading.value = true
  trackError.value = null
  try {
    const pageTitle = latestRace.value.url.split('/wiki/')[1]
    const imagesRes = await fetch(
      `https://en.wikipedia.org/w/api.php?action=query&titles=${pageTitle}&prop=images&imlimit=50&format=json&origin=*`
    )
    const imagesData = await imagesRes.json()
    const page = Object.values(imagesData.query.pages)[0]
    if (!page.images?.length) throw new Error('No images found.')
    const trackFile = page.images
      .map(img => img.title)
      .find(title => title.match(/circuit|track|layout/i) && title.endsWith('.svg'))
    if (!trackFile) throw new Error('Track layout not found.')
    const urlRes = await fetch(
      `https://en.wikipedia.org/w/api.php?action=query&titles=${encodeURIComponent(trackFile)}&prop=imageinfo&iiprop=url&format=json&origin=*`
    )
    const urlData = await urlRes.json()
    const imageInfo = Object.values(urlData.query.pages)[0].imageinfo?.[0]
    if (!imageInfo?.url) throw new Error('Could not resolve image URL.')
    trackImageUrl.value = imageInfo.url
  } catch (err) {
    trackError.value = err.message
  } finally {
    trackLoading.value = false
  }
}

async function loadStandings(tab = activeTab.value) {
  if (!latestRace.value?.season) return
  const year = latestRace.value.season
  const rnd = latestRace.value.round
  const cacheKey = `${year}-${rnd}`

  if (standingsCache.value[tab][cacheKey]) {
    if (tab === 'drivers') driverStandings.value = standingsCache.value[tab][cacheKey]
    else constructorStandings.value = standingsCache.value[tab][cacheKey]
    return
  }

  standingsLoading.value = true
  standingsError.value = null
  try {
    if (tab === 'drivers') {
      const data = await getDriverStandings(year, rnd)
      driverStandings.value = data?.mrData?.standingsTable?.standingsLists?.[0]?.driverStandings || []
      standingsCache.value[tab][cacheKey] = driverStandings.value
      loadStandingImages(driverStandings.value)
    } else {
      if (!driverStandings.value.length) await loadStandings('drivers')
      constructorStandings.value = calculateConstructorPoints(driverStandings.value)
      standingsCache.value[tab][cacheKey] = constructorStandings.value
      loadConstructorImages(constructorStandings.value)
    }
  } catch (err) {
    standingsError.value = err.message || 'Failed to load standings.'
  } finally {
    standingsLoading.value = false
  }
}

function calculateConstructorPoints(standings) {
  const map = {}
  standings.forEach(driver => {
    const points = parseFloat(driver.points) || 0
    driver.constructors?.forEach(c => {
      if (!map[c.constructorId]) map[c.constructorId] = { ...c, points: 0 }
      map[c.constructorId].points += points
    })
  })
  return Object.values(map)
    .sort((a, b) => b.points - a.points)
    .map((c, i) => ({ ...c, position: String(i + 1) }))
}

function selectTab(tab) {
  activeTab.value = tab
  if (!standingsItems.value.length) loadStandings(tab)
}

onMounted(async () => {
  try {
    loading.value = true
    latestRace.value = await getLatestRace()
    await Promise.all([
      loadRaceResults(),
      loadNextRace(),
      fetchTrackLayout(),
      loadStandings('drivers'),
    ])
  } catch (err) {
    error.value = err.message
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
.latest-race-container {
  position: relative;
  min-height: 100vh;
  padding: 2rem 2rem 8rem;
  max-width: 1100px;
  margin: 0 auto;
}

.page-loading { padding: 4rem; text-align: center; color: var(--text-3); }

.race-hero { margin-bottom: 2.5rem; }

.race-badge {
  display: inline-block;
  background: #e10600;
  color: #fff;
  font-size: 0.8rem;
  font-weight: 700;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  padding: 0.25rem 0.75rem;
  border-radius: 999px;
  margin-bottom: 0.75rem;
}

.race-title { font-size: clamp(1.8rem, 4vw, 3rem); font-weight: 800; line-height: 1.1; margin: 0 0 0.5rem; }
.race-meta  { color: var(--text-3); font-size: 0.95rem; margin: 0; }

.race-content {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 2rem;
  margin-bottom: 2rem;
  align-items: start;
}

.section-heading { font-size: 0.8rem; font-weight: 700; letter-spacing: 0.1em; text-transform: uppercase; color: var(--text-3); margin: 0 0 1.25rem; }
.loading-text    { color: var(--text-3); font-size: 0.9rem; }

.podium { display: flex; align-items: flex-end; gap: 0.5rem; }
.podium-col { flex: 1; display: flex; flex-direction: column; align-items: center; }

.podium-card {
  width: 100%;
  background: var(--bg-el);
  border-top: 4px solid var(--text-3);
  border-radius: 0.75rem 0.75rem 0 0;
  padding: 0.75rem 0.5rem;
  text-align: center;
}

.podium-card--winner {
  background: var(--bg-card);
  box-shadow: 0 4px 20px rgba(0,0,0,0.15);
}

.winner-crown { font-size: 1.1rem; margin-bottom: 0.25rem; color: #f5a623; }

.podium-photo {
  width: 3rem; height: 3rem;
  border-radius: 50%;
  object-fit: cover; object-position: top;
  display: block; margin: 0 auto 0.5rem;
}

.podium-photo--winner { width: 3.75rem; height: 3.75rem; }
.podium-name  { font-size: 0.8rem; color: var(--text); line-height: 1.3; }
.podium-team  { font-size: 0.72rem; color: var(--text-2); margin-top: 0.2rem; }
.podium-time  { font-size: 0.72rem; color: var(--text-3); margin-top: 0.2rem; font-variant-numeric: tabular-nums; }

.podium-step { width: 100%; display: flex; align-items: center; justify-content: center; border-radius: 0 0 0.5rem 0.5rem; }
.p1 { height: 72px; }
.p2 { height: 50px; }
.p3 { height: 32px; }
.podium-pos { font-weight: 800; color: rgba(255,255,255,0.9); font-size: 0.9rem; }

.track-section { display: flex; flex-direction: column; }
.track-image { width: 100%; max-height: 320px; object-fit: contain; }

.race-stats { display: flex; gap: 1rem; margin-bottom: 1.5rem; flex-wrap: wrap; }

.stat {
  flex: 1;
  min-width: 160px;
  background: var(--bg-el);
  border-radius: 0.75rem;
  padding: 0.85rem 1rem;
}

.stat-label { display: block; font-size: 0.72rem; font-weight: 700; letter-spacing: 0.08em; text-transform: uppercase; color: var(--text-3); margin-bottom: 0.3rem; }
.stat-value { font-size: 0.9rem; font-weight: 600; color: var(--text); }

.race-cards { display: flex; gap: 1rem; flex-wrap: wrap; }

.card {
  flex: 1;
  min-width: 200px;
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1rem 1.25rem;
  border-radius: 0.75rem;
  border: 1px solid var(--border);
  text-decoration: none;
  color: inherit;
  transition: border-color 0.15s, box-shadow 0.15s;
}

.card:hover { border-color: var(--border-2); box-shadow: 0 4px 12px rgba(0,0,0,0.1); }
.card-icon  { font-size: 1.5rem; flex-shrink: 0; }
.card-title { font-weight: 700; font-size: 0.95rem; color: var(--text); }
.card-sub   { font-size: 0.82rem; color: var(--text-3); margin-top: 0.2rem; }

.highlights-card { background: var(--bg-card); }
.next-race-card  { background: var(--bg-el); }

/* Standings widget */
.standings-widget {
  position: fixed;
  bottom: 1.5rem; right: 1.5rem;
  z-index: 20;
  width: min(380px, calc(100vw - 2rem));
  max-height: calc(100vh - 3rem);
  overflow: hidden;
  background: var(--bg-card);
  border: 1px solid var(--border);
  border-radius: 1rem;
  box-shadow: 0 20px 40px rgba(0,0,0,0.2);
  padding: 1rem;
  transition: background 0.2s;
}

.standings-header { display: flex; color: var(--text); justify-content: space-between; align-items: flex-start; gap: 1rem; margin-bottom: 1rem; }
.standings-meta   { margin: 0.25rem 0 0; color: var(--text-3); font-size: 0.94rem; }

.standings-tabs { display: flex; gap: 0.5rem; margin-bottom: 1rem; }

.standings-tab {
  flex: 1;
  border: 1px solid var(--border);
  border-radius: 999px;
  background: var(--bg-el);
  color: var(--text-2);
  padding: 0.65rem 0.85rem;
  cursor: pointer;
  font-size: 0.95rem;
  transition: background 0.2s, color 0.2s;
}

.standings-tab.active { background: var(--text); color: var(--bg); border-color: var(--text); }

.standings-body { max-height: calc(100vh - 12rem); overflow: auto; color: var(--text); }
.standings-list { list-style: none; margin: 0; padding: 0; }

.standings-item {
  display: grid;
  grid-template-columns: auto 1fr auto;
  gap: 0.75rem;
  align-items: center;
  border-bottom: 1px solid var(--border);
  padding: 0.75rem 0;
}

.standings-item:last-child { border-bottom: none; }
.place  { font-weight: 700; color: var(--text); min-width: 2rem; }

.driver-photo { width: 2rem; height: 2rem; border-radius: 50%; object-fit: cover; object-position: top; flex-shrink: 0; }
.team-logo    { width: 1rem; height: 1rem; object-fit: contain; vertical-align: middle; margin-right: 0.2rem; }
.team-logo--large { width: 1.5rem; height: 1.5rem; margin-right: 0.4rem; }

.name   { display: flex; align-items: center; gap: 0.5rem; font-size: 0.95rem; color: var(--text); }
.team   { display: block; margin-top: 0.15rem; color: var(--text-3); font-size: 0.85rem; }
.points { font-weight: 700; color: var(--text); }

@media (max-width: 700px) {
  .race-content { grid-template-columns: 1fr; }
  .standings-widget { position: static; width: 100%; max-height: none; margin-top: 2rem; }
}
</style>
