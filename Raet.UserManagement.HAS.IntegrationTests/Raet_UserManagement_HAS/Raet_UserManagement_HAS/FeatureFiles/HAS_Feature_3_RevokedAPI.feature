Feature:HAS Feature-3:Test RevokedAPI
	Create Revoked event request in EAEvents table & verify API response

@DEV_API
Scenario Outline: TestRevokeAPI-DEV
	Given I Have RevokeApiUrl https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/revoked
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	#And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as <URL>
	When I Post Request
	Then I will get Created response

	Examples: 
	| URL                                                                                                                                                                                                                                                                                                                                                                                                                                |
	| {  "UntilDateTime": "2020-02-10T00:00:00+00:00",  "effectiveAuthorization": {    "tenantId": "188a2e34-410b-41af-a501-8e99482a8e8e",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Create Employee",      "application": "Feb-28PipeLine",      "description": "Test Enrichment"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} |
	| {  "UntilDateTime": "2020-02-10T00:00:00+00:00",  "effectiveAuthorization": {    "tenantId": "188a2e34-410b-41af-a501-8e99482a8e8e",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Update Employee",      "application": "Feb-28PipeLine",      "description": "Test Enrichment"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} |
	| {  "UntilDateTime": "2020-02-10T00:00:00+00:00",  "effectiveAuthorization": {    "tenantId": "188a2e34-410b-41af-a501-8e99482a8e8e",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Delete Employee",      "application": "Feb-28PipeLine",      "description": "Test Enrichment"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} |

@FAT_API
Scenario Outline: TestRevokeAPI-FAT
	Given I Have RevokeApiUrl https://raetgdpr-fat.azurewebsites.net/api/EffectiveAuthorizationEvents/revoked
	And I have Request type as Post
	And I have Content-Type Header value as application/json
	#And I have x-raet-tenent-id Header value as user1@youforceonedevclient1.onmicrosoft.com
	And I have Authorization Header value from AuthenticationAPI-Response
	And I have APIBody value as <URL>
	When I Post Request
	Then I will get Created response

	Examples: 
	| URL                                                                                                                                                                                                                                                                                                                                                                                                                                |
	| {  "UntilDateTime": "2020-02-10T00:00:00+00:00",  "effectiveAuthorization": {    "tenantId": "188a2e34-410b-41af-a501-8e99482a8e8e",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Create Employee",      "application": "Feb-28PipeLine",      "description": "Test Enrichment"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} |
	| {  "UntilDateTime": "2020-02-10T00:00:00+00:00",  "effectiveAuthorization": {    "tenantId": "188a2e34-410b-41af-a501-8e99482a8e8e",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Update Employee",      "application": "Feb-28PipeLine",      "description": "Test Enrichment"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} |
	| {  "UntilDateTime": "2020-02-10T00:00:00+00:00",  "effectiveAuthorization": {    "tenantId": "188a2e34-410b-41af-a501-8e99482a8e8e",    "user": {      "context": "Youforce.Users",      "id": "RO276870"    },    "permission": {      "id": "Delete Employee",      "application": "Feb-28PipeLine",      "description": "Test Enrichment"    },    "target": {      "context": "Youforce.Users",      "id": "IC112070"    }  }} |
