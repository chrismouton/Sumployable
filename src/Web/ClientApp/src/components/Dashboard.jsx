import { Link } from 'react-router-dom';
import { LayoutDashboard, Briefcase, ArrowRight } from 'lucide-react';

export function Dashboard() {
  return (
    <div className="saas-page">
      <div className="saas-header">
        <div className="saas-header-left">
          <LayoutDashboard size={22} aria-hidden="true" />
          <h1>Dashboard</h1>
        </div>
      </div>

      <div className="dash-welcome">
        <div className="dash-welcome-card">
          <span className="dash-welcome-icon" aria-hidden="true">
            <Briefcase size={32} />
          </span>
          <h2>Welcome to Sumployable</h2>
          <p>Track and manage your job applications in one place.</p>
          <Link to="/job-applications" className="lp-btn lp-btn-primary">
            View Job Applications
            <ArrowRight size={16} />
          </Link>
        </div>
      </div>
    </div>
  );
}
