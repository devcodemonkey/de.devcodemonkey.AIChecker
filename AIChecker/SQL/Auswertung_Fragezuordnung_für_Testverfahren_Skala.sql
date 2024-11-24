select rs."Value" ,count(*) 
from "ResultSets" rs
join "Results" r on r."ResultSetId" = rs."ResultSetId"
where rs."Value" like '%Fragezu%'
group by rs."Value";

-- Anzahl Fragen
select 
coalesce (qc."Value", 'Gesamt') 	as "Kategorie", 	
count(*) 							as "Anzahl"		
from "Questions" q 
join "QuestionCategories" qc on qc."QuestionCategoryId" = q."QuestionCategoryId"
where qc."Value" like '%gpt-4o-mini%' and q."IsCorrect" 
group by rollup(qc."Value")
order by 
	case when qc."Value" is null then 1 else 0 end; 


select 
m."Value",
r."RequestEnd", r."RequestStart",
r."RequestEnd" - r."RequestStart"
from "Results" r 
join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId"
join "Models" m on r."ModelId" = m."ModelId" 
where rs."Value" like 'Fragezuordnung für Testverfahren Skala (Nr. 6) Performance Test'
order by m."Value" ;

-- Auswertung
drop table temp_table_ResultSets;
create temp table temp_table_ResultSets
("ResultSets" TEXT);

insert into temp_table_ResultSets
values	
	('Fragezuordnung für Testverfahren Skala (Nr. 1) Outlook allgemein korrekt geprüfte Fragen'),
	('Fragezuordnung für Testverfahren Skala (Nr. 2) Teams Citrix korrekt geprüfte Fragen'),
	('Fragezuordnung für Testverfahren Skala (Nr. 3) Teams allgemein korrekt geprüfte Fragen'),
	('Fragezuordnung für Testverfahren Skala (Nr. 4) Azubi FAQ korrekt geprüfte Fragen');

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
	('Fragezuordnung für Testverfahren Skala (Nr. 8) mixtral-8x7b-instruct-v0.1')
   
insert into temp_table_ResultSets
values
	('Fragezuordnung für Testverfahren Skala (Nr. 10) Meta-Llama-3.1-70B')

-- Zeiten des  des Tests
 	
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



-- Zeiten
select
rs."Value" ,
r."Asked" ,
length(r."Asked"),
length(r."Message"),
m."Value" ,
r."RequestEnd", r."RequestStart",
r."RequestEnd" - r."RequestStart" as "Dauer"
from "Results" r 
join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId"
join "Models" m on r."ModelId" = m."ModelId" 
where rs."Value" in (select * from temp_table_ResultSets)
order by "Dauer" desc ;

select count(*)
from "SystemResourceUsage" sru;

-- Valid JSON Format

select
	COUNT(*) FILTER (WHERE r."IsJson" = true) AS "Anzahl ValidJsonFormat",
	COUNT(*) FILTER (WHERE r."IsJson" = false) AS "Anzahl InvalidJsonFormat"	
from "Results" r 
join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId" 
where 
rs."Value" in (select * from temp_table_ResultSets)


-- Verschiedenen Bewertungen
select
	(r."Message"::jsonb ->> 'Bewertung')::numeric as "Bewertung"	
	,count(*) as "Anzahl"
from "Results" r 
join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId" 
where 
rs."Value" in (select * from temp_table_ResultSets) 
and r."IsJson" = true
group by (r."Message"::jsonb ->> 'Bewertung')::numeric
order by count(*) desc ;


-- doppelte Maximalwerte
with maxbewertungen as (
    select distinct 
        r."QuestionsId",        
        (r."Message"::jsonb ->> 'Bewertung')::numeric as "Bewertung",
        max((r."Message"::jsonb ->> 'Bewertung')::numeric) over (partition by r."QuestionsId") as "MaxBewertung",
        count(*) over (partition by r."QuestionsId", (r."Message"::jsonb ->> 'Bewertung')::numeric) as "MaxValueCount"
    from "Results" r
    join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId"
    left join "Questions" q on q."QuestionId" = r."QuestionsId"
    where 
        rs."Value" in (select * from temp_table_ResultSets)  
        and r."IsJson" = true
)
select "QuestionsId", "Bewertung", "MaxValueCount" as "doppelte Maximalwerte"
from maxbewertungen
where "Bewertung" = "MaxBewertung" 
order by "MaxValueCount";

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
    left join "Questions" q on q."QuestionId" = r."QuestionsId"
    where 
        rs."Value" in (select * from temp_table_ResultSets)  
        and r."IsJson" = true
)
select "ResultSet","QuestionsId", "AnswerIdResult", "AnswerIdQuestions(Korrekt)", "Bewertung", "MaxValueCount"
from maxbewertungen
where "Bewertung" = "MaxBewertung"
and "AnswerIdQuestions(Korrekt)" = "AnswerIdResult"
order by "MaxValueCount";

--- 3 Besten werte, Berchnung, der Treffer
with maxbewertungen as (
    select
        rs."Value" as "ResultSet",
        r."QuestionsId",
        r."AnswerId" as "AnswerIdResult",
        q."AnswerId" as "AnswerIdQuestions(Korrekt)",
        (r."Message"::jsonb ->> 'Bewertung')::numeric as "Bewertung",
        max((r."Message"::jsonb ->> 'Bewertung')::numeric) over (partition by r."QuestionsId") as "MaxBewertung",
        count(*) over (partition by r."QuestionsId", (r."Message"::jsonb ->> 'Bewertung')::numeric) as "MaxValueCount",
        rank() over (partition by r."QuestionsId" order by (r."Message"::jsonb ->> 'Bewertung')::numeric desc) as rank
    from "Results" r
    join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId"
    left join "Questions" q on q."QuestionId" = r."QuestionsId"
    where 
        rs."Value" in (select * from temp_table_ResultSets)  
        and r."IsJson" = true
)
select "ResultSet", "QuestionsId", "AnswerIdResult", "AnswerIdQuestions(Korrekt)", "Bewertung", "MaxValueCount"
from maxbewertungen
where rank <= 3
and "AnswerIdQuestions(Korrekt)" = "AnswerIdResult"
order by "ResultSet", "QuestionsId", "Bewertung" desc, "MaxValueCount";

-- Tokens
select 
	sum(r."CompletionTokens") as "CompletionTokens"
	,sum(r."PromptTokens") as "PromptTokens"
	,sum(r."TotalTokens") as "TotalTokens"
from "Results" r 
join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId" 
where rs."Value" in (select * from temp_table_ResultSets)


select * from "Questions" q 
where q."QuestionId" = '9c0db720-2886-43a4-a4c1-b888f953ad46';
