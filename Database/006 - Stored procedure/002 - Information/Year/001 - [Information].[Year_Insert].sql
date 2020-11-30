USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[Year_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[Year_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-25
-- Description:	Insert new Year into [Information].[Year] table
-- =============================================

ALTER PROCEDURE [Information].[Year_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @YearDescription VARCHAR(255),
    @IsLeapYear BIT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-25 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Information].[Year]
    (
        CreatedByUserId,
        SourceId,
        YearDescription,
        IsLeapYear
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @YearDescription,
        @IsLeapYear
    )
END
GO