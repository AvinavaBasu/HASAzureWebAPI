Feature:HAS Feature-7:Test GetPermissionDetails
	Get all permission details of application by providing application name

@DEV_API
Scenario: TestGetPermissionAPI-DEV
	Given I Have GetPermissionDetailsAPIUrl https://raetgdpr-eventreporting-dev.azurewebsites.net/api/ReportDetail/Permission/
	And I have Request type as Get
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as e5fbe68d-0122-48bc-bf0c-b8e4fa49b037
	And I have Authorization Header value from Authentication Token
	When I Post Request
	Then I will get OK response
	And API response should display the created permisson name

@FAT_API
Scenario: TestGetPermissionAPI-FAT
	Given I Have GetPermissionDetailsAPIUrl https://raetgdpr-eventreporting-fat.azurewebsites.net/api/ReportDetail/Permission/
	And I have Request type as Get
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as e5fbe68d-0122-48bc-bf0c-b8e4fa49b037
	And I have Authorization Header value from Authentication Token
	When I Post Request
	Then I will get OK response
	And API response should display the created permisson name
