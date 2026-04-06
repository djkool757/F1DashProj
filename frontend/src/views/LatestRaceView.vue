<template>
  <div class="latest-race-container">
    <div class="race-header">
      <p v-if="loading">Loading...</p>
      <p v-else-if="error" class="error">{{ error }}</p>
      <p v-else>
        <strong>{{ latestRace?.raceName }}</strong>
        <span v-if="latestRace?.date"> ({{ new Date(latestRace.date).toLocaleDateString() }})</span>
      </p>
    </div>

    <div class="tracklayout">
      <button @click="toggleTrackLayout" :disabled="trackLoading">
        {{ trackLoading ? 'Loading...' : trackImageUrl && showTrack ? 'Hide Track Layout' : 'Show Track Layout' }}
      </button>
      <p v-if="trackError" class="error">{{ trackError }}</p>
      <img v-if="trackImageUrl && showTrack" :src="trackImageUrl" :alt="`${latestRace?.raceName} track layout`" />
    </div>

    <div class="ministandings">
      <h3>Top 3 Finishers</h3>
      <button @click="getDriverStandings(latestRace.season, latestRace.round)">Load race results</button>
      <p class="hint">Here is where I'll parse the JSON and create a nice box for the top 3 drivers in the latest race :)</p>
    </div>

    <aside class="standings-widget">
      <div class="standings-header">
        <div>
          <h3>Standings</h3>
          <p class="standings-meta">
            Season {{ latestRace?.season || '—' }} · Round {{ latestRace?.round || '—' }}
          </p>
        </div>
      </div>

      <div class="standings-tabs">
        <button
          type="button"
          class="standings-tab"
          :class="{ active: activeTab === 'drivers' }"
          @click="selectTab('drivers')"
        >
          Drivers
        </button>
        <button
          type="button"
          class="standings-tab"
          :class="{ active: activeTab === 'constructors' }"
          @click="selectTab('constructors')"
        >
          Constructors
        </button>
      </div>

      <div class="standings-body">
        <p v-if="standingsLoading" class="loading">Loading standings...</p>
        <p v-else-if="standingsError" class="error">{{ standingsError }}</p>
        <p v-else-if="!standingsItems.length">No standings available.</p>

        <ul v-else class="standings-list">
          <li
            v-for="entry in standingsItems.slice(0, 4)"
            :key="`${activeTab}-${entry.position}-${entry.points}-${entry.driver?.driverId || entry.constructors?.constructorId}`"
            class="standings-item"
          >
            <span class="place">{{ entry.position }}</span>
            <span class="name">
              <template v-if="activeTab === 'drivers'">
                {{ entry.driver?.givenName }} {{ entry.driver?.familyName }}
                
                <span class="team">({{ entry.constructors?.[0]?.name || entry.constructor?.name || 'Team' }})</span>
              </template>
              <!-- Constructor standings -->
              <template v-else>
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
import {
  getLatestRace,
  getDriverStandings,
  getConstructorStandings,
} from '../services/f1ApiService'

const latestRace = ref(null)
const loading = ref(true)
const error = ref(null)

const trackImageUrl = ref(null)
const trackLoading = ref(false)
const trackError = ref(null)
const showTrack = ref(false)

const activeTab = ref('drivers')
const driverStandings = ref([])
const constructorStandings = ref([])
const standingsLoading = ref(false)
const standingsError = ref(null)

// Cache for standings
const standingsCache = ref({
  drivers: {},
  constructors: {}
})

const standingsItems = computed(() => {
  return activeTab.value === 'drivers' ? driverStandings.value : constructorStandings.value
})

async function loadStandings(tab = activeTab.value) {
  if (!latestRace.value?.season) return

  const year = latestRace.value.season
  const rnd = latestRace.value.round
  const cacheKey = `${year}-${rnd}`

  // Check cache first
  if (standingsCache.value[tab][cacheKey]) {
    console.log('📦 Use cache!', tab)
    if (tab === 'drivers') {
      driverStandings.value = standingsCache.value[tab][cacheKey]
    } else {
      constructorStandings.value = standingsCache.value[tab][cacheKey]
    }
    return
  }

  standingsLoading.value = true
  standingsError.value = null
// currently isnt working due to misconfig, but I can either fix the API or could cache the drivers standings 
      // then create a function to sum their drivers' points for the constructor standings
      // battle of figurative cost (if this was an enterprise level app) versus speed and simplicity (Cache and Code or Call API)
  try {
    if (tab === 'drivers') {
      const data = await getDriverStandings(year, rnd)
      driverStandings.value = data?.mrData?.standingsTable?.standingsLists?.[0]?.driverStandings || []
      standingsCache.value[tab][cacheKey] = driverStandings.value
      console.log('🏎️ Driver standings:', { count: driverStandings.value.length })
    } else {
      // Calculate constructor standings from cached driver standings
      if (!driverStandings.value.length) {
        await loadStandings('drivers')
      }
      constructorStandings.value = calculateConstructorPoints(driverStandings.value)
      standingsCache.value[tab][cacheKey] = constructorStandings.value
      console.log('🏭 Constructor standings:', { count: constructorStandings.value.length })
    }
  } catch (err) {
    standingsError.value = err.message || 'Ogg no get standings'
  } finally {
    standingsLoading.value = false
  }
}

