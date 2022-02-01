Project Inception Worksheet
=====================================

## Summary of Our Approach to Software Development
Here at Advanced Innovations we are using a Scrum team process and following and Agile approach. We will be splitting up the development of our project into several small sprints that each focus on a different part of the application. In doing so, we will be able to efficently tackle difficult tasks.


## Initial Vision Discussion with Stakeholders
Our shared vision is to create a Discord bot that can be added into servers by their owners. This bot will store user and server information to a database. Members of that server can then log into our website and see statistics such as the most popular time that games are played, what the most popular games are, what the most popular time to play a specific game is, the average play time for each game, who plays the most, and who is active the most. 

### Description of Clients/Users
[Personas](https://github.com/AbrahamVela/AdvancedInnovations/blob/dev/milestone_3/Personas.md)

    Our users are Discord server owners and the members of their servers.



### List of Stakeholders and their Positions (if applicable)
    Abraham Vela -Developer
    Ethan Sutton -Developer
    Korbin Cardoza -Developer
    Scot Morse -Supervisor
    Mack Doretta -Analyst
    Erica Keegan -Architect
    Easton Crist -Tester

## Initial Requirements Elaboration and Elicitation
[Requirements](https://github.com/AbrahamVela/AdvancedInnovations/blob/main/milestone_3/Requirements.md)

### Elicitation Questions
    1. 
    2.  
    3. ...

### Elicitation Interviews 
From out interview we learned that extracting the data from different Discord servers will be one of the hardest parts. It's crucial that we are able to gather the correct information about the games being played and who is playing them. Even though this seems like a small portion of the project compared to the graphing of time played and other visual aspects of our site, without implementing this feature we will be unable to continue with any other parts of our site.


## List of Needs and Features
[Needs and Features](https://github.com/AbrahamVela/AdvancedInnovations/blob/main/milestone_2/Initial%20Scope.md)

## Initial Modeling
[Mind Map](https://miro.com/app/board/uXjVOVoolCc=/)
### Use Case Diagrams
    Diagrams:
[Use Case Link](https://lucid.app/lucidchart/767d06b4-d277-4abd-bcdd-fa9da39c98db/edit?invitationId=inv_4713b97e-f17d-422e-9177-85d394e00b10)

[Use Case Svg](https://github.com/AbrahamVela/AdvancedInnovations/blob/dev/milestone_3/UseCase.svg)

### Sequence Diagrams

## Other Modeling
    Diagrams, UI wireframes, page flows, ...

[UI Modeling](https://github.com/AbrahamVela/AdvancedInnovations/blob/main/milestone_3/UI_Models/UIModeling.md)

[Data Modeling Link](https://lucid.app/lucidchart/1c741263-ac10-4d1f-961f-78cfc9b589e8/edit?invitationId=inv_ea20b1c5-9e1b-4d51-9b29-4496ffae4947)

[Data Modeling Svg](https://github.com/AbrahamVela/AdvancedInnovations/blob/dev/milestone_3/DataModeling.svg)

## Identify Non-Functional Requirements
    1. Security
    2. Compatibility
    3. Usability

## Identify Functional Requirements (In User Story Format)

E: Epic  
U: User Story  
T: Task  

A. [E] Create a bot that gathers user and game statisitics from added servers.

1. [U] As a user I would like to see the server I'm a part of user statisitics.

    a. [T] Instal proper packaging to access discord.

    b. [T] Retrieve a simple piece of data.

    c. [T] Retrieve a simple piece of user data.

    d. [T] Retrieve more user data.
2. [U] As a user I would like to see the server I'm a part of games statisitics.

    a. [T] Instal proper packaging to access discord.

    b. [T] Retrieve a simple piece of data.

    c. [T] Retrieve a simple piece of game data.

    d. [T] Retrieve more games data.

B. [E] Set up database using database model.

1. [U] Make sure the database(sql) is connected to the code(mvc) through entity framework.

    a. [T] Make proper scripts.

    b. [T] Have connection string set up.

    c. [T]Do a test run and display something simple.

C. [E] Make an algorithm that graphs user statistics.

1. [U] As a user I want to see a graph of user(s) statistics.

    a. [T] Import something like QuickGraph.

    b. [T] Write an algorithm that graphs one user statistic.

    c. [T] Apply the above algorithm to more users and stats.

D. [E] Make a page that shows all registered servers that are using the bot.

1. [U] As a user I want to see a page.

    a. [T] Create a page.

    b. [T] Have the page display something.

2. [U] As a user I want to see a page with registered servers using the bot.

    a. [T] Have the page show registered servers using the bot.

    b. [T] Make the data of registered server using the bot into a list.

E. [E] Make a page that shows active times for users.

1. [U] As a user I want to see a page.

    a. [T] Create a page.

    b. [T] Have the page display something.

2. [U] As a user I want to see a page with active times for users.

    a. [T] Have the page show active times for users

    b. [T] Make the data of active times for users into a graph.

F. [E] Make a page that shows active times for games being played.

1. [U] As a user I want to see a page.

    a. [T] Create a page

    b. [T] Have the page display something

2. [U] As a user I want to see a page with active times for games being played.

    a. [T] Have the page show active times for games being played.

    b. [T] Make the data of active times for games being played into a graph.

## Initial Architecture Envisioning
[Initial Architecture](https://github.com/AbrahamVela/AdvancedInnovations/blob/main/milestone_2/system_network%20architecture%20(Updated).svg)

## Agile Data Modeling
    Diagrams, SQL modeling (dbdiagram.io), UML diagrams

## Timeline and Release Plan
    Schedule: 
    Milestone 1: 1/11/2022
    Milestone 2: 1/18/2022
    Milestone 3: 1/25/2022   
    Milestone 4: 2/1/2022
    Sprint 1: 2/15/2022
    Sprint 2: 3/1/2022
    Sprint 3: 3/15/2022
    Sprint 4: 4/11/2022
    Sprint 5: 4/25/2022
    Sprint 6: 5/9/2022
    Sprint 7: 5/23/2022
    Sprint 8: 6/6/2022
