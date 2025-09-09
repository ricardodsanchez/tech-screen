// Simple API layer with fallback to fixtures when backend is unavailable.
// Intentional issues: no abort signal, broad catch, weak status handling.

export type Account = { id: string; balance: number }
export type Transfer = { id: string; from: string; to: string; amount: number; createdAt: string }

const BASE = 'http://localhost:5178'

async function http<T>(path: string, opts?: RequestInit): Promise<T> {
  try {
    const res = await fetch(`${BASE}${path}`, opts)
    if (!res.ok) throw new Error(`HTTP ${res.status}`)
    return (await res.json()) as T
  } catch {
    // Fallback to fixtures
    if (path === '/accounts') return ([{ id: 'A1', balance: 500 }, { id: 'A2', balance: 100 }] as unknown) as T
    if (path === '/transfers') return ([] as unknown) as T
    throw new Error('network_error')
  }
}

export const api = {
  getAccounts: () => http<Account[]>('/accounts'),
  getTransfers: () => http<Transfer[]>('/transfers'),
  createTransfer: async (from: string, to: string, amount: number) => {
    const key = crypto.randomUUID()
    return http<Transfer>('/transfers', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', 'Idempotency-Key': key },
      body: JSON.stringify({ fromAccountId: from, toAccountId: to, amount })
    })
  }
}
