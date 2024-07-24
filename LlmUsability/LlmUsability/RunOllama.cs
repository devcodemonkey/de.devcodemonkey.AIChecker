using AutoGen.Ollama;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoGen.Ollama.Extension;
using AutoGen.Core;

namespace LlmUsability
{
    internal class RunOllama
    {
        public async Task RunAsync()
        {
            // example: https://microsoft.github.io/autogen-for-net/articles/AutoGen.Ollama/Chat-with-llama.html
            string userMessage = @"### START FRAGE ###
---

**Von:** Max Mustermann
**An:** IT-Dienstleister, Öffentlicher Dienst
**Betreff:** Probleme bei der Anmeldung im Intranet

---

Sehr geehrtes IT-Team,

ich habe seit heute Morgen Probleme, mich im Intranet unseres öffentlichen Dienstes anzumelden. Bisher hat alles problemlos funktioniert, aber seit heute erhalte ich die folgende Fehlermeldung, wenn ich versuche, mich einzuloggen:

**Fehlermeldung:** ""Benutzername oder Passwort ist ungültig.""

Ich habe sichergestellt, dass mein Benutzername und Passwort korrekt eingegeben wurden und habe auch versucht, mein Passwort zurückzusetzen, jedoch ohne Erfolg. Zusätzlich habe ich den Browser-Cache geleert und verschiedene Browser ausprobiert (Chrome, Firefox, Edge), aber das Problem bleibt bestehen.

Hier einige Details zu meinem System:

- **Betriebssystem:** Windows 10 Pro
- **Browser:** Chrome Version 91.0.4472.124, Firefox Version 89.0.2, Edge Version 91.0.864.64
- **Fehlerzeitpunkt:** 4. Juli 2024, ca. 08:30 Uhr

Ich bitte um Ihre Unterstützung, da ich auf einige wichtige Informationen im Intranet zugreifen muss, um meine Aufgaben zu erledigen. Können Sie bitte prüfen, ob es aktuell Probleme mit dem Anmeldesystem gibt oder ob mein Account möglicherweise gesperrt wurde?

Vielen Dank im Voraus für Ihre Hilfe.

Mit freundlichen Grüßen,

Max Mustermann
Abteilung für Öffentlichkeitsarbeit
Öffentlicher Dienst
max.mustermann@oeffentlicher-dienst.de
Tel: 01234 / 567890
### END FRAGE ###
### START ANTWORT ###
Sehr geehrter Herr Mustermann,

vielen Dank für Ihre Nachricht und die detaillierten Informationen zu Ihrem Problem. Wir bedauern die Unannehmlichkeiten, die Ihnen dadurch entstanden sind.

Wir haben Ihre Anfrage überprüft und folgende Maßnahmen durchgeführt:

1. **Accountprüfung:** Ihr Benutzerkonto wurde überprüft und es scheint, dass es am System kein generelles Problem mit der Anmeldung gibt. Ihr Konto ist aktiv und es gibt keine Sperrungen.

2. **Passwortrücksetzung:** Wir haben zur Sicherheit Ihr Passwort zurückgesetzt. Sie erhalten in einer separaten E-Mail ein temporäres Passwort. Bitte ändern Sie dieses Passwort nach der ersten Anmeldung.

3. **Browser-Cookies und Cache:** Da Sie bereits den Cache und die Cookies gelöscht haben, schlagen wir zusätzlich vor, die gespeicherten Passwörter im Browser zu überprüfen und zu löschen, um eventuelle Konflikte zu vermeiden.

4. **Systemwartung:** Es könnte hilfreich sein, den Rechner neu zu starten, um eventuelle temporäre Fehler zu beheben.

Bitte folgen Sie diesen Schritten, um Ihr temporäres Passwort zu ändern:

1. Melden Sie sich mit dem temporären Passwort im Intranet an.
2. Nach der Anmeldung werden Sie aufgefordert, Ihr Passwort zu ändern.
3. Erstellen Sie ein neues, sicheres Passwort und bestätigen Sie die Änderung.

Sollten Sie weiterhin Probleme haben, melden Sie sich bitte bei uns. Wir stehen Ihnen jederzeit zur Verfügung, um weitere Schritte zu unternehmen.

Vielen Dank für Ihre Geduld und Ihr Verständnis.

Mit freundlichen Grüßen,

IT-Support Team
Öffentlicher Dienst
support@oeffentlicher-dienst.de
Tel: 01234 / 567891
### ENDE ANTWORT ###";

            string systemMessage = @"Du bist ein KI-Assistent der beurteilt, wie gut die folgende Frage zur Antwort passt. Bewerte auf einer Skala von 1 bis 10000, wobei 1 schlecht und 10000 perfekt ist.
Du Antwortest immer im JSON-Format. Jede Antwort sollte in diesem Format strukturiert sein:
{
  ""Bewertung"": ""Wert aus Skala"",
}
";


            using var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:11434"),
            };

            var ollamaAgent = new OllamaAgent(
                httpClient: httpClient,
                name: "ollama",
                modelName: "llama3.1:8b",
                systemMessage: systemMessage)
                .RegisterMessageConnector();

            var output = await ollamaAgent.SendAsync(userMessage);

            Console.WriteLine(output.FormatMessage());

        }
    }
}
