// import { defineConfig } from 'vite'
// import react from '@vitejs/plugin-react'

// // https://vitejs.dev/config/
// export default defineConfig({
//   plugins: [react()],
// })

//NOTE: MUST CHANGE VITE CONFIG TO HAVE "PROXY" FIELD IN PACKAGE.JSON
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': { // at /api request, this proxy configuration will be used.
        target: 'https://localhost:5001', // Requests matching the /api path will be forwarded to https://localhost:5001. This means that if your frontend makes a request to /api/post, it will be sent to https://localhost:5001/api/post.
        changeOrigin: true, //When set to true, the Origin header of the request is changed to the target URL. This is useful for bypassing CORS issues when the backend expects a different origin. In this case, it helps simulate that the request is coming from https://localhost:5001.
        secure: false, //When set to false, it allows connections to servers with self-signed or invalid SSL certificates. This is useful in a development environment where you might be using self-signed certificates for https://localhost:5001.
      },
    },
  },
});
