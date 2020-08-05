USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[FlexContractAttribute_GetByFlexContractAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[FlexContractAttribute_GetByFlexContractAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Get FlexContractAttribute info from [Customer].[FlexContractAttribute] table by FlexContractAttributeDescription
-- =============================================

ALTER PROCEDURE [Customer].[FlexContractAttribute_GetByFlexContractAttributeDescription]
    @FlexContractAttributeDescription VARCHAR(255),
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        FlexContractAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        FlexContractAttributeDescription
    FROM 
        [Customer].[FlexContractAttribute] 
    WHERE 
        FlexContractAttributeDescription = @FlexContractAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
