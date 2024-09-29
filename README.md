# Project Description

This is currently a `beta program`.

The program is designed to compare various Large Language Models (LLMs) based on their performance, accuracy, and resource efficiency. The primary goal is to evaluate LLMs across multiple dimensions, including inference speed, model size, and fine-tuning capabilities. The program can connect to:

- LM Studio API
- OpenAI API

## Commands:

To get help, simply run `AIChecker.exe` followed by the desired command.

### Supported Commands:

1. **help**

   - **Description**: Display more information on a specific command.
   - **Command**: `help <command>`

2. **info**

   - **Description**: Shows information about the developer of the AiChecker.
   - **Command**: `info`

3. **version**

   - **Description**: Display version information.
   - **Command**: `version`

4. **recreateDatabase**

   - **Description**: Recreates the database.
   - **Command**: `recreateDatabase`

5. **viewResultSets**

   - **Description**: View all result sets.
   - **Command**: `viewResultSets`

6. **importQuestions**

   - **Description**: Imports questions and answers into the database from a file.
   - **Command**: `importQuestions -p <Path>`
     - `-p`: Specifies the path to the file containing the questions and answers.

7. **viewResults**

   - **Description**: Displays the results of a specific result set with optional formatting.
   - **Command**: `viewResults -r <ResultSet> [-f]`
     - `-r`: Specifies the result set to view.
     - `-f`: Optionally format the table for a better view.

8. **viewAverage**

   - **Description**: Displays the average time of API requests for a specific result set.
   - **Command**: `viewAverage -r <ResultSet>`
     - `-r`: Specifies the result set for which the average time is displayed.

9. **viewUsedGpu**

   - **Description**: Displays information about the currently used GPU.
   - **Command**: `view-used-gpu`

10. **deleteAllQuestions**

- **Description**: Deletes all questions and answers from the database.
- **Command**: `deleteAllQuestions`

11. **deleteResultSet**

- **Description**: Deletes a specific result set from the database.
- **Command**: `deleteResultSet -r <ResultSet>`
  - `-r`: Specifies the result set to delete.

12. **createMoreQuestions**

- **Description**: Creates more questions based on a system prompt and saves them under a specified result set.
- **Command**: `createMoreQuestions -r <ResultSet> -s <SystemPrompt> [-t <MaxTokens>] [-p <Temperature>]`
  - `-r`: Result set name.
  - `-s`: System prompt or path for question generation.
  - `-t`: Maximum number of tokens (optional).
  - `-p`: Temperature value (optional).
  - `--environmentTokenName`: The environment token name to set the bearer token for the API. (Default: `null`)
  - `--source`: The source URL for the API request. (Default: `http://localhost:1234/v1/chat/completions`, Lm Studio default endpoint)
  - `--model`: The model name to be used for generation. (Default: `nothing set`)

13.  **sendToLMS**

- **Description**: Sends an API request to LmStudio and saves the result to the database.
- **Command**: `sendToLms -m <Message> -s <SystemPrompt> -r <ResultSet> [-t <MaxTokens>] [-p <Temperature>] [-c <RequestCount>] [-u] [-i <SaveInterval>] [-w]`
- 
- `-m`: **Message** (required): The user message to send.
- `-s`: **SystemPrompt** (required): System prompt for the request.
- `-r`: **ResultSet** (required): Result set name to save the API response.
- `-t`: **MaxTokens** (optional, default: -1): Maximum number of tokens for the request.
- `-p`: **Temperature** (optional, default: 0.7): Controls the randomness of the output.
- `-c`: **RequestCount** (optional, default: 1): Number of requests to send.
- `-u`: **SaveProcessUsage** (optional, default: false): Saves the process usage statistics.
- `-i`: **SaveInterval** (optional, default: 5): Interval in seconds to save process usage data.
- `-w`: **WriteOutput** (optional, default: true): Whether to write the process output to the console.
- `--environmentTokenName`: **EnvironmentTokenName** (optional): Name of the environment token to set the bearer token for the API.
- `--source`: **Source URL** (optional, default: `http://localhost:1234/v1/chat/completions`): The source URL for the API.
- `--model`: **Model Name** (optional, default: "nothing set"): Specifies the model name to use for the API request.


