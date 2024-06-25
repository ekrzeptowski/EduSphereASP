import {useState} from "react";
import {LessonsClient} from "../web-eduapi-client.ts";
import {useAuth} from "../Auth";
import {Button, Form, Input, Label} from "reactstrap";
import {Link, useNavigate, useParams} from "react-router-dom";

export const CreateLessonPage = () => {
  const {token} = useAuth();
  const navigate = useNavigate();

  const {courseId} = useParams();
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");
  const [error, setError] = useState(null);

  const lessonsClient = new LessonsClient(window.location.origin.toString(), {
    fetch: (url, init) => fetch(url, {
      ...init,
      headers: {...init.headers, Authorization: `Bearer ${token}`}
    })
  });

  const createLesson = async () => {
    try {
      await lessonsClient.createLesson({title, content, courseId});
      navigate("/courses/" + courseId);
    } catch (e) {
      setError(JSON.parse(e.response).errors || "Wystąpił błąd");
    }
  }

  return (
    <div>
      <Link to={`/courses/${courseId}`}>Powrót</Link>
      <h1>Stwórz lekcję</h1>
      <p>Tutaj możesz stworzyć nową lekcję.</p>
      <Form>
        <Label for="title">Tytuł:</Label>
        <Input type="text" id="title" value={title} required onChange={e => {
          setError(null)
          setTitle(e.target.value)
        }}/>
        <Label for="content">Treść:</Label>
        <Input type="textarea" id="content" value={content} required onChange={e => {
          setError(null)
          setContent(e.target.value)
        }}/>
        <Button className="mt-2" onClick={createLesson}>Stwórz lekcję</Button>
      </Form>
      {error && <p className="text-danger">
        {Object.entries(error).map(([key, value]) => <div key={key}>{value}</div>)}
      </p>}
    </div>
  );
}