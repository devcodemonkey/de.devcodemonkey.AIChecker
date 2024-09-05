from llama_cpp import Llama

# Load your GGUF model
model = Llama(model_path=r"D:\MyModels\Models\em_german_7b_v01.Q4_K_M.gguf")

# Example function to generate a response
def generate_response(prompt):
    response = model(prompt)
    return response
print("----------------------")
print(generate_response("Wie geht es dir?"))