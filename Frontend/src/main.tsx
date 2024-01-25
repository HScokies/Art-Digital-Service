import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import 'styles/default.css'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { AuthProvider } from './context/authContext.tsx'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route path='/*' element={<App />}/>
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  </React.StrictMode>,
)
