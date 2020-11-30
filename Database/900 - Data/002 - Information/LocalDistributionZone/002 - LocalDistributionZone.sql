USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Information].[LocalDistributionZone_Insert] @CreatedByUserId, @SourceId, 'A51AE01B-50F6-4FB7-AAAA-640F5872AFBB' --SC
EXEC [Information].[LocalDistributionZone_Insert] @CreatedByUserId, @SourceId, '6FDEAAD6-4072-4D91-A352-1B1486C8E2AD' --NO
EXEC [Information].[LocalDistributionZone_Insert] @CreatedByUserId, @SourceId, '3E30A2FF-09CD-4876-B574-B2ACBF4E2F5D' --NE
EXEC [Information].[LocalDistributionZone_Insert] @CreatedByUserId, @SourceId, 'D4F07EA6-8DA4-44D2-BFE4-CD4F5206618F' --NW
EXEC [Information].[LocalDistributionZone_Insert] @CreatedByUserId, @SourceId, '3B40E8FC-EFD1-479E-9CEC-2957775E5F1C' --WM
EXEC [Information].[LocalDistributionZone_Insert] @CreatedByUserId, @SourceId, '536C2F28-788B-40D4-87C2-7309980FBFBC' --EM
EXEC [Information].[LocalDistributionZone_Insert] @CreatedByUserId, @SourceId, '137A6F80-273D-489C-B6AB-B86E4CF46FA0' --EA
EXEC [Information].[LocalDistributionZone_Insert] @CreatedByUserId, @SourceId, '9C7C45F6-2BFD-428D-8FED-FC42B227CCC5' --NT
EXEC [Information].[LocalDistributionZone_Insert] @CreatedByUserId, @SourceId, '65A345EA-553A-49D3-B7E9-22BD3A1AB9D8' --SO
EXEC [Information].[LocalDistributionZone_Insert] @CreatedByUserId, @SourceId, 'CD2EFC89-F83C-470F-BA5F-86113D30767C' --SE
EXEC [Information].[LocalDistributionZone_Insert] @CreatedByUserId, @SourceId, '8212A985-1635-459F-84BB-0103D956017E' --WN
EXEC [Information].[LocalDistributionZone_Insert] @CreatedByUserId, @SourceId, '6C7E94B8-04B3-4211-AD35-8EB41CA82BA5' --WS
EXEC [Information].[LocalDistributionZone_Insert] @CreatedByUserId, @SourceId, '3F3C8913-272B-449C-A194-5EA4D5AD2E0B' --SW