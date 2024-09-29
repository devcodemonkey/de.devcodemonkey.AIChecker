# Commands

## Create more questions

```bash
.\AIChecker.exe --createMoreQuestions "questions created with mixtral 8x instructv0.1" "path:C:\Users\d-hoe\source\repos\masterarbeit\AIChecker\AIChecker\examples\system_promt.txt"
```

## Build app on linux

```bash
dotnet publish -c Release --self-contained -r linux-x64 -p:PublishSingleFile=true
```

## Examples

```bash
./AIChecker sendToLms -m "create me a poem over 10 lines" -s "" -r "Test run 2024-09-29" -c 5
./AIChecker 

./AIChecker importQuestions -p /home/david/masterarbeit.wiki/06_00_00-Ticketexport/FAQs/FAQ-Outlook.json


```
