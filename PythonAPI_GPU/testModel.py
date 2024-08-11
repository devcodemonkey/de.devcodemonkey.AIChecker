from ctransformers import AutoModelForCausalLM

# Load the model onto the GPU
llm = AutoModelForCausalLM.from_pretrained(r"D:\MyModels\Models\sauerkrautlm-7b-v1-mistral.Q4_K_M.gguf", 
                                             model_type="llama",
                                             gpu_layers=50)

# Generate a response from the model
prompt = "AI is going to"
print(llm(prompt))
