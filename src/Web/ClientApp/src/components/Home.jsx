import { Link } from 'react-router-dom';
import {
  Zap,
  Users,
  BarChart3,
  CheckCircle,
  ArrowRight,
  Star,
  TrendingUp,
  Shield,
  Sparkles,
} from 'lucide-react';

const FEATURES = [
  {
    icon: Sparkles,
    title: 'AI-Powered Screening',
    description:
      'Automatically rank and summarise candidates against your role requirements, cutting manual review time by 70%.',
  },
  {
    icon: Users,
    title: 'Smart Talent Matching',
    description:
      'Our matching engine surfaces the most employable candidates for each opening — no keyword guessing required.',
  },
  {
    icon: BarChart3,
    title: 'Hiring Analytics',
    description:
      'Track time-to-hire, offer acceptance rates, and pipeline health with real-time dashboards built for your team.',
  },
];

const STEPS = [
  {
    number: '01',
    title: 'Post your role',
    description: 'Describe the position and let Sumployable build a structured scorecard automatically.',
  },
  {
    number: '02',
    title: 'Review ranked candidates',
    description: 'Receive a ranked shortlist with AI-generated summaries — ready to interview in minutes, not days.',
  },
  {
    number: '03',
    title: 'Hire with confidence',
    description: 'Collaborate with your team, collect structured feedback, and make data-backed decisions.',
  },
];

const TESTIMONIALS = [
  {
    quote:
      "Sumployable cut our time-to-hire from six weeks to eleven days. The AI summaries mean our hiring managers read only what matters.",
    name: 'Sarah Chen',
    role: 'Head of Talent · Nexova',
    stars: 5,
  },
  {
    quote:
      "We scaled from 20 to 120 employees in a year without growing the recruiting team. Sumployable made that possible.",
    name: 'Marcus Webb',
    role: 'CEO · Latteral',
    stars: 5,
  },
];

const STATS = [
  { value: '3 500+', label: 'Companies hiring' },
  { value: '2.4 M', label: 'Candidates assessed' },
  { value: '11 days', label: 'Average time-to-hire' },
];

function StarRow({ count }) {
  return (
    <span className="lp-stars" aria-label={`${count} out of 5 stars`}>
      {Array.from({ length: count }).map((_, i) => (
        <Star key={i} size={14} fill="currentColor" />
      ))}
    </span>
  );
}

export function Home() {
  return (
    <div className="lp-root">
      {/* ── Hero ──────────────────────────────────────────────────────────── */}
      <section className="lp-hero">
        <div className="lp-container">
          <div className="lp-hero-badge">
            <Zap size={14} />
            <span>AI-powered hiring platform</span>
          </div>

          <h1 className="lp-hero-heading">
            Hire the most
            <span className="lp-accent"> employable</span> people,
            <br />
            faster than ever before
          </h1>

          <p className="lp-hero-sub">
            Sumployable uses AI to screen, rank, and summarise every applicant — so your
            team spends time on conversations, not spreadsheets.
          </p>

          <div className="lp-hero-actions">
            <Link to="/register" className="lp-btn lp-btn-primary">
              Start for free
              <ArrowRight size={16} />
            </Link>
            <Link to="/dashboard" className="lp-btn lp-btn-ghost">
              Sign in
            </Link>
          </div>

          <p className="lp-hero-note">
            <CheckCircle size={14} />
            No credit card required &nbsp;·&nbsp; Free 14-day trial
          </p>
        </div>

        <div className="lp-hero-glow" aria-hidden="true" />
      </section>

      {/* ── Social proof strip ──────────────────────────────────────────── */}
      <section className="lp-stats-strip">
        <div className="lp-container lp-stats-inner">
          {STATS.map((s) => (
            <div key={s.label} className="lp-stat">
              <span className="lp-stat-value">{s.value}</span>
              <span className="lp-stat-label">{s.label}</span>
            </div>
          ))}
        </div>
      </section>

      {/* ── Features ────────────────────────────────────────────────────── */}
      <section className="lp-section">
        <div className="lp-container">
          <div className="lp-section-header">
            <h2 className="lp-section-title">Everything your team needs to hire well</h2>
            <p className="lp-section-sub">
              From first application to signed offer, Sumployable keeps every stage structured,
              collaborative, and fast.
            </p>
          </div>

          <div className="lp-features-grid">
            {FEATURES.map((f) => {
              const Icon = f.icon;
              return (
                <div key={f.title} className="lp-feature-card">
                  <span className="lp-feature-icon" aria-hidden="true">
                    <Icon size={22} />
                  </span>
                  <h3 className="lp-feature-title">{f.title}</h3>
                  <p className="lp-feature-desc">{f.description}</p>
                </div>
              );
            })}
          </div>
        </div>
      </section>

      {/* ── How it works ────────────────────────────────────────────────── */}
      <section className="lp-section lp-section-alt">
        <div className="lp-container">
          <div className="lp-section-header">
            <h2 className="lp-section-title">Up and running in three steps</h2>
            <p className="lp-section-sub">
              No lengthy onboarding. Most teams make their first hire within a week.
            </p>
          </div>

          <div className="lp-steps-grid">
            {STEPS.map((step, i) => (
              <div key={step.number} className="lp-step">
                <span className="lp-step-number" aria-hidden="true">{step.number}</span>
                {i < STEPS.length - 1 && (
                  <span className="lp-step-connector" aria-hidden="true">
                    <TrendingUp size={16} />
                  </span>
                )}
                <h3 className="lp-step-title">{step.title}</h3>
                <p className="lp-step-desc">{step.description}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* ── Testimonials ────────────────────────────────────────────────── */}
      <section className="lp-section">
        <div className="lp-container">
          <div className="lp-section-header">
            <h2 className="lp-section-title">Trusted by fast-growing teams</h2>
          </div>

          <div className="lp-testimonials-grid">
            {TESTIMONIALS.map((t) => (
              <figure key={t.name} className="lp-testimonial">
                <StarRow count={t.stars} />
                <blockquote className="lp-testimonial-quote">"{t.quote}"</blockquote>
                <figcaption className="lp-testimonial-author">
                  <strong>{t.name}</strong>
                  <span>{t.role}</span>
                </figcaption>
              </figure>
            ))}
          </div>
        </div>
      </section>

      {/* ── Final CTA ───────────────────────────────────────────────────── */}
      <section className="lp-cta-section">
        <div className="lp-container lp-cta-inner">
          <span className="lp-cta-icon" aria-hidden="true">
            <Shield size={28} />
          </span>
          <h2 className="lp-cta-title">Ready to hire smarter?</h2>
          <p className="lp-cta-sub">
            Join thousands of companies who trust Sumployable to build world-class teams.
          </p>
          <Link to="/register" className="lp-btn lp-btn-primary lp-btn-lg">
            Get started — it&apos;s free
            <ArrowRight size={18} />
          </Link>
          <p className="lp-hero-note lp-cta-note">
            <CheckCircle size={14} />
            14-day trial &nbsp;·&nbsp; No credit card &nbsp;·&nbsp; Cancel anytime
          </p>
        </div>
      </section>
    </div>
  );
}
