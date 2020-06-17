USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessDetail_DeleteByProcessDetailId]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[ProcessDetail_DeleteByProcessDetailId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Delete Process detail from [System].[ProcessDetail] table
-- =============================================

ALTER PROCEDURE [System].[ProcessDetail_DeleteByProcessDetailId]
    @ProcessDetailId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE
        [System].[ProcessDetail]
    SET
        EffectiveToDateTime = GETUTCDATE()
    WHERE
        ProcessDetailId = @ProcessDetailId
END
GO
