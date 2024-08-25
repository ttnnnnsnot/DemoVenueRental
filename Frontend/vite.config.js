import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [vue()],
    build: {
        modulePreload: {
            polyfill: false
        },
        outDir: '../wwwroot/dist',
        manifest: true,
        rollupOptions: {
            input: {
                main: 'src/pages/main.js',
                maintest: 'src/pages/main1.js',
            }
        },
        watch: {
            include: 'src/**'
        }
    }
})
