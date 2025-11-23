# Happy New Fear - 3D Horror Narrative Game

## Overview

**Happy New Fear** is a 3D first-person horror narrative game built with Unity. Players navigate through a suspenseful environment filled with environmental storytelling, NPC interactions, and psychological gameplay mechanics. The game combines exploration, puzzle-solving, and resource management within a horror narrative framework.

### Important Note
This is an **old project** and does **not** reflect my current coding abilities or standards. The codebase demonstrates early learning phases with inconsistent naming conventions, architectural patterns, and optimization practices. It serves as a portfolio piece to show growth and learning progression.

---

## Core Game Mechanics

### 1. **Player Movement & Interaction System**
- **First-Person Camera**: Full 3D camera control with mouse look
- **Movement**: WASD keys for navigation with customizable walk/sprint speeds
- **Crouch System**: Stealth movement with adjustable camera height
- **Step Sounds**: Dynamic footstep audio (different sounds for surfaces)
- **Raycast Interaction**: Outlining system for interactive objects and NPCs
- **Physics**: Gravity, ground detection, and ceiling collision checks

### 2. **Inventory System**
- **Item Pickup**: Proximity-based collection within a configurable radius
- **Inventory Management**: 
  - Slot-based inventory with mouse wheel switching
  - Visual icon previews with smooth transitions (LeanTween animations)
  - Item placement in 3D space (held in hands)
  - Item dropping mechanics
- **Interactive Objects**: Interactable items with grab/place mechanics
- **Grounding Detection**: Objects react to being placed/dropped on surfaces

### 3. **Door & Interactable System**
- **Multiple Door Types**:
  - Standard doors (rotation-based opening/closing)
  - Drawer-like objects (linear movement opening)
- **Door Properties**:
  - Configurable open direction (forward/backward)
  - Locking mechanism
  - Smooth lerp animations
  - Audio feedback (door open/close sounds)
- **Outline Feedback**: Visual highlighting for interactive objects using Quick Outline plugin

### 4. **NPC & Conversation System**
- **NPC AI**:
  - NavMesh agent-based pathfinding
  - Field of view detection (configurable radius and angle)
  - Obstacle detection with line-of-sight checks
  - Wandering behavior with random position selection
  - Sitting/standing animations
  - Following mechanics
- **Conversation System**:
  - Branching dialogue with multiple conversation states
  - Phone-based and direct NPC conversations
  - Audio narration with text display
  - Mission progression tied to dialogue completion
  - Character position tracking for story progression
  - Multiple speakers with individual audio sources

### 5. **Mission/Quest System**
- **Quest Tracking**:
  - Linear mission progression (0-based indexing)
  - Mission-specific objectives displayed to player
  - Objectives update based on completion triggers
- **Mission Events**:
  - Door unlock triggers based on mission progress
  - NPC behavior changes tied to mission states
  - Environmental changes and spawning based on quest stage
- **Message System**: Pop-up notifications for objectives and progress

### 6. **Sanity System**
- **Sanity Mechanics**:
  - Sanity bar that increases/decreases based on gameplay state
  - Configurable rates for increase and decrease
  - Multiple sanity stages with escalating effects
- **Audio Feedback**: Stage-appropriate ambient horror sounds
  - Mild stage sounds
  - Moderate stage sounds
  - Severe stage sounds
- **Visual Effects**: Sanity bar color-gradient feedback
- **Phone Interaction**: Music playback can temporarily restore sanity

### 7. **Flashlight & Battery System**
- **Flashlight Mechanics**:
  - Toggle on/off with 'F' key
  - Volumetric light beam (VLB) integration
- **Battery Management**:
  - Realistic battery drain system
  - Intensity levels with drain penalties
  - Low battery warning with audio cues
  - Battery flickering effects when depleted
  - Visual UI indicator with fill bar
- **Audio Effects**: Sound effects for activation, intensity changes, and flickering

### 8. **Vehicles & Transportation**
- **Vehicle Spawning**:
  - Random car spawning at configurable intervals
  - Spawning restrictions (can disable specific spawn points)
  - Automatic cleanup (destruction after time)
- **Vehicle Control**: Basic movement along X-axis
- **Car Rides**: Interactive cutscene-like sequences with NPCs

