Feature: As a user I want to be able to see all the games that have been played on the server I am a part of, so that I can find new games my friends are playing and see if anyone is playing my favorite game.


Scenario: Save cookies for a given user
	Given I am a user with a Discord account
	When I login
	Then I can save all cookies

Scenario: User can see the games played on a server
	Given I am in a server
	When I click View Games
	Then I will see the games played

Scenario: User can select a game
	Given I am a user
	  And I am on the 'Games' page
	When I click on a game image
	Then I will be redirected to another page

Scenario: Load cookies for a given user
	Given I am a user with a Discord account
	And I am on the 'Account' page
	When I load a previous sessions cookies
		And I am on the 'Account' page
	Then I can see all the servers I am in