# Import questions from a Json files
$AiChecker = "C:\Users\d-hoe\source\repos\masterarbeit\AIChecker\AIChecker\bin\Debug\net8.0\win-x64"
Set-Location $AiChecker

./AIChecker.exe importQuestions -p "C:\Users\d-hoe\source\repos\masterarbeit.wiki\06_00_00-Ticketexport\FAQs\FAQ-Outlook.json" -c "Outlook"
./AIChecker.exe importQuestions -p "C:\Users\d-hoe\source\repos\masterarbeit.wiki\06_00_00-Ticketexport\FAQs\FAQ-Teams_allgemein.json" -c "Teams allgemein"
./AIChecker.exe importQuestions -p "C:\Users\d-hoe\source\repos\masterarbeit.wiki\06_00_00-Ticketexport\FAQs\FAQ-Teams_citrix.json" -c "Teams Citrix"
./AIChecker.exe importQuestions -p "C:\Users\d-hoe\source\repos\masterarbeit.wiki\06_00_00-Ticketexport\FAQs\Azubi-FAQ.json" -c "Azubi FAQ"

