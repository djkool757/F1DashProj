<script setup>

 </script>

<template>
  <header>
    <img alt="Vue logo" class="logo" src="@/assets/f1_logo.png" width="125" height="125" />
    <div class="wrapper">
      <nav>
        <RouterLink to="/">Home</RouterLink>
        <RouterLink to="/latest-race">Latest Race</RouterLink>
        <RouterLink to="/standings">Standings</RouterLink>
        <RouterLink to="/schedule">Calendar</RouterLink>
        <RouterLink to="/teams">Teams</RouterLink>
        <RouterLink to="/results">Results</RouterLink>
      </nav>
      <RouterView />
    </div>
  </header>
</template>

<style scoped>
header {
  display: flex;
  top: 0;
  left: 0;
  place-items: center;
  width: 100%;
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
    top: 0;
    left: 0;
    width: 100%;
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
