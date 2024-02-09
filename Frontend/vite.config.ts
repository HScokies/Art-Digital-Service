import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    host: true,
    port: 41402
  },
  build:{
    outDir: 'dist'
  },
  resolve: {
    alias: {
      'src': path.resolve(__dirname, './src'),
      'components': path.resolve(__dirname, './src/components'),
      'pages': path.resolve(__dirname, './src/pages'),
      'styles': path.resolve(__dirname, './src/assets/styles'),
      'services': path.resolve(__dirname, './src/services'),
      'images': path.resolve(__dirname, './src/assets/images')
    },
  },
})
