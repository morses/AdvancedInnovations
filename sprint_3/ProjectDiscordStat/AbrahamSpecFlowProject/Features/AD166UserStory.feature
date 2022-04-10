Feature: AD166UserStory


@mytag
Scenario: See webhooks 
	Given I am on the ServerChannels page
	When I click on a channel
	Then I'll see a list of webhooks

Scenario: Begining of sending a message
	Given I am on the ChannelWebhooks page
	When I click on a webhook
	Then I'll see a send message form

Scenario: Begining of making a webhook 
	Given I am on the ServerChannels page
	When I click on a channel
	And there are no webhooks
	Then I'll see an option to create webhook

Scenario: Second step of creating a webhook
	Given I am on the ChannelWebhooks page
	When I click on Create Webhook
	Then I'll see make webhook form