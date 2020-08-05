USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[FlexContractDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[FlexContractDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Insert new FlexContract detail into [Customer].[FlexContractDetail] table
-- =============================================

ALTER PROCEDURE [Customer].[FlexContractDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @FlexContractId BIGINT,
    @FlexContractAttributeId BIGINT,
    @FlexContractDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[FlexContractDetail] WHERE FlexContractId = @FlexContractId 
        AND FlexContractAttributeId = @FlexContractAttributeId 
        AND FlexContractDetailDescription = @FlexContractDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[FlexContractDetail]
            (
                CreatedByUserId,
                SourceId,
                FlexContractId,
                FlexContractAttributeId,
                FlexContractDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @FlexContractId,
                @FlexContractAttributeId,
                @FlexContractDetailDescription
            )
        END
END
GO
