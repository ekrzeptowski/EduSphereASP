import {useAuth} from "../Auth";
import {useState} from "react";

export const LoginPage = () => {
  const {login} = useAuth();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const response = await fetch("api/auth", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({username, password})
    });
    if (response.ok) {
      const token = await response.text();
      login(token);
    } else {
      setError("Nieprawidłowe dane logowania");
    }
  }

  return (
    <div className="container">
      <div className="row">
        <div className="col-md-6 offset-md-3">
          <h2 className="text-center">Login</h2>
          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label htmlFor="username">Email</label>
              <input type="text" className="form-control" id="username" value={username} onChange={(e) => {
                setError(null);
                setUsername(e.target.value)
              }}/>
            </div>
            <div className="form-group">
              <label htmlFor="password">Hasło</label>
              <input type="password" className="form-control" id="password" value={password} onChange={(e) => {
                setError(null);
                setPassword(e.target.value)
              }}/>
            </div>
            {error && <div className="alert alert-danger my-2">{error}</div>}
            <button type="submit" className="btn btn-primary my-2">Login</button>
          </form>
        </div>
      </div>
    </div>
  );
}