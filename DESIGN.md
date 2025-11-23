# Happy New Fear - Design Document

## Architecture Overview

### High-Level System Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                        Player Systems                        │
├─────────────────────────────────────────────────────────────┤
│  PlayerMove (Input/Physics) → SprintDetector → ViewBobbing  │
│  Inventory System → Item Management → Equipment             │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                    Environment Interaction                   │
├─────────────────────────────────────────────────────────────┤
│  RaycastChecker → Interactable Objects → DoorManager        │
│  FlashLight System → Battery Management                     │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                         NPC Systems                          │
├─────────────────────────────────────────────────────────────┤
│  NPC (AI/FOV) → Pathfinding → Animation                     │
│  NpcInteract (Conversation) → Dialogue Tree                 │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                    Quest & Narrative Loop                    │
├─────────────────────────────────────────────────────────────┤
│  QuestManager (State) → MissionExecuter (Events)            │
│  Conversation Triggers → Environmental Changes              │
│  Sanity System → Player Psychology                          │
└─────────────────────────────────────────────────────────────┘
```

---

## Core Systems

### 1. Player Movement System

**Key Classes**: 
- `PlayerMove.cs` - Core movement and physics
- `SprintDetector.cs` - Sprint state management
- `ViewBobbing.cs` - Head bob animation

**Workflow**:
```
Input (WASD) 
  → PlayerMove processes movement
  → CharacterController applies physics
  → Ground detection raycast
  → Footstep audio trigger (SprintDetector)
  → View bobbing animation (ViewBobbing)
```

**Key Features**:
- Character Controller-based movement
- Configurable walk/sprint/crouch speeds
- Ceiling detection to prevent head clipping
- Ground layer raycast detection
- Step sound management with audio mixer

---

### 2. Inventory & Item System

**Key Classes**:
- `Inventory.cs` - Main inventory manager
- `IntObject.cs` - Individual item properties
- `RaycastChecker.cs` - Object detection

**Item Lifecycle**:
```
World Item (Active)
  ↓
[Raycast Detection] (RaycastChecker)
  ↓
[Highlighted with Outline]
  ↓
[Player Interaction (E key)]
  ↓
Inventory Slot (Added to List)
  ↓
[UI Icon Spawned] (LeanTween animation)
  ↓
[Item in Hand] (3D Model positioned)
  ↓
[Use or Drop]
  ↓
World Item or Destroyed
```

**Inventory Properties per Item**:
- Position in hand (3D space)
- Icon for UI display
- Usability flags (pickupable, placeable, droppable)
- Mission dependency
- Grounding state (is it on ground?)

---

### 3. Door & Interaction System

**Key Classes**:
- `DoorManager.cs` - Door logic and animation
- `DoorMissionChecker.cs` - Mission-based door locks
- `TentDoor.cs` - Special tent door behavior

**Door State Machine**:
```
┌──────────┐
│  CLOSED  │←──────────────┐
└────┬─────┘              │
     │                    │
  [Interact]      [Animation End]
     │                    │
     ↓                    │
┌──────────┐              │
│ OPENING  │─────────────→│
└──────────┘
     ↓
┌──────────┐
│   OPEN   │
└────┬─────┘
     │
  [Interact]
     ↓
┌──────────┐
│ CLOSING  │──────────────┐
└──────────┘              │
     ↑                    │
     │                    │
     └────[Animation End]─┘
```

**Door Types**:
1. **Rotation-Based**: Doors that swing open/closed
   - Configurable axes (X, Y, Z rotation)
   - Target rotation angles
   - Parent-dependent or independent

2. **Linear-Based**: Drawers and sliding objects
   - Position-based movement
   - Configurable axes (X, Y, Z movement)
   - Target position for open state

---

### 4. NPC & AI System

**Key Classes**:
- `NPC.cs` - Core AI and behavior
- `NpcInteract.cs` - Conversation triggers
- `PlagueAI_Behavoiur.cs` - Antagonist (Max Dad) behavior

**NPC State Machine**:
```
┌─────────────┐
│   PASSIVE   │
└──────┬──────┘
       │
   [FOV Check]
       │
       ↓
┌─────────────┐     [Player Lost]
│   AWARE     │────────────→ [Wander]
└──────┬──────┘
       │
    [Chase]
       │
       ↓
┌─────────────┐
│   CHASING   │
└──────┬──────┘
       │
    [Reached]
       │
       ↓
