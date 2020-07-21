USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.Customer].[FlexContract_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.Customer].[FlexContract_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-20
-- Description:	Insert new FlexContract into [Temp.Customer].[FlexContract] table
-- =============================================

ALTER PROCEDURE [Temp.Customer].[FlexContract_Insert]
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
    @ShapeFee VARCHAR(255),
    @AdminFee VARCHAR(255),
    @ImbalanceFee VARCHAR(255),
    @RiskFee VARCHAR(255),
    @GreenPremium VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-20 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Temp.Customer].[FlexContract]
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
        ShapeFee,
        AdminFee,
        ImbalanceFee,
        RiskFee,
        GreenPremium
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
        @ShapeFee,
        @AdminFee,
        @ImbalanceFee,
        @RiskFee,
        @GreenPremium
    )
END
GO
