Feature:HAS Feature-1:Test AuthenticationAPI
	Generate authentication token.

@DEV_API
Scenario: TestAuthenticationAPI-DEV
	Given I have AuthenticationAPIURI https://api-test.raet.com/authentication
	And I have Request type as Post
	And I have Content-Type Header value as application/x-www-form-urlencoded
	And I have APIBody value as client_id=J4ZWfGbbeuqf49HQAEINMi8t8QizIMiK&client_secret=ui7BgSuLwAjuYtmc&grant_type=client_credentials
	When I Post Request
	Then I will get OK response
	And I get Acces token response

@FAT_API
Scenario: TestAuthenticationAPI-FAT
	Given I have AuthenticationAPIURI https://api-test.raet.com/authentication
	And I have Request type as Post
	And I have Content-Type Header value as application/x-www-form-urlencoded
	And I have APIBody value as client_id=J4ZWfGbbeuqf49HQAEINMi8t8QizIMiK&client_secret=ui7BgSuLwAjuYtmc&grant_type=client_credentials
	When I Post Request
	Then I will get OK response
	And I get Acces token response