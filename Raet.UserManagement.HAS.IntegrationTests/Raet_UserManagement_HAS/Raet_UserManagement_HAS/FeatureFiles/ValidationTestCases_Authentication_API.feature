Feature: ValidationTestCases_Authentication_API
	Validation test cases

@DEV_API
Scenario: Post Authentication request by removing Client ID key
	Given I have AuthenticationAPIURI https://api-test.raet.com/authentication
	And I have Request type as Post
	And I have Content-Type Header value as application/x-www-form-urlencoded
	And I have APIBody value as client_secret=ui7BgSuLwAjuYtmc&grant_type=client_credentials
	When I Post Request
	Then I will get Unauthorized response

Scenario: Post Authentication request when Client ID key having null value
	Given I have AuthenticationAPIURI https://api-test.raet.com/authentication
	And I have Request type as Post
	And I have Content-Type Header value as application/x-www-form-urlencoded
	And I have APIBody value as client_id=&client_secret=ui7BgSuLwAjuYtmc&grant_type=client_credentials
	When I Post Request
	Then I will get Unauthorized response

Scenario: Post Authentication request by removing 'Client_secret' key 
	Given I have AuthenticationAPIURI https://api-test.raet.com/authentication
	And I have Request type as Post
	And I have Content-Type Header value as application/x-www-form-urlencoded
	And I have APIBody value as client_id=J4ZWfGbbeuqf49HQAEINMi8t8QizIMiK&grant_type=client_credentials
	When I Post Request
	Then I will get Unauthorized response

	Scenario: Post Authentication request when 'Client_secret' key having null value
	Given I have AuthenticationAPIURI https://api-test.raet.com/authentication
	And I have Request type as Post
	And I have Content-Type Header value as application/x-www-form-urlencoded
	And I have APIBody value as client_id=J4ZWfGbbeuqf49HQAEINMi8t8QizIMiK&client_secret=&grant_type=client_credentials
	When I Post Request
	Then I will get Unauthorized response

Scenario: Post Authentication request by removing 'grant_type' key 
	Given I have AuthenticationAPIURI https://api-test.raet.com/authentication
	And I have Request type as Post
	And I have Content-Type Header value as application/x-www-form-urlencoded
	And I have APIBody value as client_id=J4ZWfGbbeuqf49HQAEINMi8t8QizIMiK&client_secret=ui7BgSuLwAjuYtmc
	When I Post Request
	Then I will get BadRequest response

Scenario: Post Authentication request when ''grant_type' key having null value
	Given I have AuthenticationAPIURI https://api-test.raet.com/authentication
	And I have Request type as Post
	And I have Content-Type Header value as application/x-www-form-urlencoded
	And I have APIBody value as client_id=J4ZWfGbbeuqf49HQAEINMi8t8QizIMiK&client_secret=ui7BgSuLwAjuYtmc&grant_type=
	When I Post Request
	Then I will get BadRequest response