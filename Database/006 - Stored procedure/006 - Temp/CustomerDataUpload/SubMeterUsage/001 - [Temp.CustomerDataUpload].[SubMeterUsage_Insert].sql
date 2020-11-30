USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.CustomerDataUpload].[SubMeterUsage_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.CustomerDataUpload].[SubMeterUsage_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-16
-- Description:	Insert new SubMeterUsage into [Temp.CustomerDataUpload].[SubMeterUsage] table
-- =============================================

ALTER PROCEDURE [Temp.CustomerDataUpload].[SubMeterUsage_Insert]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @RowId INT,
    @SubMeterIdentifier VARCHAR(255),
    @Date VARCHAR(255),
    @TimePeriod VARCHAR(255),
    @Value VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-16 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Temp.CustomerDataUpload].[SubMeterUsage]
    (
        ProcessQueueGUID,
        RowId,
        SubMeterIdentifier,
        Date,
        TimePeriod,
        Value
    )
    VALUES
    (
        @ProcessQueueGUID,
        @RowId,
        @SubMeterIdentifier,
        @Date,
        @TimePeriod,
        @Value
    )
END
GO
