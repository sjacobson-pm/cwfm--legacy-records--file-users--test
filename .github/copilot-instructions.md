# Copilot review instructions

Use review comments for high-signal, high-confidence risks in this pull request.

## Focus on these findings
- Logic or behavioral defects that can break expected outcomes.
- Security boundary mistakes (auth, permissions, secrets handling, trust boundaries).
- API contract break risks (input/output shape, required fields, compatibility assumptions).
- Data integrity risks (corruption, loss, bad migrations, unsafe state transitions).
- Risky changes in sensitive areas such as authentication, workflows, permissions, and migrations.

## Do not comment on these
- Formatting or style issues already enforced by linters/formatters.
- Naming preferences when behavior is still correct.
- Low-impact micro-optimizations.
- Pre-existing unrelated code outside the changed diff.

## Scope of review
- Prioritize changed lines first.
- Expand to nearby context only when it is directly needed to validate correctness or security.

## Confidence threshold
- Comment only when confidence is high.
- Keep comments actionable: explain the concrete risk and the specific fix direction.
- Avoid speculative or "might be better" suggestions.

## Reduce repeat noise
- Do not re-surface previously accepted low-priority patterns.
- Treat CI-owned categories (format, lint, basic style) as CI responsibility unless you see additional correctness or security risk.

## Repository conventions for Copilot-generated commits/PR text
- Commit and PR subjects must start with an imperative, capitalized verb.
- Keep subjects at or under 70 characters.
- Do not use prefix conventions such as `[agent]:` or `[chore]:`.
- A `Co-authored-by: Copilot App <223556219+Copilot@users.noreply.github.com>` trailer is allowed when applicable.

## Agent contributor rules

See `CONTRIBUTING.md` for the full contributor guide.
Key rules for automated contributors:

- **Never create placeholder planning commits** such as
  `Initial plan`, `WIP`, or `Add placeholder`.
  All planning belongs in issue comments, PR descriptions,
  or agent reasoning — not in commit history.
- Every commit subject must follow the capitalized-
  imperative-verb rule and the 70-character limit above.
- Commit bodies must be separated from the subject by a
  blank line and wrap at 70 characters per line.
- Before considering work complete, verify all required
  status checks pass:
  - `lint` (`.github/workflows/lint.yml`)
  - `tests` (`.github/workflows/tests.yml`)
  - `commit-message-validation`
  - `pr-compliance`
- Populate all required PR template fields
  (Description, Related Issue, Types of changes, Checklist)
  before requesting review.
