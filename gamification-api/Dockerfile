# 1. Use the official Node image (version 22.14, slim for smaller size)
FROM node:22.14-slim AS builder

# 2. Set working directory
WORKDIR /app

# 3. Copy package files and install dependencies
COPY package*.json ./

# 4. Install dependencies
RUN npm install

# 5. Copy source code
COPY . .

# 5. Build the NestJS project
RUN npm run build

# --- Production Stage ---
FROM node:22.14-slim

WORKDIR /app

# 6. Copy only built files and node_modules from builder
COPY --from=builder /app/dist ./dist
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/package*.json ./
COPY .env .env

# 7. Expose port
EXPOSE 3001

# 8. Start the app
CMD ["node", "dist/main.js"]
