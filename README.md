SCP-066 Reforged (EXILED Plugin)
An advanced SCP-066 Custom Role for SCP: Secret Laboratory built with the EXILED framework. This version is "Reforged" to provide high immersion, custom audio abilities, and a unique spawn system.

🚀 Key Features
Dynamic Role Replacement: Instead of spawning from Class-D, SCP-066 has a configurable chance (default 10%) to replace a random SCP at the start of the round.

Immersive Custom Model: Automatically spawns a MapEditorReborn schematic on the player while aggressively hiding the base Zombie model and hands.

Proximity Audio System: Integrated with AudioPlayerApi to play high-quality .ogg files directly from the player's position.

Zero-Damage Melee: Prevents the base Zombie role from dealing damage, forcing players to rely on their musical abilities.

Symphony of Destruction: A radius-based attack that deals damage over time and applies the Concussed effect using Beethoven's Symphony.

🛠 Implementation Details
1. The "Invisible Zombie" Solution
To allow SCP-066 to communicate with other SCPs using the SCP Chat (V), we used the RoleTypeId.Scp0492 (Zombie) as a base. To hide the ugly zombie hands, the plugin uses a HideBodyLoop coroutine that repeatedly disables the player's SkinnedMeshRenderer.

2. Strategic Spawning
The plugin monitors the ChangingRole event. If a player is assigned an SCP role at round start, the plugin performs a "roll" based on the SpawnChance config. If successful, it overrides the initial role after a short delay to ensure stability.

3. Audio Implementation
Audio is handled via AudioPlayerApi in 2D (Non-Spatial) mode. This ensures the user hears their own music perfectly while it still propagates to nearby players through the Speaker system.

⚙ Configuration
YAML
scp066:
  is_enabled: true
  # The schematic name from MapEditorReborn/Schematics folder
  schematic_name: "Scp066"
  # Chance (1-100) to replace a regular SCP at round start
  spawn_chance: 10
  # Volume of SCP-066 sounds (0.0 to 1.0)
  volume: 0.8
  # Symphony (Beethoven) attack stats
  attack_duration: 24
  damage: 5
  attack_radius: 8
🎮 How to Play
Players can bind keys to the following console commands:

Eric Sounds: .scp066 eric (or e)

Random Music: .scp066 music (or m)

Symphony Attack: .scp066 boom (or b)

Recommended Binds:

Plaintext
cmdbind c .scp066 eric
cmdbind v .scp066 music
cmdbind x .scp066 boom
📦 Requirements
EXILED Framework (v9.13.1+)

AudioPlayerApi (Included in dependencies)

MapEditorReborn (ProjectMER) for the schematic model

NVorbis.dll (For .ogg decoding)

🔧 Installation
Place Scp066.dll in your EXILED/Plugins folder.

Place dependencies (AudioPlayerApi.dll, NVorbis.dll) in EXILED/Plugins/dependencies.

Create the following directory: EXILED/Configs/Plugins/scp066/Audio/.

Add your audio files: Eric1-3.ogg, Notes1-6.ogg, and Beethoven.ogg.

Ensure your Scp066 schematic is inside the MapEditorReborn/Schematics folder.
