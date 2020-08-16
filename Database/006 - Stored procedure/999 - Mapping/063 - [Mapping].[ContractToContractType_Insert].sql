USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractToContractType_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractToContractType_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-16
-- Description:	Insert new mapping of a Contract to a ContractType into [Mapping].[ContractToContractType] table
-- =============================================

ALTER PROCEDURE [Mapping].[ContractToContractType_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractId BIGINT,
    @ContractTypeId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-16 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].ContractToContractType
    (
        CreatedByUserId,
        SourceId,
        ContractId,
        ContractTypeId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ContractId,
        @ContractTypeId
    )
END
GO
