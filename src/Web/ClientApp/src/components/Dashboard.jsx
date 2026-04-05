import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { LayoutDashboard, Briefcase, ArrowRight } from 'lucide-react';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, LabelList } from 'recharts';
import { DashboardClient } from '../web-api-client';

export function Dashboard() {
  const [totalCount, setTotalCount] = useState(null);
  const [twoWeeksAgoCount, setTwoWeeksAgoCount] = useState(null);
  const [perDayData, setPerDayData] = useState(null);

  useEffect(() => {
    const client = new DashboardClient();
    const twoWeeksAgo = new Date(Date.now() - 7 * 24 * 60 * 60 * 1000);
    const sevenDaysAgo = new Date(Date.now() - 6 * 24 * 60 * 60 * 1000);
    Promise.all([
      client.getDashboard(),
      client.getJobApplicationCountByDate(twoWeeksAgo),
      client.getJobApplicationsPerDay(sevenDaysAgo),
    ]).then(([dashboard, countByDate, perDay]) => {
      setTotalCount(dashboard.totalJobApplications ?? 0);
      setTwoWeeksAgoCount(countByDate);
      setPerDayData(perDay.map(d => ({
        label: new Date(d.date).toLocaleDateString('en-AU', { weekday: 'short', day: 'numeric' }),
        count: d.count ?? 0,
      })));
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

      <div className="dash-chart-card">
        <h2 className="dash-chart-title">Applications in the past 7 days</h2>
        {perDayData === null ? (
          <div className="dash-chart-skeleton" aria-label="Loading chart" />
        ) : (
          <ResponsiveContainer width="100%" height={200}>
            <BarChart
              data={perDayData}
              margin={{ top: 20, right: 8, bottom: 0, left: -20 }}
              aria-label="Applications in the past 7 days bar chart"
            >
              <CartesianGrid strokeDasharray="3 3" stroke="var(--pico-muted-border-color)" vertical={false} />
              <XAxis dataKey="label" tick={{ fontSize: 12 }} />
              <YAxis allowDecimals={false} tick={{ fontSize: 12 }} />
              <Tooltip cursor={{ fill: 'color-mix(in srgb, var(--pico-primary) 10%, transparent)' }} />
              <Bar dataKey="count" name="Applications" fill="var(--pico-primary)" radius={[4, 4, 0, 0]}>
                <LabelList dataKey="count" position="top" style={{ fontSize: 11, fill: 'var(--pico-muted-color)' }} />
              </Bar>
            </BarChart>
          </ResponsiveContainer>
        )}
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
