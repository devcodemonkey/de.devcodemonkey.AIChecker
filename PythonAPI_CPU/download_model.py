from langchain_community.llms import HuggingFaceHub
huggingfacehub_api_token = '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'
llm = HuggingFaceHub(repo_id='TheBloke/em_german_13b_v01-GGUF', huggingfacehub_api_token=huggingfacehub_api_token)

input = 'Health is wealth, but is it the order of priority for most of us?'
output = llm.invoke(input)
print(output)