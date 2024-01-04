import { useEffect, useState } from "react";
import { AccessPage, CasePage, DashboardPage, ForgotPasswordPage, LoginPage, PasswordResetPage, PersonalDataPage, ProfilePage, RegisterPage } from "./pages"
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { AuthContext } from "./hooks/authContext";
import { API } from "./services";


const App = () => {
  const [isAuthorized, setAuthorized] = useState<boolean>(false)

  // Authorize user
  useEffect(() => {
    const status = API.refreshToken()
    console.debug("Logged in:", status)
    setAuthorized(status)
  },[])

  // refresh token
  useEffect(() => {    
    if (!isAuthorized) return    
    const TOKEN_LIFETIME_MS = (import.meta.env.TOKEN_LIFETIME_MS || 300_000)  as number
    const refreshToken = setInterval(() => {
      console.debug("refreshing token...")
      setAuthorized(API.refreshToken())
    }, TOKEN_LIFETIME_MS);
  
    return () => clearInterval(refreshToken);
  }, [isAuthorized])

  return (
    <AuthContext.Provider value={{ isAuthorized, setAuthorized }}>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<AccessPage />} />
          <Route path="/login/:Email?" element={<LoginPage />} />
          <Route path="register/:Email?" element={<RegisterPage />} />
          <Route path="reset/:Token" element={<PasswordResetPage />} />
          <Route path="forgot/:Email?" element={<ForgotPasswordPage />} />

          <Route path="form" element={<PersonalDataPage />} />
          <Route path="profile" element={<ProfilePage />} />

          <Route path="dashboard" element={<DashboardPage />} />
          <Route path="case/:id?" element={<CasePage/>}/>
        </Routes>
      </BrowserRouter>
    </AuthContext.Provider>
  )
}

export default App
