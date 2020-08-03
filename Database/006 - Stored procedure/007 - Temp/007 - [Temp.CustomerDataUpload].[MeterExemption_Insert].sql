USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.CustomerDataUpload].[MeterExemption_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.CustomerDataUpload].[MeterExemption_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-20
-- Description:	Insert new MeterExemption into [Temp.CustomerDataUpload].[MeterExemption] table
-- =============================================

ALTER PROCEDURE [Temp.CustomerDataUpload].[MeterExemption_Insert]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @RowId INT,
    @MPXN VARCHAR(255),
    @DateFrom VARCHAR(255),
    @DateTo VARCHAR(255),
    @ExemptionProduct VARCHAR(255),
    @ExemptionProportion VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-20 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Temp.CustomerDataUpload].[MeterExemption]
    (
        ProcessQueueGUID,
        RowId,
        MPXN,
        DateFrom,
        DateTo,
        ExemptionProduct,
        ExemptionProportion
    )
    VALUES
    (
        @ProcessQueueGUID,
        @RowId,
        @MPXN,
        @DateFrom,
        @DateTo,
        @ExemptionProduct,
        @ExemptionProportion
    )
END
GO
