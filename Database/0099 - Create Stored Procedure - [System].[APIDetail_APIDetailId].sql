USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[APIDetail_DeleteByAPIDetailId]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[APIDetail_DeleteByAPIDetailId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	End-date API detail record in [System].[APIDetail] table
-- =============================================

ALTER PROCEDURE [System].[APIDetail_DeleteByAPIDetailId]
	@APIDetailId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE
        [System].[APIDetail]
    SET
        EffectiveToDateTime = GETUTCDATE()
    WHERE
        APIDetailId = @APIDetailId
END
GO
