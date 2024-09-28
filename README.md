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

13. **sendToLMS**

- **Description**: Sends an API request to LmStudio and saves the result to the database.
- **Command**: `sendToLms -m <Message> -s <SystemPrompt> -r <ResultSet> [-t <MaxTokens>] [-p <Temperature>] [-c <RequestCount>] [-u] [-i <SaveInterval>] [-w]`
  - `-m`: The user message to send.
  - `-s`: System prompt for the request.
  - `-r`: Result set name.
  - `-t`: Maximum number of tokens (optional, default: -1).
  - `-p`: Temperature value (optional, default: 0.7).
  - `-c`: Number of requests (optional, default: 1).
  - `-u`: Save the process usage (optional, default: false).
  - `-i`: Save interval in seconds (optional, default: 5).
  - `-w`: Write process output to console (optional, default: true).

