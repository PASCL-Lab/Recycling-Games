# Recycling Games

> Two Unity-based educational mobile games that teach players about recycling and waste sorting.

## Overview

Recycling Games is a Unity project from PASCL Lab that bundles two complementary educational mobile games focused on recycling and waste management. **Recycle Dash** is an endless runner in which players collect trash while dodging obstacles and then sort what they have collected, while **Recycle Rush** is a standalone drag-and-drop sorting game where players place items into the correct recycling bins. The project is intended for educators, researchers, and players who want to combine engaging gameplay with environmental awareness, and it integrates with PlayFab for cloud-backed leaderboards.

## Features

- Two distinct game modes (Recycle Dash endless runner, Recycle Rush sorting puzzle)
- Drag-and-drop item-to-bin sorting mechanics teaching real recycling categories
- Character selection and multi-scene gameplay flow
- PlayFab integration for online high-score leaderboards
- Built for Android with PlayFab cloud sync
- Custom audio manager, UI toasts, and reusable prefabs

## Tech Stack

- Unity 2022.3.62f1 (LTS)
- C# (MonoBehaviour-based gameplay scripts)
- PlayFab SDK for authentication and leaderboards
- TextMesh Pro, Toast UI, Mesh Optimizer, and standard Unity asset packages
- Android build target

## Getting Started

### Prerequisites

- Unity Hub
- Unity Editor **2022.3.62f1** (other versions may work but are not guaranteed)
- Android Build Support module (for producing APKs)
- A PlayFab account with two titles (one for Recycle Dash, one for Recycle Rush) and a `High_Score` Legacy Leaderboard on each

### Installation

```bash
git clone https://github.com/PASCL-Lab/Recycling-Games.git
```

Then open the cloned `Recycling-Games` folder in Unity Hub using Unity 2022.3.62f1. Unity will import assets and resolve the packages listed in `Packages/manifest.json` automatically.

### Configuration

In Unity, open `Assets/PlayFabSDK/Shared/Public/Resources/PlayFabSharedSettings` and set the appropriate PlayFab Title ID before building each game (Dash Title ID for Recycle Dash, Rush Title ID for Recycle Rush). To rename the leaderboard, update the `StatisticName` field in `ScoreManager.cs`.

### Running / Usage

- **Recycle Dash:** open `Assets/GameData/Scenes/LoadingScreen.unity` and press Play, or build with the scene order: `LoadingScreen`, `CharacterSelect`, `MainGameplay`, `SortingPhase`.
- **Recycle Rush:** open `Assets/GameData/Scenes/Menu.unity` and press Play, or build with the scene order: `Menu`, `GarbageSorting`.

To produce an Android APK, switch the build target to Android in Build Settings, confirm the scene order above, and use **File -> Build Settings -> Build**.

## Project Structure

```
.
├── Assets/
│   ├── GameData/             # Scenes, UI, and game-specific assets for both games
│   ├── PlayFabSDK/           # PlayFab integration and shared settings
│   ├── Low Poly 3D Garbage & Tow Trucks/  # Vehicle and trash 3D assets
│   ├── Standard Assets/      # Unity standard assets
│   ├── TextMesh Pro/         # Text rendering
│   └── Toast UI/             # In-game toast notification system
├── Packages/                 # Unity Package Manager manifest and lock file
├── ProjectSettings/          # Unity project configuration (Unity 2022.3.62f1)
└── Report.pdf                # Project report
```

## License

This project is the intellectual property of **PASCL Lab**. All rights reserved.

Unauthorized copying, distribution, modification, or use of this codebase, in whole or in part, is strictly prohibited without prior written permission from PASCL Lab.

© 2026 PASCL Lab. All rights reserved.
