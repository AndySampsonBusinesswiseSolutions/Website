USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[CustomerToChildCustomer_GetByChildCustomerId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[CustomerToChildCustomer_GetByChildCustomerId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-06
-- Description:	Get CustomerToChildCustomer info from [Mapping].[CustomerToChildCustomer_GetByChildCustomerId] table by Child Customer Id
-- =============================================

ALTER PROCEDURE [Mapping].[CustomerToChildCustomer_GetByChildCustomerId]
    @ChildCustomerId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-06 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        CustomerToChildCustomerId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ChildCustomerId,
        CustomerId
    FROM 
        [Mapping].[CustomerToChildCustomer]
    WHERE 
        ChildCustomerId = @ChildCustomerId
END
GO
