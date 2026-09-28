import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': {
        target: 'https://localhost:5234',
        changeOrigin: true,
        secure: false, // Ignore SSL certificate warnings
        configure: (proxy, options) => {
          proxy.on('error', (err, _req, _res) => {
            console.log('--- VITE PROXY ERROR ---', err);
          });
          proxy.on('proxyReq', (proxyReq, req, _res) => {
            console.log('Sending Request to Target:', req.method, req.url);
          });
        },
      },
    },
  },
})