import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App'
// import './index.css'
// import { config } from './config/env'

// config.validateEnv();

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
