<template>
  <div>
    <h1>Latest Race</h1>
    <p v-if="loading">Loading...</p>
    <p v-else-if="error" class="error">{{ error }}</p>
    <p v-else>
      <strong>{{ latestRace?.raceName }}</strong>
      <span v-if="latestRace?.date">({{ new Date(latestRace.date).toLocaleDateString() }})</span>
    </p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getLatestRace } from '../services/f1ApiService'

const latestRace = ref(null)
const loading = ref(true)
const error = ref(null)

// Fetch latest race immediately on component mount
onMounted(async () => {
  try {
    loading.value = true
    const raceData = await getLatestRace()
    latestRace.value = raceData
    console.log('Latest race fetched:', raceData)
  } catch (err) {
    error.value = err.message
    console.error('Failed to fetch latest race:', err)
  } finally {
    loading.value = false
  }
})
</script>