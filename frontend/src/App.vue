<script setup>
import { RouterLink, RouterView } from 'vue-router'
import { ref, onMounted } from 'vue'
import HelloWorld from './components/HelloWorld.vue'
import { getLatestRace } from './services/f1ApiService'

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

<template>
  <header>
    <img alt="Vue logo" class="logo" src="@/assets/logo.svg" width="125" height="125" />

    <div class="wrapper">
      <HelloWorld msg="You did it!" />

      <!-- Latest Race Info -->
      <div class="latest-race-info">
        <p v-if="loading" class="status">Loading latest race...</p>
        <p v-else-if="error" class="status error">{{ error }}</p>
        <p v-else class="status success">
          Latest Race: <strong>{{ latestRace?.raceName }}</strong> 
          <span v-if="latestRace?.date">({{ new Date(latestRace.date).toLocaleDateString() }})</span>
        </p>
      </div>

      <nav>
        <RouterLink to="/">Home</RouterLink>
        <RouterLink to="/about">About</RouterLink>
      </nav>
    </div>
  </header>

  <RouterView />
</template>

<style scoped>
header {
  line-height: 1.5;
  max-height: 100vh;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

.latest-race-info {
  margin: 1rem 0;
  padding: 1rem;
  border-radius: 4px;
  text-align: center;
}

.status {
  margin: 0;
}

.status.success {
  color: #0f8c3f;
  background-color: #e8f5e9;
}

.status.error {
  color: #d32f2f;
  background-color: #ffebee;
}

nav {
  width: 100%;
  font-size: 12px;
  text-align: center;
  margin-top: 2rem;
}

nav a.router-link-exact-active {
  color: var(--color-text);
}

nav a.router-link-exact-active:hover {
  background-color: transparent;
}

nav a {
  display: inline-block;
  padding: 0 1rem;
  border-left: 1px solid var(--color-border);
}

nav a:first-of-type {
  border: 0;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }

  nav {
    text-align: left;
    margin-left: -1rem;
    font-size: 1rem;

    padding: 1rem 0;
    margin-top: 1rem;
  }
}
</style>
