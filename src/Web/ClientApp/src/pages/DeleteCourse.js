import {Link, useNavigate, useParams} from "react-router-dom";
import {Button} from "reactstrap";
import {CoursesClient, LessonsClient} from "../web-eduapi-client.ts";
import {useAuth} from "../Auth";

export const DeleteCoursePage = () => {
  const {courseId} = useParams();
  const navigate = useNavigate();
  const {token} = useAuth();

  const coursesClient = new CoursesClient(window.location.origin.toString(), {
    fetch: (url, init) => fetch(url, {
      ...init,
      headers: {...init.headers, Authorization: `Bearer ${token}`}
    })
  });
  const deleteCourse = async () => {
    await coursesClient.deleteCourse(parseInt(courseId, 10));
    navigate(`/courses`);
  }
  return (
    <div>
      <Link to={`/courses`}>Powrót</Link>
      <h1>Usuń kurs</h1>
      <p>Czy na pewno chcesz usunąć ten kurs?</p>
      <Button color="danger" onClick={deleteCourse}>Usuń</Button>
    </div>
  )
}