using System;
using System.Data.SqlClient;

namespace Quiz_App
{
    internal static class TheorySchemaInstaller
    {
        public static bool TryEnsureTheoryInfrastructure(out string message)
        {
            message = "Theory schema is ready.";

            if (connection_class.CurrentMode == DatabaseMode.Offline)
            {
                message = "Database access is paused.";
                return false;
            }

            try
            {
                using (SqlConnection connection = connection_class.GetConnection())
                {
                    connection.Open();
                    EnsureTheoryInfrastructure(connection);
                }

                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public static void EnsureTheoryInfrastructure(SqlConnection connection)
        {
            string sql = @"
IF OBJECT_ID(N'dbo.tbl_theory_questions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tbl_theory_questions
    (
        theory_id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        exam_fk_id INT NOT NULL,
        question_text NVARCHAR(MAX) NOT NULL,
        mark INT NOT NULL CONSTRAINT DF_tbl_theory_questions_mark DEFAULT (0),
        question_number INT NOT NULL CONSTRAINT DF_tbl_theory_questions_question_number DEFAULT (1),
        model_answer NVARCHAR(MAX) NULL,
        question_image VARBINARY(MAX) NULL,
        created_at DATETIME NOT NULL CONSTRAINT DF_tbl_theory_questions_created_at DEFAULT (GETDATE())
    );
END;

IF COL_LENGTH(N'dbo.tbl_theory_questions', N'question_image') IS NULL
BEGIN
    ALTER TABLE dbo.tbl_theory_questions ADD question_image VARBINARY(MAX) NULL;
END;

IF OBJECT_ID(N'dbo.tbl_theory_answers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tbl_theory_answers
    (
        answer_id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        theory_fk_id INT NOT NULL,
        student_fk_id INT NOT NULL,
        exam_fk_id INT NOT NULL,
        answer_text NVARCHAR(MAX) NULL,
        score DECIMAL(10,2) NULL,
        teacher_comment NVARCHAR(MAX) NULL,
        is_submitted BIT NOT NULL CONSTRAINT DF_tbl_theory_answers_is_submitted DEFAULT (0),
        last_saved_at DATETIME NULL,
        submitted_at DATETIME NULL,
        graded_at DATETIME NULL,
        created_at DATETIME NOT NULL CONSTRAINT DF_tbl_theory_answers_created_at DEFAULT (GETDATE())
    );
END;

IF COL_LENGTH(N'dbo.score', N'theory_score') IS NULL
BEGIN
    ALTER TABLE dbo.score ADD theory_score DECIMAL(10,2) NULL;
END;

IF COL_LENGTH(N'dbo.score', N'combined_score') IS NULL
BEGIN
    ALTER TABLE dbo.score ADD combined_score DECIMAL(10,2) NULL;
END;

IF COL_LENGTH(N'dbo.score', N'theory_details') IS NULL
BEGIN
    ALTER TABLE dbo.score ADD theory_details NVARCHAR(MAX) NULL;
END;

IF OBJECT_ID(N'dbo.tbl_exam_settings', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.tbl_exam_settings', N'theory_duration_minutes') IS NULL
   AND COL_LENGTH(N'dbo.tbl_exam_settings', N'theory_duration') IS NULL
BEGIN
    ALTER TABLE dbo.tbl_exam_settings ADD theory_duration_minutes INT NULL;
END;

IF OBJECT_ID(N'dbo.tbl_exam_settings', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.tbl_exam_settings', N'theory_exam_enabled') IS NULL
BEGIN
    ALTER TABLE dbo.tbl_exam_settings ADD theory_exam_enabled BIT NOT NULL CONSTRAINT DF_tbl_exam_settings_theory_exam_enabled DEFAULT (1);
END;

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'UX_tbl_theory_answers_question_student_exam'
      AND object_id = OBJECT_ID(N'dbo.tbl_theory_answers')
)
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_tbl_theory_answers_question_student_exam
        ON dbo.tbl_theory_answers(theory_fk_id, student_fk_id, exam_fk_id);
END;

IF OBJECT_ID(N'dbo.tbl_exams', N'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tbl_theory_questions_exam')
BEGIN
    ALTER TABLE dbo.tbl_theory_questions
    ADD CONSTRAINT FK_tbl_theory_questions_exam
        FOREIGN KEY (exam_fk_id) REFERENCES dbo.tbl_exams(ex_id);
END;

IF OBJECT_ID(N'dbo.tbl_theory_questions', N'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tbl_theory_answers_question')
BEGIN
    ALTER TABLE dbo.tbl_theory_answers
    ADD CONSTRAINT FK_tbl_theory_answers_question
        FOREIGN KEY (theory_fk_id) REFERENCES dbo.tbl_theory_questions(theory_id);
END;

IF OBJECT_ID(N'dbo.student_record', N'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tbl_theory_answers_student')
BEGIN
    ALTER TABLE dbo.tbl_theory_answers
    ADD CONSTRAINT FK_tbl_theory_answers_student
        FOREIGN KEY (student_fk_id) REFERENCES dbo.student_record(std_id);
END;

IF OBJECT_ID(N'dbo.tbl_exams', N'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tbl_theory_answers_exam')
BEGIN
    ALTER TABLE dbo.tbl_theory_answers
    ADD CONSTRAINT FK_tbl_theory_answers_exam
        FOREIGN KEY (exam_fk_id) REFERENCES dbo.tbl_exams(ex_id);
END;";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.CommandTimeout = 120;
                command.ExecuteNonQuery();
            }

            string procedureSql = @"
EXEC(N'
CREATE OR ALTER PROCEDURE dbo.usp_UpsertTheoryScores
    @ExamId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH TheoryTotals AS
    (
        SELECT
            ta.student_fk_id,
            ta.exam_fk_id,
            CAST(SUM(ISNULL(ta.score, 0)) AS DECIMAL(10,2)) AS theory_total,
            SUM(CASE WHEN ta.score IS NOT NULL THEN 1 ELSE 0 END) AS graded_answers,
            COUNT(*) AS answer_count
        FROM dbo.tbl_theory_answers ta
        WHERE (@ExamId IS NULL OR ta.exam_fk_id = @ExamId)
        GROUP BY ta.student_fk_id, ta.exam_fk_id
    )
    MERGE dbo.score AS target
    USING TheoryTotals AS src
        ON target.stud_fk_id = src.student_fk_id
       AND target.exam_fk_id = src.exam_fk_id
    WHEN MATCHED THEN
        UPDATE SET
            target.theory_score = src.theory_total,
            target.combined_score = CAST(ISNULL(target.score, 0) AS DECIMAL(10,2)) + src.theory_total,
            target.theory_details = ''Graded answers: '' + CAST(src.graded_answers AS NVARCHAR(20)) + '' of '' + CAST(src.answer_count AS NVARCHAR(20))
    WHEN NOT MATCHED THEN
        INSERT (score, percentage, stud_fk_id, exam_fk_id, theory_score, combined_score, theory_details)
        VALUES
        (
            0,
            0,
            src.student_fk_id,
            src.exam_fk_id,
            src.theory_total,
            src.theory_total,
            ''Graded answers: '' + CAST(src.graded_answers AS NVARCHAR(20)) + '' of '' + CAST(src.answer_count AS NVARCHAR(20))
        );
END
');";

            using (SqlCommand command = new SqlCommand(procedureSql, connection))
            {
                command.CommandTimeout = 120;
                command.ExecuteNonQuery();
            }
        }
    }
}
