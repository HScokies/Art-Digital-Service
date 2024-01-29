import { useContext } from "react"
import PermissionsContext from "src/context/premissionsContext"

export const UsePermissions = () => {
    return useContext(PermissionsContext)
}