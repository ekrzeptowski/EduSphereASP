import {useState} from "react";
import {LessonsClient} from "../web-eduapi-client.ts";
import {useAuth} from "../Auth";
import {Button, Form, Input, Label} from "reactstrap";

export const CreateLessonPage = () => {
  const {token} = useAuth();
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
    // TODO: Implement creating a lesson
  }

  return (
    <div>
      <h1>Stwórz lekcję</h1>
      <p>Tutaj możesz stworzyć nową lekcję.</p>
      <Form>
        <Label for="title">Tytuł:</Label>
        <Input type="text" id="title" value={title} onChange={e => {
          setError(null)
          setTitle(e.target.value)
        }}/>
        <Label for="content">Treść:</Label>
        <Input type="text" id="content" value={content} onChange={e => {
          setError(null)
          setContent(e.target.value)
        }}/>
        <Button className="mt-2" onClick={createLesson}>Stwórz lekcję</Button>
      </Form>
    </div>
  );
}