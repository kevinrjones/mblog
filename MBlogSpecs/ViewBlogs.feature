Feature: View blogs
	In order read all blog posts
	As a user
	I want see all blog posts when I browse to the site's home page

Scenario: Show blog posts
	Given there are multiple blog posts
	When I navigate to the home page
	Then the posts should be listed
