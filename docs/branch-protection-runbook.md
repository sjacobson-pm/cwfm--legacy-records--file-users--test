# Branch Protection Runbook — `main`

This runbook provides everything a repository administrator needs to create and maintain the branch protection ruleset for the `main` branch. It is the implementation companion to Issue #3.

---

## 1. Ruleset configuration table

Create a single **Repository ruleset** named `protect-main` targeting the `main` branch.

| # | Setting | Value | Rationale |
|---|---------|-------|-----------|
| 1 | **Ruleset name** | `protect-main` | Descriptive, scoped to this branch |
| 2 | **Enforcement status** | Active | Ruleset must be enforced immediately |
| 3 | **Target branches** | `main` (default branch, ref pattern `refs/heads/main`) | Pilot scope — `main` only |
| 4 | **Bypass actors** | See [Bypass policy](#4-bypass-policy) below | Emergency bypass only |
| 5 | **Require a pull request before merging** | ✅ Enabled | Prevents direct pushes to `main` |
| 6 | — Required approvals | `1` | At least one human approval required |
| 7 | — Require review from Code Owners | ✅ Enabled | CODEOWNERS file governs which owners must review |
| 8 | — Dismiss stale pull request approvals when new commits are pushed | ✅ Enabled | Prevents approval laundering after code changes |
| 9 | — Require approval of the most recent reviewable push | ✅ Enabled | Complements stale-approval dismissal |
| 10 | — Require conversation resolution before merging | ✅ Enabled | Open review threads must be resolved before merge |
| 11 | **Require status checks to pass** | ✅ Enabled | CI must be green before merge |
| 12 | — Require branches to be up to date before merging | ✅ Enabled | Prevents untested merge-base scenarios |
| 13 | — Required status check: `lint` | Add when workflow exists | See [Deferred checks](#deferred-status-checks) |
| 14 | — Required status check: `tests` | Add when workflow exists | See [Deferred checks](#deferred-status-checks) |
| 15 | — Required status check: `CodeQL` | Add when workflow exists | See [Deferred checks](#deferred-status-checks) |
| 16 | — Required status check: `commit-message-validation` | Add when workflow exists | See [Deferred checks](#deferred-status-checks) |
| 17 | — Required status check: `pr-compliance-check` | Add when workflow exists | See [Deferred checks](#deferred-status-checks) |
| 18 | **Require linear history** | ✅ Enabled | Keeps `git log` readable; enforces squash or rebase merge |
| 19 | **Restrict force pushes** | ✅ Enabled | Prevents history rewriting on `main` |
| 20 | **Restrict deletions** | ✅ Enabled | Prevents accidental or malicious branch deletion |
| 21 | **Allowed merge methods** | Squash merge only | Keeps linear history; one commit per PR on `main` |

> **Note on merge methods:** GitHub's merge-method restriction (squash-only) is enforced at the **repository settings** level under *Settings → General → Pull Requests*, not in the ruleset itself. Set "Allow squash merging" to enabled and disable "Allow merge commits" and "Allow rebase merging."

---

## 2. Admin runbook — step-by-step click path

### Prerequisites

- You must have **Admin** or **Owner** role on the repository.
- A `.github/CODEOWNERS` file must exist (or be created) for Code Owner review to function.

### Step 1 — Restrict allowed merge methods (repository settings)

1. Go to **Settings → General → Pull Requests**.
2. Under *Allowed merge methods*:
   - ✅ Allow squash merging
   - ❌ Allow merge commits (disable)
   - ❌ Allow rebase merging (disable)
3. Set the squash commit title to *Pull request title* and body to *Pull request body*.
4. Click **Save**.

### Step 2 — Create the ruleset

1. Go to **Settings → Rules → Rulesets**.
2. Click **New ruleset → New branch ruleset**.
3. Fill in the form:

   **Ruleset name:** `protect-main`  
   **Enforcement status:** Active

4. Under **Bypass list**, add bypass actors per the [Bypass policy](#4-bypass-policy) section. Leave empty if no emergency bypass is needed at launch.

5. Under **Target branches**, click **Add target → Include by pattern** and enter `main`.

6. Under **Rules**, enable and configure each rule:

   | Rule | Action |
   |------|--------|
   | **Restrict deletions** | Enable |
   | **Require linear history** | Enable |
   | **Require a pull request before merging** | Enable → set *Required approvals* to `1` → enable *Dismiss stale pull request approvals when new commits are pushed* → enable *Require review from Code Owners* → enable *Require approval of the most recent reviewable push* → enable *Require conversation resolution before merging* |
   | **Require status checks to pass** | Enable → enable *Require branches to be up to date before merging* → leave status check list empty for now (see [Deferred checks](#deferred-status-checks)) |
   | **Block force pushes** | Enable |

7. Click **Create**.

### Step 3 — Verify the ruleset is active

1. Navigate to **Settings → Rules → Rulesets**.
2. Confirm `protect-main` shows **Active** status and targets `refs/heads/main`.
3. Open (or create) a test PR targeting `main` and confirm the merge button is blocked.

---

## 3. Deferred status checks

The following status checks must be added to the ruleset **after** the corresponding GitHub Actions workflows are created and have run at least once on a PR (a check must have appeared in the UI at least once before GitHub allows it to be added as a required check).

| Check name | Workflow file (expected) | Action when ready |
|---|---|---|
| `lint` | `.github/workflows/lint.yml` | Settings → Rules → `protect-main` → Edit → Require status checks → Add check → type `lint` |
| `tests` | `.github/workflows/tests.yml` | Same path → Add check → type `tests` |
| `CodeQL` | `.github/workflows/codeql.yml` | Same path → Add check → type `CodeQL` |
| `commit-message-validation` | `.github/workflows/commit-validation.yml` | Same path → Add check → type `commit-message-validation` |
| `pr-compliance-check` | `.github/workflows/pr-compliance.yml` | Same path → Add check → type `pr-compliance-check` |

**How to handle "check not yet available" safely:**

- Enable the *Require status checks to pass* rule now with an **empty** check list. This arms the gate without blocking PRs on nonexistent checks.
- Track each deferred check as a follow-up issue referencing Issue #3.
- Once a workflow has run on at least one PR, add its job name to the required checks list and confirm the gate blocks merges when the check fails.

---

## 4. Bypass policy

The ruleset applies to **all actors including administrators** by default (no implicit admin bypass in GitHub rulesets, unlike legacy branch protection rules).

Emergency bypass is granted only in the following circumstances:

| Actor / role | Bypass scope | When allowed | Recording requirement |
|---|---|---|---|
| Repository administrator (designated on-call) | All rules | Declared production incident only | Must open a follow-up issue within 24 hours documenting: bypassed rule, reason, PR/commit SHA, and outcome |
| Repository owner | All rules | Declared production incident only | Same as above |

**How to add a bypass actor:**

1. Go to **Settings → Rules → Rulesets → `protect-main` → Edit**.
2. Under **Bypass list**, click **Add bypass**.
3. Select *Role: Repository admin* (or a specific team/user).
4. Set bypass mode to **Always** only if on-call admin bypass is required; otherwise use **Pull requests only**.
5. Click **Save changes**.

> **Recommendation:** Start with **no bypass actors**. Add only if an incident forces the issue, and document immediately.

---

## 5. CODEOWNERS reminder

For *Require review from Code Owners* to take effect, the repository must have a valid `.github/CODEOWNERS` file. If one does not exist:

1. Create `.github/CODEOWNERS` with at minimum:

   ```
   # All files — require review from the default code owner(s)
   * @<owner-username-or-team>
   ```

2. Commit and merge it to `main` **before** the ruleset is activated (or it will block itself).

---

## 6. Follow-up issues to open

After the ruleset is created, open the following tracking issues:

| Issue title | Trigger |
|---|---|
| Add `lint` as required status check | When `lint` workflow is merged and has run on a PR |
| Add `tests` as required status check | When `tests` workflow is merged and has run on a PR |
| Add `CodeQL` as required status check | When `codeql.yml` workflow is merged and has run on a PR |
| Add `commit-message-validation` as required status check | When commit validation workflow is merged |
| Add `pr-compliance-check` as required status check | When PR compliance workflow is merged |
