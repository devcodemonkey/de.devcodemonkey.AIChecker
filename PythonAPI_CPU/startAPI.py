from flask import Flask, request, jsonify
import llama_cpp

app = Flask(__name__)

# Load the GGUF model
model = llama_cpp.Llama(model_path=r"D:\MyModels\Models\em_german_7b_v01.Q4_K_M.gguf")

@app.route('/generate', methods=['POST'])
def generate():
    data = request.json
    prompt = data.get('prompt')
    response = model(prompt)
    return jsonify({'response': response})

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
