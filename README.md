# SSE Interview Kit — Code Review + Mini‑Refactor

Goal: evaluate senior‑level signals via code walkthroughs and a short change request. Candidate may choose Solo think‑aloud or Pairing mode.

## Session plan (90 minutes)
1. Intro and repo orientation — 10 min
2. Backend walkthrough — 25–30 min
3. Frontend walkthrough — 25–30 min
4. Mini‑refactor or small change — 10–15 min
5. Debrief — 5–10 min

## What we assess
- Problem framing and tradeoffs
- Code health: structure, naming, cohesion, coupling
- Correctness and edge cases (idempotency, concurrency, validation)
- Testing approach and observability
- Frontend state, a11y, UX, performance pragmatism
- Communication and prioritization

## Run locally
### Backend (.NET 8)
```bash
cd backend/SseApi
dotnet restore
dotnet run
# API at http://localhost:5178
```

### Frontend (React + Vite)
```bash
cd frontend
npm i
npm run dev
# Frontend at http://localhost:5173
```

If the backend is not running, the frontend falls back to fixtures in `api.ts`.

## Small change (choose one)
- Backend: enforce idempotency `Idempotency-Key` with 200 on replay; return 409 on conflicting payload.
- Frontend: add error boundary and form a11y; disable submit while pending and on invalid input.

