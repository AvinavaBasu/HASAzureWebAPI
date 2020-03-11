Feature:HAS Feature-8:Test GetReportList
	Get all generated report list

@DEV_API
Scenario:HAS Feature-8:Test TestGetReportAPI-DEV
	Given I Have GetReportApiUrl https://raetgdpr-eventreporting-dev.azurewebsites.net/api/Report/Get/188a2e34-410b-41af-a501-8e99482a8e8e
	And I have Request type as Get
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as e5fbe68d-0122-48bc-bf0c-b8e4fa49b037
	And I have Authorization Header value from Authentication Token
	When I Post Request
	Then I will get OK response
	And Response should display Reports list and list should display created report name in it

@FAT_API
Scenario:HAS Feature-8:Test TestGetReportAPI-FAT
	Given I Have GetReportApiUrl https://raetgdpr-eventreporting-fat.azurewebsites.net/api/Report/Get/188a2e34-410b-41af-a501-8e99482a8e8e
	And I have Request type as Get
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as e5fbe68d-0122-48bc-bf0c-b8e4fa49b037
	And I have Authorization Header value from Authentication Token
	When I Post Request
	Then I will get OK response
	And Response should display Reports list and list should display created report name in it