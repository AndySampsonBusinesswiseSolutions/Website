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
    @ProcessGUID UNIQUEIDENTIFIER,
    @CustomerGUID UNIQUEIDENTIFIER,
    @MPXN VARCHAR(255),
    @SubMeterIdentifier VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Temp.Customer].[SubMeter]
    (
        ProcessGUID,
        CustomerGUID,
        MPXN,
        SubMeterIdentifier
    )
    VALUES
    (
        @ProcessGUID,
        @CustomerGUID,
        @MPXN,
        @SubMeterIdentifier
    )
END
GO
