# Lint policy

The `lint` check applies file-type-appropriate rules. Configuration files live
at the repository root and are wired into `.github/workflows/lint.yml`.

## Markdown

Markdown files (`*.md`) are linted with markdownlint-cli2 using the
configuration in `.markdownlint-cli2.jsonc`. All default markdownlint rules
apply except those explicitly disabled below.

| Rule | Name | Enforcement | Rationale |
|---|---|---|---|
| MD013 | Line length | Disabled | Prose wrapping at a hard column limit reduces readability without improving correctness. Markdown docs should read naturally. |
| MD022 | Blanks around headings | Disabled | Compact reference docs and GitHub config files use tight heading spacing; enforcing blank lines adds unnecessary churn. |
| MD032 | Blanks around lists | Disabled | Compact reference docs often place lists directly adjacent to other content; this rule is too strict for dense documentation. |
| MD041 | First line heading | Disabled | PR templates and GitHub configuration files do not begin with a top-level heading by design. |
| MD051 | Link fragments | Disabled | Fragment validation produces false positives in some Markdown renderers and on GitHub. |
| MD060 | Table column style | Disabled | Both compact and padded pipe styles are valid and readable; enforcing one style adds noise to table edits. |

## YAML

YAML files (including GitHub Actions workflows and repository config) are
linted with yamllint using the configuration in `.yamllint.yml`. The base
`default` profile applies except where overridden below.

| Rule | Name | Enforcement | Rationale |
|---|---|---|---|
| truthy | Truthy values | `allowed-values: ["true", "false", "on", "off"]`, `check-keys: false` | GitHub Actions uses `on:` as a mapping key. Without this override yamllint rejects the standard `on:` trigger syntax as a bare boolean. |
| document-start | Document start marker | Disabled | The leading `---` marker is not required in GitHub Actions workflow files and omitting it is the prevailing convention. |
| line-length | Line length | Max 160 characters | Keeps workflow files readable and diff-friendly while accommodating inline shell scripts inside `run:` blocks, which frequently exceed 80–120 characters. |

## Extending the policy

To add lint coverage for a new file type, ensure the following steps are also taken:

- Add the appropriate tool and config file at the repository root.
- Wire it into `.github/workflows/lint.yml` as a new step.
- Update `docs/LINT-POLICY.md` with a section for the new file type that
  includes a brief description of the file type and the tool used to lint it,
  plus a table for each configured rule using the following columns:

| Column | Description |
|---|---|
| Rule | The rule identifier (e.g., `MD013`, `line-length`) |
| Name | A short human-readable name for the rule |
| Enforcement | The current setting or value (e.g., `Disabled`, `Max 160 characters`) |
| Rationale | Why this rule is enforced or relaxed in this repository |
