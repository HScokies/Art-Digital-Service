import { Navigate, Outlet, useLocation } from "react-router-dom";
import { Pages, UserTypes } from "src/enums";
import { UseAuth } from "src/hooks/useAuth";



interface props{
    allowed: UserTypes | undefined
}


const RequireAuth = ({allowed}: props) => {
    const {userType} = UseAuth();
    const location = useLocation();

    if (userType == allowed) return <Outlet/>
    switch(userType){
        case UserTypes.newUser:
            return <Navigate to={Pages.form} state={{ from: location }}/>
        case UserTypes.participant:
            return <Navigate to={Pages.profile} state={{ from: location }}/>
        case UserTypes.staff:
            return <Navigate to={Pages.dashboard} state={{ from: location }}/>
        default:
            return <Navigate to={Pages.auth} state={{ from: location }}/>
    }
}
export default RequireAuth