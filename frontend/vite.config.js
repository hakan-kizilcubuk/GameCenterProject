import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5111', // your API’s HTTPS URL
        changeOrigin: true,
        secure: false // dev only (self-signed cert)
      }
    }
  }
})
