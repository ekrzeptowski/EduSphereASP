import {Home} from "./components/Home";
import {LoginPage} from "./pages/Login";
import {RegisterPage} from "./pages/Register";
import {ProtectedRoute, roles} from "./ProtectedRoute";
import {CoursesPage} from "./pages/Courses";
import {LessonsPage} from "./pages/Lessons";
import {CreateCoursePage} from "./pages/CreateCourse";
import {CreateLessonPage} from "./pages/CreateLessons";
import {DeleteLessonPage} from "./pages/DeleteLesson";

const AppRoutes = [
  {
    index: true,
    element: <Home/>
  },
  {
    path: '/login',
    element: <LoginPage/>
  },
  {
    path: '/register',
    element: <RegisterPage/>
  },
  {
    path: '/courses',
    element: <ProtectedRoute level={roles.student}>
      <CoursesPage/>
    </ProtectedRoute>
  },
  {
    path: '/courses/:courseId',
    element: <ProtectedRoute level={roles.student}>
      <LessonsPage/>
    </ProtectedRoute>
  },
  {
    path: '/courses/create',
    element: <ProtectedRoute level={roles.teacher}>
      <CreateCoursePage/>
    </ProtectedRoute>
  },
  {
    path: '/courses/:courseId/lessons/create',
    element: <ProtectedRoute level={roles.teacher}>
      <CreateLessonPage/>
    </ProtectedRoute>
  },
  {
    path: '/courses/:courseId/lessons/:lessonId/edit',
    element: <ProtectedRoute level={roles.teacher}>
      <CreateLessonPage/>
    </ProtectedRoute>
  },
  {
    path: '/courses/:courseId/lessons/:lessonId/delete',
    element: <ProtectedRoute level={roles.teacher}>
      <DeleteLessonPage/>
    </ProtectedRoute>
  }
];

export default AppRoutes;
