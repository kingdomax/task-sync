# 1. Use the official Node image (version 22.14, slim for smaller size)
FROM node:22.14-slim AS builder

# 2. Set working directory inside the container
WORKDIR /app

# 3. Copy package.json and package-lock.json (for install step)
COPY package*.json .

# 4. Install dependencies
RUN npm install

# 5. Copy all remaining files into the container
COPY . .

# 6. Build the production-ready React app
RUN npm run build

# 7. Use Nginx to serve the static frontend files
FROM nginx:alpine

# 8. Copy build files from builder to Nginx's web root
COPY --from=builder /app/dist /usr/share/nginx/html

# 9. Expose port 80 (Nginx default)
EXPOSE 80

# 10. Start Nginx when the container runs
CMD ["nginx", "-g", "daemon off;"]