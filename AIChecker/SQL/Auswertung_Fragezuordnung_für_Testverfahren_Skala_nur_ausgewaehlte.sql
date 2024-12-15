-- Auswertung
drop table temp_table_ResultSets;
create temp table temp_table_ResultSets
("ResultSets" TEXT);

INSERT INTO temp_table_ResultSets
VALUES    
    --('Fragezuordnung für Testverfahren Skala auf gpt-mini (Nr. 2) Teams Citrix created Questions'),
    --('Fragezuordnung für Testverfahren Skala auf gpt-mini (Nr. 3) Outlook allgemein korrekt geprüfte Fragen'),
    --('Fragezuordnung für Testverfahren Skala auf gpt-mini (Nr. 4) Teams allgemein'),
    ('Fragezuordnung für Testverfahren Skala auf gpt-mini (Nr. 5) Azubi FAQ');

INSERT INTO temp_table_ResultSets
VALUES    
 	('Fragezuordnung für Testverfahren Skala auf gpt-4o (Nr. 1) Teams Citrix created Questions'),
    ('Fragezuordnung für Testverfahren Skala auf gpt-4o (Nr. 2) Outlook allgemein korrekt geprüfte Fragen'),
    ('Fragezuordnung für Testverfahren Skala auf gpt-4o (Nr. 3) Teams allgemein'),
    ('Fragezuordnung für Testverfahren Skala auf gpt-4o (Nr. 4) Azubi FAQ');
   

   
insert into temp_table_ResultSets
values	
	('Fragezuordnung für Testverfahren Skala (Nr. 1) Outlook allgemein korrekt geprüfte Fragen'),
	('Fragezuordnung für Testverfahren Skala (Nr. 2) Teams Citrix korrekt geprüfte Fragen'),
	('Fragezuordnung für Testverfahren Skala (Nr. 3) Teams allgemein korrekt geprüfte Fragen'),
	('Fragezuordnung für Testverfahren Skala (Nr. 4) Azubi FAQ korrekt geprüfte Fragen');
   
insert into temp_table_ResultSets
values
	('Fragezuordnung für Testverfahren Skala (Nr. 8) mixtral-8x7b-instruct-v0.1');
   
insert into temp_table_ResultSets
values
	('Fragezuordnung für Testverfahren Skala (Nr. 10) Meta-Llama-3.1-70B');
	
insert into temp_table_ResultSets
values
	('Fragezuordnung für Testverfahren Skala (Nr. 11) em_german_leo_mistral');

insert into temp_table_ResultSets
values
	('Fragezuordnung für Testverfahren Skala (Nr. 12) Meta-Llama-3.1-8B-Instruct');

insert into temp_table_ResultSets
values
	('Fragezuordnung für Testverfahren Skala (Nr. 13) Llama-3.2-3B-Instruct');

insert into temp_table_ResultSets
values
	('Fragezuordnung für Testverfahren Skala (Nr. 14) Llama-3.2-1B-Instruct')
	
insert into temp_table_ResultSets
values
	('Fragezuordnung für Testverfahren Skala (Nr. 15) Mistral-Nemo')
	
insert into temp_table_ResultSets
values
	('Fragezuordnung für Testverfahren Skala (Nr. 16) Ministral-8B');

insert into temp_table_ResultSets
values
	('Fragezuordnung für Testverfahren Skala (Nr. 17) Meta-Llama-3.3-70B');


insert into temp_table_ResultSets
values
	('Fragezuordnung für Testverfahren Skala (Nr. 18) Llama-3.2-1B-Instruct 3s');

