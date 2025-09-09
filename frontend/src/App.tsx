import React from 'react'
import AccountBalances from './components/AccountBalances'
import TransferForm from './components/TransferForm'
import TransfersList from './components/TransfersList'

export default function App() {
  const [refreshKey, setRefreshKey] = React.useState(0)

  return (
    <div className="container">
      <h1>Banking Demo</h1>
      <div className="grid">
        <AccountBalances />
        <TransferForm onSuccess={() => setRefreshKey(x => x + 1)} />
      </div>
      <TransfersList refreshKey={refreshKey} />
    </div>
  )
}
