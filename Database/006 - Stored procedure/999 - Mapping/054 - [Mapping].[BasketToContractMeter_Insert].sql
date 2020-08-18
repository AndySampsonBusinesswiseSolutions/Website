USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[BasketToContractMeter_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[BasketToContractMeter_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new mapping of a Basket to a ContractMeter into [Mapping].[BasketToContractMeter] table
-- =============================================

ALTER PROCEDURE [Mapping].[BasketToContractMeter_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @BasketId BIGINT,
    @ContractMeterId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].BasketToContractMeter
    (
        CreatedByUserId,
        SourceId,
        BasketId,
        ContractMeterId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @BasketId,
        @ContractMeterId
    )
END
GO
