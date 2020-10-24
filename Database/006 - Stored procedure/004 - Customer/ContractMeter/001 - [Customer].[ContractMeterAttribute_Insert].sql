USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ContractMeterAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ContractMeterAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-27
-- Description:	Insert new contract meter attribute into [Customer].[ContractMeterAttribute] table
-- =============================================

ALTER PROCEDURE [Customer].[ContractMeterAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractMeterAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-27 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[ContractMeterAttribute]
    (
        CreatedByUserId,
        SourceId,
        ContractMeterAttributeDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ContractMeterAttributeDescription
    )
END
GO