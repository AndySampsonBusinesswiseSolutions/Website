USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ContractAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ContractAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-27
-- Description:	Insert new contract attribute into [Customer].[ContractAttribute] table
-- =============================================

ALTER PROCEDURE [Customer].[ContractAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-27 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[ContractAttribute] WHERE ContractAttributeDescription = @ContractAttributeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[ContractAttribute]
            (
                CreatedByUserId,
                SourceId,
                ContractAttributeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @ContractAttributeDescription
            )
        END
END
GO
