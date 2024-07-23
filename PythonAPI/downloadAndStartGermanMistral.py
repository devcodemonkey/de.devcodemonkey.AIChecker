from ctransformers import AutoModelForCausalLM

# Set gpu_layers to the number of layers to offload to GPU. Set to 0 if no GPU acceleration is available on your system.
llm = AutoModelForCausalLM.from_pretrained("TheBloke/em_german_13b_v01-GGUF", model_file="em_german_13b_v01.Q4_K_M.gguf", model_type="llama", gpu_layers=50)

print(llm("AI is going to"))
