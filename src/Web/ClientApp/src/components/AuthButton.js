import {useAuth} from "../Auth";
import {Link} from "react-router-dom";
import {NavLink} from "reactstrap";

const AuthButton = () => {
  const {user, logout} = useAuth();

  const linkTo = user ? null : "/login";
  const linkText = user ? "Wyloguj" : "Logowanie";

  return (
    <NavLink tag={Link} onClick={() => user && logout()} className="text-dark" to={linkTo}>{linkText}</NavLink>
  );
}

export {AuthButton}