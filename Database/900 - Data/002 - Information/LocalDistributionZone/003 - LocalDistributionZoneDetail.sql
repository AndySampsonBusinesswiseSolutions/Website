USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @LocalDistributionZoneCodeAttributeId BIGINT = (SELECT LocalDistributionZoneAttributeId FROM [Information].[LocalDistributionZoneAttribute] WHERE LocalDistributionZoneAttributeDescription = 'Local Distribution Zone')
DECLARE @LocalDistributionZoneDescriptionAttributeId BIGINT = (SELECT LocalDistributionZoneAttributeId FROM [Information].[LocalDistributionZoneAttribute] WHERE LocalDistributionZoneAttributeDescription = 'Local Distribution Zone Description')

DECLARE @LocalDistributionZoneId BIGINT = (SELECT LocalDistributionZoneId FROM [Information].[LocalDistributionZone] WHERE LocalDistributionZoneGUID = 'A51AE01B-50F6-4FB7-AAAA-640F5872AFBB')
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneCodeAttributeId, 'SC'
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneDescriptionAttributeId, 'Scotland'

SET @LocalDistributionZoneId = (SELECT LocalDistributionZoneId FROM [Information].[LocalDistributionZone] WHERE LocalDistributionZoneGUID = '6FDEAAD6-4072-4D91-A352-1B1486C8E2AD')
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneCodeAttributeId, 'NO'
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneDescriptionAttributeId, 'Northern'

SET @LocalDistributionZoneId = (SELECT LocalDistributionZoneId FROM [Information].[LocalDistributionZone] WHERE LocalDistributionZoneGUID = '3E30A2FF-09CD-4876-B574-B2ACBF4E2F5D')
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneCodeAttributeId, 'NE'
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneDescriptionAttributeId, 'North East'

SET @LocalDistributionZoneId = (SELECT LocalDistributionZoneId FROM [Information].[LocalDistributionZone] WHERE LocalDistributionZoneGUID = 'D4F07EA6-8DA4-44D2-BFE4-CD4F5206618F')
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneCodeAttributeId, 'NW'
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneDescriptionAttributeId, 'North West'

SET @LocalDistributionZoneId = (SELECT LocalDistributionZoneId FROM [Information].[LocalDistributionZone] WHERE LocalDistributionZoneGUID = '3B40E8FC-EFD1-479E-9CEC-2957775E5F1C')
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneCodeAttributeId, 'WM'
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneDescriptionAttributeId, 'West Midlands'

SET @LocalDistributionZoneId = (SELECT LocalDistributionZoneId FROM [Information].[LocalDistributionZone] WHERE LocalDistributionZoneGUID = '536C2F28-788B-40D4-87C2-7309980FBFBC')
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneCodeAttributeId, 'EM'
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneDescriptionAttributeId, 'East Midlands'

SET @LocalDistributionZoneId = (SELECT LocalDistributionZoneId FROM [Information].[LocalDistributionZone] WHERE LocalDistributionZoneGUID = '137A6F80-273D-489C-B6AB-B86E4CF46FA0')
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneCodeAttributeId, 'EA'
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneDescriptionAttributeId, 'East Anglia'

SET @LocalDistributionZoneId = (SELECT LocalDistributionZoneId FROM [Information].[LocalDistributionZone] WHERE LocalDistributionZoneGUID = '9C7C45F6-2BFD-428D-8FED-FC42B227CCC5')
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneCodeAttributeId, 'NT'
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneDescriptionAttributeId, 'North Thames'

SET @LocalDistributionZoneId = (SELECT LocalDistributionZoneId FROM [Information].[LocalDistributionZone] WHERE LocalDistributionZoneGUID = '65A345EA-553A-49D3-B7E9-22BD3A1AB9D8')
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneCodeAttributeId, 'SO'
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneDescriptionAttributeId, 'Southern'

SET @LocalDistributionZoneId = (SELECT LocalDistributionZoneId FROM [Information].[LocalDistributionZone] WHERE LocalDistributionZoneGUID = 'CD2EFC89-F83C-470F-BA5F-86113D30767C')
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneCodeAttributeId, 'SE'
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneDescriptionAttributeId, 'South East'

SET @LocalDistributionZoneId = (SELECT LocalDistributionZoneId FROM [Information].[LocalDistributionZone] WHERE LocalDistributionZoneGUID = '8212A985-1635-459F-84BB-0103D956017E')
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneCodeAttributeId, 'WN'
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneDescriptionAttributeId, 'Wales North'

SET @LocalDistributionZoneId = (SELECT LocalDistributionZoneId FROM [Information].[LocalDistributionZone] WHERE LocalDistributionZoneGUID = '6C7E94B8-04B3-4211-AD35-8EB41CA82BA5')
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneCodeAttributeId, 'WS'
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneDescriptionAttributeId, 'Wales South'

SET @LocalDistributionZoneId = (SELECT LocalDistributionZoneId FROM [Information].[LocalDistributionZone] WHERE LocalDistributionZoneGUID = '3F3C8913-272B-449C-A194-5EA4D5AD2E0B')
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneCodeAttributeId, 'SW'
EXEC [Information].[LocalDistributionZoneDetail_Insert] @CreatedByUserId, @SourceId, @LocalDistributionZoneId, @LocalDistributionZoneDescriptionAttributeId, 'South West'
