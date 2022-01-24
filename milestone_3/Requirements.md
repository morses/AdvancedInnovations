# Requirements Workup

## Elicitation

1. Our main priority is to connect the API or a Discord bot to our application so we can start gathering some information. This will give us an idea on the amount of information that we can gather and how we should be storing it. Along with storing the information, we will need to figure out how to sign a user in to our application with their Discord account.
2. We have not yet determined if a bot will be required to monitor the users activity or if using the Discord API will be enough to give us the information we need to make a functioning tool.
3. Our scope is rather clear, we have identified what we would like to display about games, but we have yet to decide if we would like to include user stats such as who chats the most, who spends the most time in voice channels, and who is active the most. 
4. We do not yet understand the full capabilities of the Discord API. We have learned that it is a very powerful tool, but the extent of information we can gather from it is still unknown. We are also unsure on what we can do to make this a profitable project. At the very least we would like to be able to pay for the hosting costs.


## Analysis

1. Server - unique ID, name, owner. Each server has multiple users, and multiple games being played. The server and its attributes are bounded by the amount of users that it has and what games are being played by those users.

User -unique ID, name, play time: amount of time spent playing games for given period, favorite game, active time: time that the user has their discord status set to online. Servers: servers that are in our system and the user is apart of. Users are bounded by the privacy setting that the users have set themselves.
   
Game - unique ID, name, total time: total time that the game is played from all users, active time: clock time that the game is being played by users weekly, game image. Games are bounded by if they have been approved by discord. Unapproved games will not show up in the user status unless specifically added by the user.
   
User most active - unique ID, activity time. User most active is bounded by the privacy setting that the users have set themselves.
   
Most active time - unique ID, start time, end time, user id. Most active time is bounded by the privacy setting that the users have set themselves.

Popular games - unique ID, game id. Popular games are bounded by if the game has been approved by discord. Unapproved games will not show up in the user status. 
   
Popular time for games - unique ID, time, gameid. Popular time for games is bounded by if the game has been approved by discord. Unapproved games will not show up in the user status.
   
Avg playtime per game - unique ID, avg times, gameid. Avg playtime per game is bounded by if the game has been approved by discord. Unapproved games will not show up in the user status. This will also depend on if the user has not set up privacy for their account.
   
UserTimes - unique ID, start time, end time, userid. UserTimes are bounded by the privacy setting that the users have set themselves.
   
GameTimes - unique ID, start time, end time, gameid. GameTimes are bounded by the privacy setting that the users have set themselves and also if the game has been registered on Discord.

2. For the most part there are no conflicts between our entites. We do gather the time in multiple different locations, which we believe may cause some confusion in the long run. Gathering and storing the game image is also something that is new to use. We have all stored text and numbers in a database before but we do not know how to store images.
3. If we need to use a bot to gather user information then we will need to create a data model to fit what the bot will do and what connections it will have to other parts of our program.  

   
## Design and Modeling
Our first goal is to create a **data model** that will support the initial requirements.

1. Server - unique ID, name, owner. Each server has multiple users, and multiple games   being played.

   User -unique ID, name, play time: amount of time spent playing games for given period, favorite game, active time: time that the user has their discord status set to online. Servers: servers that are in our system and the user is apart of.
   
   Game - unique ID, name, total time: total time that the game is played from all users, active time: clock time that the game is being played by users weekly, game image.
   
   User most active - unique ID, activity time
   
   Most active time - unique ID, start time, end time, user id
   
   Popular games - unique ID, game id.
   
   Popular time for games - unique ID, time, gameid
   
   Avg playtime per game - unique ID, avg times, gameid
   
   UserTimes - unique ID, start time, end time, userid
   
   GameTimes - unique ID, start time, end time, gameid

2. Many Servers have many Users.
  Many Servers have many Games.
   One User most active has many users.
   One UserTimes has many users.
   One Game to many GameTimes.
   One PopularGames has many Games.
   One Avg playtime per game has many games.
   One popular time for games has many games.

3. [Lucid Chart](https://lucid.app/lucidchart/5795abc9-3576-4228-8b93-a1e454c521e1/edit?invitationId=inv_e16bcd95-235e-48d1-a8a1-70e985f167c8)


## Analysis of the Design
The next step is to determine how well this design meets the requirements _and_ fits into the existing system.

1. 
Shows most played game - This will be gathered using the Popular Games feature in our diagram.
Shows most active time for each game - This will be gathered using the Popular time for games feature in our diagram.
Show average play time per game - This will be gathered using the Avg playtime per game feature in our diagram.
Shows most active time per user - We will find this with the User Most active feature in our diagram. 
Gives awards to users ex: Most active, Most game time, Most chats - We will gather this by looking through each user per server and using the most active algorithm we can determine these. 
Shows all registered servers so users can browse for new communities - This will be done by listing each Server in our database.
Allows admins to manage servers - Each server has an owner, these owners will be able to change certain server settings.
Recommends servers to users where the most played game on the server is the users top played game- Since each server contains a list of games and their total play time we will be able to sort the servers and each user has a favorite game. We will be able to Show the users the servers that have a high total play time for their favorite game. 

2. Since our application will heavily rely on third party components (Azure, MVC, Discord API, SQL Server, .Net, Node.Js) that have strict regulations and high standards. Most of the non-functional requirements are met.The structure of our database will allow for high security measures so we do not put our users at risk of a data breach. 


