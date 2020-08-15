USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[SubAreaToSubMeter_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[SubAreaToSubMeter_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new mapping of a SubArea to a SubMeter into [Mapping].[SubAreaToSubMeter] table
-- =============================================

ALTER PROCEDURE [Mapping].[SubAreaToSubMeter_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @SubAreaId BIGINT,
    @SubMeterId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].SubAreaToSubMeter
    (
        CreatedByUserId,
        SourceId,
        SubAreaId,
        SubMeterId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @SubAreaId,
        @SubMeterId
    )
END
GO
