USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[MeterTimeswitchCode_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[MeterTimeswitchCode_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-22
-- Description:	Insert new Meter Timeswitch Code into [Information].[MeterTimeswitchCode] table
-- =============================================

ALTER PROCEDURE [Information].[MeterTimeswitchCode_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @MeterTimeswitchCodeGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-22 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[MeterTimeswitchCode] WHERE MeterTimeswitchCodeGUID = @MeterTimeswitchCodeGUID
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[MeterTimeswitchCode]
            (
                CreatedByUserId,
                SourceId,
                MeterTimeswitchCodeGUID
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @MeterTimeswitchCodeGUID
            )
        END
END
GO
