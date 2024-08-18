import os
import time
import matplotlib.pyplot as plt
import requests
import json

import subprocess

models = {
    "TheBloke/SauerkrautLM-7B-HerO-GGUF": "http://localhost:1234/v1/chat/completions",
    "TheBloke/Mixtral-8x7B-Instruct-v0.1-GGUF": "http://localhost:1234/v1/chat/completions"
    #"gpt-3.5-turbo": "https://api.openai.com/v1/chat/completions",
    #"gpt-4": "https://api.openai.com/v1/chat/completions"
}

def call(prompt, model="gpt-3.5-turbo", max_tokens=256, temperature=0.7):
    api_key = os.getenv('OPENAI_API_KEY')
    url = models[model]
    data = {
        "messages": [
            {"role": "system", "content": "You are a helpful assistant."},
            {"role": "user", "content": prompt}
        ],
        "temperature": temperature,
        "max_tokens": max_tokens,
        "model": model,
        "stream": False
    }
    headers = {
        "Content-Type": "application/json",
        "Authorization": f"Bearer {api_key}"
    }
    response = requests.post(url, data=json.dumps(data), headers=headers)
    return json.loads(response.text)["choices"][0]['message']['content']


prompt = """Product description: A home milkshake maker
Seed words: fast, healthy, compact
Product names: HomeShaker, Fit Shaker, QuickShake, Shake Maker

Product description: A pair of shoes that can fit any foot size
Seed words: adaptable, fit, omni-fit
Product names:"""

# call the prompt 10 times for each model, and measure the latency
# save the responses and latency in a json object then plot the results
results = {}
for model in models:
    print(subprocess.run(['LMS', 'server', 'start'], capture_output=True, text=True))
    print(subprocess.run(['LMS', 'load', model], capture_output=True, text=True))
    results[model] = {"latency": [], "responses": []}
    for i in range(10):        
        start = time.time()
        response = call(prompt, model=model)
        end = time.time()
        results[model]["latency"].append(end - start)
        results[model]["responses"].append(response)
    print(subprocess.run(['LMS', 'unload', 'model'], capture_output=True, text=True))
plt.figure(figsize=(10, 5))
for model in results:
    plt.plot(results[model]["latency"], label=model)
plt.legend()
plt.xlabel("Call number")
plt.ylabel("Latency (s)")
plt.show()