select * from "QuestionCategories" qc ;
select count(*) from "Questions" q 
join "QuestionCategories" qc on qc."QuestionCategoryId" = q."QuestionCategoryId" 
where qc."Value" = 'Teams allgemein'

-- SQL to check correct questions
WITH vars AS (
    SELECT
--      'Outlook created Questions over gpt-4o-mini' AS question_value,
--      'Outlook' AS question_old_value
--    	'Teams allgemein created Questions over gpt-4o-mini' as question_value,
--		'Teams allgemein' as question_old_value
--	   	'Teams Citrix created Questions over gpt-4o-mini' as question_value,
--		'Teams Citrix' as question_old_value
	   	'Azubi FAQ created Questions over gpt-4o-mini' as question_value,
		'Azubi FAQ' as question_old_value
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

-- SQL to Export Checked Questions
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
	   	'Azubi FAQ created Questions over gpt-4o-mini' as question_value,
		'Azubi FAQ' as question_old_value
),
filtered_questions AS (
    SELECT
        q2."Value" AS "Original Question",
        a."Value" AS "Answer",
        q."Value" AS "Question",
        q."Correct",        
        qc."Value" AS "Category"
    FROM "Questions" q
    JOIN "Answers" a ON q."AnswerId" = a."AnswerId"
    JOIN "QuestionCategories" qc ON qc."QuestionCategoryId" = q."QuestionCategoryId"
    JOIN "Questions" q2 ON q2."AnswerId" = q."AnswerId"
    JOIN vars ON qc."Value" = vars.question_value    
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
    fq."Category",
    (CAST(c.total_checked AS NUMERIC) / CAST(t.total_questions AS NUMERIC)) * 100 AS percentage_checked
FROM filtered_questions fq
CROSS JOIN counts c
CROSS JOIN total t
where 
	fq."Correct" is true
order by fq."Correct";



