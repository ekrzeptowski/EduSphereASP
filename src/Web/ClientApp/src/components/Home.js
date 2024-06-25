import React from 'react';
import {useAuth} from "../Auth";
import {Link} from "react-router-dom";

const Home = () => {
  const {user} = useAuth();
  return (
    <div>
      <h1>Witaj, {user?.username?.split("@")[0] || "przybyszu"}!</h1>
      <p>Twoja rola to: {user?.role.toLowerCase() || "niezalogowany"}</p>
      <p>Witaj na stronie głównej aplikacji.</p>
      
      <Link to="/courses">Przejdź do kursów</Link>
    </div>
  );
}

export {Home};