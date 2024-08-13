```
pip install nmake
pip install wheel


# install cmake from https://cmake.org/download/

set PATH="C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Tools\MSVC\14.40.33807\bin\Hostx64\x64\cl.exe":%PATH%

$env:CMAKE_ARGS="-DGGML_CUDA=on"
pip install llama-cpp-python --verbose

pip install llama-cpp-python --extra-index-url https://abetlen.github.io/llama-cpp-python/whl/cpu --force-reinstall
pip install llama-cpp-python --extra-index-url https://abetlen.github.io/llama-cpp-python/whl/cu124
```
