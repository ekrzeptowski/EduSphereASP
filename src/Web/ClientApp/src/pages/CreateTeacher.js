import {useState} from "react";
import {useAuth} from "../Auth";
import {Button, Form, Input} from "reactstrap";
import {useNavigate} from "react-router-dom";

export const CreateTeacherPage = () => {
  const {token} = useAuth();
  const navigate = useNavigate();
  const [teacher, setTeacher] = useState({userName: '', password: ''});
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleChange = (e) => {
    const {name, value} = e.target;
    setTeacher({...teacher, [name]: value});
  };
  
  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const result = await fetch("/api/auth/register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`
        },
        body: JSON.stringify(teacher)
      });
      if (!result.ok) {
        throw await result.json();
      }
      setLoading(false);
      if (result) {
        navigate("/");
      }
    } catch (error) {
      console.log(error);
      setLoading(false);
      setError(error.errors || "Wystąpił błąd.");
    }
  };

  return (
    <div>
      <h1>Tworzenie nauczyciela</h1>
      <Form onSubmit={handleSubmit}>
        <div>
          <label>Email</label>
          <Input
            type="email"
            name="userName"
            value={teacher.userName}
            onChange={handleChange}
          />
        </div>
        <div>
          <label>Hasło</label>
          <Input
            type="password"
            name="password"
            value={teacher.password}
            onChange={handleChange}
          />
        </div>
        <Button type="submit" disabled={loading}>
          Dodaj nauczyciela
        </Button>
        {error && <div className="alert alert-danger mt-2">{Object.keys(error).map(
          key => <div key={key}>{error[key]}</div>
        )}</div>}
      </Form>
    </div>
  );
}