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

    SELECT
		APIDetail.APIDetailDescription
	FROM
		System.Page
	INNER JOIN
		Mapping.PageToProcess
		ON PageToProcess.PageId = Page.PageId
	INNER JOIN
		System.Process
		ON Process.ProcessId = PageToProcess.ProcessId
	INNER JOIN
		Mapping.APIToPageToProcess
		ON APIToPageToProcess.PageToProcessId = PageToProcess.PageToProcessId
	INNER JOIN
		System.APIDetail
		ON APIDetail.APIId = APIToPageToProcess.APIId
	INNER JOIN
		System.APIAttribute
		ON APIAttribute.APIAttributeId = APIDetail.APIAttributeId
		AND APIAttribute.APIAttributeDescription IN ('ApplicationURL')
END
GO
