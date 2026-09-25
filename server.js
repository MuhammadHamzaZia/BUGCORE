import express from 'express';
import path from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

const app = express();
const PORT = process.env.PORT || 3000;

app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(express.static('public'));

// User State (Clean Initial State)
let currentUser = {
  id: "usr-101",
  username: "developer_hamza",
  email: "developer@bugcore.io",
  role: "Developer",
  slackUserId: "U0812345678",
  slackUsername: "hamza.dev",
  notifyThreshold: "High",
  muteLowPriority: false,
  channelAlertsEnabled: true,
  dmAlertsEnabled: true
};

let botSettings = {
  botToken: "SLACK_BOT_TOKEN_PLACEHOLDER",
  signingSecret: "SLACK_SIGNING_SECRET_PLACEHOLDER",
  appId: "A08SLACKAPP01",
  defaultChannel: "#bugs-triage",
  installationStatus: "Installed & Authorized",
  hmacVerification: "Active (SHA-256)",
  activeScopes: [
    "chat:write",
    "commands",
    "channels:read",
    "im:write",
    "reactions:read",
    "users:read"
  ]
};

// CLEAN INITIAL STATE: NO PRE-CREATED DEMO PROJECTS OR ISSUES
let projectRoutings = [];
let issues = [];
let auditLogs = [];