┌─────────────┐
│  ATTACKING  │
└─────────────┘
```

**AI Features**:
- **Field of View (FOV)**: Radius-based detection with angle constraints
- **Pathfinding**: NavMesh Agent for realistic navigation
- **Obstacle Detection**: Line-of-sight checks for line-of-sight verification
- **Wandering**: Random target selection within wander radius
- **Animation**: Mecanim animator integration

**Key AI Parameters**:
```csharp
detectionRadius: 10f
detectionAngle: 45f
playerLossDelay: 5f
wanderRadius: 20f
```

---

### 5. Conversation System

**Key Classes**:
- `Conversation.cs` - Dialogue manager
- `NpcInteract.cs` - Conversation triggers

**Conversation Flow**:
```
[NPC Interaction Triggered]
  ↓
[currentCoversation Index Set]
  ↓
[PlayerMove.canMove = false] (Lock player)
  ↓
[Conversation Audio Plays] (with text display)
  ↓
[CurrentAudio Counter Increments]
  ↓
[All dialogue finished?]
  ├─ No: Wait for next audio
  │
  └─ Yes: hasFinishedConv = true
            ↓
            [Trigger Mission Events]
            ↓
            [PlayerMove.canMove = true]
```

**Conversation Types**:
1. **Phone Conversation** (`isNonPhoneConv = false`)
   - Uses PhoneManager UI
   - Phone interface displayed

2. **Direct Conversation** (`isNonPhoneConv = true`)
   - NPC dialogue
   - Optional multiple speakers
   - Audio containers for character voices

---

### 6. Quest & Mission System

**Key Classes**:
- `QuestManager.cs` - Global quest state (Singleton)
- `MissionExecuter.cs` - Mission-specific events
- `SleepMission.cs` - Sleep objective implementation
- `SupermarketMission.cs` - Supermarket objective

**Quest Progression Flow**:
```
┌─────────────────────────────┐
│  Game Start (Mission 0)     │
│  "Wake Up"                  │
└────────────┬────────────────┘
             ↓
    [Check Mission Conditions]
             ↓
    [Mission Counter Advances]
             ↓
┌─────────────────────────────┐
│   Mission N                 │
│   (Quest Objective Text)    │
└────────────┬────────────────┘
             ↓
    [Completion Trigger]
    (dialogue, location, action)
             ↓
    [currentMission++]
             ↓
    [Next mission begins]
```

**Mission Data**:
```csharp
string[] Missions = {
    "Wake Up",
    "Go to Supermarket",
    "Find Items",
    "Return Home",
    // ... etc
}
```

---

### 7. Sanity System

**Key Classes**:
- `SanitySystem.cs` - Sanity mechanics
- `SanityEffect1.cs` - Visual/audio effects

**Sanity Mechanics**:
```
Current Sanity: 0-100
     │
     ├─ Stage 0: [0-25]    → Mild sounds
     ├─ Stage 1: [25-50]   → Moderate sounds
     ├─ Stage 2: [50-75]   → Severe sounds
     └─ Stage 3: [75-100]  → None/Clear

Sanity Change Events:
  • Scary encounter: -10 sanity/sec
  • Phone music: +5 sanity/sec
  • Time passage: -1 sanity/5sec
  • Safe location: +2 sanity/sec
```

**Sanity Effects**:
- **Audio**: Stage-appropriate ambient horror sounds
- **Visual**: Bar color gradient transitions
- **Gameplay**: May affect camera shake or movement speed (configurable)

---

### 8. Flashlight & Battery System

**Key Classes**:
- `FlashLightManager.cs` - Toggle and state
- `FlashBattery.cs` - Battery drain and intensity

**Battery Drain Model**:
```
baseDrainRate: 1% per second
intensityLevel: 1-5
drainPenalty: (intensityLevel - 1) * batteryDrainPenalty

totalDrain = baseDrainRate + drainPenalty

When battery < lowBatteryThreshold:
  • Audio flicker sounds
  • Visual flicker effects
  • Light intensity fluctuates
```

**Battery States**:
```
100% ─────────────────→ 0%
 │
 ├─ Normal (green bar)
 ├─ Low (yellow bar) ──→ Flicker
 └─ Dead (red bar) ────→ Off
```

---

### 9. Vehicle System

**Key Classes**:
- `CarSpawner.cs` - Vehicle spawning manager
- `CarControl.cs` - Vehicle movement
- `CarRide.cs` - Vehicle interaction sequences

**Vehicle Spawn Logic**:
```
[Spawn Timer Ticking]
  ↓
[Timer Reaches 0]
  ↓
[Select Random Car Type]
  ↓
