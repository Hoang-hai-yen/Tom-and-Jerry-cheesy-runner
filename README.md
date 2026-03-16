# 🧀 Tom and Jerry: Chessy Runner

> A 3D Endless Runner game built with Unity — dodge obstacles, collect cheese, and survive Tom's relentless chase!

🎮 **[Play on itch.io](https://zussic.itch.io/tom-and-jerry-chessy-runner)**

![Screenshot](https://img.itch.zone/aW1hZ2UvNDEwOTE0OC8yNDYwMjY4NS5wbmc=/250x600/FQdjvQ.png)
![Screenshot](https://img.itch.zone/aW1hZ2UvNDEwOTE0OC8yNDYwMjY5Mi5wbmc=/250x600/T55cvO.png)
![Screenshot](https://img.itch.zone/aW1hZ2UvNDEwOTE0OC8yNDYwMjY5OS5wbmc=/250x600/TQ%2FAnU.png)

![Unity](https://img.shields.io/badge/Unity-000000?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Platform](https://img.shields.io/badge/Platform-HTML5%20%7C%20PC-blue?style=for-the-badge)

---

## 📖 Description

**Tom and Jerry: Chessy Runner** is a 3D endless runner inspired by the classic cartoon. The player controls Jerry in an infinite chase, dodging obstacles, collecting cheese, and surviving as long as possible while Tom actively hunts them down.

The project emphasizes **performance optimization** and **scalability**, leveraging **Object Pooling** and **Procedural Generation** to deliver a smooth infinite-running experience with minimal memory overhead.

---

## 🎮 Gameplay

- **Run forever** — the map generates infinitely using procedural chunk spawning
- **Dodge obstacles** — jump, slide, and switch lanes to avoid barriers
- **Collect cheese** — rack up your score with cheese pickups and multipliers
- **Use power-ups** — Magnet, Speed Boost, Shield, and Score Multiplier
- **Survive Tom** — Tom chases you; if he catches up, it's Game Over

---

## 🕹️ Controls

| Action | Key |
|--------|-----|
| Jump | `↑` Up Arrow |
| Slide | `↓` Down Arrow |
| Move Left | `←` Left Arrow |
| Move Right | `→` Right Arrow |

---

## ✨ Core Features

- **Procedural infinite map generation** using reusable map chunks
- **Object Pooling system** for obstacles, items, and map segments — minimal GC pressure
- **Gradually increasing speed** for dynamic difficulty scaling
- **4 Power-ups**: Magnet, Speed Boost, Shield, Score Multiplier
- **Enemy chase mechanic** — Tom follows the player with a delayed Game Over trigger
- **Cheese-based scoring** with temporary multipliers

---

## 🛠️ Core Scripts

| Script | Responsibility |
|--------|---------------|
| `PlayerMovement.cs` | Player movement, jumping, sliding, and buff handling |
| `TomFollower.cs` | Enemy AI and chase-based Game Over logic |
| `MapSpawner.cs` | Infinite map chunk spawning and recycling |
| `ItemPoolManager.cs` | Central object pooling manager |
| `ScoreManager.cs` | Score tracking and multiplier logic |
| `GameOverManager.cs` | Game state handling and final score display |

---

## ⚙️ Tech Stack

- **Engine**: Unity
- **Language**: C#
- **Rendering**: ShaderLab / HLSL (custom shaders)
- **Version Control**: Git

---

## 🚀 Getting Started

### Requirements
- Unity Editor (2021.3 LTS or compatible)
- Visual Studio or any C#-supported IDE

### Setup
```bash
git clone https://github.com/Hoang-hai-yen/Tom-and-Jerry-cheesy-runner.git
```
1. Open Unity Hub → **Open Project** → select the cloned folder
2. Ensure **TextMeshPro** is imported
3. Open the **Lobby** scene → Press **Play**

---

## 👥 Contributors

| Name | Student ID | GitHub |
|------|-----------|--------|
| Hoang Hai Yen *(Leader)* | 23521847 | [@Hoang-hai-yen](https://github.com/Hoang-hai-yen) |
| Dang Pham Nguyet Sang | 23521336 | [@Sanniverse](https://github.com/Sanniverse) |

**Supervisor:** Quan Chi Khanh An — anqck@uit.edu.vn

---

## 📄 License

This project is released under the **MIT License** — free to use, modify, and distribute for educational and non-commercial purposes.