// Navigation Layout HTML Helper
function renderPage(title, activeTab, content, flashMessage = null) {
  const isAdmin = currentUser.role === "Administrator";
  return `
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <title>${title} - BUGCORE MantisNetMvc</title>
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" />
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
  <style>
    :root {
      --mantis-topbar-bg: #0f172a;
      --mantis-sidebar-bg: #0b1120;
      --mantis-sidebar-hover: #1e293b;
      --mantis-sidebar-active: #0d9488;
      --mantis-brand-teal: #14b8a6;
    }
    body {
      background-color: #f8fafc;
      color: #0f172a;
      font-family: system-ui, -apple-system, sans-serif;
      min-height: 100vh;
    }
    .topbar-mantis {
      background-color: var(--mantis-topbar-bg);
      border-bottom: 1px solid #1e293b;
      height: 56px;
      z-index: 1030;
    }
    .sidebar-mantis {
      background-color: var(--mantis-sidebar-bg);
      width: 250px;
      min-height: calc(100vh - 56px);
      border-right: 1px solid #1e293b;
    }
    .sidebar-mantis .nav-link {
      color: #94a3b8;
      padding: 0.75rem 1.25rem;
      font-size: 0.925rem;
      display: flex;
      align-items: center;
      gap: 0.75rem;
      transition: all 0.15s ease;
      border-left: 3px solid transparent;
    }
    .sidebar-mantis .nav-link:hover {
      color: #f8fafc;
      background-color: var(--mantis-sidebar-hover);
    }
    .sidebar-mantis .nav-link.active {
      color: #ffffff;
      background-color: #1e293b;
      border-left-color: var(--mantis-brand-teal);
      font-weight: 600;
    }
    .user-badge {
      font-size: 0.7rem;
      padding: 0.2rem 0.5rem;
      border-radius: 4px;
      font-weight: 700;
      text-transform: uppercase;
    }
    .card-dark {
      background-color: #0f172a;
      border-color: #1e293b;
      color: #f8fafc;
    }
    .card-dark .card-header {
      background-color: #1e293b;
      border-color: #334155;
    }
    .btn-emerald {
      background-color: #10b981;
      color: #fff;
      border: none;
      font-weight: 600;
    }
    .btn-emerald:hover {
      background-color: #059669;
      color: #fff;
    }
    .btn-cyan {
      background-color: #0284c7;
      color: #fff;
      border: none;
      font-weight: 600;
    }
    .btn-cyan:hover {
      background-color: #0369a1;
      color: #fff;
    }
  </style>
</head>
<body class="d-flex flex-column">
  <!-- Top Navigation Header -->
  <header class="topbar-mantis d-flex align-items-center justify-content-between px-3 text-white sticky-top">
    <div class="d-flex align-items-center gap-3">
      <a href="/Dashboard" class="d-flex align-items-center text-white text-decoration-none fw-bold fs-5 gap-2">
        <i class="bi bi-bug-fill text-teal-400 text-info"></i>
        <span>BUGCORE</span>
        <span class="badge bg-slate-800 text-teal-400 border border-teal-600 font-monospace small px-2 py-1">.NET MVC</span>
      </a>
      <div class="vr bg-slate-700 mx-1 d-none d-md-block" style="height: 24px;"></div>
      <div class="d-none d-md-flex align-items-center gap-2">
        <span class="text-slate-400 small">Active Project:</span>
        <select class="form-select form-select-sm bg-slate-900 text-white border-slate-700" style="width: 220px;" onchange="alert('Switched project context to: ' + this.value)">
          ${projectRoutings.length > 0 
            ? projectRoutings.map(p => `<option value="${p.id}">${p.name}</option>`).join('')
            : '<option value="">No Active Projects</option>'
          }
        </select>
      </div>
    </div>

    <!-- User & Role Profile Header Control -->
    <div class="d-flex align-items-center gap-3">
      <div class="dropdown">
        <button class="btn btn-sm btn-outline-light dropdown-toggle d-flex align-items-center gap-2" type="button" data-bs-toggle="dropdown">
          <i class="bi bi-person-circle"></i>
          <span>${currentUser.username}</span>
          <span class="user-badge ${isAdmin ? 'bg-danger' : 'bg-info text-dark'}">${currentUser.role}</span>
        </button>
        <ul class="dropdown-menu dropdown-menu-end dropdown-menu-dark">
          <li><h6 class="dropdown-header">Role Switcher (Testing Mode)</h6></li>
          <li>
            <form action="/api/role/switch" method="POST" class="px-2 py-1">
              <input type="hidden" name="role" value="${isAdmin ? 'Developer' : 'Administrator'}" />
              <button type="submit" class="btn btn-sm w-100 ${isAdmin ? 'btn-outline-info' : 'btn-outline-danger'}">
                Switch to ${isAdmin ? 'Developer / Employee' : 'Administrator'}
              </button>
            </form>
          </li>
          <li><hr class="dropdown-divider"></li>
          <li><a class="dropdown-item" href="/Slack"><i class="bi bi-slack me-2"></i>My Slack Identity</a></li>
        </ul>
      </div>
    </div>
  </header>

  <div class="d-flex flex-grow-1">
    <!-- Left Navigation Sidebar -->
    <aside class="sidebar-mantis d-flex flex-column py-3">
      <a href="/Issue/Create" class="btn btn-emerald mx-3 mb-3 d-flex align-items-center justify-content-center gap-2 py-2">
        <i class="bi bi-plus-circle-fill"></i>
        <span>Report Defect</span>
      </a>

      <nav class="nav flex-column mb-auto">
        <a class="nav-link ${activeTab === 'dashboard' ? 'active' : ''}" href="/Dashboard">
          <i class="bi bi-speedometer2"></i> My View
        </a>
        <a class="nav-link ${activeTab === 'issues' ? 'active' : ''}" href="/Issue">
          <i class="bi bi-list-task"></i> View All Issues
        </a>
        <a class="nav-link ${activeTab === 'projects' ? 'active' : ''}" href="/Project">
          <i class="bi bi-diagram-3"></i> Manage Projects
        </a>

        <div class="px-3 pt-3 pb-1 text-uppercase text-slate-500 font-monospace" style="font-size: 0.7rem; letter-spacing: 0.05em;">
          Integrations & ChatOps
        </div>

        <a class="nav-link ${activeTab === 'slack-workbench' ? 'active' : ''}" href="/Slack">
          <i class="bi bi-slack text-emerald-400"></i> Slack ChatOps
        </a>

        ${isAdmin ? `
        <a class="nav-link ${activeTab === 'slack-admin' ? 'active' : ''}" href="/Slack/Admin">
          <i class="bi bi-shield-lock text-purple-400"></i> Slack Admin Portal
        </a>
        ` : ''}

        <a class="nav-link ${activeTab === 'git' ? 'active' : ''}" href="/GitWebhook">
          <i class="bi bi-github text-cyan-400"></i> GitHub Linker
        </a>
        <a class="nav-link ${activeTab === 'teams' ? 'active' : ''}" href="/Teams">
          <i class="bi bi-microsoft text-indigo-400"></i> MS Teams Connector
        </a>
        <a class="nav-link ${activeTab === 'email' ? 'active' : ''}" href="/EmailGateway">
          <i class="bi bi-envelope-at text-amber-400"></i> SMTP Gateway
        </a>

        ${isAdmin ? `
        <div class="px-3 pt-3 pb-1 text-uppercase text-slate-500 font-monospace" style="font-size: 0.7rem; letter-spacing: 0.05em;">
          Administration
        </div>
        <a class="nav-link ${activeTab === 'users' ? 'active' : ''}" href="/Admin/Users">
          <i class="bi bi-people"></i> User Accounts
        </a>
        ` : ''}
      </nav>

      <div class="px-3 pt-3 border-top border-slate-800 text-slate-400 small">
        <div class="d-flex align-items-center justify-content-between">
          <span>Slack Status:</span>
          <span class="badge bg-emerald-900 text-emerald-300 border border-emerald-700">Ready</span>
        </div>
      </div>
    </aside>

    <!-- Main Content Body -->
    <main class="flex-grow-1 p-4 bg-slate-950 text-slate-100 overflow-auto" style="min-width: 0;">
      ${flashMessage ? `
      <div class="alert alert-${flashMessage.type} alert-dismissible fade show mb-4" role="alert">
        <i class="bi ${flashMessage.type === 'success' ? 'bi-check-circle-fill' : 'bi-exclamation-triangle-fill'} me-2"></i>
        ${flashMessage.text}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
      </div>
      ` : ''}

      ${content}
    </main>
  </div>

  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
  `;
}

