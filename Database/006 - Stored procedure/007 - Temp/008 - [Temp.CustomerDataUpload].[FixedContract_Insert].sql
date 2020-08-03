USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.CustomerDataUpload].[FixedContract_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.CustomerDataUpload].[FixedContract_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-20
-- Description:	Insert new FixedContract into [Temp.CustomerDataUpload].[FixedContract] table
-- =============================================

ALTER PROCEDURE [Temp.CustomerDataUpload].[FixedContract_Insert]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @RowId INT,
    @ContractReference VARCHAR(255),
    @MPXN VARCHAR(255),
    @Supplier VARCHAR(255),
    @ContractStartDate VARCHAR(255),
    @ContractEndDate VARCHAR(255),
    @Product VARCHAR(255),
    @RateCount VARCHAR(255),
    @StandingCharge VARCHAR(255),
    @CapacityCharge VARCHAR(255),
    @Rate VARCHAR(255),
    @Value VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-20 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Temp.CustomerDataUpload].[FixedContract]
    (
        ProcessQueueGUID,
        RowId,
        ContractReference,
        MPXN,
        Supplier,
        ContractStartDate,
        ContractEndDate,
        Product,
        RateCount,
        StandingCharge,
        CapacityCharge,
        Rate,
        Value
    )
    VALUES
    (
        @ProcessQueueGUID,
        @RowId,
        @ContractReference,
        @MPXN,
        @Supplier,
        @ContractStartDate,
        @ContractEndDate,
        @Product,
        @RateCount,
        @StandingCharge,
        @CapacityCharge,
        @Rate,
        @Value
    )
END
GO
