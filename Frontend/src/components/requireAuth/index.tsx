import { Navigate, Outlet } from "react-router-dom";
import { Pages, UserTypes } from "src/enums";
import { UseAuth } from "src/hooks/useAuth";



interface props{
    allowed: UserTypes | undefined
}


const RequireAuth = ({allowed}: props) => {
    const {userType} = UseAuth();
    if (userType == allowed) return <Outlet/>
    switch(userType){
        case UserTypes.newUser:
            return <Navigate to={Pages.form}/>
        case UserTypes.participant:
            return <Navigate to={Pages.profile}/>
        case UserTypes.staff:
            return <Navigate to={Pages.dashboard}/>
        default:
            return <Navigate to={Pages.auth}/>
    }
}
export default RequireAuth