// -----------------------------------------------------------------------------
// ROUTES
// -----------------------------------------------------------------------------

app.get('/', (req, res) => res.redirect('/Dashboard'));

app.get('/Dashboard', (req, res) => {
  const content = `
    <div class="d-flex justify-content-between align-items-center mb-4 pb-3 border-bottom border-slate-800">
      <div>
        <h3 class="fw-bold mb-1"><i class="bi bi-speedometer2 text-info me-2"></i>My View Defect Dashboard</h3>
        <p class="text-slate-400 mb-0">Overview of assigned defects, project activity, and Slack ChatOps triggers</p>
      </div>
      <div class="d-flex gap-2">
        <a href="/Project" class="btn btn-sm btn-emerald"><i class="bi bi-diagram-3 me-1"></i>Add Project</a>
        <a href="/Issue/Create" class="btn btn-sm btn-cyan"><i class="bi bi-plus-lg me-1"></i>New Issue</a>
      </div>
    </div>

    <!-- Summary Counter Cards -->
    <div class="row g-3 mb-4">
      <div class="col-md-3">
        <div class="card card-dark p-3 shadow-sm border-start border-4 border-info">
          <div class="text-slate-400 small">Active Projects</div>
          <div class="fs-2 fw-bold text-white">${projectRoutings.length}</div>
        </div>
      </div>
      <div class="col-md-3">
        <div class="card card-dark p-3 shadow-sm border-start border-4 border-warning">
          <div class="text-slate-400 small">Total Open Issues</div>
          <div class="fs-2 fw-bold text-white">${issues.length}</div>
        </div>
      </div>
      <div class="col-md-3">
        <div class="card card-dark p-3 shadow-sm border-start border-4 border-success">
          <div class="text-slate-400 small">Resolved Defects</div>
          <div class="fs-2 fw-bold text-white">0</div>
        </div>
      </div>
      <div class="col-md-3">
        <div class="card card-dark p-3 shadow-sm border-start border-4 border-purple-500" style="border-color: #a855f7 !important;">
          <div class="text-slate-400 small">Slack Active Users</div>
          <div class="fs-2 fw-bold text-white">1</div>
        </div>
      </div>
    </div>

    <!-- Active Defect Grid -->
    <div class="card card-dark shadow-sm mb-4">
      <div class="card-header d-flex justify-content-between align-items-center py-3">
        <h5 class="fw-bold mb-0"><i class="bi bi-list-task text-teal-400 me-2"></i>Defect Triage Grid</h5>
        <a href="/Issue" class="btn btn-sm btn-outline-light">View All (${issues.length})</a>
      </div>
      <div class="card-body p-0">
        ${issues.length === 0 ? `
          <div class="text-center py-5 px-3">
            <i class="bi bi-clipboard-check text-slate-600 fs-1 mb-2 d-block"></i>
            <h5 class="fw-bold text-slate-300 mb-1">No Active Defects Found</h5>
            <p class="text-slate-400 small mb-3">Your workspace is completely clean. Click below to create your first defect or configure projects.</p>
            <div class="d-flex justify-content-center gap-2">
              <a href="/Project" class="btn btn-sm btn-outline-info"><i class="bi bi-diagram-3 me-1"></i>Create Project</a>
              <a href="/Issue/Create" class="btn btn-sm btn-emerald"><i class="bi bi-plus-circle me-1"></i>Report Defect</a>
            </div>
          </div>
        ` : `
          <div class="table-responsive">
            <table class="table table-dark table-hover mb-0 align-middle">
              <thead>
                <tr class="text-slate-400 small border-bottom border-slate-700">
                  <th>Issue Key</th>
                  <th>Summary</th>
                  <th>Project</th>
                  <th>Priority</th>
                  <th>Status</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                ${issues.map(iss => `
                  <tr>
                    <td class="fw-bold text-info font-monospace">${iss.id}</td>
                    <td class="fw-semibold text-white">${iss.title}</td>
                    <td><span class="badge bg-slate-800 text-slate-300 border border-slate-700">${iss.project}</span></td>
                    <td><span class="badge bg-warning text-dark">${iss.priority}</span></td>
                    <td><span class="badge bg-info text-dark">${iss.status}</span></td>
                    <td>
                      <button onclick="alert('Defect details: ${iss.title}')" class="btn btn-sm btn-outline-info py-1 px-2">
                        <i class="bi bi-eye"></i> Details
                      </button>
                    </td>
                  </tr>
                `).join('')}
              </tbody>
            </table>
          </div>
        `}
      </div>
    </div>
  `;
  res.send(renderPage("Dashboard", "dashboard", content));
});

