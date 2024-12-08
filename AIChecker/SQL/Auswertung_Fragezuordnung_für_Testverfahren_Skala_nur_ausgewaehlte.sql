-- Auswertung
drop table temp_table_ResultSets;
create temp table temp_table_ResultSets
("ResultSets" TEXT);

insert into temp_table_ResultSets
values
	('Fragezuordnung für Testverfahren Skala (Nr. 16) Ministral-8B')

insert into temp_table_ResultSets
values
	('Fragezuordnung für Testverfahren Skala (Nr. 8) mixtral-8x7b-instruct-v0.1');

insert into temp_table_ResultSets
values
	('Fragezuordnung für Testverfahren Skala (Nr. 13) Llama-3.2-3B-Instruct');

insert into temp_table_ResultSets
values
	('Fragezuordnung für Testverfahren Skala (Nr. 14) Llama-3.2-1B-Instruct')

-- Zeiten
with GroupedResults as (
    select 
        coalesce(replace(rs."Value", 'Fragezuordnung für Testverfahren Skala', ''), 'Gesamt') as "Test",
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

-- Hardware Using
select
	sru."ProcessId"
	,sru."ProcessName" 
	,sum(sru."GpuMemoryUsage") / count(*) 	as "TotalGpuMemoryUsage"
	,sum(sru."GpuUsage") / count(*)			as "TotalGpuUsage"
	,sum(sru."MemoryUsage")	/ count(*)		as "TotalMemoryUsage"
	,sum(sru."CpuUsage") / count(*)			as "TotalCpuUsage"
	,max(sru."GpuUsage")
from "ResultSets" rs 
join "SystemResourceUsage" sru on rs."ResultSetId" = sru."ResultSetId" 
where 
	rs."Value" in (select * from temp_table_ResultSets)
group by
	sru."ProcessId"
	,sru."ProcessName" 
order by 
max(sru."GpuUsage")				desc
	,sum(sru."GpuMemoryUsage") 	desc 
	sum(sru."GpuUsage")			desc
	,sum(sru."MemoryUsage")		desc
	,sum(sru."CpuUsage")		desc
limit 10
;
select * from "SystemResourceUsage" sru 
where sru."GpuUsage" is null


	
