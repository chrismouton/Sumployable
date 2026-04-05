import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { LayoutDashboard, Briefcase, ArrowRight } from 'lucide-react';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, LabelList, PieChart, Pie, Cell, Legend } from 'recharts';

const COLORS = ['#4e79a7', '#f28e2b', '#e15759', '#76b7b2', '#59a14f', '#edc948', '#b07aa1'];
import { DashboardClient } from '../web-api-client';

export function Dashboard() {
  const [totalCount, setTotalCount] = useState(null);
  const [twoWeeksAgoCount, setTwoWeeksAgoCount] = useState(null);
  const [perDayData, setPerDayData] = useState(null);
  const [roleTypeData, setRoleTypeData] = useState(null);
  const [processStatusData, setProcessStatusData] = useState(null);
  const [inProgressData, setInProgressData] = useState(null);

  useEffect(() => {
    const client = new DashboardClient();
    const twoWeeksAgo = new Date(Date.now() - 7 * 24 * 60 * 60 * 1000);
    const sevenDaysAgo = new Date(Date.now() - 6 * 24 * 60 * 60 * 1000);
    Promise.all([
      client.getDashboard(),
      client.getJobApplicationCountByDate(twoWeeksAgo),
      client.getJobApplicationsPerDay(sevenDaysAgo),
      client.getJobApplicationCountByRoleType(),
      client.getJobApplicationCountByProcessStatus(),
      client.getActiveInProgressApplications(),
    ]).then(([dashboard, countByDate, perDay, roleType, processStatus, inProgress]) => {
      setTotalCount(dashboard.totalJobApplications ?? 0);
      setTwoWeeksAgoCount(countByDate);
      setPerDayData(perDay.map(d => ({
        label: new Date(d.date).toLocaleDateString('en-AU', { weekday: 'short', day: 'numeric' }),
        count: d.count ?? 0,
      })));
      setRoleTypeData(roleType.map(d => ({ name: d.roleTypeName, value: d.count ?? 0 })));
      setProcessStatusData(processStatus.map(d => ({ name: d.processStatusName, value: d.count ?? 0 })));
      setInProgressData(inProgress);
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

      <div className="dash-pie-row">
        <div className="dash-chart-card">
          <h2 className="dash-chart-title">By role type</h2>
          {roleTypeData === null ? (
            <div className="dash-chart-skeleton" aria-label="Loading chart" />
          ) : (
            <ResponsiveContainer width="100%" height={260}>
              <PieChart aria-label="Job applications by role type pie chart">
                <Pie data={roleTypeData} dataKey="value" nameKey="name" cx="35%" cy="50%" outerRadius={80}>
                  {roleTypeData.map((_, i) => <Cell key={i} fill={COLORS[i % COLORS.length]} />)}
                </Pie>
                <Tooltip formatter={(value, name) => [value, name]} />
                <Legend layout="vertical" align="right" verticalAlign="middle" formatter={(value, entry) => `${value} (${entry.payload.value})`} />
              </PieChart>
            </ResponsiveContainer>
          )}
        </div>

        <div className="dash-chart-card">
          <h2 className="dash-chart-title">By process status</h2>
          {processStatusData === null ? (
            <div className="dash-chart-skeleton" aria-label="Loading chart" />
          ) : (
            <ResponsiveContainer width="100%" height={260}>
              <PieChart aria-label="Job applications by process status pie chart">
                <Pie data={processStatusData} dataKey="value" nameKey="name" cx="35%" cy="50%" outerRadius={80}>
                  {processStatusData.map((_, i) => <Cell key={i} fill={COLORS[i % COLORS.length]} />)}
                </Pie>
                <Tooltip formatter={(value, name) => [value, name]} />
                <Legend layout="vertical" align="right" verticalAlign="middle" formatter={(value, entry) => `${value} (${entry.payload.value})`} />
              </PieChart>
            </ResponsiveContainer>
          )}
        </div>
      </div>

      <div className="dash-chart-card">
        <h2 className="dash-chart-title">Active in-progress applications</h2>
        {inProgressData === null ? (
          <div className="dash-chart-skeleton" aria-label="Loading table" />
        ) : inProgressData.length === 0 ? (
          <p className="dash-empty-state">No in-progress applications.</p>
        ) : (
          <div className="dash-table-wrapper">
            <table>
              <thead>
                <tr>
                  <th>Company</th>
                  <th>Role</th>
                  <th>Process Status</th>
                  <th>Application Date</th>
                  <th>Location</th>
                  <th>Commute</th>
                </tr>
              </thead>
              <tbody>
                {inProgressData.map((row, i) => (
                  <tr key={i}>
                    <td>{row.companyName ?? '—'}</td>
                    <td>{row.roleName}</td>
                    <td>{row.processStatusName}</td>
                    <td>{new Date(row.applicationDate).toLocaleDateString('en-AU')}</td>
                    <td>{row.location ?? '—'}</td>
                    <td>{row.commuteName}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
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
