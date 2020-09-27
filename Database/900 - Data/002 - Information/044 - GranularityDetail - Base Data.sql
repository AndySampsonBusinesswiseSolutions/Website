USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @GranularityCodeGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Granularity Code')
DECLARE @GranularityDescriptionGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Granularity Description')
DECLARE @GranularityDisplayDescriptionGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Granularity Display Description')
DECLARE @IsTimePeriodGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Is Time Period')
DECLARE @IsElectricityDefaultGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Is Electricity Default')
DECLARE @IsGasDefaultGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Is Gas Default')
DECLARE @ForecastAPIGUIDGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast API GUID')

--Five Minute
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '4D55BB09-9F8F-4AB6-917E-23B1D09E71AD')
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityCodeGranularityAttributeId, 'FiveMinute'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityDescriptionGranularityAttributeId, 'Five Minute'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityDisplayDescriptionGranularityAttributeId, 'Five Minutely'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @IsTimePeriodGranularityAttributeId, 'True'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastAPIGUIDGranularityAttributeId, '828CCEB3-8156-44E1-8DAB-5F987484795E'

--Half Hour
SET @GranularityId = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = 'CEA433FB-5327-4747-95CB-0FEFD1D2AD6B')
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityCodeGranularityAttributeId, 'HalfHour'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityDescriptionGranularityAttributeId, 'Half Hour'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityDisplayDescriptionGranularityAttributeId, 'Half Hourly'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @IsTimePeriodGranularityAttributeId, 'True'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @IsElectricityDefaultGranularityAttributeId, 'True'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastAPIGUIDGranularityAttributeId, '6C138EB1-6B8D-42D5-8B81-FCE8F536D09F'

--Date
SET @GranularityId = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '71C54EC0-6415-42D4-9C8C-6D8B8513F2FE')
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityCodeGranularityAttributeId, 'Date'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityDescriptionGranularityAttributeId, 'Day'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityDisplayDescriptionGranularityAttributeId, 'Daily'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @IsGasDefaultGranularityAttributeId, 'True'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastAPIGUIDGranularityAttributeId, 'DAD4DACC-FAC2-4B3D-8EFB-5823352ADF5D'

--Week
SET @GranularityId = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '8FD4C63A-84D5-4A03-B488-1A99C2331726')
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityCodeGranularityAttributeId, 'Week'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityDescriptionGranularityAttributeId, 'Week'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityDisplayDescriptionGranularityAttributeId, 'Weekly'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastAPIGUIDGranularityAttributeId, '3C15305A-4D4E-4288-8D90-E2382782FB4E'

--Month
SET @GranularityId = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '2AB4DC83-C0D0-4C5F-AC95-6A948802E430')
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityCodeGranularityAttributeId, 'Month'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityDescriptionGranularityAttributeId, 'Month'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityDisplayDescriptionGranularityAttributeId, 'Monthly'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastAPIGUIDGranularityAttributeId, 'B8344648-FD3C-4F4E-91CA-F27B397BBBDC'

--Quarter
SET @GranularityId = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '8029270C-1ECB-43B0-B313-9082890CDC8B')
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityCodeGranularityAttributeId, 'Quarter'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityDescriptionGranularityAttributeId, 'Quarter'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityDisplayDescriptionGranularityAttributeId, 'Quarterly'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastAPIGUIDGranularityAttributeId, 'F9E29252-F769-4FE8-BB10-5F3C406C98F7'

--Year
SET @GranularityId = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '3799717D-303B-458F-8A38-5DFA934ED431')
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityCodeGranularityAttributeId, 'Year'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityDescriptionGranularityAttributeId, 'Year'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @GranularityDisplayDescriptionGranularityAttributeId, 'Yearly'
EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastAPIGUIDGranularityAttributeId, ''