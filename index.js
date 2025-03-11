const { execSync } = require('child_process');
const core = require('@actions/core');
const os = require('os');
const ospath = require('path');

try {
  // GitHub Actions inputlarını al
  const path = core.getInput('path') || process.env.GITHUB_WORKSPACE;
  const config = core.getInput('config') || ospath.join(process.env.GITHUB_WORKSPACE, 'changeset.config.json');

  const pairsString = core.getInput('pairs');
  const pairs = pairsString.split('\n').map(item => item.trim()).filter(Boolean);
  const joinedPairs = pairs.join(' ');

  // PowerShell betiğini parametrelerle çalıştır
  execSync(`pwsh -File "${ospath.join(__dirname, 'run-action.ps1')}" -path "${path}" -config "${config}" -pairs ${joinedPairs}`,
    { stdio: 'inherit', shell: "pwsh", cwd: process.env.GITHUB_WORKSPACE }
  );
}
catch (err) {
  process.exitCode = 1;
  const msg = err.toString().replace('%', '%25').replace('\r', '%0D').replace('\n', '%0A');
  process.stdout.write("::error::" + msg + os.EOL);
}
