import {Navigate} from "react-router-dom";
import {useAuth} from "./Auth";

export const roles = {
  administrator: 1, teacher: 2, student: 3,
};

export const ProtectedRoute = ({children, level = 99}) => {
  const {user} = useAuth();
  const {role} = user || {};
  if (!user || roles[role.toLowerCase()] > roles[level]) {
    // user is not authenticated
    return <Navigate to="/login"/>;
  }
  return children;
};