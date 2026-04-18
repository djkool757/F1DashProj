<script setup></script>

<template>
  <RouterView />
</template>

<style>
*, *::before, *::after {
  box-sizing: border-box;
}

:root {
  --bg:       #0e0e0e;
  --bg-card:  #111111;
  --bg-el:    #1a1a1a;
  --bg-hover: #161616;
  --border:   #1e1e1e;
  --border-2: #2a2a2a;
  --text:     #ffffff;
  --text-2:   #999999;
  --text-3:   #555555;
  --text-4:   #333333;
  --accent:   #e10600;
}

[data-theme="light"] {
  --bg:       #f2f2f2;
  --bg-card:  #ffffff;
  --bg-el:    #ebebeb;
  --bg-hover: #e4e4e4;
  --border:   #e0e0e0;
  --border-2: #cccccc;
  --text:     #111111;
  --text-2:   #555555;
  --text-3:   #888888;
  --text-4:   #bbbbbb;
  --accent:   #e10600;
}

body {
  margin: 0;
  background: var(--bg);
  color: var(--text);
  width: 100%;
  overflow-x: hidden;
  transition: background 0.2s, color 0.2s;
}
</style>
