import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { JobApplicationsClient, CreateJobApplicationCommand, UpdateJobApplicationCommand } from '../web-api-client';
import { Briefcase, ArrowLeft } from 'lucide-react';

function toDateInputValue(date) {
  if (!date) return '';
  const d = new Date(date);
  if (isNaN(d.getTime())) return '';
  return d.toISOString().substring(0, 10);
}

export function JobApplicationForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEdit = Boolean(id);

  const [lookups, setLookups] = useState(null);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [generalError, setGeneralError] = useState('');
  const [fieldErrors, setFieldErrors] = useState({});

  const [roleName, setRoleName] = useState('');
  const [companyName, setCompanyName] = useState('');
  const [applicationDate, setApplicationDate] = useState(toDateInputValue(new Date()));
  const [roleType, setRoleType] = useState('');
  const [status, setStatus] = useState('');
  const [processStatus, setProcessStatus] = useState('');
  const [source, setSource] = useState('');
  const [commute, setCommute] = useState('');
  const [location, setLocation] = useState('');
  const [advertisedSalary, setAdvertisedSalary] = useState('');
  const [url, setUrl] = useState('');
  const [note, setNote] = useState('');

  useEffect(() => {
    const load = async () => {
      try {
        const data = await new JobApplicationsClient().getJobApplications();
        setLookups(data);

        if (data.roleTypes?.length) setRoleType(String(data.roleTypes[0].id));
        if (data.statuses?.length) setStatus(String(data.statuses[0].id));
        if (data.processStatuses?.length) setProcessStatus(String(data.processStatuses[0].id));
        if (data.sources?.length) setSource(String(data.sources[0].id));
        if (data.commutes?.length) {
          const hybrid = data.commutes.find((l) => l.title?.toLowerCase() === 'hybrid');
          setCommute(String(hybrid ? hybrid.id : data.commutes[0].id));
        }

        if (isEdit) {
          const app = data.jobApplications?.find((a) => a.id === parseInt(id, 10));
          if (app) {
            setRoleName(app.roleName || '');
            setCompanyName(app.companyName || '');
            setApplicationDate(toDateInputValue(app.applicationDate));
            setRoleType(String(app.roleType ?? ''));
            setStatus(String(app.status ?? ''));
            setProcessStatus(String(app.processStatus ?? ''));
            setSource(String(app.source ?? ''));
            setCommute(String(app.commute ?? ''));
            setLocation(app.location || '');
            setAdvertisedSalary(app.advertisedSalary || '');
            setUrl(app.url || '');
            setNote(app.note || '');
          } else {
            setGeneralError('Application not found.');
          }
        }
      } catch {
        setGeneralError('Failed to load data. Please go back and try again.');
      } finally {
        setLoading(false);
      }
    };
    load();
  }, [id, isEdit]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSubmitting(true);
    setGeneralError('');
    setFieldErrors({});

    try {
      const client = new JobApplicationsClient();
      const fields = {
        roleName,
        companyName: companyName || undefined,
        applicationDate: applicationDate ? new Date(applicationDate) : undefined,
        roleType: roleType !== '' ? parseInt(roleType, 10) : undefined,
        status: status !== '' ? parseInt(status, 10) : undefined,
        processStatus: processStatus !== '' ? parseInt(processStatus, 10) : undefined,
        source: source !== '' ? parseInt(source, 10) : undefined,
        commute: commute !== '' ? parseInt(commute, 10) : undefined,
        location: location || undefined,
        advertisedSalary: advertisedSalary || undefined,
        url: url || undefined,
        note: note || undefined,
      };

      if (isEdit) {
        const cmd = UpdateJobApplicationCommand.fromJS({ id: parseInt(id, 10), ...fields });
        await client.updateJobApplication(parseInt(id, 10), cmd);
      } else {
        const cmd = CreateJobApplicationCommand.fromJS(fields);
        await client.createJobApplication(cmd);
      }

      navigate('/job-applications');
    } catch (err) {
      try {
        const body = typeof err.response === 'string' ? JSON.parse(err.response) : err.response;
        if (body?.errors) {
          const mapped = {};
          for (const key of Object.keys(body.errors)) {
            const camelKey = key.charAt(0).toLowerCase() + key.slice(1);
            mapped[camelKey] = body.errors[key]?.[0] || '';
          }
          setFieldErrors(mapped);
        } else {
          setGeneralError('Failed to save. Please try again.');
        }
      } catch {
        setGeneralError('Failed to save. Please try again.');
      }
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) {
    return (
      <div className="saas-page">
        <div className="ja-loading" aria-live="polite" aria-busy="true">
          <div className="ja-skeleton" />
          <div className="ja-skeleton ja-skeleton-sm" />
          <div className="ja-skeleton ja-skeleton-sm" />
        </div>
      </div>
    );
  }

  return (
    <div className="saas-page">
      <div className="saas-header">
        <div className="saas-header-left">
          <Briefcase size={22} aria-hidden="true" />
          <h1>{isEdit ? 'Edit Application' : 'New Application'}</h1>
        </div>
        <button className="secondary" type="button" onClick={() => navigate('/job-applications')}>
          <ArrowLeft size={16} aria-hidden="true" />
          Back
        </button>
      </div>

      {generalError && (
        <div className="ja-error" role="alert" style={{ marginBottom: 'var(--pico-block-spacing-vertical)' }}>
          <p>{generalError}</p>
        </div>
      )}

      <article>
        <form onSubmit={handleSubmit}>
          <div className="ja-form-grid">
            <div>
              <label htmlFor="roleName">
                Role Name <span aria-hidden="true">*</span>
              </label>
              <input
                id="roleName"
                type="text"
                value={roleName}
                onChange={(e) => setRoleName(e.target.value)}
                maxLength={200}
                required
                aria-invalid={fieldErrors.roleName ? 'true' : undefined}
                aria-describedby={fieldErrors.roleName ? 'roleName-error' : undefined}
              />
              {fieldErrors.roleName && (
                <small id="roleName-error" className="error">{fieldErrors.roleName}</small>
              )}
            </div>

            <div>
              <label htmlFor="companyName">Company Name</label>
              <input
                id="companyName"
                type="text"
                value={companyName}
                onChange={(e) => setCompanyName(e.target.value)}
              />
            </div>

            <div>
              <label htmlFor="applicationDate">Application Date</label>
              <input
                id="applicationDate"
                type="date"
                value={applicationDate}
                onChange={(e) => setApplicationDate(e.target.value)}
              />
            </div>

            <div>
              <label htmlFor="roleType">Role Type</label>
              <select id="roleType" value={roleType} onChange={(e) => setRoleType(e.target.value)}>
                {lookups?.roleTypes?.map((l) => (
                  <option key={l.id} value={l.id}>{l.title}</option>
                ))}
              </select>
            </div>

            <div>
              <label htmlFor="status">Status</label>
              <select id="status" value={status} onChange={(e) => setStatus(e.target.value)}>
                {lookups?.statuses?.map((l) => (
                  <option key={l.id} value={l.id}>{l.title}</option>
                ))}
              </select>
            </div>

            <div>
              <label htmlFor="processStatus">Process Status</label>
              <select id="processStatus" value={processStatus} onChange={(e) => setProcessStatus(e.target.value)}>
                {lookups?.processStatuses?.map((l) => (
                  <option key={l.id} value={l.id}>{l.title}</option>
                ))}
              </select>
            </div>

            <div>
              <label htmlFor="source">Source</label>
              <select id="source" value={source} onChange={(e) => setSource(e.target.value)}>
                {lookups?.sources?.map((l) => (
                  <option key={l.id} value={l.id}>{l.title}</option>
                ))}
              </select>
            </div>

            <div>
              <label htmlFor="commute">Commute</label>
              <select id="commute" value={commute} onChange={(e) => setCommute(e.target.value)}>
                {lookups?.commutes?.map((l) => (
                  <option key={l.id} value={l.id}>{l.title}</option>
                ))}
              </select>
            </div>

            <div>
              <label htmlFor="location">Location</label>
              <input
                id="location"
                type="text"
                value={location}
                onChange={(e) => setLocation(e.target.value)}
              />
            </div>

            <div>
              <label htmlFor="advertisedSalary">Advertised Salary</label>
              <input
                id="advertisedSalary"
                type="text"
                value={advertisedSalary}
                onChange={(e) => setAdvertisedSalary(e.target.value)}
              />
            </div>

            <div className="ja-form-full">
              <label htmlFor="url">Job URL</label>
              <input
                id="url"
                type="url"
                value={url}
                onChange={(e) => setUrl(e.target.value)}
              />
            </div>

            <div className="ja-form-full">
              <label htmlFor="note">Notes</label>
              <textarea
                id="note"
                value={note}
                onChange={(e) => setNote(e.target.value)}
                rows={4}
              />
            </div>
          </div>

          <div className="ja-form-actions">
            <button
              type="button"
              className="secondary"
              onClick={() => navigate('/job-applications')}
            >
              Cancel
            </button>
            <button type="submit" aria-busy={submitting || undefined}>
              {isEdit ? 'Save Changes' : 'Add Application'}
            </button>
          </div>
        </form>
      </article>
    </div>
  );
}
