Feature: ValidationTestCases_Granted_API
	Validation test cases

Background:Get Authorization token using AuthenticationAPI
    Given I have AuthenticationAPIURI https://api-test.raet.com/authentication
	And I have Request type as Post
	And I have Content-Type Header value as application/x-www-form-urlencoded
	And I have APIBody value as client_id=J4ZWfGbbeuqf49HQAEINMi8t8QizIMiK&client_secret=ui7BgSuLwAjuYtmc&grant_type=client_credentials
	When I Post Request
	Then I will get OK response
	And I get Acces token response

@DEV_API
  Scenario: Post Granted Event Request by Remove 'fromDateTIme' block in API body value
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	#And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as {   "effectiveAuthorization": {    "tenantId": "HAS2",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Veiw Employee",      "application": "HAS-Reporting",      "description": "Test"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }}
	When I Post Request
	Then I will get BadRequest response
	And API response should display error message as The FromDateTime field is required.

Scenario: Post Granted Event Request when 'fromDateTime' value is null in API body value
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as {  "fromDateTime": "",  "effectiveAuthorization": {    "tenantId": "HAS2",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Veiw Employee",      "application": "HAS-Reporting",      "description": "Test"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }}
	When I Post Request
	Then I will get BadRequest response
	And API response should display error message as The FromDateTime field is required.

Scenario Outline: Post Granted Event Request sending invalid dates to 'fromDateTIme' input in API body value
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as <Input>
	When I Post Request
	Then I will get BadRequest response
	And API response should display error message as Could not convert string to DateTime
	
	Examples: 
	| Input                                                                                                                                                                                                                                                                                                                                                          |
	| {  "fromDateTime": "2019-13-25T05:17:27+00:00",  "effectiveAuthorization": {    "tenantId": "HAS2",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Veiw Employee",      "application": "HAS-Reporting",      "description": "Test"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} |
	| {  "fromDateTime": "2019-09-31T05:17:27+00:00",  "effectiveAuthorization": {    "tenantId": "HAS2",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Veiw Employee",      "application": "HAS-Reporting",      "description": "Test"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} |
	| {  "fromDateTime": "2019-02-30T05:17:27+00:00",  "effectiveAuthorization": {    "tenantId": "HAS2",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Veiw Employee",      "application": "HAS-Reporting",      "description": "Test"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} |

Scenario: Post Granted Event Request by removing 'TenantID' block in API body value
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as {  "fromDateTime": "2019-10-25T05:17:27+00:00",  "effectiveAuthorization": {        "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Veiw Employee",      "application": "HAS-Reporting",      "description": "Test"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }}
	When I Post Request
	Then I will get BadRequest response
	And API response should display error message as The TenantId field is required.

Scenario: Post Granted Event Request when 'TenantID' value is null in API body value
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as {  "fromDateTime": "2019-10-25T05:17:27+00:00",  "effectiveAuthorization": {    "tenantId": "",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Veiw Employee",      "application": "HAS-Reporting",      "description": "Test"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }}
	When I Post Request
	Then I will get BadRequest response
	And API response should display error message as The TenantId field is required.

Scenario: Post Granted Event Request by removing 'User' block in API body value
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as {  "fromDateTime": "2019-10-25T05:17:27+00:00",  "effectiveAuthorization": {    "tenantId": "HAS2",       "permission": {      "id": "Veiw Employee",      "application": "HAS-Reporting",      "description": "Test"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }}
	When I Post Request
	Then I will get BadRequest response
	And API response should display error message as The User field is required.

Scenario: Post Granted Event Request when 'User' blocks 'context' input contains null value in API body value
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as {  "fromDateTime": "2019-10-25T05:17:27+00:00",  "effectiveAuthorization": {    "tenantId": "HAS2",    "user": {      "context": "",      "id": "RO276870"    },    "permission": {      "id": "Veiw Employee",      "application": "HAS-Reporting",      "description": "Test"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} 
	When I Post Request
	Then I will get BadRequest response
	And API response should display error message as The Context field is required.

Scenario: Post Granted Event Request when 'User' blocks 'id' input contains null value in API body value
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as {  "fromDateTime": "2019-10-25T05:17:27+00:00",  "effectiveAuthorization": {    "tenantId": "HAS2",    "user": {      "context": "Youforce.Users",      "id": ""    },    "permission": {      "id": "Veiw Employee",      "application": "HAS-Reporting",      "description": "Test"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} 
	When I Post Request
	Then I will get BadRequest response
	And API response should display error message as The Id field is required.

Scenario: Post Granted Event Request by removing 'permission' block in API body value
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as {  "fromDateTime": "2019-10-25T05:17:27+00:00",  "effectiveAuthorization": {    "tenantId": "HAS2",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },     "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} 
	When I Post Request
	Then I will get BadRequest response
	And API response should display error message as The Permission field is required.

Scenario: Post Granted Event Request when 'permission' blocks 'id' input contains null value in API body value
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as {  "fromDateTime": "2019-10-25T05:17:27+00:00",  "effectiveAuthorization": {    "tenantId": "HAS2",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "",      "application": "HAS-Reporting",      "description": "Test"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} 
	When I Post Request
	Then I will get BadRequest response
	And API response should display error message as The Id field is required.

Scenario: Post Granted Event Request when 'permission' blocks 'application' input contains null value in API body value
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as {  "fromDateTime": "2019-10-25T05:17:27+00:00",  "effectiveAuthorization": {    "tenantId": "HAS2",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Veiw Employee",      "application": "",      "description": "Test"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} 
	When I Post Request
	Then I will get BadRequest response
	And API response should display error message as The Application field is required.

Scenario: Post Granted Event Request when 'permission' blocks 'description' input contains null value in API body value
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as {  "fromDateTime": "2019-10-25T05:17:27+00:00",  "effectiveAuthorization": {    "tenantId": "HAS2",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Veiw Employee",      "application": "HAS-Reporting",      "description": ""    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} 
	When I Post Request
	Then I will get Created response
	And API response should display Created Event details

Scenario: Post Granted Event Request  by removing 'target' block in API body value
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as {  "fromDateTime": "2019-10-25T05:17:27+00:00",  "effectiveAuthorization": {    "tenantId": "HAS2",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Veiw Employee",      "application": "HAS-Reporting",      "description": "Test"    }      }} 
	When I Post Request
	Then I will get Created response
	And API response should display Created Event details

Scenario: Post Granted Event Request when 'target' blocks 'context' input contains null value in API body value
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as {  "fromDateTime": "2019-10-25T05:17:27+00:00",  "effectiveAuthorization": {    "tenantId": "HAS2",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Veiw Employee",      "application": "HAS-Reporting",      "description": "Test"    },    "target": {      "context": "",      "id": "IC112070"    }  }} 
	When I Post Request
	Then I will get BadRequest response
	And API response should display error message as The Context field is required.

Scenario: Post Granted Event Request when 'target' blocks 'id' input contains null value in API body value
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as {  "fromDateTime": "2019-10-25T05:17:27+00:00",  "effectiveAuthorization": {    "tenantId": "HAS2",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Veiw Employee",      "application": "HAS-Reporting",      "description": "Test"    },    "target": {      "context": "Youforce.Users",      "id": ""    }  }} 
	When I Post Request
	Then I will get BadRequest response
	And API response should display error message as The Id field is required.