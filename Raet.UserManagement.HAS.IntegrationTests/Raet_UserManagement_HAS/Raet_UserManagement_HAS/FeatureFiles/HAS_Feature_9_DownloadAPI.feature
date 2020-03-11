Feature:HAS Feature-9:Test DownloadAPI
	Download HAS-Report by sending GUID value

@DEV_API1
Scenario: TestDownloadReportAPI-DEV
	Given I Have DownloadReportApiUrl https://raetgdpr-eventreporting-dev.azurewebsites.net/api/Report/Download?tenantId=188a2e34-410b-41af-a501-8e99482a8e8e&guid=
	And I have Request type as Get
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as e5fbe68d-0122-48bc-bf0c-b8e4fa49b037
	And I have Authorization Header value from Authentication Token
	When I Post Request
	Then I will get OK response
	Then I get Report details response

@FAT_API1
Scenario: TestDownloadReportAPI-FAT
	Given I Have DownloadReportApiUrl https://raetgdpr-eventreporting-fat.azurewebsites.net/api/Report/Download?tenantId=188a2e34-410b-41af-a501-8e99482a8e8e&guid=
	And I have Request type as Get
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as e5fbe68d-0122-48bc-bf0c-b8e4fa49b037
	And I have Authorization Header value from Authentication Token
	When I Post Request
	Then I will get OK response
	Then I get Report details response