// Employee Slack Workbench
app.get('/Slack', (req, res) => {
  const flash = req.query.msg ? { type: req.query.type || 'success', text: req.query.msg } : null;
  const isAdmin = currentUser.role === "Administrator";

  const content = `
    <div class="d-flex flex-wrap justify-content-between align-items-center mb-4 pb-3 border-bottom border-slate-800">
      <div>
        <div class="d-flex align-items-center gap-2">
          <i class="bi bi-slack text-emerald-400 fs-2"></i>
          <div>
            <h3 class="fw-bold mb-0">My Slack ChatOps Workbench</h3>
            <span class="text-slate-400 small">Link Personal Slack Identity, Customize Alert Thresholds, & View Slash Commands</span>
          </div>
        </div>
      </div>
      <div class="d-flex gap-2 align-items-center">
        <span class="badge bg-slate-800 text-emerald-400 border border-emerald-700 px-3 py-2">
          <i class="bi bi-person-check-fill me-1"></i> ${currentUser.role} Account
        </span>
        ${isAdmin ? `
          <a href="/Slack/Admin" class="btn btn-sm btn-purple text-white fw-bold shadow-sm" style="background-color: #8b5cf6;">
            <i class="bi bi-shield-lock-fill me-1"></i> Switch to Admin Governance Portal
          </a>
        ` : ''}
      </div>
    </div>

    <div class="row g-4">
      <div class="col-lg-6">
        <div class="card card-dark shadow">
          <div class="card-header py-3">
            <h5 class="fw-bold text-white mb-0"><i class="bi bi-link-45deg me-2 text-cyan-400"></i>Self-Service Identity Binding (OAuth)</h5>
            <small class="text-slate-400">Link your personal Slack profile Member ID to your BUGCORE user session.</small>
          </div>
          <div class="card-body p-4">
            <form action="/api/slack/bind-identity" method="POST">
              <div class="mb-3">
                <label class="form-label text-slate-300">Slack User ID (\`U...\` Member ID)</label>
                <input type="text" name="slackUserId" class="form-control bg-slate-900 border-slate-700 text-white font-monospace" value="${currentUser.slackUserId}" placeholder="e.g. U0812345678" required />
                <small class="text-slate-400">Found in Slack Profile -> More Actions -> Copy Member ID.</small>
              </div>
              <div class="mb-3">
                <label class="form-label text-slate-300">Slack Username</label>
                <div class="input-group">
                  <span class="input-group-text bg-slate-800 border-slate-700 text-slate-400">@</span>
                  <input type="text" name="slackUsername" class="form-control bg-slate-900 border-slate-700 text-white" value="${currentUser.slackUsername}" placeholder="username" required />
                </div>
              </div>
              <div class="d-flex align-items-center justify-content-between pt-2 border-top border-slate-800">
                <div class="text-emerald-400 small"><i class="bi bi-check-circle-fill me-1"></i>Identity Ready</div>
                <button type="submit" class="btn btn-emerald px-4"><i class="bi bi-save me-1"></i>Save Binding</button>
              </div>
            </form>
          </div>
        </div>
      </div>

      <div class="col-lg-6">
        <div class="card card-dark shadow">
          <div class="card-header py-3">
            <h5 class="fw-bold text-white mb-0"><i class="bi bi-bell-fill me-2 text-warning"></i>Personal Notification Thresholds</h5>
            <small class="text-slate-400">Control when Slack sends you direct messages and channel mentions.</small>
          </div>
          <div class="card-body p-4">
            <form action="/api/slack/update-preferences" method="POST">
              <div class="mb-3">
                <label class="form-label text-slate-300">Minimum Alert Priority Severity</label>
                <select name="notifyThreshold" class="form-select bg-slate-900 border-slate-700 text-white">
                  <option value="All">All Defect Notifications</option>
                  <option value="High" selected>High & Immediate Priority Only</option>
                  <option value="Immediate">Immediate Blockers Only</option>
                </select>
              </div>
              <div class="form-check form-switch mb-3">
                <input class="form-check-input" type="checkbox" name="muteLowPriority" id="muteLow">
                <label class="form-check-label text-slate-200" for="muteLow">Mute Low Priority Defect Broadcasts</label>
              </div>
              <div class="d-flex justify-content-end pt-2 border-top border-slate-800">
                <button type="submit" class="btn btn-cyan"><i class="bi bi-sliders me-1"></i>Save Notification Rules</button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  `;
  res.send(renderPage("Slack ChatOps Workbench", "slack-workbench", content, flash));
});

