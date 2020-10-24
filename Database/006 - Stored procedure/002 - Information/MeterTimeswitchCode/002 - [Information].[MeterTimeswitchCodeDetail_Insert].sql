USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[MeterTimeswitchCodeDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[MeterTimeswitchCodeDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-22
-- Description:	Insert new Meter Timeswitch Code detail into [Information].[MeterTimeswitchCodeDetail] table
-- =============================================

ALTER PROCEDURE [Information].[MeterTimeswitchCodeDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @MeterTimeswitchCodeId BIGINT,
    @MeterTimeswitchCodeAttributeId BIGINT,
    @MeterTimeswitchCodeDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-22 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Information].[MeterTimeswitchCodeDetail]
    (
        CreatedByUserId,
        SourceId,
        MeterTimeswitchCodeId,
        MeterTimeswitchCodeAttributeId,
        MeterTimeswitchCodeDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @MeterTimeswitchCodeId,
        @MeterTimeswitchCodeAttributeId,
        @MeterTimeswitchCodeDetailDescription
    )
END
GO