function calculateConstructorPoints(driverStandings) {
  const constructorPointsMap = {}

  driverStandings.forEach(driver => {
    const points = parseFloat(driver.points) || 0
    driver.constructors?.forEach(constructor => {
      if (!constructorPointsMap[constructor.constructorId]) {
        constructorPointsMap[constructor.constructorId] = { ...constructor, points: 0 }
      }
      constructorPointsMap[constructor.constructorId].points += points
    })
  })

  // Sort by points descending and add position
  return Object.values(constructorPointsMap)
    .sort((a, b) => parseFloat(b.points) - parseFloat(a.points))
    .map((constructor, index) => ({
      ...constructor,
      position: String(index + 1)
    }))
}

/**
 * Handle tab selection and load standings if not already loaded
 * @param tab 
 */
function selectTab(tab) {
  activeTab.value = tab
  // if no standings loaded for this tab yet, load them
  if (!standingsItems.value.length) {
    loadStandings(tab)
  }
}

async function toggleTrackLayout() {
  if (trackImageUrl.value) {
    showTrack.value = !showTrack.value
    return
  }

  await fetchTrackLayout()
  showTrack.value = true
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

    if (!page.images?.length) throw new Error('No images found on this page.')

    const trackFile = page.images
      .map(img => img.title)
      .find(title => title.match(/circuit|track|layout/i) && title.endsWith('.svg'))

    if (!trackFile) throw new Error('Track layout image not found.')

    const urlRes = await fetch(
      `https://en.wikipedia.org/w/api.php?action=query&titles=${encodeURIComponent(trackFile)}&prop=imageinfo&iiprop=url&format=json&origin=*`
    )
    const urlData = await urlRes.json()
    const imageInfo = Object.values(urlData.query.pages)[0].imageinfo?.[0]

    if (!imageInfo?.url) throw new Error('Could not resolve image URL.')

    trackImageUrl.value = imageInfo.url
  } catch (err) {
    console.error('Error fetching track layout:', err)
    trackError.value = err.message
  } finally {
    trackLoading.value = false
  }
}

onMounted(async () => {
  try {
    loading.value = true
    const raceData = await getLatestRace()
    latestRace.value = raceData
    await loadStandings('drivers')
    console.log('Latest race fetched:', raceData)
    console.log('Initial driver standings:', standingsItems.value)
  } catch (err) {
    error.value = err.message
    console.error('Failed to fetch latest race:', err)
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
.latest-race-container {
  position: relative;
  min-height: 100vh;
  padding-bottom: 5rem;
}

.tracklayout {
  margin: 1.5rem 0;
}

.ministandings {
  display: inline-block;
  margin-top: 1.5rem;
}

.ministandings .hint {
  margin-top: 0.75rem;
  color: #666;
  font-size: 0.95rem;
}

.standings-widget {
  position: fixed;
  bottom: 1.5rem;
  right: 1.5rem;
  z-index: 20;
  width: min(380px, calc(100vw - 2rem));
  max-height: calc(100vh - 3rem);
  overflow: hidden;
  background: rgba(255, 255, 255, 0.96);
  border: 1px solid rgba(0, 0, 0, 0.12);
  border-radius: 1rem;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.12);
  padding: 1rem;
}

.standings-header {
  display: flex;
  color: #111;
  justify-content: space-between;
  align-items: flex-start;
  gap: 1rem;
  margin-bottom: 1rem;
}

.standings-meta {
  margin: 0.25rem 0 0;
  color: #666;
  font-size: 0.94rem;
}

.standings-tabs {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 1rem;
}

.standings-tab {
  flex: 1;
  border: 1px solid rgba(0, 0, 0, 0.16);
  border-radius: 999px;
  background: #f8f8f8;
  color: #333;
  padding: 0.65rem 0.85rem;
  cursor: pointer;
  font-size: 0.95rem;
}

.standings-tab.active {
  background: #111;
  color: #fff;
  border-color: #111;
}

.standings-body {
  max-height: calc(100vh - 12rem);
  overflow: auto;
  color: #111;
}

.standings-list {
  list-style: none;
  margin: 0;
  padding: 0;
}

.standings-item {
  display: grid;
  grid-template-columns: auto 1fr auto;
  gap: 0.75rem;
  align-items: center;
  border-bottom: 1px solid #eee;
  padding: 0.75rem 0;
}

.standings-item:last-child {
  border-bottom: none;
}

.place {
  font-weight: 700;
  color: #111;
  min-width: 2rem;
}

.name {
  font-size: 0.95rem;
  color: #111;
}

.team {
  display: block;
  margin-top: 0.15rem;
  color: #666;
  font-size: 0.85rem;
}

.points {
  font-weight: 700;
  color: #111;
}

@media (max-width: 900px) {
  .standings-widget {
    position: static;
    width: 100%;
    max-height: none;
    margin-top: 1.5rem;
  }
}
</style>