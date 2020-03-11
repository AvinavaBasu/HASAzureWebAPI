Feature: HAS_UI_OpenHASReportingApplication
	In order to avoid silly mistakes
	

@UI
Scenario: Open Visma|Raet login page and verify 1
	Given I Open the Application in browser
	Then I should get Domain Selector page
	And Page should display username text field
	And Page should display Contiune button 

@UI
Scenario: Login and Verify HAS-Reporing Home page 1
	Given I'am Domain Selector page
	When I Enter user1@youforceonetestclient1.onmicrosoft.com value into Username field
	And Click on Continue button
	And I Enter Email ID as user1@youforceonetestclient1.onmicrosoft.com 
	And Clicks on Next button
	And I Enter Password as Youforce4 
	And Clicks on Sign in button
	And I click No button in Stay signed in? page
	Then I should get Raet HAS page
	And I close Browser