[Check if Spawn Location Available]
  ├─ Yes: Instantiate Car
  │       └─ Auto-destroy after 20 seconds
  │
  └─ No: Skip spawn
```

---

### 10. Environmental System

**Key Classes**:
- `TvManager.cs` - TV video playback
- `EatPizza.cs` - Eating mechanics
- `knifeCut.cs` - Cutting mechanics
- `FurnaceManager.cs` - Furnace controls
- `FlickerLight.cs` - Dynamic light flickering

**Environmental Interactions**:
- TV system plays randomized video content
- Pizza consumption triggers animations
- Knife usage requires specific inventory item
- Furnace has on/off states
- Lights can flicker based on electrical state

---

## Data Flow Examples

### Example 1: Simple Interaction Flow

```
Player Raycast
  ├─ Hit Object with IntObject component
  ├─ RaycastChecker enables Outline
  ├─ Display raycast text "Press E to interact"
  │
[Player Presses E]
  │
  ├─ Inventory.ChangeInventory() called
  ├─ Item added to InventoryItems list
  ├─ Inventory icon spawned with LeanTween animation
  ├─ Item positions in hand (mainpos)
  │
Item Now Active in Player Hands
  ├─ Display in 3D space
  ├─ Can be used (knife cut, etc.)
  ├─ Can be dropped back to world
```

### Example 2: Quest Progress Flow

```
Conversation Triggered
  ├─ NpcInteract.conversationIsGoing = true
  ├─ PlayerMove.canMove = false
  ├─ Conversation.ConversationOn = true
  │
[Audio Plays]
  ├─ Text displayed via DialogueText
  ├─ CurrentAudio counter increments
  │
[Conversation Ends]
  ├─ hasFinishedConv = true
  ├─ MissionExecuter checks completion
  ├─ QuestManager.currentMission++
  ├─ New objective displayed
  ├─ PlayerMove.canMove = true
  │
Next Mission Phase Begins
```

### Example 3: NPC Detection Flow

```
NPC Initialization
  ├─ FOVRoutine started (coroutine)
  ├─ Setup: detectionRadius = 10, detectionAngle = 45
  │
[Frame Update]
  ├─ Physics.SphereCast for nearby objects
  ├─ Filter by targetMask layer
  ├─ Check if PlayerIsInDetectionRange
  │
[Player In Range]
  ├─ Direction to player calculated
  ├─ Angle check: dot product > cos(angle)
  ├─ Line-of-sight raycast
  │
[Player Detected]
  ├─ canSeeTarget = true
  ├─ NavMeshAgent destination = player position
  ├─ Trigger appropriate animation
  │
[Player Lost]
  ├─ playerLossDelayTimer starts
  ├─ After delay: lostPlayer = true
  ├─ Return to wandering
```

---

## Event Triggers & Callbacks

### Mission-Critical Events

| Event | Trigger | Result |
|-------|---------|--------|
| `OnConversationEnd` | `hasFinishedConv = true` | Mission check, progression |
| `OnDoorOpened` | Door.Interacte() called | Audio play, mission check |
| `OnInventoryChange` | Item added/removed | UI update, mission check |
| `OnNPCDetected` | FOV check passes | AI state change, audio cue |
| `OnSanityThreshold` | Sanity hits stage threshold | Audio trigger, effects |
| `OnBatteryEmpty` | Battery = 0% | Flashlight disabled |

---

## Performance Considerations

### Expensive Operations
- **Physics.SphereCast**: FOV detection (per NPC, every update)
- **NavMesh pathfinding**: Updated when player moves
- **Raycast**: Every frame for player interaction detection
- **Coroutines**: FOV routine, conversation audio timing

### Optimization Opportunities (Not Implemented in Old Code)
- FOV check frequency reduction (every N frames instead of every frame)
- Object pooling for frequently spawned items
- NavMesh agent update frequency reduction
- Raycast result caching with time-based invalidation
- LOD system for distant NPCs

---

## Known Issues & Technical Debt

2. **Magic Numbers**: Hard-coded values throughout scripts
3. **Tight Coupling**: Heavy reliance on GameObject.FindGameObjectWithTag()
4. **No Error Handling**: Missing null checks and error states
5. **Coroutine Management**: No centralized coroutine lifecycle
6. **Architecture**: Systems could use event-based decoupling

---

## Future Improvements

1. Implement event system (Observer pattern)
2. Use dependency injection for system initialization
3. Configuration files for all magic numbers
4. Comprehensive logging/debugging system
5. Performance profiling and optimization
6. Unit test coverage
7. Proper asset naming conventions
8. Documentation generation from code comments

