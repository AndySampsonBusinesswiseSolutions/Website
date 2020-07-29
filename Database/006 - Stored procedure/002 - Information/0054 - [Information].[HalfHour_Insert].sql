USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[HalfHour_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[HalfHour_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-29
-- Description:	Insert new HalfHour into [Information].[HalfHour] table
-- =============================================

ALTER PROCEDURE [Information].[HalfHour_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @HalfHourDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-29 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[HalfHour] WHERE HalfHourDescription = @HalfHourDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[HalfHour]
            (
                CreatedByUserId,
                SourceId,
                HalfHourDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @HalfHourDescription
            )
        END
END
GO
