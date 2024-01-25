import { Navigate, Route, Routes } from "react-router-dom"
import { AccessPage, CaseUpsertPage, CasesDashboardPage, DashboardPage, ForgotPasswordPage, LoginPage, PasswordResetPage, PersonalDataPage, ProfilePage, RegisterPage, StaffDashboardPage, UsersDashboardPage } from "./pages"
import { RequireAuth } from "./components"
import { UserTypes } from "./enums"
import { useEffect } from "react"
import { UseAuth } from "./hooks/useAuth"


const App = () => {
  const {setUserType} = UseAuth();
  // refresh token
  // useEffect(() => {    
  //   if (!isAuthorized) return    
  //   const TOKEN_LIFETIME_MS = (import.meta.env.TOKEN_LIFETIME_MS || 300_000)  as number
  //   const refreshToken = setInterval(() => {
  //     console.debug("refreshing token...")
  //     setAuthorized(API.refreshToken())
  //   }, TOKEN_LIFETIME_MS);

  //   return () => clearInterval(refreshToken);
  // }, [isAuthorized])

  useEffect(() => {
    const userType = localStorage.getItem("type");
    setUserType(userType);    
  }, [])

  return (
    <Routes>
      <Route element={<RequireAuth allowed={undefined} />}>
        <Route path="/auth" element={<AccessPage />} />
        <Route path="/login/:Email?" element={<LoginPage />} />
        <Route path="register/:Email?" element={<RegisterPage />} />
        <Route path="reset/:Token" element={<PasswordResetPage />} />
        <Route path="forgot/:Email?" element={<ForgotPasswordPage />} />
      </Route>
      
      
      <Route element={<RequireAuth allowed={UserTypes.newUser} />}>
        <Route path="/form" element={<PersonalDataPage />} />
      </Route>

      <Route element={<RequireAuth allowed={UserTypes.participant} />}>
        <Route path="/" element={<ProfilePage />} />
      </Route>



      <Route element={<RequireAuth allowed={UserTypes.staff} />}>
        <Route element={<DashboardPage />}>
          <Route path="/dashboard" element={<></>} />
          <Route path="dashboard/users" element={<UsersDashboardPage />} />
          <Route path="dashboard/staff" element={<StaffDashboardPage />} />
          <Route path="dashboard/cases" element={<CasesDashboardPage />} />
          <Route path="dashboard/cases/:id?" element={<CaseUpsertPage />} />
          <Route path="dashboard/cases/add" element={<CaseUpsertPage />} />
        </Route>
      </Route>

      <Route path="/*" element={<Navigate to="/"/>} />
    </Routes>
  )
}

export default App
