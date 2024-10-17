# Project Description

This project is currently in a **beta program**.

The beta releases are deployed to: [https://devcodemonkey.de/AiChecker/](https://devcodemonkey.de/AiChecker/)

The program is designed to compare various Large Language Models (LLMs) based on their performance, accuracy, and resource efficiency. The primary goal is to evaluate LLMs across multiple dimensions, including inference speed, model size, and fine-tuning capabilities. The program can connect to:

- LM Studio API
- OpenAI API

# Third-Party Licenses

This software uses third-party libraries, which can be looked up under the following link: [Third-Party Licenses](https://github.com/devcodemonkey/de.devcodemonkey.AIChecker/blob/main/THIRD_PARTY_LICENSES.md)

## Commands

To get help, simply run `AIChecker.exe` followed by the desired command.

### Supported Commands

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
   - **Command**: `recreateDatabase [-f | --force]`
     - `-f, --force`: Force recreation of the database (optional).

5. **viewResultSets**

   - **Description**: View all result sets.
   - **Command**: `viewResultSets`

6. **importQuestions**

   - **Description**: Imports questions and answers into the database from a file.
   - **Command**: `importQuestions -p <Path> | --path <Path>`
     - `-p, --path`: Specifies the path to the file containing the questions and answers.

7. **viewResults**

   - **Description**: Displays the results of a specific result set with optional formatting.
   - **Command**: `viewResults -r <ResultSet> [-f | --formatTable]`
     - `-r, --resultSet`: Specifies the result set to view.
     - `-f, --formatTable`: Optionally format the table for a better view.

8. **viewAverage**

   - **Description**: Displays the average time of API requests for a specific result set.
   - **Command**: `viewAverage -r <ResultSet> | --resultSet <ResultSet>`
     - `-r, --resultSet`: Specifies the result set for which the average time is displayed.

9. **deleteAllQuestions**

   - **Description**: Deletes all questions and answers from the database.
   - **Command**: `deleteAllQuestions`

10. **deleteResultSet**

    - **Description**: Deletes a specific result set from the database.
    - **Command**: `deleteResultSet -r <ResultSet> | --resultSet <ResultSet>`
      - `-r, --resultSet`: Specifies the result set to delete.

11. **createMoreQuestions**

    - **Description**: Creates more questions based on a system prompt and saves them under a specified result set.
    - **Command**: `createMoreQuestions -r <ResultSet> -s <SystemPrompt> [-t <MaxTokens>] [-p <Temperature>] [--environmentTokenName <Name>] [--source <URL>] [--model <Model>]`
      - `-r, --resultSet`: Result set name.
      - `-s, --systemPrompt`: System prompt or path for question generation.
      - `-t, --maxTokens`: Maximum number of tokens (optional).
      - `-p, --temperature`: Temperature value (optional).
      - `--environmentTokenName`: The environment token name to set the bearer token for the API.
      - `--source`: The source URL for the API request.
      - `--model`: The model name to be used for generation.

12. **sendToLMS**

    - **Description**: Sends an API request to LmStudio and saves the result to the database.
    - **Command**: `sendToLms -m <Message> -s <SystemPrompt> -r <ResultSet> [-t <MaxTokens>] [-p <Temperature>] [-c <RequestCount>] [-u] [-i <SaveInterval>] [-w] [--environmentTokenName <Name>] [--source <URL>] [--model <Model>]`
      - `-m, --message`: The user message to send.
      - `-r, --resultSet`: Result set name to save the API response.
      - `-s, --systemPrompt`: System prompt for the request (optional).
      - `-t, --maxTokens`: Maximum number of tokens for the request (optional).
      - `-p, --temperature`: Temperature value (optional).
      - `-c, --requestCount`: Number of requests to send (optional).
      - `-u, --saveProcessUsage`: Save process usage statistics (optional).
      - `-i, --saveInterval`: Interval in seconds to save process usage data (optional).
      - `-w, --writeOutput`: Write the process output to the console (optional).
      - `--environmentTokenName`: Name of the environment token to set the bearer token for the API.
      - `--source`: The source URL for the API request.
      - `--model`: Specifies the model name to use for the API request.

13. **viewProcessUsage**

    - **Description**: View usage of each process.
    - **Command**: `viewProcessUsage`

14. **exportPromptRank**

    - **Description**: Exports the ranking of the prompts.
    - **Command**: `exportPromptRank -r <ResultSet> [-t <FileType>] [-o]`
      - `-r, --resultSet`: Specifies the result set name.
      - `-t, --fileType`: File type to export (optional, default is PDF, possible options: PDF, HTML, Markdown).
      - `-o, --notOpenFolder`: Prevent opening the folder after export (optional).

15. **rankPrompt**

    - **Description**: Tests prompts and creates a ranking for them.
    - **Command**: `rankPrompt -r <ResultSet> -m <Models> -p <PromptRequirements> [-t <MaxTokens>]`
      - `-r, --resultSet`: Specifies the result set name.
      - `-m, --models`: Specifies the models to test (comma-separated).
      - `-p, --promptRequirements`: Prompt requirements.
      - `-t, --maxTokens`: Maximum number of tokens (optional).

16. **model**

    - **Description**: Manage models.
    - **Command**: `model [-v | --view] [-a | --add] [-l | --load] [-u | --unload]`
      - `-v, --view`: View all models.
      - `-a, --add`: Add a new model.
      - `-l, --load`: Load a model.
      - `-u, --unload`: Unload a model.

17. **database**
    - **Description**: Start, stop, or backup the database.
    - **Command**: `database [-s | --stop] [-r | --start] [-b | --backup]`
      - `-s, --stop`: Stop the database.
      - `-r, --start`: Start the database.
      - `-b, --backup`: Backup the database.
