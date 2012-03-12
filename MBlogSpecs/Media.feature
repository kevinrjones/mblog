Feature: Images
	In order to correctly view the blog
	As a user
	I want to be able to see images

@image
@ignore
Scenario: View an image
	Given An image in the database
	When I view a correct image URL
	Then the result should be the image in the browser
