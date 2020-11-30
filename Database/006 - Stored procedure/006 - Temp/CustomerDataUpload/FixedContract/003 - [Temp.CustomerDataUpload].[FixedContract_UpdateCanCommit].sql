USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.CustomerDataUpload].[FixedContract_UpdateCanCommit]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.CustomerDataUpload].[FixedContract_UpdateCanCommit] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Update Can Commit flag in [Temp.CustomerDataUpload].[FixedContract] table
-- =============================================

ALTER PROCEDURE [Temp.CustomerDataUpload].[FixedContract_UpdateCanCommit]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @RowId INT,
    @CanCommit BIT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE
        [Temp.CustomerDataUpload].[FixedContract]
    SET
        CanCommit = @CanCommit
    WHERE
        ProcessQueueGUID = @ProcessQueueGUID
        AND RowId = @RowId
END
GO
