import {Link, useNavigate, useParams} from "react-router-dom";
import {Button} from "reactstrap";
import {LessonsClient} from "../web-eduapi-client.ts";
import {useAuth} from "../Auth";

export const DeleteLessonPage = () => {
  const {courseId, lessonId} = useParams();
  const navigate = useNavigate();
  const {token} = useAuth();

  const LessonClient = new LessonsClient(window.location.origin.toString(), {
    fetch: (url, init) => fetch(url, {
      ...init,
      headers: {...init.headers, Authorization: `Bearer ${token}`}
    })
  });
  const deleteLesson = async () => {
    await LessonClient.deleteLesson(parseInt(lessonId, 10));
    navigate(`/courses/${courseId}`);
  }
  return (
    <div>
      <Link to={`/courses/${courseId}`}>Powrót</Link>
      <h1>Usuń lekcję</h1>
      <p>Czy na pewno chcesz usunąć tę lekcję?</p>
      <Button color="danger" onClick={deleteLesson}>Usuń</Button>
    </div>
  )
}