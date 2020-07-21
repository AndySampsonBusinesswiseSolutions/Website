USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.Customer].[FixedContract_GetByProcessQueueGUID]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.Customer].[FixedContract_GetByProcessQueueGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-21
-- Description:	Get FixedContract from [Temp.Customer].[FixedContract] table by Process Queue GUID
-- =============================================

ALTER PROCEDURE [Temp.Customer].[FixedContract_GetByProcessQueueGUID]
    @ProcessQueueGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-21 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
        ProcessQueueGUID,
        ContractReference,
        MPXN,
        Supplier,
        ContractStartDate,
        ContractEndDate,
        Product,
        RateCount,
        StandingCharge,
        CapacityCharge,
        Rate,
        Value
    FROM
        [Temp.Customer].[FixedContract]
    WHERE
        ProcessQueueGUID = @ProcessQueueGUID
END
GO
