USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[Quarter_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[Quarter_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-29
-- Description:	Insert new Quarter into [Information].[Quarter] table
-- =============================================

ALTER PROCEDURE [Information].[Quarter_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @QuarterDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-29 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[Quarter] WHERE QuarterDescription = @QuarterDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[Quarter]
            (
                CreatedByUserId,
                SourceId,
                QuarterDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @QuarterDescription
            )
        END
END
GO
