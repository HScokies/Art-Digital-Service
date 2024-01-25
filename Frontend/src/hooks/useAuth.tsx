import { useContext } from "react";
import AuthContext from "src/context/authContext";

export const UseAuth = () => {
    return useContext(AuthContext)
}