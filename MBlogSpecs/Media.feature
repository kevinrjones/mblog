Feature: Images
	In order to correctly view the blog
	As a user
	I want to be able to see images

@image
Scenario: View an image
	Given That I navigate to a blog post that contains an image
	When I view the blog post
	Then the image should be visible
