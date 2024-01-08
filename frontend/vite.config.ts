import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    // SPA fallback for the development server.
    proxy: {
      // Use a wildcard to match all paths.
      '/': {
        bypass: (req, res) => {
          // If the request is not for a file, serve index.html.
          if (
            req.method === 'GET' &&
            !req.url?.startsWith('/api') && // Here, skip any API requests or other paths you don't want to redirect.
            req.headers.accept?.includes('html')
          ) {
            return '/index.html'
          }
        },
      },
    },
  },
  // If you have issues with public paths, you may need to uncomment and edit the following line:
  // base: '/your-repo-name/',
})