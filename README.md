# IGME 690 Project
 Repo for the class project.

## Milestone 2 Goals
I want to move away from the idea of a turn-based game for this milestone and instead focus on projectile spawning, collision detection, and game logic. The game should play in quick rounds with the bullets shot being bounced off of the walls until a player is eliminated. I plan to continue  to utilize the HLAPI package components and scripts for this milestone.

Here are the things I want to get done:
Players able to shoot projectiles at any angle using the mouse (spawned from a pool of projectiles objects).
Collision detection for projectiles and players that resolve with player elimination on hit.
Collision detection between projectiles and walls that resolve with the projectiles bouncing off of them.
Scene reset (for all players) when one player remains alive.


## Milestone 3 Goals
Fix collision detection between players and bullets so that players (both host and client) turn red and lose health when they collide with a bullet.
Get bullets to reflect off of walls rather than phase through them.

To properly reach these goals, I will have to refactor the project to incorporate UNet's server and client functions. I have been relying on the NetworkIdentity and NetworkManager components to exchange data between the client and server, but these aren't enough to get the game functional.
