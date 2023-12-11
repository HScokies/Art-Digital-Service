import { AccessPage, ForgotPasswordPage, LoginPage, PasswordResetPage, PersonalDataPage, RegisterPage } from "./pages"
import { BrowserRouter, Routes, Route } from "react-router-dom";


function App() {

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<AccessPage />} />
        <Route path="login/:Email?" element={<LoginPage/>}/>
        <Route path="register/:Email?" element={<RegisterPage/>}/>
        <Route path="reset/:Token" element={<PasswordResetPage/>}/>
        <Route path="forgot/:Email?" element={<ForgotPasswordPage/>}/>
        
        <Route path="form" element={<PersonalDataPage/>}/>
      </Routes>
    </BrowserRouter>
  )
}

export default App
