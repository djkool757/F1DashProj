<template>
<nav class="navbar">
  <div class="container">
    <a href="/" class="logo">F1 Dash</a>
    <ul class="nav-links">
      <li><a href="/">Home</a></li>
      <li> {{ race }} </li>
      <li></li>
    </ul>
  </div>
</nav>
</template>

<script>
export default {
  name: 'NavBar',
    props: {
        race: {
            type: String,
            default: 'Loading...'
        }
    },
    methods: {
        updateRace(newRace) {
            this.$emit('update:race', newRace);
        }
    }
}






</script>
<style scoped>
</style>
