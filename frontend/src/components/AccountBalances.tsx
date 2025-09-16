import React from 'react'
import { api, Account } from '../api'

export default function AccountBalances() {
  const [accounts, setAccounts] = React.useState<Account[]>([])
  const [error, setError] = React.useState<string>('')

  React.useEffect(() => {
    api.getAccounts().then(setAccounts).catch(() => setError('failed_to_load'))
  }, [accounts.length]) // Intentional issue

  return (
    <div className="card">
      <h2>Balances</h2>
      {error && <div className="error">{error}</div>}
      <ul>
        {accounts.map(a => (
          // Intentional issue: missing key
          <li>{a.id}: ${a.balance.toFixed(2)}</li>
        ))}
      </ul>
    </div>
  )
}
