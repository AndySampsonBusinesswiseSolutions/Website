USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ContractDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ContractDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-16
-- Description:	Insert new Contract detail into [Customer].[ContractDetail] table
-- =============================================

ALTER PROCEDURE [Customer].[ContractDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractId BIGINT,
    @ContractAttributeId BIGINT,
    @ContractDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-16 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[ContractDetail]
    (
        CreatedByUserId,
        SourceId,
        ContractId,
        ContractAttributeId,
        ContractDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ContractId,
        @ContractAttributeId,
        @ContractDetailDescription
    )
END
GO