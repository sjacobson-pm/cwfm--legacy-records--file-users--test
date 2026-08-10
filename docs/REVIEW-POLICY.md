# Code Review Policy

This document is the source of truth for how code review works in this repository. All other review controls — pre-commit hooks, CI checks, Copilot instructions, CODEOWNERS, and branch protection rules — are derived from the decisions recorded here.

If you are new to the repo, read this first. If something in a hook, CI job, or PR review seems surprising, this document explains why it exists.

---

## 1. Pre-commit hook

The local pre-commit hook runs before every commit and will **block the commit** if any of the following are found:

- **Secrets** — credentials, tokens, API keys, or anything that looks like a secret embedded in source files.
- **Banned patterns** — specific strings or file patterns that are explicitly prohibited in this repository (see the hook configuration for the current list).
- **Malformed commit messages** — commit messages that do not follow the required format (imperative verb, normal casing, e.g. `Add login validation` or `Fix null check in migration`).

Everything else the hook notices is **advisory only** — it will print a warning but will not stop the commit. You are responsible for deciding whether to act on advisory output before opening a PR.

---

## 2. CI checks

CI is the authoritative enforcement layer. The following checks run on every pull request and must pass before merge:

| Check | What it enforces |
|---|---|
| **Linting** | Code style and static analysis rules |
| **Tests** | Automated test suite (unit and integration) |
| **CodeQL** | Security vulnerability scanning |
| **Dependency review** | Flags newly introduced dependencies with known vulnerabilities |
| **Commit message format** | Validates every commit message in the PR against the required format |

CI owns these categories. Copilot PR review and human reviewers should not duplicate them — if something belongs to CI, CI catches it.

---

## 3. Copilot PR review

Copilot reviews every pull request automatically. It focuses on issues that automated tools are not well-positioned to catch:

**Copilot will comment on:**
- Logic errors — code that compiles and passes tests but does the wrong thing
- Security boundary mistakes — e.g. missing authorization checks, unsafe deserialization, incorrect trust boundaries
- API contract breaks — changes that alter a public interface in a way that could silently break callers

**Copilot will not comment on:**
- Code style or formatting (CI linting owns this)
- Test coverage percentages (CI owns this)
- Anything already enforced by a CI check

When Copilot flags something, treat it as you would a senior reviewer comment: consider it seriously, respond to it in the PR, and either fix it or explain why it does not apply.

---

## 4. Risk tiers

Every change in this repository falls into one of five risk tiers. The tier determines what review is required before merge.

### Tier 1 — Low risk, CI-only
Routine changes with limited blast radius. CI passing is sufficient.

**Examples:**
- `docs/**` (except governance documents such as `docs/REVIEW-POLICY.md`)
- `*.md`
- `tests/fixtures/**`
- Dependency version bumps with no API surface change
- Cosmetic refactors with full test coverage

> **Note:** Changes to `docs/REVIEW-POLICY.md` and other governance documents (CODEOWNERS, contributing guides) are treated as Tier 2 or higher because they affect how this repository is operated. When in doubt, apply Tier 2 and request a senior reviewer.

### Tier 2 — Feature logic, one senior reviewer
Changes that introduce or modify application behavior. Requires one senior developer to approve in addition to passing CI.

**Examples:**
- `src/**` (new features or modifications to existing business logic)
- `lib/**`
- New API endpoints with no auth or schema changes
- Configuration changes that affect runtime behavior

### Tier 3 — Planning documentation
Changes to planning documents that drive agent-generated code. Because an agentic developer will translate these documents directly into code, edits here have real downstream effects on the codebase. Requires approval from a business owner or architecture owner before merge.

**Examples:**
- `docs/planning/**`
- Any specification, architectural decision record, or design document that an agent is expected to implement
- Feature briefs or requirement files that describe intended system behavior

> **Why this matters:** When a developer (human or agent) reads a planning document and generates code from it, errors or ambiguities in the plan propagate into the implementation. A business or architecture reviewer catching a problem at the planning stage is far cheaper than catching it after code has been written and reviewed.

### Tier 4 — Auth, schema, migrations, regulated behavior
High-sensitivity changes that affect security boundaries, data integrity, or regulated functionality. Requires approval from a security owner or architecture owner in addition to CI and a senior reviewer.

**Examples:**
- `src/auth/**`, `src/authz/**`
- Database schema files (`*.sql`, `schema.*`, `migrations/**`)
- Any code in a regulated domain (e.g. financial calculations, audit trails, PII handling)
- Changes to access control logic or session management

### Tier 5 — Workflows, permissions, infra, CODEOWNERS
Repository-level controls and infrastructure. Requires both a platform owner and a security owner to approve.

**Examples:**
- `.github/workflows/**`
- `.github/CODEOWNERS`
- Infrastructure-as-code (`infra/**`, `terraform/**`, `*.tf`)
- Permission and role configuration files

---

## 5. Escalation triggers — immediate human review before merge

The following changes require a human reviewer to sign off **before** the PR can merge, regardless of tier assignment or CI status:

1. **Auth or authorization changes** — any modification to how users are authenticated or what they are permitted to do
2. **Schema or migration changes** — any change to database schemas or migration scripts
3. **Workflow or permissions changes** — any change to GitHub Actions workflows, repository permissions, or CODEOWNERS
4. **Public API contract changes** — any change that alters the interface consumed by external callers (renaming, removing, or changing the behavior of public endpoints or exported functions)
5. **Regulated-domain logic** — any change to code paths that handle regulated data or behavior (e.g. financial records, audit logs, PII)

If your change touches any of these areas, request a human review before you expect the PR to merge. Do not rely on CI passing as a signal that the change is ready.

---

## 6. Identifying agent-assisted commits

When a commit is authored or co-authored by the Copilot coding agent, it must include the following Git trailer in the commit message:

```
Co-authored-by: Copilot App <223556219+Copilot@users.noreply.github.com>
```

This trailer is the only required signal. No commit message prefix (such as `[bot]` or `copilot:`) is used.

Reviewers can search for this trailer to identify agent-assisted commits in the repository history. The presence of the trailer does not change the review requirements — the same tier rules and escalation triggers apply.

---

## 7. Weekly governance review

Once a week, an automated digest is generated from the previous week's merged pull requests. The digest is produced by a scheduled GitHub Actions workflow (`.github/workflows/governance-digest.yml`) that runs every Monday morning. The workflow:

1. Queries merged PRs from the previous seven days
2. Categorizes them by tier and flags escalation triggers and agent-assisted commits
3. Opens a new GitHub Issue titled `Governance digest — <date range>` and assigns it to the CODEOWNERS

The digest covers:

- PRs merged by tier
- Any Tier 4 or Tier 5 changes merged in the period
- Any escalation triggers that were activated
- Any agent-assisted PRs and their outcomes

The digest is published as a **GitHub Issue** in this repository and assigned to the CODEOWNERS. The assigned codeowners are responsible for:

1. Reviewing the digest within two business days
2. Flagging any merges that should not have proceeded under this policy
3. Closing the issue once the review is complete, or opening follow-up issues for any concerns

If you are a codeowner, watch for these weekly issues and treat them as a lightweight audit checkpoint, not a formality.

---

## Quick reference

| Layer | Blocks merge? | Scope |
|---|---|---|
| Pre-commit hook | Yes (secrets, banned patterns, bad commit message) | Local only |
| CI | Yes (lint, tests, CodeQL, dep review, commit format) | All PRs |
| Copilot PR review | No (advisory) | All PRs |
| Human reviewer | Yes (Tier 2+, escalation triggers) | As required by tier |

When in doubt about which tier applies, assume the higher tier and ask in the PR.
