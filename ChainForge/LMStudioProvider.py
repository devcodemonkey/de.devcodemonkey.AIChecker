import requests
from chainforge.providers import provider

class LMStudioProvider(CustomProviderProtocol):
    def __init__(self, base_url: str = "http://localhost:1234/v1"):
        self.base_url = base_url

    def generate(self, prompt: str, model: str, temperature: float = 0.7, max_tokens: int = -1, stream: bool = True):
        url = f"{self.base_url}/chat/completions"
        headers = {
            "Content-Type": "application/json"
        }
        data = {
            "model": model,
            "messages": [
                {"role": "system", "content": "Always answer in rhymes."},
                {"role": "user", "content": prompt}
            ],
            "temperature": temperature,
            "max_tokens": max_tokens,
            "stream": stream
        }

        response = requests.post(url, headers=headers, json=data)

        if response.status_code == 200:
            return response.json()  # Assuming the API returns a JSON response
        else:
            raise Exception(f"Failed to connect to LM Studio API: {response.status_code} - {response.text}")

# Example usage
# provider = LMStudioProvider()
# response = provider.generate(prompt="Introduce yourself.", model="model-identifier")
# print(response)
