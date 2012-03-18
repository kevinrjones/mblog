Feature: Images
	In order to correctly view the blog
	As a user
	I want to be able to see images

@image
Scenario: View an image
	Given A user
	When they view a blog post with an image
	Then the image should be visible
