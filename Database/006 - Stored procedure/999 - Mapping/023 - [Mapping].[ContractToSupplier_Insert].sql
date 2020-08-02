USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractToSupplier_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractToSupplier_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-27
-- Description:	Insert new mapping of a Contract to a Supplier into [Mapping].[ContractToSupplier] table
-- =============================================

ALTER PROCEDURE [Mapping].[ContractToSupplier_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractId BIGINT,
    @SupplierId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-27 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].ContractToSupplier
    (
        CreatedByUserId,
        SourceId,
        ContractId,
        SupplierId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ContractId,
        @SupplierId
    )
END
GO
