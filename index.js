const { execSync } = require('child_process');
const core = require('@actions/core');
const os = require('os');
const ospath = require('path');

try {
  const dir = process.env.GITHUB_WORKSPACE;
  let path = core.getInput('path') || dir;
  let config = core.getInput('config') || ospath.join(dir, 'changeset.config.json');

  const pairsString = core.getInput('pairs');
  const pairs = pairsString.split('\n').map(item => item.trim()).filter(Boolean);
  const joinedPairs = pairs.join(' ');

  if (config.startsWith('./')) {
    config = ospath.join(dir, config.substring(2));
    console.log(`Normalize config path: ${config}`);
  }

  let filePath;
  const actionPath = process.env.GITHUB_ACTION_PATH || __dirname;

  if (os.platform() === 'win32') {
    console.log('Running on Windows');
    filePath = ospath.join(actionPath, 'dist', 'win-x64', 'ChangeSet.exe');
  } else if (os.platform() === 'linux') {
    console.log('Running on Linux');
    filePath = ospath.join(actionPath, 'dist', 'linux-x64', 'ChangeSet');
    execSync(`chmod +x "${filePath}"`);
  } else if (os.platform() === 'darwin') {
    console.log('Running on MacOS');
    filePath = ospath.join(actionPath, 'dist', 'osx-x64', 'ChangeSet');
    execSync(`chmod +x "${filePath}"`);
  } else {
    throw new Error('Unsupported OS');
  }

  const command = `"${filePath}" -path="${path}" -config="${config}" ${joinedPairs}`;
  console.log(command);
  execSync(command, { stdio: 'inherit' });
}
catch (err) {
  process.exitCode = 1;
  const msg = err.toString().replace('%', '%25').replace('\r', '%0D').replace('\n', '%0A');
  process.stdout.write("::error::" + msg + os.EOL);
}