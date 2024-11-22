-- Einzelne Zeiten
select 
m."Value",
r."RequestEnd", r."RequestStart",
r."RequestEnd" - r."RequestStart"
from "Results" r 
join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId"
join "Models" m on r."ModelId" = m."ModelId" 
where rs."Value" like 'Fragezuordnung für Testverfahren Skala auf gpt-mini (Nr. 1) Performance Test'
order by m."Value" ;


-- Zeiten des  des Tests
select 
replace(rs."Value", 'Fragezuordnung für Testverfahren Skala','') as "Test",
count(*) as "Anzahl Anfragen",
(max(r."RequestEnd") - min(r."RequestStart")) as "Dauer",
(max(r."RequestEnd") - min(r."RequestStart")) / count(*) as "Durchschnittliche Dauer" 
from "Results" r 
join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId" 
where rs."Value" like 'Fragezuordnung für Testverfahren Skala auf gpt-mini (Nr. 1) Performance Test'
group by rs."Value";

-- Verschiedenen Bewertungen
select
	(r."Message"::jsonb ->> 'Bewertung')::numeric as "Bewertung"	
	,count(*) as "Anzahl"
from "Results" r 
join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId" 
where 
rs."Value" like 'Fragezuordnung für Testverfahren Skala auf gpt-mini%' and
r."IsJson" = true
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
        rs."Value" like 'Fragezuordnung für Testverfahren Skala auf gpt-mini%' 
        and r."IsJson" = true
)
select "QuestionsId", "Bewertung", "MaxValueCount" as "doppelte Maximalwerte"
from maxbewertungen
where "Bewertung" = "MaxBewertung" 
order by "MaxValueCount"

-- Treffer/-quote
with maxbewertungen as (
    select
    	rs."Value" as "ResultSet",
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
        rs."Value" like 'Fragezuordnung für Testverfahren Skala auf gpt-mini%' 
        and r."IsJson" = true
)
select "ResultSet", "QuestionsId", "AnswerIdResult", "AnswerIdQuestions(Korrekt)", "Bewertung", "MaxValueCount"
from maxbewertungen
where "Bewertung" = "MaxBewertung"
and "AnswerIdQuestions(Korrekt)" = "AnswerIdResult"
order by "ResultSet","MaxValueCount";


-- Tokens
select 
	sum(r."CompletionTokens") as "CompletionTokens"
	,sum(r."PromptTokens") as "PromptTokens"
	,sum(r."TotalTokens") as "TotalTokens"
	,sum(r."PromptCachedTokens") as "CashedTokens"
	,min(r."RequestStart") - max(r."RequestEnd") as "CompleteTime" 
from "Results" r 
join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId" 
where rs."Value" like 'Fragezuordnung für Testverfahren Skala auf gpt-4o (Nr. 1) Teams Citrix created Questions';

select 
	min(r."CompletionTokens") as "CompletionTokens"
	,min(r."PromptTokens") as "PromptTokens"
	,min(r."TotalTokens") as "TotalTokens"	
from "Results" r 
join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId" 
where rs."Value" like 'Fragezuordnung für Testverfahren Skala auf gpt-mini%';

---- GPT 4

select *
from "Results" r 
join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId"
where rs."Value" = 'Fragezuordnung für Testverfahren Skala auf gpt-mini (Nr. 6) Cashing Test - Azubi FAQ'

-- Tokens 
select 
	sum(r."CompletionTokens") as "CompletionTokens"
	,sum(r."PromptTokens") as "PromptTokens"
	,sum(r."TotalTokens") as "TotalTokens"
	,sum(r."PromptCachedTokens") as "CashedTokens"
	,min(r."RequestStart") - max(r."RequestEnd") as "CompleteTime" 
from "Results" r 
join "ResultSets" rs on r."ResultSetId" = rs."ResultSetId" 
where rs."Value" like 'Fragezuordnung für Testverfahren Skala auf gpt-4o%';

with maxbewertungen as (
    select
    	rs."Value" as "ResultSet",
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
        rs."Value" like 'Fragezuordnung für Testverfahren Skala auf gpt-4o%' 
        and r."IsJson" = true
)
select "ResultSet", "QuestionsId", "AnswerIdResult", "AnswerIdQuestions(Korrekt)", "Bewertung", "MaxValueCount"
from maxbewertungen
where "Bewertung" = "MaxBewertung"
and "AnswerIdQuestions(Korrekt)" = "AnswerIdResult"
order by "ResultSet","MaxValueCount";



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
        rs."Value" like 'Fragezuordnung für Testverfahren Skala auf gpt-4o%' 
        and r."IsJson" = true
)
select "ResultSet", "QuestionsId", "AnswerIdResult", "AnswerIdQuestions(Korrekt)", "Bewertung", "MaxValueCount"
from maxbewertungen
where rank <= 1
and "AnswerIdQuestions(Korrekt)" = "AnswerIdResult"
order by "ResultSet", "QuestionsId", "Bewertung" desc, "MaxValueCount";



select * from "ResultSets" rs; 
select * from "Models" m ;

