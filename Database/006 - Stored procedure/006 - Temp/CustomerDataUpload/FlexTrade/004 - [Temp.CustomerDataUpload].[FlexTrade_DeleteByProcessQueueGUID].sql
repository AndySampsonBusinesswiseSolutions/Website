USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.CustomerDataUpload].[FlexTrade_DeleteByProcessQueueGUID]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.CustomerDataUpload].[FlexTrade_DeleteByProcessQueueGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-28
-- Description:	Delete FlexTrade from [Temp.CustomerDataUpload].[FlexTrade] table by Process Queue GUID by Process Queue GUID
-- =============================================

ALTER PROCEDURE [Temp.CustomerDataUpload].[FlexTrade_DeleteByProcessQueueGUID]
    @ProcessQueueGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-28 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE
    FROM
        [Temp.CustomerDataUpload].[FlexTrade]
    WHERE
        ProcessQueueGUID = @ProcessQueueGUID
END
GO
