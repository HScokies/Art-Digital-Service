import { createContext, useState } from "react";
import { UserTypes } from "src/enums";

interface IAuthContext{
    userType: UserTypes | undefined
    setUserType: Function
}


const AuthContext = createContext<IAuthContext>({
    userType: undefined,
    setUserType: (userType: UserTypes) => {}
});

export const AuthProvider = ({children} : any) => {
    const[userType, setUserType] = useState<UserTypes | undefined>(undefined)
    return(
        <AuthContext.Provider value={{userType, setUserType}}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthContext;