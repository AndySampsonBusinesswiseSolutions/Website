USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.Customer].[SubMeter_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.Customer].[SubMeter_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-15
-- Description:	Insert new SubMeter into [Temp.Customer].[SubMeter] table
-- =============================================

ALTER PROCEDURE [Temp.Customer].[SubMeter_Insert]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @MPXN VARCHAR(255),
    @SubMeterIdentifier VARCHAR(255),
    @SerialNumber VARCHAR(255),
    @SubArea VARCHAR(255),
    @Asset VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-15 -> Andrew Sampson -> Initial development of script
    -- 2020-07-20 -> Andrew Sampson -> Updates to handle new upload template
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Temp.Customer].[SubMeter]
    (
        ProcessQueueGUID,
        MPXN,
        SubMeterIdentifier,
        SerialNumber,
        SubArea,
        Asset
    )
    VALUES
    (
        @ProcessQueueGUID,
        @MPXN,
        @SubMeterIdentifier,
        @SerialNumber,
        @SubArea,
        @Asset
    )
END
GO
