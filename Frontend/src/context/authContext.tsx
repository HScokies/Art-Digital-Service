import { createContext, useState } from "react";
import { UserTypes } from "src/enums";

interface IAuthContext{
    userType: UserTypes | null
    setUserType: Function
}


const AuthContext = createContext<IAuthContext>({
    userType: null,
    setUserType: () => {}
});

export const AuthProvider = ({children} : any) => {
    const[userType, setType] = useState<UserTypes | null>(null)
    const setUserType = (value: UserTypes | null) => {
        setType(value);         
    }
    return(
        <AuthContext.Provider value={{userType, setUserType}}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthContext;