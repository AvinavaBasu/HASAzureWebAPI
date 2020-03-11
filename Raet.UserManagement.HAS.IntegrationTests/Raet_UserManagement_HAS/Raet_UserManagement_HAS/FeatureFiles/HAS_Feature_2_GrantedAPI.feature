Feature:HAS Feature-2:Test GrantedAPI
	Create Granted evetn request in EAEvents table & verify API response

@DEV_API
Scenario Outline: TestGrantedAPI-DEV
	Given I Have GrantedApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	#And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as <URL>
	When I Post Request
	Then I will get Created response
	And I get GUID, Permission and Application details from response body

	Examples: 
	| URL                                                                                                                                                                                                                                                                                                                                                                                                                               |
	| {  "fromDateTime": "2019-02-15T00:00:00+00:00",  "effectiveAuthorization": {    "tenantId": "188a2e34-410b-41af-a501-8e99482a8e8e",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Create Employee12",      "application": "Feb-27NewFramework",      "description": "Test Enrichment"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} |
	| {  "fromDateTime": "2019-02-15T00:00:00+00:00",  "effectiveAuthorization": {    "tenantId": "188a2e34-410b-41af-a501-8e99482a8e8e",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Update Employee12",      "application": "Feb-27NewFramework",      "description": "Test Enrichment"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} |
	| {  "fromDateTime": "2019-02-15T00:00:00+00:00",  "effectiveAuthorization": {    "tenantId": "188a2e34-410b-41af-a501-8e99482a8e8e",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Delete Employee12",      "application": "Feb-27NewFramework",      "description": "Test Enrichment"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} |

@FAT_API
Scenario Outline: TestGrantedAPI-FAT
	Given I Have GrantedApiUrl https://raetgdpr-fat.azurewebsites.net/api/EffectiveAuthorizationEvents/granted
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	#And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as <URL>
	When I Post Request
	Then I will get Created response
	And I get GUID, Permission and Application details from response body

	Examples: 
	| URL                                                                                                                                                                                                                                                                                                                                                                                                                               |
	| {  "fromDateTime": "2019-02-15T00:00:00+00:00",  "effectiveAuthorization": {    "tenantId": "188a2e34-410b-41af-a501-8e99482a8e8e",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Create Employee12",      "application": "Feb-27NewFramework",      "description": "Test Enrichment"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} |
	| {  "fromDateTime": "2019-02-15T00:00:00+00:00",  "effectiveAuthorization": {    "tenantId": "188a2e34-410b-41af-a501-8e99482a8e8e",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Update Employee12",      "application": "Feb-27NewFramework",      "description": "Test Enrichment"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} |
	| {  "fromDateTime": "2019-02-15T00:00:00+00:00",  "effectiveAuthorization": {    "tenantId": "188a2e34-410b-41af-a501-8e99482a8e8e",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Delete Employee12",      "application": "Feb-27NewFramework",      "description": "Test Enrichment"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} |
