import sys, os
from huggingface_hub import login
from transformers import AutoTokenizer
# example call: 
# .\.venv\Scripts\activate
# python GetTokens.py "jphme/em_german_mistral_v01" "Hallo Welt"
# python GetTokens.py "meta-llama/Llama-3.2-1B-Instruct" "Hallo Welt, wie geht es dir?"
# sys.argv = ["GetTokens.py", "meta-llama/Llama-3.2-1B-Instruct", "Hallo Welt"]

def check_huggingface_credentials():
    # Check if the token is in the Hugging Face cache
    token_cache_path = os.path.expanduser('~/.cache/huggingface/token')
    if os.path.exists(token_cache_path):
        print("Token found in Hugging Face cache.")
        return True

    # Check if the token is in Git credentials
    try:
        result = subprocess.run(
            ['git', 'credential', 'fill'],
            input='host=huggingface.co\n',
            capture_output=True,
            text=True,
            check=True
        )
        for line in result.stdout.splitlines():
            if 'password=' in line:
                print("Token found in Git credentials.")
                return True
        return False
    except subprocess.CalledProcessError:
        print("Failed to access Git credentials.")
        return False

# Check if credentials exist, otherwise prompt for login
if not check_huggingface_credentials():
    print("No credentials found. Please log in.")
    login()

# load the tokenizer
tokenizer = AutoTokenizer.from_pretrained(sys.argv[1])

# Tokenize the input text
tokens = tokenizer(sys.argv[2])

# Anzahl der Tokens
num_tokens = len(tokens["input_ids"])
print(f"Anzahl der Tokens: {num_tokens}")
