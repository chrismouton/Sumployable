import { Counter } from "./components/Counter";
import { Tasks } from "./components/Todo";
import { Home } from "./components/Home";
import { LoginPage } from "./components/api-authorization/LoginPage";
import { RegisterPage } from "./components/api-authorization/RegisterPage";
import { ProtectedRoute } from "./components/api-authorization/ProtectedRoute";
import { Dashboard } from "./components/Dashboard";
import { JobApplications } from "./components/JobApplications";
import { JobApplicationForm } from "./components/JobApplicationForm";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/todo',
    element: <ProtectedRoute><Tasks /></ProtectedRoute>
  },
  {
    path: '/dashboard',
    element: <ProtectedRoute><Dashboard /></ProtectedRoute>
  },
  {
    path: '/job-applications',
    element: <ProtectedRoute><JobApplications /></ProtectedRoute>
  },
  {
    path: '/job-applications/new',
    element: <ProtectedRoute><JobApplicationForm /></ProtectedRoute>
  },
  {
    path: '/job-applications/:id/edit',
    element: <ProtectedRoute><JobApplicationForm /></ProtectedRoute>
  },
  {
    path: '/login',
    element: <LoginPage />
  },
  {
    path: '/register',
    element: <RegisterPage />
  }
];

export default AppRoutes;
