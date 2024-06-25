import {useAuth} from "../Auth";
import {CoursesClient} from "../web-eduapi-client.ts";
import {useState} from "react";
import {Button, Form, Input, Label} from "reactstrap";
import {useNavigate} from "react-router-dom";

export const CreateCoursePage = () => {
  const {token} = useAuth();
  const navigate = useNavigate();

  const coursesClient = new CoursesClient(window.location.origin.toString(), {
    fetch: (url, init) => fetch(url, {
      ...init,
      headers: {...init.headers, Authorization: `Bearer ${token}`}
    })
  });

  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");

  const [error, setError] = useState(null);

  const createCourse = async () => {
    try {
      const result = await coursesClient.createCourse({title, description});
      if (result) {
        navigate(`/courses/${result}`, {replace: true});
      }
    } catch (error) {
      setError(JSON.parse(error.response).errors || "Wystąpił błąd.");
    }
  }
  return (
    <div>
      <h1>Stwórz nowy kurs</h1>
      <p>Tutaj możesz stworzyć nowy kurs.</p>
      <Form>
        <Label for="title">Tytuł:</Label>
        <Input type="text" id="title" value={title} onChange={e => {
          setError(null)
          setTitle(e.target.value)
        }}/>
        <Label for="description">Opis:</Label>
        <Input type="text" id="description" value={description} onChange={e => {
          setError(null)
          setDescription(e.target.value)
        }}/>
        <Button className="mt-2" onClick={createCourse}>Stwórz kurs</Button>
      </Form>
      {error && <div className="alert alert-danger mt-2">{Object.keys(error).map(
        key => <div key={key}>{error[key]}</div>
      )}</div>}
    </div>
  );
}