### 9. **Environmental Interactions**
- **Object Destruction**: Market objects can be destroyed
- **Furniture Interactions**: Couches, doors, furnaces with specific behaviors
- **Special Objects**:
  - Pizza eating mechanics with animation triggers
  - TV system with video playback
  - Furnace management
  - Knife cutting mechanics
- **Sleeping Mechanics**: Designated sleeping areas with mission progression

### 10. **Audio System**
- **Positional Audio**: 3D audio sources for spatial awareness
- **Audio Containers**: Organized audio source management for NPCs
- **Sound Effects**: 
  - Player footsteps
  - Inventory interactions
  - Door mechanisms
  - Flashlight activation
  - Environmental ambience
- **Music System**: Phone-based music player for mood management

### 11. **Visual Effects**
- **Outline Rendering**: Quick Outline plugin for interactive object highlighting
- **Volumetric Lighting**: Volumetric Light Beam (VLB) for dynamic lighting effects
- **Camera Effects**:
  - View bobbing while moving
  - Screen flashes for sanity events
- **Particle Effects**: VLB dust particles for atmospheric lighting
- **Retro VHS/TV Effects**: Post-processing effects for horror atmosphere

### 12. **Level Management**
- **Scene Transitions**: Fade in/out with customizable speed
- **Level Loader**: Handles scene switching with transition effects
- **Persistent Data**: QuestManager singleton for quest state persistence

---

## Project Structure

```
Assets/
├── Scripts/
│   ├── Player/
│   │   ├── PlayerMove.cs           - Player movement and controls
│   │   ├── PlayerController.cs     - Player input handling
│   │   ├── ViewBobbing.cs          - Camera bobbing effect
│   │   └── SprintDetector.cs       - Sprint state detection
│   │
│   ├── Inventory/
│   │   ├── Inventory.cs            - Main inventory manager
│   │   ├── IntObject.cs            - Interactive object properties
│   │   └── RaycastChecker.cs       - Raycast detection for objects
│   │
│   ├── Doors/
│   │   ├── DoorManager.cs          - Door open/close logic
│   │   ├── DoorMissionChecker.cs   - Mission-dependent doors
│   │   └── TentDoor.cs             - Special tent door behavior
│   │
│   ├── NPCs/
│   │   ├── NPC.cs                  - NPC AI and behaviors
│   │   ├── NpcInteract.cs          - NPC conversation triggers
│   │   ├── DaveControl.cs          - Dave NPC specific logic
│   │   ├── MaxDad.cs               - Max Dad (antagonist) behavior
│   │   └── TriggerJeff.cs          - Jeff spawning mechanics
│   │
│   ├── Conversations/
│   │   └── Conversation.cs         - Dialogue system
│   │
│   ├── Quests/
│   │   ├── QuestManager.cs         - Quest progression
│   │   ├── MissionExecuter.cs      - Mission-specific events
│   │   ├── SleepMission.cs         - Sleep objective handler
│   │   └── SupermarketMission.cs   - Supermarket objective
│   │
│   ├── Sanity/
│   │   ├── SanitySystem.cs         - Sanity mechanics
│   │   └── SanityEffect1.cs        - Sanity visual effects
│   │
│   ├── Flashlight/
│   │   ├── FlashLightManager.cs    - Flashlight control
│   │   └── FlashBattery.cs         - Battery drain and management
│   │
│   ├── Vehicles/
│   │   ├── CarSpawner.cs           - Car spawning system
│   │   ├── CarControl.cs           - Car movement
│   │   └── CarRide.cs              - Car ride sequences
│   │
│   ├── Environment/
│   │   ├── TvManager.cs            - TV system and video playback
│   │   ├── EatPizza.cs             - Pizza eating mechanics
│   │   ├── knifeCut.cs             - Knife cutting mechanics
│   │   ├── FurnaceManager.cs       - Furnace interactions
│   │   ├── Couch.cs                - Couch behaviors
│   │   └── FlickerLight.cs         - Dynamic lighting flicker
│   │
│   ├── Managers/
│   │   ├── LevelLoader.cs          - Scene transitions
│   │   ├── PhoneManager.cs         - Phone UI and functions
│   │   ├── AudioSourceContainer.cs - Audio management
│   │   ├── SoundManager.cs         - Sound effect management
│   │   └── ObjectManager.cs        - Object lifecycle management
│   │
│   └── Utilities/
│       ├── callingChecker.cs       - Calling validation
│       ├── RaycastChecker.cs       - Raycast utilities
│       └── TreeFixNav.cs           - NavMesh utilities
│
├── Animations/
│   ├── CameraMoveSlow.anim        - Camera animation
│   └── Fan Spin.anim              - Fan rotation animation
│
├── Animators/
│   ├── Cube_235.controller        - NPC animator controller
│   └── Draenus.controller         - Character animator controller
│
├── Audio/
│   └── Sounds/                     - All audio files
│
├── Materials/
│   └── TextureMaterial/            - Material assets
│
├── Models/
│   └── (3D models)
│
├── Prefabs/
│   ├── Player.prefab              - Player character prefab
│   └── Tree.prefab                - Environmental prefab
│
├── Scenes/
│   └── (Game scenes)
│
├── UI/
└── TextMesh Pro/              - UI text mesh resources


```

