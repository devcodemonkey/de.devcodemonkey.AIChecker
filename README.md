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
     - `-f, --force`: Force recreation of the database.

5. **viewResultSets**

   - **Description**: View all result sets.
   - **Command**: `viewResultSets`

6. **viewResults**

   - **Description**: Displays the results of a specific result set with optional formatting.
   - **Command**: `viewResults -r <ResultSet> [-f | --formatTable]`
     - `-r, --resultSet`: Specifies the result set to view.
     - `-f, --formatTable`: Format the table for better viewing.

7. **viewAverage**

   - **Description**: Displays the average time of API requests for a specific result set.
   - **Command**: `viewAverage -r <ResultSet>`
     - `-r, --resultSet`: Specifies the result set for which the average time is displayed.

8. **importQuestions**

   - **Description**: Imports questions and answers into the database from a file.
   - **Command**: `importQuestions -p <Path> -c <Category>`
     - `-p, --path`: Path to the file containing questions and answers.
     - `-c, --category`: Category of the questions and answers.

9. **importQuestionsFromResults**

   - **Description**: Imports questions from results into the database.
   - **Command**: `importQuestionsFromResults -r <ResultSet> -c <Category>`
     - `-r, --resultSet`: Name of the result set.
     - `-c, --category`: Category of the questions.

10. **deleteAllQuestions**

    - **Description**: Deletes all questions and answers from the database.
    - **Command**: `deleteAllQuestions`

11. **deleteResultSet**

    - **Description**: Deletes a specific result set from the database.
    - **Command**: `deleteResultSet -r <ResultSet>`
    - `-r, --resultSet`: Name of the result set.

12. **createMoreQuestions**

    - **Description**: Creates more questions based on a system prompt and saves them under a specified result set.
    - **Command**: `createMoreQuestions -r <ResultSet> -s <SystemPrompt> -m <Model> -c <Category> [-t <MaxTokens>] [-p <Temperature>]`
    - `-r, --resultSet`: Name of the result set.
    - `-s, --systemPrompt`: System prompt or file path (path:<file-path>).
    - `-m, --model`: Name of the model.
    - `-c, --category`: Category for the questions.
    - `-t, --maxTokens`: Maximum number of tokens (optional).
    - `-p, --temperature`: Temperature value (optional).

13. **sendToLMS**

    - **Description**: Sends an API request to LmStudio and saves the result to the database.
    - **Command**: `sendToLms -m <Message> -r <ResultSet> -s <SystemPrompt> [-t <MaxTokens>] [-p <Temperature>] [-c <RequestCount>] [-u] [-i <SaveInterval>] [-w] [--environmentTokenName <Name>] [--source <URL>] [--model <Model>]`
    - `-m, --message`: User message.
    - `-r, --resultSet`: Name of the result set.
    - `-s, --systemPrompt`: System prompt (default: "You are a helpful assistant").
    - `-t, --maxTokens`: Maximum number of tokens (optional).
    - `-p, --temperature`: Temperature value (optional).
    - `-c, --requestCount`: Number of requests to send (optional).
    - `-u, --saveProcessUsage`: Save process usage statistics.
    - `-i, --saveInterval`: Interval in seconds to save process usage data.
    - `-w, --writeOutput`: Write the process output to the console.
    - `--environmentTokenName`: Environment token name for the API.
    - `--source`: Source URL for the API request.
    - `--model`: Model name for the API request.

14. **rankPrompt**

    - **Description**: Tests prompts and creates a ranking for them.
    - **Command**: `rankPrompt -r <ResultSet> -m <Models> -p <PromptRequirements> [-t <MaxTokens>]`
    - `-r, --resultSet`: Name of the result set.
    - `-m, --models`: Comma-separated list of models to test.
    - `-p, --promptRequirements`: Requirements for the prompts.
    - `-t, --maxTokens`: Maximum number of tokens (optional).

15. **exportPromptRank**

    - **Description**: Exports the ranking of the prompts.
    - **Command**: `exportPromptRank -r <ResultSet> [-t <FileType>] [-o]`
    - `-r, --resultSet`: Name of the result set.
    - `-t, --fileType`: File type for export (default: PDF; options: PDF, HTML, Docx, Markdown).
    - `-o, --notOpenFolder`: Do not open the folder after export.

16. **database**

    - **Description**: Manage the database (start, stop, backup, restore, recreate).
    - **Command**: `database [-s | --stop] [-r | --start] [-b | --backup] [-o | --restore] [--branch <Branch>] [--recreateDatabase] [-f | --force]`
    - `-s, --stop`: Stop the database.
    - `-r, --start`: Start the database.
    - `-b, --backup`: Backup the database.
    - `-o, --restore`: Restore the database.
    - `--branch`: Branch to restore the database from.
    - `--recreateDatabase`: Recreate the database.
    - `-f, --force`: Force the operation.

17. **model**

    - **Description**: Manage models (view, add, load, unload).
    - **Command**: `model [-v | --view] [-a | --add] [-l | --load] [-u | --unload]`
    - `-v, --view`: View all models.
    - `-a, --add`: Add a new model.
    - `-l, --load`: Load a model.
    - `-u, --unload`: Unload a model.

18. **checkJson**

    - **Description**: Check the JSON format of the results.
    - **Command**: `checkJson [-r <ResultSet>] [-o | --showOutput]`
    - `-r, --resultSet`: Name of the result set to check.
    - `-o, --showOutput`: Display the output of the results.

19. **viewProcessUsage**
    - **Description**: View the usage of each process.
    - **Command**: `viewProcessUsage`
