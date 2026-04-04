import { Link, NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from './api-authorization/AuthContext';
import { ThemeToggle } from './ThemeToggle';

function AuthLinks() {
  const { isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = async (e) => {
    e.preventDefault();
    await logout();
    navigate('/login');
  };

  const navLinkClass = ({ isActive }) => isActive ? 'nav-link-active' : undefined;

  if (isAuthenticated) {
    return (
      <>
        <li><NavLink to="/dashboard" className={navLinkClass}>Dashboard</NavLink></li>
        <li><NavLink to="/job-applications" className={navLinkClass}>Job Applications</NavLink></li>
        <li><a href="#" onClick={handleLogout}>Log out</a></li>
      </>
    );
  }
  return (
    <li><Link to="/dashboard">Log in</Link></li>
  );
}

export function NavMenu() {
  return (
    <header>
      <nav>
        <ul>
          <li><Link to="/">Sumployable</Link></li>
        </ul>
        <ul>
          <AuthLinks />
          <li aria-hidden="true" className="nav-separator"></li>
          <li><ThemeToggle /></li>
        </ul>
      </nav>
    </header>
  );
}
