# Contributing

Thank you for contributing to this repository. Please read
this document before opening a pull request or commit.

## Issue-first workflow

Every change must be linked to an open issue. Open or find
an issue before starting work, and reference it in your PR.

## Commit message conventions

### Subject line

- Start with a capitalized imperative verb
  (e.g., `Add`, `Fix`, `Update`, `Remove`)
- Maximum 70 characters
- No trailing period
- No prefix conventions such as `feat:`, `fix:`,
  `[agent]:`, or `[chore]:`

**Good:**

```text
Add CodeQL security scanning workflow
```

**Bad — placeholder planning commit:**

```text
Initial plan
```

**Bad — prefix convention:**

```text
[agent]: add codeql workflow
```

### Body (optional)

- Separate from the subject with a blank line
- Wrap each line at 70 characters or fewer
- Explain *what* and *why*, not *how*

### Trailers

`Co-authored-by` trailers are allowed only at the end of
the commit message, after a blank line.

## Pull request conventions

Before requesting review, make sure your PR:

- Has a descriptive title that follows the same
  capitalized-imperative-verb rule as commit subjects
- Fills in all required sections of the PR template
  (Description, Related Issue, Types of changes, Checklist)
- References the related issue (e.g., `Closes #13`)
- Passes all required status checks

## Lint policy

The `lint` check applies file-type-appropriate rules so that prose-heavy
documentation and strict workflow/config files are not held to the same
line-length standard.

| File type | Tool | Line-length rule |
|---|---|---|
| Markdown (`*.md`) | markdownlint-cli2 | Not enforced (`MD013: false`) — write prose naturally |
| YAML (workflows, config) | yamllint | Max 160 characters |

**Markdown** files are linted with markdownlint. The line-length rule (`MD013`)
is disabled because wrapping prose at a hard column limit reduces readability
without improving correctness. All other default markdownlint rules apply.

**YAML** files are linted with yamllint using the configuration in
`.yamllint.yml`. Line length is capped at 160 characters to keep workflow
files readable and diff-friendly. Inline scripts inside workflow steps may
require longer lines, and this limit accommodates them.

Both configurations live at the repository root and can be extended as new
file types are added.

## Status checks

The following checks must pass before merging:

| Check | Workflow |
|---|---|
| `lint` | `.github/workflows/lint.yml` |
| `tests` | `.github/workflows/tests.yml` |
| `commit-message-validation` | `.github/workflows/commit-message-validation.yml` |
| `pr-compliance` | `.github/workflows/pr-compliance.yml` |

Verify these pass in the GitHub Actions tab before
requesting review.

## Notes for automated contributors

- **Do not create placeholder planning commits** such as
  `Initial plan`, `WIP`, or `Add placeholder`. Planning
  belongs in issue comments, PR descriptions, reasoning,
  or planning documents (e.g., `.planning/`, `.specify/`),
  not in commit history.
- Inspect the required status checks listed above and
  confirm they all pass before considering work complete.
- Follow the commit message rules above on every commit,
  including the first one.
- Populate all required PR template fields before handoff.
