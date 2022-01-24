# Requirements Workup

## Elicitation

1. Our main priority is to connect the API or a Discord bot to our application so we can start gathering some information. This will give us an idea on the amount of information that we can gather and how we should be storing it. Along with storing the information, we will need to figure out how to sign a user in to our application with their Discord account.
2. We have not yet determined if a bot will be required to monitor the users activity or if using the Discord API will be enough to give us the information we need to make a functioning tool.
3. Our scope is rather clear, we have identified what we would like to display about games, but we have yet to decide if we would like to include user stats such as who chats the most, who spends the most time in voice channels, and who is active the most. 
4. We do not yet understand the full capabilities of the Discord API. We have learned that it is a very powerful tool, but the extent of information we can gather from it is still unknown. We are also unsure on what we can do to make this a profitable project. At the very least we would like to be able to pay for the hosting costs.


## Analysis

Go through all the information gathered during the previous round of elicitation.  

1. For each attribute, term, entity, relationship, activity ... precisely determine its bounds, limitations, types and constraints in both form and function.  Write them down.
2. Do they work together or are there some conflicting requirements, specifications or behaviors?
3. Have you discovered if something is missing?  
4. Return to Elicitation activities if unanswered questions remain.

1. 
   Server - unique ID, name, owner. Each server has multiple users, and multiple games   being played.

   User -unique ID, name, play time: amount of time spent playing games for given period, favorite game, active time: time that the user has their discord status set to online. Servers: servers that are in our system and the user is apart of.
   
   Game - unique ID, name, total time: total time that the game is played from all users, active time: clock time that the game is being played by users weekly, game image.
   
   User most active - unique ID, activity time
   
   Most active time - unique ID, start time, end time, user id
   
   Popular games - unique ID, game id.
   
   Popular time for games - unique ID, time, gameid
   
   Avg playtime per game - unique ID, avg times, gameid
   
   UserTimes - unique ID, start time, end time, userid
   
   GameTimes - unique ID, start time, end time, gameid
   


## Design and Modeling
Our first goal is to create a **data model** that will support the initial requirements.

1. Identify all entities;  for each entity, label its attributes; include concrete types
2. Identify relationships between entities.  Write them out in English descriptions.
3. Draw these entities and relationships in an _informal_ Entity-Relation Diagram.
4. If you have questions about something, return to elicitation and analysis before returning here.

## Analysis of the Design
The next step is to determine how well this design meets the requirements _and_ fits into the existing system.

1. Does it support all requirements/features/behaviors?
    * For each requirement, go through the steps to fulfill it.  Can it be done?  Correctly?  Easily?
2. Does it meet all non-functional requirements?
    * May need to look up specifications of systems, components, etc. to evaluate this.

