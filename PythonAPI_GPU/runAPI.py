from flask import Flask, request, jsonify
from ctransformers import AutoModelForCausalLM

app = Flask(__name__)

# Load the GGUF model onto the GPU
model = AutoModelForCausalLM.from_pretrained(r"D:\MyModels\Models\sauerkrautlm-7b-v1-mistral.Q4_K_M.gguf", 
                                             model_type="llama",
                                             gpu_layers=50)

@app.route('/generate', methods=['POST'])
def generate():
    data = request.json
    user_prompt = data.get('user')
    system_prompt = data.get('system', "You are an AI assistant skilled in providing helpful and accurate responses. Always answer in a polite and informative manner.")
    temperature = data.get('temperature', 0.7)  # Default temperature set to 0.7

    # Combine system prompt with user input
    full_prompt = system_prompt + "\nUser: " + user_prompt + "\nAI:"

    # Generate response with specified temperature
    response = model(full_prompt, temperature=temperature)

    print(response)

    return jsonify({'response': response})

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
