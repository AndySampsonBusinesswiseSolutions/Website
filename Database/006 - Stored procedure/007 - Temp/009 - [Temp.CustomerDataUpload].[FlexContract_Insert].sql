USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.CustomerDataUpload].[FlexContract_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.CustomerDataUpload].[FlexContract_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-20
-- Description:	Insert new FlexContract into [Temp.CustomerDataUpload].[FlexContract] table
-- =============================================

ALTER PROCEDURE [Temp.CustomerDataUpload].[FlexContract_Insert]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @RowId INT,
    @ContractReference VARCHAR(255),
    @BasketReference VARCHAR(255),
    @MPXN VARCHAR(255),
    @Supplier VARCHAR(255),
    @ContractStartDate VARCHAR(255),
    @ContractEndDate VARCHAR(255),
    @Product VARCHAR(255),
    @StandingCharge VARCHAR(255),
    @RateType VARCHAR(255),
    @Value VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-20 -> Andrew Sampson -> Initial development of script
    -- 2020-07-26 -> Andrew Sampson -> Changed for scalability
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Temp.CustomerDataUpload].[FlexContract]
    (
        ProcessQueueGUID,
        RowId,
        ContractReference,
        BasketReference,
        MPXN,
        Supplier,
        ContractStartDate,
        ContractEndDate,
        Product,
        StandingCharge,
        RateType,
        Value
    )
    VALUES
    (
        @ProcessQueueGUID,
        @RowId,
        @ContractReference,
        @BasketReference,
        @MPXN,
        @Supplier,
        @ContractStartDate,
        @ContractEndDate,
        @Product,
        @StandingCharge,
        @RateType,
        @Value
    )
END
GO
