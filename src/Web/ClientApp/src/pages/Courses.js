import {useAuth} from "../Auth";
import {CoursesClient, EnrollmentsClient} from "../web-eduapi-client.ts";
import {useState, useEffect} from "react";
import {Link} from "react-router-dom";
import {roles} from "../ProtectedRoute";
import {Button} from "reactstrap";

export const CoursesPage = () => {
  const {user, token} = useAuth();

  const coursesClient = new CoursesClient(window.location.origin.toString(), {
    fetch: (url, init) => fetch(url, {
      ...init,
      headers: {...init.headers, Authorization: `Bearer ${token}`}
    })
  });

  const [page, setPage] = useState(1);
  const [courses, setCourses] = useState(null);

  const enrollmentsClient = new EnrollmentsClient(window.location.origin.toString(), {
    fetch: (url, init) => fetch(url, {
      ...init,
      headers: {...init.headers, Authorization: `Bearer ${token}`}
    })
  });

  const enroll = async (courseId) => {
    try {
      const result = await enrollmentsClient.createEnrollment({courseId});
      console.log(result);
    } catch (error) {
      console.error(error);
    }
    const updatedCourses = await coursesClient.getCoursesWithPagination(page, 10);
    setCourses(updatedCourses);
  }

  useEffect(() => {
    const fetchCourses = async () => {
      const result = await coursesClient.getCoursesWithPagination(page, 10);
      setCourses(result);
    };

    fetchCourses();
  }, [page]);

  if (!courses) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h1>Kursy</h1>
      <p>Ilość kursów: {courses.totalCount}</p>
      <p>Strona: {page}</p>
      <Button onClick={() => setPage(page - 1)} disabled={page === 1}>Poprzednia</Button>
      <Button onClick={() => setPage(page + 1)} disabled={page === Math.ceil(courses.totalCount / 10)}>Następna</Button>

      {roles[user.role.toLowerCase()] <= roles.teacher && <div><Link to="/courses/create">Dodaj nowy kurs</Link></div>}

      <h2>Lista kursów:</h2>
      <ul>
        {courses.items.map(course => (
          <li key={course.id}>{course.title}
            <ul>
              <li>Id: {course.id}</li>
              <li>Opis: {course.description}</li>
              {roles[user.role.toLowerCase()] > roles.teacher &&
                <>
                  <li>Zapisany: {course.isEnrolled ? "tak" : "nie"}</li>
                  {!course.isEnrolled && <li>
                    <Button onClick={() => enroll(course.id)}>Zapisz się</Button>
                  </li>}
                </>
              }
              {(course.isEnrolled || roles[user.role.toLowerCase()] <= roles.teacher) &&
                <div><Link to={`/courses/${course.id}`}>Przejdź do kursu</Link></div>}
              {roles[user.role.toLowerCase()] <= roles.teacher &&
                <div><Link to={`/courses/${course.id}/edit`}>Edytuj kurs</Link></div>}
              {roles[user.role.toLowerCase()] <= roles.teacher &&
                <div><Link to={`/courses/${course.id}/delete`}>Usuń kurs</Link></div>}
            </ul>
          </li>
        ))}
      </ul>
    </div>
  );
}