USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractMeterToProduct_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractMeterToProduct_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-27
-- Description:	Insert new mapping of a ContractMeter to a Product into [Mapping].[ContractMeterToProduct] table
-- =============================================

ALTER PROCEDURE [Mapping].[ContractMeterToProduct_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractMeterId BIGINT,
    @ProductId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-27 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].ContractMeterToProduct
    (
        CreatedByUserId,
        SourceId,
        ContractMeterId,
        ProductId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ContractMeterId,
        @ProductId
    )
END
GO
