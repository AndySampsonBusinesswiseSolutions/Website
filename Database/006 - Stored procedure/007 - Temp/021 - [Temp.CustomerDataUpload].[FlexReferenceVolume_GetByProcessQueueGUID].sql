USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.CustomerDataUpload].[FlexReferenceVolume_GetByProcessQueueGUID]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.CustomerDataUpload].[FlexReferenceVolume_GetByProcessQueueGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-21
-- Description:	Get FlexReferenceVolume from [Temp.CustomerDataUpload].[FlexReferenceVolume] table by Process Queue GUID
-- =============================================

ALTER PROCEDURE [Temp.CustomerDataUpload].[FlexReferenceVolume_GetByProcessQueueGUID]
    @ProcessQueueGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-21 -> Andrew Sampson -> Initial development of script
    -- 2020-08-13 -> Andrew Sampson -> Add CanCommit column
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
        ProcessQueueGUID,
        RowId,
        ContractReference,
        DateFrom,
        DateTo,
        Volume,
        CanCommit
    FROM
        [Temp.CustomerDataUpload].[FlexReferenceVolume]
    WHERE
        ProcessQueueGUID = @ProcessQueueGUID
END
GO
