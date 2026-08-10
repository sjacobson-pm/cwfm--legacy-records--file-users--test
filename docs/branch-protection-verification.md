# Branch Protection Verification — `main`

This document provides test scenarios and an evidence template for verifying that the `protect-main` ruleset is functioning correctly. Use it after applying the ruleset per the [runbook](branch-protection-runbook.md).

---

## 1. Verification checklist

Run each scenario below and record the result in the [Evidence template](#2-evidence-template).

### Scenario A — Merge blocked without approval

**Goal:** Confirm that a PR cannot be merged before any approval is given.

1. Open a PR targeting `main` from a feature branch with at least one commit.
2. Do **not** request or add any reviewers.
3. Attempt to merge the PR.

**Expected:** Merge button is disabled. GitHub displays: *"Review required — At least 1 approving review is required."*

---

### Scenario B — Stale approval dismissed after new commit

**Goal:** Confirm that pushing a new commit after an approval invalidates the approval.

1. Open a PR targeting `main`.
2. Have a reviewer approve the PR.
3. Confirm the approval is shown as accepted.
4. Push an additional commit to the PR branch.
5. Check the PR review status.

**Expected:** The previous approval is dismissed and the PR shows *"Changes requested"* or is reset to *"Review required."* The PR cannot be merged until re-approved.

---

### Scenario C — Merge blocked on failing status check

> **Note:** This scenario requires at least one required status check to be configured in the ruleset. If no checks have been added yet, defer this scenario and track it as a follow-up.

**Goal:** Confirm that a failing CI check blocks the merge button.

1. Open a PR targeting `main`.
2. Introduce a deliberate failure (e.g. a lint error or failing test) that triggers a required check to fail.
3. Wait for the check to complete and report failure.
4. Attempt to merge the PR (even with an approval present).

**Expected:** Merge button is disabled. GitHub displays: *"Some checks were not successful."*

---

### Scenario D — Merge blocked with unresolved conversation

**Goal:** Confirm that an open review thread prevents merge.

1. Open a PR targeting `main`.
2. Have a reviewer add a review comment that starts a conversation (not a blocking review, just a comment thread).
3. Do **not** resolve the conversation.
4. Approve the PR separately.
5. Attempt to merge.

**Expected:** Merge button is disabled. GitHub displays: *"X conversation(s) must be resolved before merging."*

---

### Scenario E — Force push blocked

**Goal:** Confirm that force pushing to `main` is rejected.

1. On a local clone, attempt to force push to `main`:

   ```bash
   git push --force origin main
   ```

**Expected:** Push is rejected with: *"remote: error: GH013: Repository rule violations found for refs/heads/main."*

---

### Scenario F — Branch deletion blocked

**Goal:** Confirm that deleting the `main` branch is rejected.

1. On a local clone, attempt to delete `main` remotely:

   ```bash
   git push origin --delete main
   ```

**Expected:** Push is rejected with a branch protection / ruleset violation error.

---

### Scenario G — Direct push blocked (no PR)

**Goal:** Confirm that pushing a commit directly to `main` without a PR is rejected.

1. On a local clone with commit(s) not on `main`, attempt a direct push:

   ```bash
   git push origin HEAD:main
   ```

**Expected:** Push is rejected. GitHub requires a pull request.

---

## 2. Evidence template

Copy this template into the body of Issue #3 (or a linked comment) when closing the issue.

```markdown
## Branch protection verification evidence

**Ruleset name:** protect-main  
**Applied by:** @<github-username>  
**Applied on:** YYYY-MM-DD  
**Ruleset export / screenshot:** [attach or link]

---

### Scenario results

| Scenario | Result | Evidence link / screenshot | Notes |
|---|---|---|---|
| A — Merge blocked without approval | ✅ Pass / ❌ Fail | [link] | |
| B — Stale approval dismissed | ✅ Pass / ❌ Fail | [link] | |
| C — Merge blocked on failing check | ✅ Pass / ❌ Fail / ⏳ Deferred | [link] | Deferred until check workflow exists |
| D — Merge blocked with unresolved conversation | ✅ Pass / ❌ Fail | [link] | |
| E — Force push blocked | ✅ Pass / ❌ Fail | [link] | |
| F — Branch deletion blocked | ✅ Pass / ❌ Fail | [link] | |
| G — Direct push blocked | ✅ Pass / ❌ Fail | [link] | |

---

### Ruleset configuration dump

Paste the JSON export of the ruleset (GitHub API or UI export) here, or attach a screenshot of each settings section:

- [ ] General settings (name, enforcement, target)
- [ ] Pull request rules (approvals, stale dismissal, CODEOWNERS, conversation resolution)
- [ ] Status check rules (list of required checks, up-to-date requirement)
- [ ] Linear history setting
- [ ] Force push / deletion restriction
- [ ] Bypass list (empty or documented)

---

### Deferred decisions / follow-up issues

| Item | Follow-up issue | Status |
|---|---|---|
| Add `lint` as required status check | #<issue-number> | Open |
| Add `tests` as required status check | #<issue-number> | Open |
| Add `CodeQL` as required status check | #<issue-number> | Open |
| Add `commit-message-validation` as required status check | #<issue-number> | Open |
| Add `pr-compliance-check` as required status check | #<issue-number> | Open |

---

### Admin sign-off

- [ ] All non-deferred scenarios passed
- [ ] Deferred scenarios tracked in follow-up issues
- [ ] Ruleset configuration matches the runbook table
- [ ] This evidence reviewed and approved by a second admin or security owner
```
