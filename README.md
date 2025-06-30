# ChangeSet GitHub Action

ChangeSet is a GitHub Action designed to help you manage and apply changesets in your CI/CD workflows. This action enables teams to track, organize, and automate changes across your codebase directly from GitHub Actions, making collaboration and change management seamless.

## Features

- **Automated Variable Management:** Apply variables automatically in your workflows.
- **CI/CD Integration:** Easily integrate into existing GitHub Actions pipelines.
- **Team Collaboration:** Supports multi-contributor workflows.
- **Customizable:** Adaptable to different project needs and structures.

## Usage

Add the ChangeSet Action to your workflow:

```yaml
name: Apply ChangeSet

on:
  push:
    branches:
      - main

jobs:
  changeset:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Run ChangeSet Action
        uses: W4TR1X/ChangeSet@v1
        with:
          # Add action-specific inputs here
```
[Sample Usage](https://github.com/W4TR1X/ChangeSet/blob/master/.github/workflows/action-test.yml)

## Development

To test or develop locally:

1. Clone the repository.
2. Install dependencies if required.
3. Modify and test the action according to [GitHub Actions documentation](https://docs.github.com/en/actions).

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

MIT
