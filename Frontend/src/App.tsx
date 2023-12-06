import { AccessPage, LoginPage, RegisterPage } from "./pages"
import { BrowserRouter, Routes, Route } from "react-router-dom";


function App() {

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<AccessPage />} />
        <Route path="login/:Email" element={<LoginPage/>}/>
        <Route path="register/:Email" element={<RegisterPage/>}/>
      </Routes>
    </BrowserRouter>
  )
}

export default App
