# Jerry's Infinite Run: Endless Runner Project

## Contributors

**Leader:**

* Hoang Hai Yen – 23521847 – https://github.com/Hoang-hai-yen

**Members:**

* Dang Pham Nguyet Sang – 23521336 – https://github.com/Sanniverse

**Supervisors:**

* Quan Chi Khanh An – anqck@uit.edu.vn

---

## Description

**Jerry's Infinite Run** is a 3D **Endless Runner** game developed using **Unity Engine**. The player controls Jerry in an endless chase, dodging obstacles, collecting cheese, and surviving as long as possible while being pursued by Tom.

The project focuses heavily on **performance optimization** and **scalability**, utilizing **Object Pooling** and **Procedural Generation** to create an infinite running environment with minimal memory overhead. As the game progresses, the difficulty increases through higher running speed and more complex obstacle patterns.

Core gameplay elements include lane-based movement, jumping, sliding, collectible-based scoring, temporary power-ups, and a unique **chase-based Game Over mechanic** where the enemy actively captures the player instead of an instant failure.

---

## How to Use

### Requirements

* Unity Editor (compatible version)
* Visual Studio or any C#-supported IDE

### Setup Instructions

1. **Clone the repository:**

   ```bash
   git clone git@github.com:Hoang-hai-yen/Tom-and-Jerry-cheesy-runner.git
   ```

2. **Open the project:**

   * Launch Unity Hub
   * Select **Open Project** and choose the cloned folder

3. **Verify dependencies:**

   * Ensure **TextMeshPro** is imported and properly configured

4. **Run the game:**

   * Open the **Lobby** scene
   * Press **Play** in the Unity Editor

### Controls (PC)

| Action     | Key         |
| ---------- | ----------- |
| Jump       | Up Arrow    |
| Slide      | Down Arrow  |
| Move Left  | Left Arrow  |
| Move Right | Right Arrow |

---

## Additional Information

### Core Features

* Procedural infinite map generation using reusable map chunks
* Centralized object pooling system for obstacles, items, and patterns
* Gradually increasing player speed for difficulty scaling
* Multiple power-ups: Magnet, Speed Boost, Shield, and Score Multiplier
* Cheese-based scoring system with temporary multipliers
* Enemy chase system triggering delayed Game Over

### Core Scripts

| Script               | Responsibility                                       |
| -------------------- | ---------------------------------------------------- |
| `PlayerMovement.cs`  | Player movement, jumping, sliding, and buff handling |
| `TomFollower.cs`     | Enemy AI and chase-based Game Over logic             |
| `MapSpawner.cs`      | Infinite map chunk spawning and recycling            |
| `ItemPoolManager.cs` | Central object pooling manager                       |
| `ScoreManager.cs`    | Score tracking and multiplier logic                  |
| `GameOverManager.cs` | Game state handling and final score display          |

---

## Code of Conduct

This project follows standard academic and open-source integrity principles:

* No plagiarism of third-party code without attribution
* Respectful collaboration among contributors
* Clear documentation of external assets and libraries
* Proper use of version control and commit history

---

## License

This project is released under the **MIT License**, allowing free use, modification, and distribution for educational and non-commercial purposes unless stated otherwise.

---

*Developed with Unity & C#* 🎮
