# Gold-Team
Group Name: Gold Team

Project Name: Rolled War 2


Members:

Joshua Emerling joshuaem

Matthew Holder  mjholder

Ryan Sandidge rdsandid

Project for CSE 442

Gitter: [![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/Gold_Team/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

### Project Description:

  This project will be a first person shooter where the players and enemy's will be in wheel chairs. This will effect how the environment is laid out, how they interact with the environment and how they use their weapons. Players will only be able to use a pistol when moving and bigger guns when they are not since these will require both hands to be free. Players will be able to acquire new guns through various means. The project will consist of 2 game modes Deathmatch where the players are placed into teams and Free For All where it is every man for himself. It will include both single and multilayer. It will be done using Unity and C# as well as Blender for graphics.

### MVP:
Our MVP will include the following:

 - A Lobbying System that allows players to choose a game or be randomly placed into one. 
 -  A UI that players can view there current inventory(in game) and choose game modes.
 - An AI that will be behave like other players.
 - An interactive environment with ramps, power ups(boosts), elevators and traps.
 - Two different Game Modes: Team Death Match and FFA.
 - Realistic wheel chair mechanics (ie. Players will not be able to use stairs).
 - Players will only be able to use a pistol when moving and will need to be stopped to use better guns.
 - A variety of guns with different benefits.
 - An inventory of weapons obtained throughout the game.
 - Finite Hit Points that can be replenished by picking up certain consumables found throughout the game.

 

### Add-on Features:

  1. Leveling/Experience System to allow players to upgrade weapons and their wheel chairs
  
  2. Classes/Loadout System to allow players to save acquired weapons.

  3. Multiple and dynamically generated maps.
  
### User Story:
  One very underrepresented group of people in First Person Shooters is the handicap community. We need a game that incorporates FPS features while showing what it is like for a person in a wheel chair. This means that the character would be unable to do things that a person with the ability to walk could. For example someone in a wheel chair would be unable to use stairs, forcing them to find ramps and/or elevators to get to the desired location. The game should still be entertaining and provide a twist to your typical FPS.


### Team Roles:
See wiki Page 2.


  


### Alpha
YouTube video: https://youtu.be/LonUqTFIchg

Release v0.2.0

How to Run:
  The Alpha Release now has support for Windows, Linux32 and Linux64. In the Gold Team folder extract the respective folder and execute the file listed below. 

**IMPORTANT(Windows only) Do not run the exe titled Rolled War II v0.1.0.exe located in the Rolled War II folder as this is the prototype. Unless you wanted to run the protype for some reason then go for it.**

    Windows: Folder: Windows.zip  Executable: RWII.exe
    Linux64: Folder: Linux 64.zip Executable: RWII.x86_64
    Linux32: Folder: Linux 32.zip Executable: RWII.x86
    
### Help Wanted
  There are 3 issuses with a help wanted tag they are as follows:
  
 **Muzzle Flashes #38** 
 Description:
 This task will involve using a particle system to create muzzle flashes when a player shoots.
To do this you will need to add a empty game object with a particle system component to each gun prefab. It is very important that this object is NOT the first child of the gun or it will break functionality.
To edit the prefab first drag it into the scene create the changes and hit apply in the inspector window.

The shoot function is called in the weapon mechanics script. It is defined in inventory system. Upon shooting the script should retrieve the particle system component from the child and play it for a short time. This should not be done when the weapon id==5 as this is the grenade.

It is also possible just to create the object and not add it to the prefab either way it will work

**Contact username: joshuaemer**



 
  

  

