USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.CustomerDataUpload].[MeterUsage_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.CustomerDataUpload].[MeterUsage_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-16
-- Description:	Insert new MeterUsage into [Temp.CustomerDataUpload].[MeterUsage] table
-- =============================================

ALTER PROCEDURE [Temp.CustomerDataUpload].[MeterUsage_Insert]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @RowId INT,
    @MPXN VARCHAR(255),
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

    INSERT INTO [Temp.CustomerDataUpload].[MeterUsage]
    (
        ProcessQueueGUID,
        RowId,
        MPXN,
        Date,
        TimePeriod,
        Value
    )
    VALUES
    (
        @ProcessQueueGUID,
        @RowId,
        @MPXN,
        @Date,
        @TimePeriod,
        @Value
    )
END
GO
