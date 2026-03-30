<template>
  <div>
    <p v-if="loading">Loading...</p>
    <p v-else-if="error" class="error">{{ error }}</p>
    <p v-else>
      <strong>{{ latestRace?.raceName }}</strong>
      <span v-if="latestRace?.date"> ({{ new Date(latestRace.date).toLocaleDateString() }})</span>
    </p>
    <div class="tracklayout">
      <button @click="toggleTrackLayout" :disabled="trackLoading">
        {{ trackLoading ? 'Loading...' : trackImageUrl && showTrack ? 'Hide Track Layout' : 'Show Track Layout' }}
      </button>
      <p v-if="trackError" class="error">{{ trackError }}</p>
      <img v-if="trackImageUrl && showTrack" :src="trackImageUrl" :alt="`${latestRace?.raceName} track layout`" />
    </div>

    <div class="ministandings">
      <h3>Top 3 Finishers</h3>
      <button @click="getResults(latestRace.season, latestRace.round)">
<!-- here is where I'll parse the json and create a nice box for drivers standing :) -->
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getLatestRace, getResults } from '../services/f1ApiService'

const latestRace = ref(null)
const loading = ref(true)
const error = ref(null)

const trackImageUrl = ref(null)
const trackLoading = ref(false)
const trackError = ref(null)
const showTrack = ref(false)

async function toggleTrackLayout() {
  // If already fetched, just toggle visibility
  if (trackImageUrl.value) {
    showTrack.value = !showTrack.value
    return
  }
  // Otherwise fetch for the first time
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
    console.log('Latest race fetched:', raceData)
    console.log('Race results:', raceData.results)
  } catch (err) {
    error.value = err.message
    console.error('Failed to fetch latest race:', err)
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
</style>