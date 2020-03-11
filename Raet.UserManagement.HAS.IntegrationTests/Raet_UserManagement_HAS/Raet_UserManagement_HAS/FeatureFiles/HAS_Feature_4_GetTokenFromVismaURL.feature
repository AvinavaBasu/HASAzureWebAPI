Feature:HAS Feature-4:Test Authorization token
	Get Authorization token from Visam URL

@DEV_API
Scenario: Login to VISMA account and copy token from URL-DEV
	Given I open the Application
	When Enter the username
	And Click on Continue Button
	And I Enter EmailID
	And Click on Next Button
	And I Enter Password
	And Clicks on Sign in Button
	And I click on No Button
	Then I copy Authorization token from URL
	And I close the browser

@FAT_API
Scenario: Login to VISMA account and copy token from URL-FAT
	Given I open the Application
	When Enter the username
	And Click on Continue Button
	And I Enter EmailID
	And Click on Next Button
	And I Enter Password
	And Clicks on Sign in Button
	And I click on No Button
	Then I copy Authorization token from URL
	And I close the browser