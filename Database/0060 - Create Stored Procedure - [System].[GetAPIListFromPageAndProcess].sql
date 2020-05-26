USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[GetAPIListFromPageAndProcess]'))
   exec('CREATE PROCEDURE [System].[GetAPIListFromPageAndProcess] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [System].[GetAPIListFromPageAndProcess]
	@Page varchar(255),
	@Process varchar(255),
	@EffectiveDate DATETIME = NULL
AS
BEGIN
-- =============================================
-- Author:      Andrew Sampson
-- Create date: 2020-05-26
-- Description: Gets a list of APIs from a Page/Process combination
--
-- Parameters:  None
-- Returns:     List of API application URLs
--
-- Change History:
--   2020-05-26 - Initial Creation - Andrew Sampson - Initial creation of stored procedure
-- =============================================

	SET NOCOUNT ON;

	SET @EffectiveDate = ISNULL(@EffectiveDate, GETDATE())

    SELECT
		APIDetail.APIDetailDescription
	FROM
		System.Page
	INNER JOIN
		System.PageDetail
		ON PageDetail.PageId = Page.PageId
		AND @EffectiveDate BETWEEN PageDetail.EffectiveFromDateTime AND PageDetail.EffectiveToDateTime
		AND PageDetail.PageDetailDescription = @Page
	INNER JOIN
		System.PageAttribute
		ON PageAttribute.PageAttributeId = PageDetail.PageAttributeId
		AND @EffectiveDate BETWEEN PageAttribute.EffectiveFromDateTime AND PageAttribute.EffectiveToDateTime
		AND PageAttribute.PageAttributeDescription = 'PageName'
	INNER JOIN
		Mapping.PageToProcess
		ON PageToProcess.PageId = Page.PageId
		AND @EffectiveDate BETWEEN PageToProcess.EffectiveFromDateTime AND PageToProcess.EffectiveToDateTime
	INNER JOIN
		System.Process
		ON Process.ProcessId = PageToProcess.ProcessId
		AND @EffectiveDate BETWEEN Process.EffectiveFromDateTime AND Process.EffectiveToDateTime
	INNER JOIN
		System.ProcessDetail
		ON ProcessDetail.ProcessId = Process.ProcessId
		AND @EffectiveDate BETWEEN ProcessDetail.EffectiveFromDateTime AND ProcessDetail.EffectiveToDateTime
		AND ProcessDetail.ProcessDetailDescription = @Process
	INNER JOIN
		System.ProcessAttribute
		ON ProcessAttribute.ProcessAttributeId = ProcessDetail.ProcessAttributeId
		AND @EffectiveDate BETWEEN ProcessAttribute.EffectiveFromDateTime AND ProcessAttribute.EffectiveToDateTime
		AND ProcessAttribute.ProcessAttributeDescription = 'ProcessName'
	INNER JOIN
		Mapping.APIToPageToProcess
		ON APIToPageToProcess.PageToProcessId = PageToProcess.PageToProcessId
		AND @EffectiveDate BETWEEN APIToPageToProcess.EffectiveFromDateTime AND APIToPageToProcess.EffectiveToDateTime
	INNER JOIN
		System.APIDetail
		ON APIDetail.APIId = APIToPageToProcess.APIId
		AND @EffectiveDate BETWEEN APIDetail.EffectiveFromDateTime AND APIDetail.EffectiveToDateTime
	INNER JOIN
		System.APIAttribute
		ON APIAttribute.APIAttributeId = APIDetail.APIAttributeId
		AND APIAttribute.APIAttributeDescription IN ('ApplicationURL')
		AND @EffectiveDate BETWEEN APIAttribute.EffectiveFromDateTime AND APIAttribute.EffectiveToDateTime
	WHERE
		@EffectiveDate BETWEEN Page.EffectiveFromDateTime AND Page.EffectiveToDateTime
END
GO
