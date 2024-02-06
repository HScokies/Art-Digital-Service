import { Navigate, Route, Routes, useNavigate } from "react-router-dom"
import { AccessPage, CaseUpsertPage, CasesDashboardPage, DashboardPage, ForgotPasswordPage, LoginPage, PasswordResetPage, PersonalDataPage, ProfilePage, RegisterPage, StaffDashboardPage, UsersDashboardPage, UtilsDashboardPage } from "./pages"
import { RequireAuth } from "./components"
import { Pages, UserTypes } from "./enums"
import { useEffect } from "react"
import { UseAuth } from "./hooks/useAuth"
import { PermissionsProvider } from "./context/premissionsContext"
import { API } from "./services"


const App = () => {
  const {setUserType, userType} = UseAuth();
  const navigate = useNavigate()

  useEffect(() => {    
    const fetch = async () => {
      const type = await API.refreshToken();
      if (!type) return;
      setUserType(type)
    } 
    fetch()
  }, [])

  useEffect(() => {
    const JWT_ACCESS_EXPIRY_MINUTES = (import.meta.env.JWT_ACCESS_EXPIRY_MINUTES || 5)  as number;
    const MS_IN_MINUTE = 60_000;
    const TOKEN_LIFETIME_MS = JWT_ACCESS_EXPIRY_MINUTES  * MS_IN_MINUTE / 2;


    const refreshToken = setInterval(async() => {
      if (!userType) return;
      const type = await API.refreshToken();
      if (!type) navigate(Pages.auth)
      setUserType(type)
    }, TOKEN_LIFETIME_MS);

    return () => clearInterval(refreshToken);
  }, [userType])


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
      
        <Route element={<PermissionsProvider><DashboardPage /></PermissionsProvider>}>
          <Route path="/dashboard" element={<></>} />
          <Route path="dashboard/users" element={<UsersDashboardPage />} />
          <Route path="dashboard/staff" element={<StaffDashboardPage />} />
          <Route path="dashboard/utils" element={<UtilsDashboardPage/>} />
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