// Admin Slack Governance
app.get('/Slack/Admin', (req, res) => {
  if (currentUser.role !== "Administrator") {
    return res.status(430).send(renderPage("Access Denied", "slack-admin", `
      <div class="alert alert-danger p-4 my-4">
        <h4 class="fw-bold"><i class="bi bi-shield-x me-2"></i>Access Level Restricted</h4>
        <p class="mb-0">The Slack Admin Governance Portal is strictly reserved for Administrators. Switch your role using the top-right profile dropdown to view this portal.</p>
      </div>
    `));
  }

  const flash = req.query.msg ? { type: req.query.type || 'success', text: req.query.msg } : null;

  const content = `
    <div class="d-flex flex-wrap justify-content-between align-items-center mb-4 pb-3 border-bottom border-slate-800">
      <div>
        <div class="d-flex align-items-center gap-2">
          <i class="bi bi-shield-lock-fill text-purple-400 fs-2"></i>
          <div>
            <h3 class="fw-bold mb-0">Slack ChatOps Admin Governance Portal</h3>
            <span class="text-slate-400 small">Workspace OAuth Tokens, Channel-to-Project Routing, and HMAC-SHA256 Security Compliance</span>
          </div>
        </div>
      </div>
      <div class="d-flex gap-2 align-items-center">
        <span class="badge bg-purple-900 text-purple-200 border border-purple-700 px-3 py-2" style="background-color: #4c1d95; color: #ddd6fe;">
          <i class="bi bi-shield-check me-1"></i> Admin Governance Mode
        </span>
        <a href="/Slack" class="btn btn-sm btn-outline-light">
          <i class="bi bi-person-workspace me-1"></i> Developer Workbench
        </a>
      </div>
    </div>

    <div class="card card-dark shadow mb-4">
      <div class="card-header py-3">
        <h5 class="fw-bold text-white mb-0"><i class="bi bi-diagram-3 me-2 text-emerald-400"></i>Central Project-to-Channel Broadcasting Matrix</h5>
        <small class="text-slate-400">Map internal BUGCORE project defect broadcasts to target Slack channels.</small>
      </div>
      <div class="card-body p-4">
        ${projectRoutings.length === 0 ? `
          <div class="text-center py-4">
            <p class="text-slate-400 mb-2">No projects configured in matrix. Create a project first to bind Slack channels.</p>
            <a href="/Project" class="btn btn-sm btn-emerald"><i class="bi bi-plus-lg me-1"></i>Create New Project</a>
          </div>
        ` : `
          <form action="/api/slack/save-routing" method="POST">
            <div class="table-responsive">
              <table class="table table-dark table-hover align-middle border-slate-800">
                <thead>
                  <tr class="text-slate-400 small">
                    <th>Internal Project Name</th>
                    <th>Target Slack Channel</th>
                    <th>Broadcasting Status</th>
                  </tr>
                </thead>
                <tbody>
                  ${projectRoutings.map((p) => `
                    <tr>
                      <td class="fw-bold text-white">${p.name}</td>
                      <td>
                        <input type="text" name="channels[${p.id}]" class="form-control form-control-sm bg-slate-900 border-slate-700 text-white font-monospace" value="${p.channel}" required />
                      </td>
                      <td>
                        <div class="form-check form-switch">
                          <input class="form-check-input" type="checkbox" name="broadcast[${p.id}]" ${p.broadcast ? 'checked' : ''}>
                          <label class="form-check-label text-slate-300 small">Enabled</label>
                        </div>
                      </td>
                    </tr>
                  `).join('')}
                </tbody>
              </table>
            </div>
            <div class="d-flex justify-content-end mt-3">
              <button type="submit" class="btn btn-emerald px-4"><i class="bi bi-save me-1"></i>Save Channel Matrix</button>
            </div>
          </form>
        `}
      </div>
    </div>
  `;
  res.send(renderPage("Slack Admin Governance", "slack-admin", content, flash));
});

