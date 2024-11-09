set my.question_value = 'Outlook created Questions over gpt-4o-mini';
set my.question_old_value = 'Outlook'
show my.question_value;


SELECT 
    a."Value" AS Answer,
    q."Value" AS Question,
    qc."Value" AS Category,
    q."Correct",
    q."QuestionId"
FROM "Questions" q
JOIN "Answers" a ON q."AnswerId" = a."AnswerId"
JOIN "QuestionCategories" qc ON qc."QuestionCategoryId" = q."QuestionCategoryId"
WHERE qc."Value" = current_setting('my.question_value')
  AND q."Correct" IS NULL
ORDER BY RANDOM()
LIMIT 5;


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




