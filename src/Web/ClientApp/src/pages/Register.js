import {useState} from "react";

export const RegisterPage = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const response = await fetch("api/auth/register", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({username, password})
    });
    const data = response.ok ? {redirect: "/login"} : await response.json();
    if (data.redirect) window.location.href = data.redirect;
    if (data.errors) setError(data.errors);
  }

  return (
    <div className="container">
      <div className="row">
        <div className="col-md-6 offset-md-3">
          <h2 className="text-center">Rejestracja</h2>
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
            {error && <div className="alert alert-danger my-2">{error.map(e => <p>{e}</p>)}</div>}
            <button type="submit" className="btn btn-primary my-2">Zarejestruj</button>
          </form>
        </div>
      </div>
    </div>
  );
}