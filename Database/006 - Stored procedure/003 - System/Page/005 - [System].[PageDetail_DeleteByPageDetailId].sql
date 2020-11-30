USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[PageDetail_DeleteByPageDetailId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[PageDetail_DeleteByPageDetailId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Delete Page detail from [System].[PageDetail] table
-- =============================================

ALTER PROCEDURE [System].[PageDetail_DeleteByPageDetailId]
    @PageDetailId BIGINT
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
        [System].[PageDetail]
    SET
        EffectiveToDateTime = GETUTCDATE()
    WHERE
        PageDetailId = @PageDetailId
END
GO
