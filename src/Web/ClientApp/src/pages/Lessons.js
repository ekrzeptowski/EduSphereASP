import {CoursesClient} from "../web-eduapi-client.ts";
import {useAuth} from "../Auth";
import {useEffect, useState} from "react";
import {Link, useParams} from "react-router-dom";
import {roles} from "../ProtectedRoute";

export const LessonsPage = () => {
  const {courseId} = useParams();
  const {user, token} = useAuth();

  const coursesClient = new CoursesClient(window.location.origin.toString(), {
    fetch: (url, init) => fetch(url, {
      ...init,
      headers: {...init.headers, Authorization: `Bearer ${token}`}
    })
  });

  const [courseData, setCourseData] = useState(null);

  useEffect(() => {
    const fetchLessons = async () => {
      const result = await coursesClient.getCourse(courseId);
      setCourseData(result);
    };

    fetchLessons();
  }, []);

  if (!courseData) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <Link to="/courses">Powrót do listy kursów</Link>
      <h1>{courseData.title}</h1>
      <h2>{courseData.description}</h2>
      <p>Ilość lekcji: {courseData.lessons.length}</p>

      {roles[user.role.toLowerCase()] <= roles.teacher &&
        <Link to={`/courses/${courseId}/lessons/create`}>Dodaj nową lekcję</Link>
      }
      <h2>Lista lekcji:</h2>
      <ul>
        {courseData.lessons.map(lesson => (
            <li key={lesson.id}>{lesson.title}
              <ul>
                <li>Id: {lesson.id}</li>
                <li>Opis: {lesson.content}</li>
              </ul>
            </li>
          )
        )}
      </ul>
    </div>
  )
    ;
}