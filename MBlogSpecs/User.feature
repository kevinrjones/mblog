Feature: Login
	In order to mange blogs
	As a user
	I want to be able to log in and log out

Scenario: Logged out
	Given I am logged out
	When I navigate to the home page
	Then the login button should be visible

Scenario: Logged In
	Given I am logged in
	When I navigate to the home page
	Then the logout button should be visible
