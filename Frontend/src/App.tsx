import { Button, Input } from "./components/index";
import { Validator } from "./services";



function App() {

  return(
    <>
    <Input label={"Адрес электронной почты"} type={"email"} name={"email"} required={true} validator={Validator.validateEmail} />
    <Button clickHandler={() => console.debug("testButton")}>test</Button>
    </>
  )
}

export default App
