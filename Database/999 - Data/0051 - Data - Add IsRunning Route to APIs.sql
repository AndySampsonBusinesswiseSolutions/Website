USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @APIId BIGINT = (SELECT APIId FROM [System].[API] WHERE APIGUID = '87AFEDA8-6A0F-4143-BF95-E08E78721CF5') --ValidateProcessGUID.api
DECLARE @APIAttributeId BIGINT = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'IsRunning Route')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'StoreUsageUpload/IsRunning'

SET @APIId = (SELECT APIId FROM [System].[API] WHERE APIGUID = '38D3A9E1-A060-4464-B971-8DC523B6A42D') --ArchiveProcessQueue.api
SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'IsRunning Route')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'ArchiveProcessQueue/IsRunning'

SET @APIId = (SELECT APIId FROM [System].[API] WHERE APIGUID = 'F916F19F-9408-4969-84DC-9905D2FEFB0B') --ValidatePageGUID.api
SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'IsRunning Route')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'ValidatePageGUID/IsRunning'

SET @APIId = (SELECT APIId FROM [System].[API] WHERE APIGUID = '56371F02-4120-41C9-82F9-4408309684D1') --CheckPrerequisiteAPI.api
SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'IsRunning Route')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'CheckPrerequisiteAPI/IsRunning'

SET @APIId = (SELECT APIId FROM [System].[API] WHERE APIGUID = '99681B37-575F-47E5-95E3-608063EA513E') --ValidateEmailAddress.api
SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'IsRunning Route')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'ValidateEmailAddress/IsRunning'

SET @APIId = (SELECT APIId FROM [System].[API] WHERE APIGUID = '26FEFFE8-49F7-4458-98ED-FD5F6C65C7C2') --ValidatePassword.api
SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'IsRunning Route')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'ValidatePassword/IsRunning'

SET @APIId = (SELECT APIId FROM [System].[API] WHERE APIGUID = 'CEC56745-C1C5-4E67-805B-159A8A5E991D') --ValidateEmailAddressPasswordMapping.api
SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'IsRunning Route')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'ValidateEmailAddressPasswordMapping/IsRunning'

SET @APIId = (SELECT APIId FROM [System].[API] WHERE APIGUID = '3BBFC2B6-2572-43CD-921A-A237000AC248') --StoreLoginAttempt.api
SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'IsRunning Route')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'StoreLoginAttempt/IsRunning'

SET @APIId = (SELECT APIId FROM [System].[API] WHERE APIGUID = '0C1BAFAA-586D-48BB-8D0B-B0B56BE0CCD2') --LockUser.api
SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'IsRunning Route')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'LockUser/IsRunning'

SET @APIId = (SELECT APIId FROM [System].[API] WHERE APIGUID = '94DD0DCB-DDC3-45A9-9C3D-D83922CF3110') --StoreUsageUpload.api
SET @APIAttributeId = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'IsRunning Route')
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'StoreUsageUpload/IsRunning'