with vars as (
select
	'Outlook created Questions over gpt-4o-mini' as question_value,
	'Outlook' as question_old_value
)
select
	q2."Value" as "Original Question",
	a."Value" as "Answer",
	q."Value" as "Question",
--	qc."Value" as "Category",
	q."Correct",
	q."QuestionId"
from
	"Questions" q
join "Answers" a on
	q."AnswerId" = a."AnswerId"
join "QuestionCategories" qc on
	qc."QuestionCategoryId" = q."QuestionCategoryId"
join "Questions" q2 on
	q2."AnswerId" = q."AnswerId"
join vars on
	qc."Value" = vars.question_value
where
	q."Correct" is null
	and exists (
	select
		1
	from
		"QuestionCategories" qc_old
	where
		qc_old."QuestionCategoryId" = q2."QuestionCategoryId"
		and qc_old."Value" = vars.question_old_value				
  )
order by
	random()
limit 5;

 
set my.question_value = 'Outlook created Questions over gpt-4o-mini';
set my.question_old_value = 'Outlook'
show my.question_value;
show my.question_old_value;

select *
from "Questions" q 
where q."Correct" is not null; 

WITH counts AS (
    SELECT 
        COUNT(*) AS total_checked
    FROM "Questions" q
    JOIN "QuestionCategories" qc ON qc."QuestionCategoryId" = q."QuestionCategoryId"
	WHERE qc."Value" = current_setting('my.question_value') AND q."Correct" IS NOT NULL
), 
total AS (
    SELECT 
        COUNT(*) AS total_questions
    FROM "Questions" q
    JOIN "QuestionCategories" qc ON qc."QuestionCategoryId" = q."QuestionCategoryId"
	WHERE qc."Value" = current_setting('my.question_value')
)
SELECT 
    (CAST(c.total_checked AS NUMERIC) / CAST(t.total_questions AS NUMERIC)) * 100 AS percentage
FROM counts c, total t;


-- SQL to check correct questions
WITH vars AS (
    SELECT
        'Outlook created Questions over gpt-4o-mini' AS question_value,
        'Outlook' AS question_old_value
),
filtered_questions AS (
    SELECT
        q2."Value" AS "Original Question",
        a."Value" AS "Answer",
        q."Value" AS "Question",
        q."Correct",
        q."QuestionId",
        qc."Value" AS "Category"
    FROM "Questions" q
    JOIN "Answers" a ON q."AnswerId" = a."AnswerId"
    JOIN "QuestionCategories" qc ON qc."QuestionCategoryId" = q."QuestionCategoryId"
    JOIN "Questions" q2 ON q2."AnswerId" = q."AnswerId"
    JOIN vars ON qc."Value" = vars.question_value
    WHERE q."Correct" IS NULL
      AND EXISTS (
        SELECT 1
        FROM "QuestionCategories" qc_old
        WHERE qc_old."QuestionCategoryId" = q2."QuestionCategoryId"
          AND qc_old."Value" = vars.question_old_value
      )
),
counts AS (
    SELECT
        COUNT(*) AS total_checked
    FROM "Questions" q
    JOIN "QuestionCategories" qc ON qc."QuestionCategoryId" = q."QuestionCategoryId"
    JOIN vars ON qc."Value" = vars.question_value
    WHERE q."Correct" IS NOT NULL
), 
total AS (
    SELECT
        COUNT(*) AS total_questions
    FROM "Questions" q
    JOIN "QuestionCategories" qc ON qc."QuestionCategoryId" = q."QuestionCategoryId"
    JOIN vars ON qc."Value" = vars.question_value
)
SELECT 
    fq."Original Question",
    fq."Answer",
    fq."Question",
    fq."Correct",
    fq."QuestionId",
    fq."Category",
    (CAST(c.total_checked AS NUMERIC) / CAST(t.total_questions AS NUMERIC)) * 100 AS percentage_checked
FROM filtered_questions fq
CROSS JOIN counts c
CROSS JOIN total t
ORDER BY RANDOM()
LIMIT 5;