// View Issues Page
app.get('/Issue', (req, res) => {
  const content = `
    <div class="d-flex justify-content-between align-items-center mb-4 pb-3 border-bottom border-slate-800">
      <div>
        <h3 class="fw-bold mb-1"><i class="bi bi-list-task text-teal-400 me-2"></i>View All Issues Grid</h3>
        <p class="text-slate-400 mb-0">Search, filter, and transition defect states across all active projects</p>
      </div>
      <a href="/Issue/Create" class="btn btn-emerald"><i class="bi bi-plus-lg me-1"></i>Report Defect</a>
    </div>

    <div class="card card-dark shadow-sm">
      <div class="card-body p-0">
        ${issues.length === 0 ? `
          <div class="text-center py-5">
            <i class="bi bi-inbox text-slate-600 fs-1 mb-2 d-block"></i>
            <h5 class="fw-bold text-slate-300">No Defects In System</h5>
            <p class="text-slate-400 small mb-3">There are currently no issues reported.</p>
            <a href="/Issue/Create" class="btn btn-emerald"><i class="bi bi-plus-circle me-1"></i>File New Defect</a>
          </div>
        ` : `
          <div class="table-responsive">
            <table class="table table-dark table-hover mb-0 align-middle">
              <thead>
                <tr class="text-slate-400 small border-bottom border-slate-700">
                  <th>Key</th>
                  <th>Summary</th>
                  <th>Project</th>
                  <th>Priority</th>
                  <th>Status</th>
                  <th>Assignee</th>
                </tr>
              </thead>
              <tbody>
                ${issues.map(iss => `
                  <tr>
                    <td class="fw-bold text-info font-monospace">${iss.id}</td>
                    <td class="fw-semibold text-white">${iss.title}</td>
                    <td><span class="badge bg-slate-800 text-slate-300 border border-slate-700">${iss.project}</span></td>
                    <td><span class="badge bg-warning text-dark">${iss.priority}</span></td>
                    <td><span class="badge bg-info text-dark">${iss.status}</span></td>
                    <td class="text-slate-300">${iss.assignee}</td>
                  </tr>
                `).join('')}
              </tbody>
            </table>
          </div>
        `}
      </div>
    </div>
  `;
  res.send(renderPage("View Issues", "issues", content));
});

// Create Issue Page
app.get('/Issue/Create', (req, res) => {
  const content = `
    <div class="max-w-2xl">
      <div class="d-flex align-items-center gap-2 mb-4">
        <a href="/Issue" class="btn btn-sm btn-outline-light"><i class="bi bi-arrow-left"></i> Back</a>
        <h3 class="fw-bold mb-0">Report New Defect Record</h3>
      </div>

      <div class="card card-dark shadow">
        <div class="card-body p-4">
          <form action="/api/issues" method="POST">
            <div class="mb-3">
              <label class="form-label text-slate-300">Project Name</label>
              ${projectRoutings.length > 0 ? `
                <select name="project" class="form-select bg-slate-900 border-slate-700 text-white">
                  ${projectRoutings.map(p => `<option value="${p.name}">${p.name}</option>`).join('')}
                </select>
              ` : `
                <input type="text" name="project" class="form-control bg-slate-900 border-slate-700 text-white" placeholder="Project Name" required />
              `}
            </div>
            <div class="mb-3">
              <label class="form-label text-slate-300">Summary / Title</label>
              <input type="text" name="title" class="form-control bg-slate-900 border-slate-700 text-white" placeholder="Brief summary of the issue" required />
            </div>
            <div class="row g-3 mb-3">
              <div class="col-md-6">
                <label class="form-label text-slate-300">Priority</label>
                <select name="priority" class="form-select bg-slate-900 border-slate-700 text-white">
                  <option value="Normal">Normal</option>
                  <option value="High">High</option>
                  <option value="Immediate">Immediate</option>
                </select>
              </div>
              <div class="col-md-6">
                <label class="form-label text-slate-300">Category</label>
                <input type="text" name="category" class="form-control bg-slate-900 border-slate-700 text-white" value="General Defect" />
              </div>
            </div>
            <div class="mb-3">
              <label class="form-label text-slate-300">Detailed Description</label>
              <textarea name="description" rows="4" class="form-control bg-slate-900 border-slate-700 text-white" placeholder="Steps to reproduce, stack trace, or details..."></textarea>
            </div>
            <div class="d-flex justify-content-end gap-2">
              <a href="/Issue" class="btn btn-outline-light">Cancel</a>
              <button type="submit" class="btn btn-emerald px-4"><i class="bi bi-check-circle me-1"></i>Submit Defect Record</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  `;
  res.send(renderPage("Report Issue", "issues", content));
});