-- Laufzeiten	
with GroupedResults as (
    select 
        coalesce(replace(rs."Value", 'Fragezuordnung für Testverfahren Skala auf', ''), 'Gesamt') as "Test",
        count(*) as "Anzahl Anfragen",
        max(r."RequestEnd") - min(r."RequestStart") as "Dauer",
        (max(r."RequestEnd") - min(r."RequestStart")) / count(*) as "Durchschnittliche Dauer"
    from "Results" r 
    join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId" 
    where rs."Value" in (select * from temp_table_ResultSets)
    group by rs."Value"
),
TotalRow as (
    select 
        'Gesamt' as "Test",
        sum("Anzahl Anfragen") as "Anzahl Anfragen",
        sum("Dauer") as "Dauer",
        sum("Dauer") / sum("Anzahl Anfragen") as "Durchschnittliche Dauer"
    from GroupedResults
)
select * 
from (
    select * from GroupedResults
    union all
    select * from TotalRow
) CombinedResults
order by 
    (case when "Test" = 'Gesamt' then 1 else 0 end),
    "Test";

-- Valid JSON Format
select
	COUNT(*) FILTER (WHERE r."IsJson" = true) AS "Anzahl ValidJsonFormat",
	COUNT(*) FILTER (WHERE r."IsJson" = false) AS "Anzahl InvalidJsonFormat",
	100.0 * COUNT(*) FILTER (WHERE r."IsJson" = true) / NULLIF(COUNT(*), 0) AS "Prozent"
from "Results" r 
join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId" 
where 
rs."Value" in (select * from temp_table_ResultSets)

-- Treffer/-quote
with maxbewertungen as (
    select    	
    	rs."Value" as  "ResultSet",
        r."QuestionsId",
        r."AnswerId" as "AnswerIdResult",
        q."AnswerId" as "AnswerIdQuestions(Korrekt)",
        (r."Message"::jsonb ->> 'Bewertung')::numeric as "Bewertung",
        max((r."Message"::jsonb ->> 'Bewertung')::numeric) over (partition by r."QuestionsId") as "MaxBewertung",
        count(*) over (partition by r."QuestionsId", (r."Message"::jsonb ->> 'Bewertung')::numeric) as "MaxValueCount"
    from "Results" r
    join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId"
    join "Questions" q on q."QuestionId" = r."QuestionsId"
    where 
        rs."Value" in (select * from temp_table_ResultSets)  
        and r."IsJson" = true
)
select "ResultSet","QuestionsId", "AnswerIdResult", "AnswerIdQuestions(Korrekt)", "Bewertung", "MaxValueCount"
from maxbewertungen
where "Bewertung" = "MaxBewertung"
and "AnswerIdQuestions(Korrekt)" = "AnswerIdResult"
order by "MaxValueCount";

-- GPU Auslastung
select 
	sru."ProcessId"
	,sru."ProcessName" 
	,round(cast(sum(sru."GpuUsage") as decimal) / cast(count(*) as decimal),2) as "AvgGpuUsage"
    ,cast(sum(sru."GpuUsage") as decimal) as "TotalGpuUsage"
	,count(*) as "CountOfGpuUsage"
from "ResultSets" rs 
join "SystemResourceUsage" sru on rs."ResultSetId" = sru."ResultSetId" 
where 
	rs."Value" in (select * from temp_table_ResultSets)
	and sru."ProcessId" = -1
group by
	sru."ProcessId"
	,sru."ProcessName";

-- Hardware Auslastung
select
	sru."ProcessId"
	,sru."ProcessName" 
	,round(cast(sum(sru."GpuMemoryUsage") as decimal) / cast(count(*) as decimal),2) as "AvgGpuMemoryUsage"
    ,round(cast(sum(sru."MemoryUsage") as decimal) / cast(count(*) as decimal),2) as "AvgMemoryUsage"
    ,round(cast(sum(sru."CpuUsage") as decimal) / cast(count(*) as decimal),2) as "AvgCpuUsage"
from "ResultSets" rs 
join "SystemResourceUsage" sru on rs."ResultSetId" = sru."ResultSetId" 
where 
	rs."Value" in (select * from temp_table_ResultSets)
	and sru."ProcessId" <> -1
group by
	sru."ProcessId"
	,sru."ProcessName" 
order by 
	sum(sru."GpuMemoryUsage") 	desc 	
	,sum(sru."MemoryUsage")		desc
	,sum(sru."CpuUsage")		desc
limit 10;

