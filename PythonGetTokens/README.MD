# How to use

## Requirements

- Python installed (https://www.python.org/downloads/)
- venv (https://docs.python.org/3/library/venv.html), venv is included in Python 3.3 and later, so you don't need to install it separately.

## Installation on Windows

```pwsh
$path = "C:\temp"
Set-location $path
# clone repository
git clone https://github.com/devcodemonkey/de.devcodemonkey.AIChecker.git
# navigate to the folder, where the repository was cloned
Set-location "$path\de.devcodemonkey.AIChecker\PythonGetTokens"
# create virtual environment
python -m venv venv
.\venv\Scripts\activate
# install required packages
pip install -r requirements.txt
```

## Run the Script

```pwsh
python GetTokens.py "jphme/em_german_mistral_v01" "Hallo Welt"
```
