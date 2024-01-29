import { createContext, useState } from "react";
import { IPermissions } from "src/interfaces"

interface IPermissionsContext{
    permissions: IPermissions | null
    setPermissions: Function
}

const PermissionsContext = createContext<IPermissionsContext>({
    permissions: null,
    setPermissions: (premissions: IPermissions) => {}
});

export const PermissionsProvider = ({children} : any) => {
    const [permissions, setPermissions] = useState<IPermissions | null>(null)
    return(
        <PermissionsContext.Provider value={{permissions, setPermissions}}>
            {children}
        </PermissionsContext.Provider>
    )
}

export default PermissionsContext;
