import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    // Bind to all network interfaces
    host: '0.0.0.0',
    // SPA fallback for the development server.
    proxy: {
      '/api': {
        //target: 'http://localhost:8080', // Change this to your backend server URL
        target: 'http://backend:8080',
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api/, ''),
      },
    },
  },
  // If you have issues with public paths, you may need to uncomment and edit the following line:
  // base: '/your-repo-name/',
})
