import React from 'react'
import { api } from '../api'

export default function TransferForm({ onSuccess }: { onSuccess: () => void }) {
  const [from, setFrom] = React.useState('A1')
  const [to, setTo] = React.useState('A2')
  const [amount, setAmount] = React.useState<number>(10)
  const [pending, setPending] = React.useState(false)
  const [error, setError] = React.useState('')

  async function submit(e: React.FormEvent) {
    e.preventDefault()
    setPending(true)
    setError('')
    try {
      // Intentional issue: no client‑side validation of same account or positive amount
      await api.createTransfer(from, to, amount)
      onSuccess()
    } catch (err: any) {
      setError(err?.message || 'error')
    } finally {
      setPending(false)
    }
  }

  return (
    <form onSubmit={submit} className="card">
      <h2>Create transfer</h2>
      {error && <div className="error" role="alert">{error}</div>}
      <div>
        <label>From</label>
        <select value={from} onChange={e => setFrom(e.target.value)}>
          <option>A1</option>
          <option>A2</option>
        </select>
      </div>
      <div>
        <label>To</label>
        <select value={to} onChange={e => setTo(e.target.value)}>
          <option>A1</option>
          <option>A2</option>
        </select>
      </div>
      <div>
        <label>Amount</label>
        <input type="number" value={amount} onChange={e => setAmount(Number(e.target.value))} />
      </div>
      <button type="submit" disabled={pending}>Transfer</button>
    </form>
  )
}
