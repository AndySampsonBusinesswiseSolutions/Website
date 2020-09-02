USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[TimePeriodDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[TimePeriodDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-02
-- Description:	Insert new TimePeriod detail into [Information].[TimePeriodDetail] table
-- =============================================

ALTER PROCEDURE [Information].[TimePeriodDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @TimePeriodId BIGINT,
    @TimePeriodAttributeId BIGINT,
    @TimePeriodDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[TimePeriodDetail] WHERE TimePeriodId = @TimePeriodId 
        AND TimePeriodAttributeId = @TimePeriodAttributeId 
        AND TimePeriodDetailDescription = @TimePeriodDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[TimePeriodDetail]
            (
                CreatedByUserId,
                SourceId,
                TimePeriodId,
                TimePeriodAttributeId,
                TimePeriodDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @TimePeriodId,
                @TimePeriodAttributeId,
                @TimePeriodDetailDescription
            )
        END
END
GO
