Feature:HAS Feature-5:Test GenerateReportAPI
	Generate HAS-Report by sending Application, Permission Start & End Date details.

@DEV_API
Scenario: TestGenerateReportAPI-DEV
	Given I Have GenerateReportApiUrl https://raetgdpr-eventreporting-dev.azurewebsites.net/api/Report/Generate
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as e5fbe68d-0122-48bc-bf0c-b8e4fa49b037
	And I have Authorization Header value from Authentication Token
	And I have APIBody value for GenerateAPI
	#And I have APIBody value as { "application": "FlexBenefits", "permissions": [   "Employee","Manager","Beheer" ],  "startDate": "2019-05-01T00:00:00Z",  "endDate": "2019-11-08T05:49:52+00:00",  "fileName": "ScriptVijay_FexBenefitNotificationReport"}
	When I Post Request
	Then I will get OK response
	And I get FIleName and GUID response

@FAT_API
Scenario: TestGenerateReportAPI-FAT
	Given I Have GenerateReportApiUrl https://raetgdpr-eventreporting-fat.azurewebsites.net/api/Report/Generate
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as e5fbe68d-0122-48bc-bf0c-b8e4fa49b037
	And I have Authorization Header value from Authentication Token
	And I have APIBody value for GenerateAPI
	#And I have APIBody value as { "application": "FlexBenefits", "permissions": [   "Employee","Manager","Beheer" ],  "startDate": "2019-05-01T00:00:00Z",  "endDate": "2019-11-08T05:49:52+00:00",  "fileName": "ScriptVijay_FexBenefitNotificationReport"}
	When I Post Request
	Then I will get OK response
	And I get FIleName and GUID response
