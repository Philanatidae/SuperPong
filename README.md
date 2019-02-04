# Super Ping Pong

<https://pjrader1.itch.io/super-ping-pong>

This is the public repository for the Super Ping Pong game released on itch.io. Development has stopped, however I am releasing the code for whoever is curious about how the game was coded.

## Building
Super Ping Pong (Super Pong) requires [MonoGame](http://www.monogame.net) and VS2017 (or Visual Studio for Mac). After installing VS2017 and MonoGame, open `SuperPong/SuperPong.sln`. You can then build and run the game.

## Technical Notes
This was mostly a project for myself to experiment with an event based model and new shader effects.

Super Ping Pong was the basis for my open-source ECS (Entity-Component-System) framework [Ashley](https://github.com/pjrader1/Audrey).

The main game state (see `SuperPong.States.MainGameState`) uses an ECS with an event system. The ECS systems (`SuperPong.Systems`) emit events, and listeners (see `SuperPong.Directors`) take these events to tie all the systems together. This allows the systems to be abstracted from each other, while still being able to come together to create the game. The three main directors (`PongDirector`, `FluctuationDirector`, `AstheticsDirector`) contain the main logic of the game, each with a specific function (`PongDirector` manages the base pong game, `FluctuationDirector` manages the fluctuations which occur, and `AstheticsDirector` handles visual effects like ball explosions).

MonoGame (and XNA in general) is not known for having many available UI frameworks to use, so I created my own. This can be found in `SuperPong.UI`. UI frameworks are notoriously difficult to create, and looking back there is absolutely room for improvement. However, given that this is the first UI framework that I built, it did its job well.

Super Ping Pong takes advantage of an abstraction I've nicknamed "InputMethods". All input is abstracted into an `InputMethod`, which generates a constant `InputSnapshot` per tick (see `SuperPong.Input`). `InputSnapshot` PODs are abstracted from their originator. A keyboard and a controller will appear identical to the core game logic. Since the source of an `InputSnapshot` is trivial, this can be taken advantage of when creating non-human players. Computer controlled players observe the game world in the same way that systems do, and generate an `InputSnapshot` based on the current game state. This allows non-human controlled logic to be completely decoupled from the core gameplay logic.

## License

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
