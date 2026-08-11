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

## Contributor rules
See `CONTRIBUTING.md` for guidance on how to best contribute
to this repository.