---

## Technical Details

### Dependencies & Assets
- **Unity Version**: Compatible with modern Unity versions (2020+)
- **Input System**: Legacy input system (WASD, mouse, function keys)
- **UI Framework**: TextMesh Pro for text rendering
- **Animation**: Mecanim animator system
- **AI Navigation**: NavMesh agents for NPC pathfinding
- **Physics**: Built-in Rigidbody and Character Controller
- **Audio**: Legacy AudioSource system

### Key Design Patterns (Legacy)
- **Singleton Pattern**: QuestManager for global quest state
- **Component-Based Design**: MonoBehaviour components for modular systems
- **Coroutine-Based Events**: Timing and delayed actions
- **Tag-Based References**: Finding game objects by string tags
- **Serialized Fields**: Inspector-based configuration

---

## Gameplay Features

### Story & Narrative
- Linear narrative progression through quest stages
- Environmental storytelling through level design
- NPC interactions that advance the story
- Phone-based narrative elements

### Mechanics Depth
- Resource management (battery for flashlight)
- Psychological pressure (sanity system)
- Environmental awareness (FOV detection by NPCs)
- Strategic movement (crouch for stealth)

### Player Challenges
- Inventory management with limited slots
- Time pressure through sanity mechanics
- Navigation with limited light sources
- NPCs that respond to player proximity and actions

---

## Code Quality Notes

### Areas for Improvement (Old Code)
- **Naming Conventions**: Some variables use unclear or inconsistent names
- **Code Organization**: Scripts could benefit from better separation of concerns
- **Magic Numbers**: Hard-coded values scattered throughout
- **Documentation**: Limited inline comments and method documentation
- **Performance**: Not optimized for large-scale deployment
- **Architecture**: Tightly coupled systems could be more modular

### What Would Be Done Differently Today
- Clear separation between gameplay logic and UI
- Dependency injection for system management
- Event system for loose coupling
- Asset serialization for configuration
- Comprehensive unit testing
- Better script organization with namespaces
- Performance profiling and optimization passes

---

## Controls

| Input | Action |
|-------|--------|
| **W/A/S/D** | Move |
| **Mouse** | Look around |
| **Shift** | Sprint |
| **Ctrl** | Crouch |
| **Mouse Wheel** | Switch inventory items |
| **E** | Interact with objects/doors |
| **F** | Toggle flashlight |
| **Tab** | Special action |
| **LMB** | Use held item (knife, etc.) |

---

## License

License information kept private. Contact for licensing inquiries.

---

## Credits

**Developer**: Vdark
**Engine**: Unity  

---

## Lessons Learned

This project demonstrates:
- Game architecture with multiple interconnected systems
- Complex state management (quests, sanity, inventory)
- NPC AI and pathfinding integration
- Audio-visual synchronization
- UI system implementation
- Physics-based interactions

It also serves as a cautionary example of technical debt—showing the importance of clean architecture and consistent naming conventions from the start.

---

**Status**: Archived Portfolio Project  
**Year**: ~2023-2024 (Approximate)
