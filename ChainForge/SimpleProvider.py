from chainforge.providers import provider

@provider(name="MyProvider", emoji="🔄")  # Using a more common emoji
def mirror_the_prompt(prompt: str, **kwargs) -> str:
    return prompt[::-1]
