from flask import Flask, request, jsonify
from ctransformers import AutoModelForCausalLM

app = Flask(__name__)

# Load the model
llm = AutoModelForCausalLM.from_pretrained(
    "TheBloke/em_german_13b_v01-GGUF", 
    model_file="em_german_13b_v01.Q4_K_M.gguf", 
    model_type="llama", 
    gpu_layers=50
)

@app.route('/generate', methods=['POST'])
def generate():
    try:
        data = request.get_json()
        prompt = data.get('prompt')
        temperature = data.get('temperature', 0.7)  # Default temperature is 0.7
        max_tokens = data.get('max_tokens', 100)  # Default max tokens is 100
        system_prompt = data.get('system_prompt', "")

        # Generate the completion
        response = llm.generate(
            prompt,
            temperature=temperature,
            max_tokens=max_tokens,
            system_prompt=system_prompt
        )
        
        return jsonify({'completion': response})
    except Exception as e:
        return jsonify({'error': str(e)}), 500

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
