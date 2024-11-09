set my.question_value = 'Outlook created Questions over gpt-4o-mini';
set my.question_old_value = 'Outlook'
show my.question_value;
show my.question_old_value;


WITH vars AS (
    SELECT 
        'Outlook created Questions over gpt-4o-mini' AS question_value,
        'Outlook' AS question_old_value
)
SELECT 
    a."Value" AS "Answer",
    q."Value" AS "Question",
    qc."Value" AS "Category",
    q."Correct",
    q."QuestionId",
    q2."Value" AS "AdditionalQuestion"
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
  );


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




