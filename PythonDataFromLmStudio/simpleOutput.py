import requests
import json

# Define the URL and the payload to run through LM Studio
url = "http://localhost:1234/v1/chat/completions"

data = {
    "messages": [
        {"role": "system", "content": "Always answer in rhymes."},
        {"role": "user", "content": "Introduce yourself."}
    ],
    "temperature": 0.7,
    "max_tokens": -1,
    "stream": False
}

# Set the headers
headers = {
    "Content-Type": "application/json"
}

# Make the request
response = requests.post(url, data=json.dumps(data), headers=headers)

# Print the response
print(response.text)