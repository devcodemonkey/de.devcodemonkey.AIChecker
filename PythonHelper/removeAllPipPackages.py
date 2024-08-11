import subprocess

# Step 1: List all installed packages
installed_packages = subprocess.check_output(['pip', 'freeze']).decode().splitlines()

# Step 2: Uninstall each package
for package in installed_packages:
    package_name = package.split('==')[0]
    subprocess.call(['pip', 'uninstall', '-y', package_name])

print("All pip packages have been removed.")
