USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @ApplicationId BIGINT = (SELECT ApplicationId FROM [System].[Application] WHERE ApplicationGUID = 'E78B2351-B1E6-464F-8D90-3793FACDABF4')
DECLARE @ApplicationAttributeId BIGINT = (SELECT ApplicationAttributeId FROM [System].[ApplicationAttribute] WHERE ApplicationAttributeDescription = 'Prerequisite Application GUID')

EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '535FDE7A-8720-4B72-BF68-DAC8FB95FBE9' --ValidateSiteDataApp
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '1DDDA8F8-F996-4B08-A28A-19F4FB0C922D' --ValidateMeterDataApp
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '35FA9EE3-77EC-4D1D-B622-443D28DA1608' --ValidateSubMeterDataApp
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '77B71231-880F-4470-83C9-0ED845BDDDCA' --ValidateMeterUsageDataApp
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '4E754A46-5F17-47BF-9D40-C7A95412EEFB' --ValidateSubMeterUsageDataApp
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '8253F798-A8F4-404D-B2BC-5DC87EFE839B' --ValidateCustomerDataApp
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '0BA4C8E7-3723-4106-A117-656F6871BC99' --ValidateMeterExemptionDataApp
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'B8F5A9D3-CD9F-44F2-B3EA-DCECA0F7CCFF' --ValidateFixedContractDataApp
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'BE59D94B-61BE-4900-B336-36AEB8904973' --ValidateFlexContractDataApp
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'D520F4DC-F582-45D7-A53C-608156991C6E' --ValidateFlexReferenceVolumeDataApp
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'AFFEE0C0-D660-4859-A7E9-93C3F2BC5492' --ValidateFlexTradeDataApp