// Manage Projects Page
app.get('/Project', (req, res) => {
  const flash = req.query.msg ? { type: req.query.type || 'success', text: req.query.msg } : null;

  const content = `
    <div class="d-flex justify-content-between align-items-center mb-4 pb-3 border-bottom border-slate-800">
      <div>
        <h3 class="fw-bold mb-1"><i class="bi bi-diagram-3 text-info me-2"></i>Project Hierarchy & Management</h3>
        <p class="text-slate-400 mb-0">Create and configure organizational projects and Slack broadcasting channels</p>
      </div>
    </div>

    <!-- Create Project Form Card -->
    <div class="card card-dark shadow mb-4">
      <div class="card-header py-3">
        <h5 class="fw-bold text-white mb-0"><i class="bi bi-plus-circle text-emerald-400 me-2"></i>Create New Organizational Project</h5>
      </div>
      <div class="card-body p-4">
        <form action="/api/projects/create" method="POST">
          <div class="row g-3">
            <div class="col-md-6">
              <label class="form-label text-slate-300">Project Name</label>
              <input type="text" name="projectName" class="form-control bg-slate-900 border-slate-700 text-white" placeholder="e.g. Core Software Suite" required />
            </div>
            <div class="col-md-6">
              <label class="form-label text-slate-300">Target Slack Channel</label>
              <input type="text" name="slackChannel" class="form-control bg-slate-900 border-slate-700 text-white font-monospace" placeholder="e.g. #dev-alerts" required />
            </div>
          </div>
          <div class="d-flex justify-content-end mt-3">
            <button type="submit" class="btn btn-emerald px-4"><i class="bi bi-folder-plus me-1"></i>Create Project</button>
          </div>
        </form>
      </div>
    </div>

    <!-- Active Projects List -->
    <div class="card card-dark shadow">
      <div class="card-header py-3">
        <h5 class="fw-bold text-white mb-0">Existing Projects (${projectRoutings.length})</h5>
      </div>
      <div class="card-body p-4">
        ${projectRoutings.length === 0 ? `
          <div class="text-center py-4 text-slate-400">
            <i class="bi bi-folder2-open fs-2 text-slate-600 mb-2 d-block"></i>
            No projects exist yet. Use the form above to add your first project.
          </div>
        ` : `
          <div class="row g-3">
            ${projectRoutings.map(p => `
              <div class="col-md-6">
                <div class="p-3 bg-slate-900 rounded border border-slate-800 d-flex justify-content-between align-items-center">
                  <div>
                    <h6 class="fw-bold text-white mb-1">${p.name}</h6>
                    <small class="text-emerald-400 font-monospace"><i class="bi bi-slack me-1"></i>${p.channel}</small>
                  </div>
                  <span class="badge bg-slate-800 text-info border border-slate-700">${p.activeIssues} Issues</span>
                </div>
              </div>
            `).join('')}
          </div>
        `}
      </div>
    </div>
  `;
  res.send(renderPage("Projects", "projects", content, flash));
});

// GitHub Linker
app.get('/GitWebhook', (req, res) => {
  const content = `
    <div class="mb-4 pb-3 border-bottom border-slate-800">
      <h3 class="fw-bold mb-1"><i class="bi bi-github text-cyan-400 me-2"></i>GitHub Commit Linker</h3>
      <p class="text-slate-400 mb-0">Auto-link git commits and pull requests to BUGCORE issue keys</p>
    </div>
    <div class="card card-dark p-4">
      <h5 class="fw-bold text-white mb-2">Webhook Receiver Endpoint</h5>
      <div class="p-3 bg-slate-900 border border-slate-800 rounded font-monospace text-cyan-400 mb-3">
        https://your-domain.com/api/git/webhook
      </div>
    </div>
  `;
  res.send(renderPage("GitHub Linker", "git", content));
});

// MS Teams
app.get('/Teams', (req, res) => {
  res.send(renderPage("MS Teams Connector", "teams", `
    <div class="mb-4 pb-3 border-bottom border-slate-800">
      <h3 class="fw-bold mb-1"><i class="bi bi-microsoft text-indigo-400 me-2"></i>Microsoft Teams Connector</h3>
      <p class="text-slate-400">Adaptive Card notifications for Microsoft Teams webhooks</p>
    </div>
    <div class="card card-dark p-4">
      <h5 class="fw-bold text-white mb-2">Connector Ready</h5>
      <p class="text-slate-300">Ready to accept MS Teams Webhook URLs upon project creation.</p>
    </div>
  `));
});

