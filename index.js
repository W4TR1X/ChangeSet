const { execSync } = require('child_process');
const core = require('@actions/core');
const os = require('os');
const ospath = require('path');

try {
  // GitHub Actions inputlarını al
  const path = core.getInput('path') ?? __dirname;
  const config = core.getInput('config') ?? __dirname + '/changeset.config.json';

  const pairsString = core.getInput('pairs');
  const pairs = pairsString.split('\n').map(item => item.trim()).filter(Boolean);  
  const joinedPairs = pairs.join(' ');

  // PowerShell betiğini parametrelerle çalıştır
  execSync(`pwsh -File "${ospath.join(__dirname, 'run-action.ps1')}" -path "${path}" -config "${config}" -pairs ${joinedPairs}`,
    { stdio: 'inherit', shell: "pwsh" }
  );
}
catch (err) {
  process.exitCode = 1;
  const msg = err.toString().replace('%', '%25').replace('\r', '%0D').replace('\n', '%0A');
  process.stdout.write("::error::" + msg + os.EOL);
}
