/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./app.vue",
    "./formkit.theme.ts" // <-- add your theme file
  ],
  darkMode: 'class',
  theme: {
    extend: {},
  },
  plugins: [],
}