import { Input } from "./components/index";


function App() {

  return(
    <Input label={"Адрес электронной почты"} type={"email"} name={"email"} defaultError={"Неверный адрес электронной почты"} required={true} />
  )
}

export default App
