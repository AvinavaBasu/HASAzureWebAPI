Feature:HAS Feature-6:Test GetApplicationsList
	Get all application names present in the HAS-Application

@DEV_API
Scenario: TestGetApplicationAPI-DEV
	Given I Have GetApplicationApiUrl https://raetgdpr-eventreporting-dev.azurewebsites.net/api/ReportDetail/Application/188a2e34-410b-41af-a501-8e99482a8e8e
	And I have Request type as Get
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as e5fbe68d-0122-48bc-bf0c-b8e4fa49b037
	And I have Authorization Header value from Authentication Token
	When I Post Request
	Then I will get OK response
	And  API response should display the created application name

@FAT_API
Scenario: TestGetApplicationAPI-FAT
	Given I Have GetApplicationApiUrl https://raetgdpr-eventreporting-fat.azurewebsites.net/api/ReportDetail/Application/188a2e34-410b-41af-a501-8e99482a8e8e
	And I have Request type as Get
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as e5fbe68d-0122-48bc-bf0c-b8e4fa49b037
	And I have Authorization Header value from Authentication Token
	When I Post Request
	Then I will get OK response
	And  API response should display the created application name