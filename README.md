# 🧶 SCP-066 Reforged

![Version](https://img.shields.io/badge/Version-0.8.9-blue.svg)
![Framework](https://img.shields.io/badge/Framework-EXILED-red.svg)
![Platform](https://img.shields.io/badge/Platform-SCP:%20SL-lightgrey.svg)

**SCP-066 Reforged** is an advanced Custom Role for SCP: Secret Laboratory. It focuses on high immersion, custom proximity audio, and a professional spawn-balancing system.

---

## 🚀 Key Features

* **Smart SCP Replacement**: SCP-066 has a configurable **10% chance** to replace a regular SCP at the start of the round, maintaining team balance.
* **Immersive Audio Engine**: Powered by `AudioPlayerApi`, providing 2D-global proximity audio for all sounds.
* **Invisible Viewmodel Strategy**: Aggressively hides Zombie hands and body while keeping access to the **SCP Team Chat**.
* **Symphony Attack**: A powerful AoE attack using Beethoven's Symphony that deals damage and applies effects to nearby humans.
* **Lore-Accurate Scaling**: Automatically scales the player to **0.35x**, ensuring you stay small and stealthy.

---

## 📥 Installation & Requirements

### 🛠️ Requirements
* **EXILED Framework** (v9.13.1+).
* **AudioPlayerApi** (In dependencies).
* **MapEditorReborn** (For schematic support).
* **NVorbis.dll** (Essential for .ogg decoding).

### 📂 Setup Guide
1. Place `Scp066.dll` in your `EXILED/Plugins` folder.
2. Place `AudioPlayerApi.dll` and `NVorbis.dll` in `EXILED/Plugins/dependencies`.
3. Upload audio files (`Eric1-3.ogg`, `Notes1-6.ogg`, `Beethoven.ogg`) to:
   `EXILED/Configs/Plugins/scp066/Audio/`.
4. Ensure your yarn schematic is in `MapEditorReborn/Schematics`.

---

## ⚙ Configuration

Full control via the standard EXILED config file:

yaml
scp066:
  is_enabled: true
  spawn_chance: 10 # Chance to replace an SCP
  schematic_name: "Scp066" # Must match MER folder name
  volume: 0.8 # Audio volume (0.0 - 1.0)
  attack_duration: 24 # Duration of Beethoven attack
  attack_radius: 8 # Radius for damage/sound

## 🎮 Player Controls
  
  Key,Command,Action
  C,.scp066 eric,"Play random ""Eric!"" shout"
  V,.scp066 music,Play random musical note
  X,.scp066 boom,Activate Symphony Attack

## 👨‍💻 Admin Commands
scp066 give <ID/Name> — Grant the role to a player.
scp066 remove <ID/Name> — Remove the role from a player.

## 💡 Note on Implementation
This plugin uses 2D (Non-Spatial) Audio for the user. This ensures the player always hears their own music while it propagates correctly to others via the Speaker system.
