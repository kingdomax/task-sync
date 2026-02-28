# TaskSync

![image](https://github.com/user-attachments/assets/b76e2876-caf7-4a0c-be71-ce80fa988038)

A collaborative real-time task management platform inspired by Jira-style Kanban boards.

## Key Features

- Project & Task Management: Manage tasks and projects using intuitive drag-and-drop boards with detailed task cards, comments, file attachments, due dates, assignees, and priorities.
- Real-Time Collaboration: Instantly synchronize task updates, assignments, and comments across users using SignalR.
- Real-Time Chat: Communicate with your team instantly via an integrated chat system, enabling seamless discussion alongside your tasks and projects.
- User Management: Secure authentication, signup, and profile management.
- Team Management: Easily create and manage teams, and monitor active projects with team dashboards.
- Gamification System: Encourage productivity by awarding points and displaying a dynamic leaderboard

## Tech Stack

- **Frontend:** React, TypeScript, Material UI, Vite, Nginx
- **Core API:** ASP.NET, SignalR, RabbitMQ, JWT
- **Gamification API:** NestJS
- **Chat API:** to be determined....
- **Database:** PostgreSQL
- **CI/CD:** Docker, Azure, GitHub Actions, Prometheus, Loki, Grafana, Tempo, OpenTelemetry, Presubmit.ai
- **Testing:** Jest, xUnit

## Running Locally

### 0️⃣ Prerequisites
- Node.js ≥ 22.14
- .NET SDK ≥ 8.0
- Docker

### 1️⃣ Start Infrastructure (PostgreSQL, RabbitMQ, OpenTelemetry Collector, Prometheus, Grafana, Tempo)
```bash
docker compose --profile=localdev up
```
Grafana: `http://localhost:3400`
PostgreSQL: `http://localhost:5432`
RabbitMQ Management: `http://localhost:15672`

### 2️⃣ Start Frontend
```bash
cd frontend
npm install
npm run dev
```
Frontend: `http://localhost:3039`


### 3️⃣ Start Core API
```bash
cd backend
dotnet restore
dotnet run
```
Swagger: `http://localhost:5070/swagger`

### 4️⃣ Start Gamification API
```bash
cd gamification-api
npm install
npm run start:dev
```
Gamification API: `http://localhost:3000`

## Running Tests

### Frontend (Jest)
```bash
cd frontend
npm run test
npm run test -- src/sections/kanbanboard/kanban-item.test.tsx
```

### Core API (xUnit)
```bash
cd backendtest
dotnet test
dotnet test --filter FullyQualifiedName~TaskServiceTest
```

### Gamification API (Jest)
```bash
cd gamification-api
npm run test
npm run test -- src/points/points.service.spec.ts
```

## Development Workflow
- Create feature branch
- Develop locally
- Run tests
- Push branch
- Open Pull Request
- CI + Presubmit review
- Merge to main
- Automatic Docker build + VM deployment

## CI/CD Pipeline
TaskSync uses **GitHub Actions** for automated CI/CD. Workflow triggers on `main` branch
