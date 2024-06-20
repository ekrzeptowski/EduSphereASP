import {Counter} from "./components/Counter";
import {Home} from "./components/Home";
import {LoginPage} from "./pages/Login";
import {RegisterPage} from "./pages/Register";

const AppRoutes = [
  {
    index: true,
    element: <Home/>
  },
  {
    path: '/counter',
    element: <Counter/>
  },
  {
    path: '/login',
    element: <LoginPage/>
  },
  {
    path: '/register',
    element: <RegisterPage/>
  }
];

export default AppRoutes;
