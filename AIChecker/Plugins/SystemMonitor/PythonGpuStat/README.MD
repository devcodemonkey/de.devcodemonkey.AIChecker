# Gpu Stat

## Restore Project

Sure! Here's a short instruction for restoring a Python virtual environment on **Windows** using `requirements.txt`:

1. **Create a Virtual Environment**:

   ```bash
   python -m venv .venv
   ```

2. **Activate the Virtual Environment**:

   ```bash
   .venv\Scripts\activate
   ```

3. **Install Dependencies**:
   ```bash
   pip install -r requirements.txt
   ```

This will create the `.venv` directory, activate the virtual environment, and install all required packages.

## Create exe file

First, ensure you have `gpustat` installed in your Python environment:

```bash
pip install gpustat
```

1. **Install PyInstaller:**

   ```bash
   pip install pyinstaller
   ```

2. **Generate the standalone executable:**

   Get Version

   ```bash
   gpustat --version
   ```

   Use PyInstaller to bundle `gpustat` and its dependencies:

   ```bash
   pyinstaller --onefile -n gpustat.exe -c gpustat-script.py
   cp gpustat.exe gpustat-1_1_1.exe
   ```

   `--onefile`: This generates a single executable file.
   `-n gpustat.exe`: This names the executable.
   `-c:` Runs the executable in console mode.

   ```python
   # gpustat-script.py
   import gpustat

   if __name__ == "__main__":
       gpustat.main()
   ```

## Save installed Packages

```bash
pip freeze > requirements.txt
```

## install packages manually

```bash
pip install gpustat
pip install pyinstaller
```

## Gpu Commands

```
cd dist
./gpustat.exe --show-full-cmd --json
```

## Links

<https://github.com/wookayin/gpustat>
