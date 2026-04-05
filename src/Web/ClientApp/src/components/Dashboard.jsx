import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { LayoutDashboard, Briefcase, ArrowRight } from 'lucide-react';
import { DashboardClient } from '../web-api-client';

export function Dashboard() {
  const [totalCount, setTotalCount] = useState(null);
  const [twoWeeksAgoCount, setTwoWeeksAgoCount] = useState(null);

  useEffect(() => {
    const client = new DashboardClient();
    const twoWeeksAgo = new Date(Date.now() - 7 * 24 * 60 * 60 * 1000);
    Promise.all([
      client.getDashboard(),
      client.getJobApplicationCountByDate(twoWeeksAgo),
    ]).then(([dashboard, countByDate]) => {
      setTotalCount(dashboard.totalJobApplications ?? 0);
      setTwoWeeksAgoCount(countByDate);
    });
  }, []);

  return (
    <div className="saas-page">
      <div className="saas-header">
        <div className="saas-header-left">
          <LayoutDashboard size={22} aria-hidden="true" />
          <h1>Dashboard</h1>
        </div>
      </div>

      <div className="dash-stats-row">
        <div className="dash-stat-card">
          <div className="dash-stat-value">{totalCount ?? '—'}</div>
          <div className="dash-stat-label">Count to Date</div>
        </div>
        <div className="dash-stat-card">
          <div className="dash-stat-value">{twoWeeksAgoCount ?? '—'}</div>
          <div className="dash-stat-label">Count as at 1 Week Ago</div>
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
