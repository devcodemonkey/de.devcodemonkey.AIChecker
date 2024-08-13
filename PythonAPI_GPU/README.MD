# Setup python in VSCode

## Install python and ancaonda

```pwsh
# serach for python version
winget search Python.Python.
# install python version e.g.
winget install Python.Python.3.12
```

### set anaconda to path variable

To add Conda to your PATH:

- Open the Start menu, search for "Environment Variables," and select "Edit the user environment variables."
- In the System Properties window, click "Environment Variables."
- Find the Path variable under "user variables" and click "Edit."
- Click "New" and add the path to your Conda installation (e.g., `C:\Users\d-hoe\anaconda3\Scripts`).
- Click "OK" to save your changes.

## create a new project

`STRG+SHIFT+P -> Python: Create Envirement -> venv`

## activacte venv

```
.\venv\Scripts\activate
```

## install requiered packages in conda

```
conda install transformers
```

## Install required package

```python
pip install ctransformers[cuda]
# if it does't work use
pip install ctransformers[cuda] --force-reinstall
pip install Flask
```

### Save installed Packages

```
pip freeze > requirements.txt
```

### Load packages

```
pip install -r requirements.txt

```

## Links

https://code.visualstudio.com/docs/python/python-tutorial
https://code.visualstudio.com/docs/python/environments#:~:text=To%20create%20local%20environments%20in,environment%20types%3A%20Venv%20or%20Conda.
