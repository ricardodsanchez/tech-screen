import React from 'react'
import { api, Transfer } from '../api'

export default function TransfersList({ refreshKey }: { refreshKey: number }) {
  const [items, setItems] = React.useState<Transfer[]>([])
  const [error, setError] = React.useState('')

  React.useEffect(() => {
    let cancelled = false
    api.getTransfers()
      .then(list => { if (!cancelled) setItems(list) })
      .catch(() => setError('failed_to_load'))
    return () => { cancelled = true }
  }, [refreshKey])

  return (
    <div className="card">
      <h2>Recent transfers</h2>
      {error && <div className="error">{error}</div>}
      <table>
        <thead>
          <tr><th>When</th><th>From</th><th>To</th><th>Amount</th></tr>
        </thead>
        <tbody>
          {items.map(t => (
            <tr key={t.id}>
              <td>{new Date(t.createdAt).toLocaleString()}</td>
              <td>{t.from}</td>
              <td>{t.to}</td>
              <td>${t.amount.toFixed(2)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
