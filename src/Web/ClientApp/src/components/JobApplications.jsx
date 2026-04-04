import { useEffect, useState } from 'react';
import { JobApplicationsClient } from '../web-api-client';
import { Briefcase, RefreshCw, Plus } from 'lucide-react';

function getLookupTitle(lookups, id) {
  if (!lookups) return '—';
  const item = lookups.find((l) => l.id === id);
  return item ? item.title : '—';
}

function getStatusColor(title) {
  if (!title) return 'default';
  const t = title.toLowerCase();
  if (t.includes('interview')) return 'orange';
  if (t.includes('offer')) return 'green';
  if (t.includes('reject')) return 'red';
  if (t.includes('withdraw')) return 'slate';
  if (t.includes('applied')) return 'blue';
  return 'default';
}

function formatDate(date) {
  if (!date) return '—';
  return new Date(date).toLocaleDateString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  });
}

export function JobApplications() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const load = async () => {
    setLoading(true);
    setError(null);
    try {
      const result = await new JobApplicationsClient().getJobApplications();
      setData(result);
    } catch {
      setError('Failed to load job applications. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
  }, []);

  return (
    <div className="saas-page">
      <div className="saas-header">
        <div className="saas-header-left">
          <Briefcase size={22} aria-hidden="true" />
          <h1>Job Applications</h1>
        </div>
        <button className="secondary" disabled>
          <Plus size={16} aria-hidden="true" />
          Add Application
        </button>
      </div>

      {loading && (
        <div className="ja-loading" aria-live="polite" aria-busy="true">
          <div className="ja-skeleton" />
          <div className="ja-skeleton ja-skeleton-sm" />
          <div className="ja-skeleton ja-skeleton-sm" />
        </div>
      )}

      {error && !loading && (
        <div className="ja-error" role="alert">
          <p>{error}</p>
          <button onClick={load} className="secondary">
            <RefreshCw size={14} aria-hidden="true" />
            Retry
          </button>
        </div>
      )}

      {!loading && !error && data && (
        <>
          {data.jobApplications.length === 0 ? (
            <div className="ja-empty">
              <Briefcase size={40} aria-hidden="true" />
              <p>No applications yet</p>
              <small>Add your first application to start tracking your job search.</small>
            </div>
          ) : (
            <div className="ja-table-wrap">
              <table className="ja-table">
                <thead>
                  <tr>
                    <th>Company</th>
                    <th>Role</th>
                    <th>Status</th>
                    <th>Applied</th>
                    <th>Location</th>
                    <th>Commute</th>
                  </tr>
                </thead>
                <tbody>
                  {data.jobApplications.map((app) => {
                    const processStatus = getLookupTitle(data.processStatuses, app.processStatus);
                    const commute = getLookupTitle(data.commutes, app.commute);
                    const statusColor = getStatusColor(processStatus);
                    return (
                      <tr key={app.id}>
                        <td>{app.companyName || '—'}</td>
                        <td>{app.roleName}</td>
                        <td>
                          <span className={`status-badge status-badge--${statusColor}`}>
                            {processStatus}
                          </span>
                        </td>
                        <td>{formatDate(app.applicationDate)}</td>
                        <td>{app.location || '—'}</td>
                        <td>{commute}</td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          )}
        </>
      )}
    </div>
  );
}
