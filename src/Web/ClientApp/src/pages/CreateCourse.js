import {useAuth} from "../Auth";
import {CoursesClient} from "../web-eduapi-client.ts";
import {useEffect, useState} from "react";
import {Button, Form, Input, Label} from "reactstrap";
import {useNavigate, useParams} from "react-router-dom";

export const CreateCoursePage = () => {
  const {courseId} = useParams();
  const {token} = useAuth();
  const navigate = useNavigate();

  const coursesClient = new CoursesClient(window.location.origin.toString(), {
    fetch: (url, init) => fetch(url, {
      ...init,
      headers: {...init.headers, Authorization: `Bearer ${token}`}
    })
  });

  const defaultData = async () => {
    if (!courseId) {
      return;
    }
    const course = await coursesClient.getCourse(parseInt(courseId, 10));
    setTitle(course.title);
    setDescription(course.description);
  }

  useEffect(() => {
    defaultData();
  }, []);


  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");

  const [error, setError] = useState(null);

  const createCourse = async () => {
    try {
      if (courseId) {
        await coursesClient.updateCourse(courseId, {
          title,
          description,
          courseId: parseInt(courseId, 10)
        });
        navigate("/courses/" + courseId);
        return;
      }
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
      <h1>{courseId ? "Edytuj" : "Stwórz nowy"} kurs</h1>
      <p>Tutaj możesz {courseId ? "edytować" : "stworzyć nowy"} kurs.</p>
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
        <Button className="mt-2" onClick={createCourse}>{courseId ? "Zapisz" : "Stwórz"} kurs</Button>
      </Form>
      {error && <div className="alert alert-danger mt-2">{Object.keys(error).map(
        key => <div key={key}>{error[key]}</div>
      )}</div>}
    </div>
  );
}