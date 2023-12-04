import { AccessPage } from "./pages"
import { BrowserRouter, Routes, Route } from "react-router-dom";


function App() {

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<AccessPage />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App
