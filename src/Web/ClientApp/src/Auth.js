import {createContext, useContext, useEffect, useMemo, useState} from "react";
import {useNavigate} from "react-router-dom";
import {useLocalStorage} from "./useLocalStorage";
import {jwtDecode} from "jwt-decode";

const AuthContext = createContext();

const AuthProvider = ({children}) => {
  const [token, setToken] = useLocalStorage("token", null);
  const navigate = useNavigate();

  const login = (newToken) => {
    setToken({token: newToken});
    navigate("/", {replace: true});
  };

  const logout = () => {
    setToken(null);
    navigate("/", {replace: true});
  }

  const user = token?.token && jwtDecode(token.token);

  const contextValue = useMemo(
    () => ({
      token,
      user,
      login,
      logout
    }),
    [token]
  );

  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
};

export const useAuth = () => {
  return useContext(AuthContext);
};

export default AuthProvider;