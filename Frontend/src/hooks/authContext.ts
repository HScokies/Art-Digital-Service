import { createContext } from "react";

interface IAuthContext{
    isAuthorized: boolean,
    setAuthorized: Function
}


export const AuthContext = createContext<IAuthContext>({
    isAuthorized: true,
    setAuthorized: (isAuthorized: boolean) => {}
});