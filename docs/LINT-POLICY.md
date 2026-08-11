# Lint policy

The `lint` check applies file-type-appropriate rules so that prose-heavy
documentation and strict workflow/config files are not held to the same
line-length standard.

## Rules by file type

| File type | Tool | Line-length rule |
|---|---|---|
| Markdown (`*.md`) | markdownlint-cli2 | Not enforced (`MD013: false`) — write prose naturally |
| YAML (workflows, config) | yamllint | Max 160 characters |

## Markdown

Markdown files are linted with markdownlint-cli2 using the configuration in
`.markdownlint-cli2.jsonc`. The line-length rule (`MD013`) is disabled because
wrapping prose at a hard column limit reduces readability without improving
correctness. All other default markdownlint rules apply.

## YAML

YAML files (including GitHub Actions workflows and repository config) are
linted with yamllint using the configuration in `.yamllint.yml`. Line length
is capped at 160 characters to keep workflow files readable and diff-friendly.
Inline scripts inside workflow steps may require longer lines, and this limit
accommodates them.

## Extending the policy

Both configurations live at the repository root. To add lint coverage for a
new file type, add the appropriate tool and config file, then wire it into
`.github/workflows/lint.yml` as a new step.
