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

## Contributor workflow requirements
- Read `CONTRIBUTING.md` before starting work and follow it.
- Inspect required status checks before handoff and ensure your own PR
  passes:
  - `lint`
  - `tests`
  - `commit-message-validation`
  - `pr-compliance`
- Do not consider work complete if any required check is failing.
- Do not create placeholder/planning commits such as `Initial plan`,
  `WIP`, `Draft`, or `Add placeholder`.
- Keep planning in issue comments, PR descriptions, documentation files,
  or agent reasoning rather than commit history.

## Commit message rules
- Subject must start with a capitalized imperative verb.
- Subject must be 70 characters or fewer.
- Subject must not end with a period.
- Subject must not use prefixes such as `[agent]:`, `[chore]:`,
  `feat:`, or `fix:`.
- Commit body must be separated from the subject by a blank line.
- Commit body lines must wrap at 70 characters or fewer.
- `Co-authored by:` trailers are allowed and must appear only at the end
  of the commit message.

## Pull request expectations
- Use the repository PR template and complete all required fields.
- Link the related issue in the `Related Issue` section.
- Ensure the PR is ready to pass `pr-compliance` before handoff.

## Pull request template
- Use the PR template provided in `.github/pull_request_template.md`.
- Do not remove or alter template sections.
- Fill in each section as appropriate:
  - If a section does not apply, write `N/A` instead of deleting it.
  - Do not leave sections blank or remove them from the PR body.
- Keep all section headings intact.
- The PR template is validated by the `pr-compliance` check, so
  preserving structure is required.

## Contributor rules
See `CONTRIBUTING.md` for guidance on how to best contribute
to this repository.

## Lint policy
See `docs/LINT-POLICY.md` for the file-type-specific lint rules applied by
the `lint` status check, and how to extend the policy.
