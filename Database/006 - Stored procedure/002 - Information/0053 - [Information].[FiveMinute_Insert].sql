USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[FiveMinute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[FiveMinute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-29
-- Description:	Insert new FiveMinute into [Information].[FiveMinute] table
-- =============================================

ALTER PROCEDURE [Information].[FiveMinute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @FiveMinuteDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-29 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[FiveMinute] WHERE FiveMinuteDescription = @FiveMinuteDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[FiveMinute]
            (
                CreatedByUserId,
                SourceId,
                FiveMinuteDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @FiveMinuteDescription
            )
        END
END
GO
