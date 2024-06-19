import {Counter} from "./components/Counter";
import {Home} from "./components/Home";
import {LoginPage} from "./pages/Login";

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
  }
];

export default AppRoutes;