// SMTP Gateway
app.get('/EmailGateway', (req, res) => {
  res.send(renderPage("SMTP Gateway", "email", `
    <div class="mb-4 pb-3 border-bottom border-slate-800">
      <h3 class="fw-bold mb-1"><i class="bi bi-envelope-at text-amber-400 me-2"></i>SMTP Email Gateway</h3>
      <p class="text-slate-400">Email notifications and inbound email ticket creation</p>
    </div>
    <div class="card card-dark p-4">
      <h5 class="fw-bold text-white mb-2">SMTP Gateway Ready</h5>
    </div>
  `));
});

// Admin Users
app.get('/Admin/Users', (req, res) => {
  if (currentUser.role !== "Administrator") return res.redirect('/Dashboard');
  res.send(renderPage("User Accounts", "users", `
    <div class="mb-4 pb-3 border-bottom border-slate-800">
      <h3 class="fw-bold mb-1"><i class="bi bi-people text-danger me-2"></i>User Accounts</h3>
    </div>
    <div class="card card-dark p-4">
      <table class="table table-dark table-hover mb-0">
        <thead><tr class="text-slate-400 small"><th>User</th><th>Email</th><th>Role</th></tr></thead>
        <tbody>
          <tr><td class="fw-bold text-white">${currentUser.username}</td><td>${currentUser.email}</td><td><span class="badge bg-danger">${currentUser.role}</span></td></tr>
        </tbody>
      </table>
    </div>
  `));
});

// -----------------------------------------------------------------------------
// API ACTIONS
// -----------------------------------------------------------------------------

// Create Project API
app.post('/api/projects/create', (req, res) => {
  const { projectName, slackChannel } = req.body;
  if (projectName) {
    projectRoutings.push({
      id: projectRoutings.length + 1,
      name: projectName,
      channel: slackChannel || '#dev-alerts',
      broadcast: true,
      autoThread: true,
      activeIssues: 0
    });
  }
  res.redirect('/Project?msg=' + encodeURIComponent(`Project '${projectName}' created successfully!`) + '&type=success');
});

// Bind Identity API
app.post('/api/slack/bind-identity', (req, res) => {
  const { slackUserId, slackUsername } = req.body;
  if (slackUserId) currentUser.slackUserId = slackUserId;
  if (slackUsername) currentUser.slackUsername = slackUsername;
  res.redirect('/Slack?msg=' + encodeURIComponent('Slack OAuth Identity bound successfully!') + '&type=success');
});

// Update Preferences API
app.post('/api/slack/update-preferences', (req, res) => {
  res.redirect('/Slack?msg=' + encodeURIComponent('Personal notification threshold settings updated.') + '&type=success');
});

// Save Routing API
app.post('/api/slack/save-routing', (req, res) => {
  const { channels, broadcast } = req.body;
  if (channels) {
    projectRoutings.forEach(p => {
      if (channels[p.id]) p.channel = channels[p.id];
      p.broadcast = !!(broadcast && broadcast[p.id]);
    });
  }
  res.redirect('/Slack/Admin?msg=' + encodeURIComponent('Channel routing matrix successfully saved.') + '&type=success');
});

// Create Issue API
app.post('/api/issues', (req, res) => {
  const { project, title, priority, category, description } = req.body;
  const newId = `BUG-${1001 + issues.length}`;
  issues.unshift({
    id: newId,
    title: title || 'Untitled Issue',
    project: project || 'General Project',
    category: category || 'General Defect',
    priority: priority || 'Normal',
    severity: 'Major',
    status: 'New',
    reporter: currentUser.username,
    assignee: 'Unassigned',
    created: new Date().toISOString().replace('T', ' ').substring(0, 16),
    description: description || ''
  });

  // Increment active issues counter on project if exists
  const projObj = projectRoutings.find(p => p.name === project);
  if (projObj) projObj.activeIssues += 1;

  res.redirect('/Issue');
});

// Role Switch Handler
app.post('/api/role/switch', (req, res) => {
  const { role } = req.body;
  if (role) currentUser.role = role;
  if (role === 'Administrator') {
    res.redirect('/Slack/Admin?msg=' + encodeURIComponent('Switched to Administrator mode.') + '&type=success');
  } else {
    res.redirect('/Slack?msg=' + encodeURIComponent('Switched to Developer mode.') + '&type=success');
  }
});

app.get('*', (req, res) => res.redirect('/Dashboard'));

app.listen(PORT, () => {
  console.log(`BUGCORE MantisNetMvc platform server running on port ${PORT}`);
});
