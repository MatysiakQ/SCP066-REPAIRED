# SCP-066 Repaired

> 🇵🇱 [Polski](#polski) | 🇬🇧 [English](#english)

---

# Polski

![Version](https://img.shields.io/badge/wersja-1.0.0-blue)
![EXILED](https://img.shields.io/badge/EXILED-compatible-brightgreen)
![License](https://img.shields.io/badge/license-MIT-green)

Naprawiona i przepisana wersja pluginu [SCP-066 autorstwa RisottoMan](https://github.com/RisottoMan/SCP-066), dostosowana do najnowszego silnika EXILED i ProjectMER.

> 💙 **Szczególne podziękowania dla [RisottoMan](https://github.com/RisottoMan)** za oryginalny plugin oraz **PaRRot** za pomoc przy naprawie i przepisaniu.

---

## ✨ Co zostało zmienione względem oryginału
- Przepisany spawn — zamiast per-gracz, jedna kość na całą rundę (15% szansy)
- Naprawiona kompatybilność z najnowszym EXILED i ProjectMER
- Naprawiony system audio oparty na SCPSLAudioApi
- Dodana ochrona przed konfliktem z innymi CustomRole (np. SCP-153)
- Poprawiony teleport spawnu do HCZ Armory
- Zmniejszone HP do 2000

---

## Wymagania
- [EXILED](https://github.com/ExMod-Team/EXILED) (najnowsza wersja)
- [Exiled.CustomRoles](https://github.com/ExMod-Team/EXILED) (dołączone do EXILED)
- [MapEditorReborn / ProjectMER](https://github.com/Michal78900/MapEditorReborn)
- [SCPSLAudioApi](https://github.com/DayLightDevelopment/SCPSLAudioApi)

---

## Instalacja

### 1. Plugin
Wrzuć skompilowany plik `Scp066.dll` do folderu:
```
EXILED/Plugins/
```

### 2. Schematic (model)
Wrzuć folder ze schematem `Scp066` do folderu MapEditorReborn:
```
EXILED/Configs/MapEditorReborn/Schematics/Scp066/
```

### 3. Audio
Wrzuć pliki dźwiękowe do folderu:
```
EXILED/Configs/Plugins/scp066/Audio/
```

Wymagane pliki (format `.ogg`, **mono**, **48kHz**):
```
Beethoven.ogg   — główny atak symfoniczny
Eric1.ogg
Eric2.ogg
Eric3.ogg       — dźwięki Erica (losowane)
Notes1.ogg
Notes2.ogg
Notes3.ogg
Notes4.ogg
Notes5.ogg
Notes6.ogg      — dźwięki muzyczne (losowane)
```

---

## Struktura folderów (końcowy wynik)
```
EXILED/
├── Plugins/
│   └── Scp066.dll
├── Configs/
│   ├── Plugins/
│   │   └── scp066/
│   │       └── Audio/
│   │           ├── Beethoven.ogg
│   │           ├── Eric1.ogg
│   │           ├── Eric2.ogg
│   │           ├── Eric3.ogg
│   │           ├── Notes1.ogg
│   │           ├── Notes2.ogg
│   │           ├── Notes3.ogg
│   │           ├── Notes4.ogg
│   │           ├── Notes5.ogg
│   │           └── Notes6.ogg
│   └── MapEditorReborn/
│       └── Schematics/
│           └── Scp066/
│               └── Scp066.json (+ assety)
```

---

## Konfiguracja
Po pierwszym uruchomieniu serwera config pojawi się automatycznie w:
```
EXILED/Configs/config_gameplay.yml
```

| Opcja | Domyślnie | Opis |
|---|---|---|
| `is_enabled` | `true` | Włącza/wyłącza plugin |
| `debug` | `false` | Logi debugowania w konsoli |
| `schematic_name` | `Scp066` | Nazwa schematu z MapEditorReborn |
| `spawn_chance` | `10` | Szansa spawnu w % (10 = 10%) |
| `volume` | `0.8` | Głośność dźwięków (0.0 - 1.0) |
| `attack_duration` | `24` | Czas trwania ataku Beethovena (sekundy) |
| `damage` | `5` | Obrażenia na sekundę podczas ataku |
| `attack_radius` | `8` | Zasięg ataku i słyszalności dźwięku |
| `cooldown_eric` | `10` | Cooldown dźwięku Eric (sekundy) |
| `cooldown_music` | `15` | Cooldown dźwięków Notes (sekundy) |
| `cooldown_attack` | `45` | Cooldown ataku Beethovena (sekundy) |

---

## Jak działa
- Na starcie każdej rundy plugin rzuca kością — **10% szansy** że SCP-066 w ogóle się pojawi
- Wybiera **losowego SCPa** z puli (pomija SCP-079 i graczy którzy już mają inną CustomRole)
- Wybrany gracz spawnuje się w **HCZ Armory**
- SCP-066 ma **2000 HP**, **500 HumeShield** i zmniejszoną skalę postaci
- Porusza się wolniej niż normalny SCP

### Umiejętności (bindy gracza)
| Komenda | Skrót | Opis |
|---|---|---|
| `.scp066 eric` | `.scp066 e` | Odtwarza losowy dźwięk Erica |
| `.scp066 music` | `.scp066 m` | Odtwarza losową melodię |
| `.scp066 boom` | `.scp066 b` | Aktywuje atak Beethovena |

### Atak Beethovena
- Odtwarza `Beethoven.ogg` słyszalny w zasięgu ataku
- Przez **24 sekundy** zadaje **5 obrażeń/s** wszystkim graczom w zasięgu
- Trafieni gracze dostają efekt **Concussed** na 2 sekundy
- Cooldown: **45 sekund**

---

## Komendy (Remote Admin)
| Komenda | Opis |
|---|---|
| `scp066 give <nick/id>` | Nadaje rolę SCP-066 graczowi |
| `scp066 remove <nick/id>` | Odbiera rolę SCP-066 graczowi |

> Wymagane uprawnienie: `scp066.admin`

---

## Autor
**Matysiak** — wersja 1.0.0 Repaired

Oparty na oryginalnym pluginie **[SCP-066 by RisottoMan](https://github.com/RisottoMan/SCP-066)**
Podziękowania: **RisottoMan**, **PaRRot**

---
---

# English

![Version](https://img.shields.io/badge/version-1.0.0-blue)
![EXILED](https://img.shields.io/badge/EXILED-compatible-brightgreen)
![License](https://img.shields.io/badge/license-MIT-green)

A repaired and rewritten version of the [SCP-066 plugin by RisottoMan](https://github.com/RisottoMan/SCP-066), updated for the latest EXILED and ProjectMER engine.

> 💙 **Special thanks to [RisottoMan](https://github.com/RisottoMan)** for the original plugin and **PaRRot** for help with the repair and rewrite.

---

## ✨ What changed from the original
- Rewritten spawn logic — instead of per-player rolls, one dice roll per round (15% chance)
- Fixed compatibility with latest EXILED and ProjectMER
- Fixed audio system based on SCPSLAudioApi
- Added protection against conflicts with other CustomRoles (e.g. SCP-153)
- Fixed spawn teleport to HCZ Armory
- Reduced HP to 2000

---

## Requirements
- [EXILED](https://github.com/ExMod-Team/EXILED) (latest version)
- [Exiled.CustomRoles](https://github.com/ExMod-Team/EXILED) (included with EXILED)
- [MapEditorReborn / ProjectMER](https://github.com/Michal78900/MapEditorReborn)
- [SCPSLAudioApi](https://github.com/DayLightDevelopment/SCPSLAudioApi)

---

## Installation

### 1. Plugin
Drop the compiled `Scp066.dll` into:
```
EXILED/Plugins/
```

### 2. Schematic (model)
Place the `Scp066` schematic folder in the MapEditorReborn directory:
```
EXILED/Configs/MapEditorReborn/Schematics/Scp066/
```

### 3. Audio
Place audio files in:
```
EXILED/Configs/Plugins/scp066/Audio/
```

Required files (`.ogg` format, **mono**, **48kHz**):
```
Beethoven.ogg   — main symphony attack sound
Eric1.ogg
Eric2.ogg
Eric3.ogg       — Eric sounds (randomly picked)
Notes1.ogg
Notes2.ogg
Notes3.ogg
Notes4.ogg
Notes5.ogg
Notes6.ogg      — music notes sounds (randomly picked)
```

---

## Folder Structure (final result)
```
EXILED/
├── Plugins/
│   └── Scp066.dll
├── Configs/
│   ├── Plugins/
│   │   └── scp066/
│   │       └── Audio/
│   │           ├── Beethoven.ogg
│   │           ├── Eric1.ogg
│   │           ├── Eric2.ogg
│   │           ├── Eric3.ogg
│   │           ├── Notes1.ogg
│   │           ├── Notes2.ogg
│   │           ├── Notes3.ogg
│   │           ├── Notes4.ogg
│   │           ├── Notes5.ogg
│   │           └── Notes6.ogg
│   └── MapEditorReborn/
│       └── Schematics/
│           └── Scp066/
│               └── Scp066.json (+ assets)
```

---

## Configuration
After the first server launch, the config will appear automatically in:
```
EXILED/Configs/config_gameplay.yml
```

| Option | Default | Description |
|---|---|---|
| `is_enabled` | `true` | Enables/disables the plugin |
| `debug` | `false` | Debug logs in console |
| `schematic_name` | `Scp066` | Schematic name from MapEditorReborn |
| `spawn_chance` | `10` | Spawn chance in % (10 = 10%) |
| `volume` | `0.8` | Sound volume (0.0 - 1.0) |
| `attack_duration` | `24` | Duration of Beethoven attack (seconds) |
| `damage` | `5` | Damage per second during attack |
| `attack_radius` | `8` | Attack and audio range |
| `cooldown_eric` | `10` | Eric sound cooldown (seconds) |
| `cooldown_music` | `15` | Notes sound cooldown (seconds) |
| `cooldown_attack` | `45` | Beethoven attack cooldown (seconds) |

---

## How it works
- At the start of each round the plugin rolls the dice — **10% chance** that SCP-066 will appear at all
- Picks **one random SCP** from the pool (skips SCP-079 and players who already have another CustomRole)
- The chosen player spawns at **HCZ Armory**
- SCP-066 has **2000 HP**, **500 HumeShield** and a reduced player scale
- Moves slower than a normal SCP

### Abilities (player binds)
| Command | Alias | Description |
|---|---|---|
| `.scp066 eric` | `.scp066 e` | Plays a random Eric sound |
| `.scp066 music` | `.scp066 m` | Plays a random music note |
| `.scp066 boom` | `.scp066 b` | Activates the Beethoven attack |

### Beethoven Attack
- Plays `Beethoven.ogg` audible within attack range
- For **24 seconds** deals **5 damage/s** to all players in range
- Hit players receive the **Concussed** effect for 2 seconds
- Cooldown: **45 seconds**

---

## Commands (Remote Admin)
| Command | Description |
|---|---|
| `scp066 give <nick/id>` | Grants the SCP-066 role to a player |
| `scp066 remove <nick/id>` | Removes the SCP-066 role from a player |

> Required permission: `scp066.admin`

---

## Author
**Matysiak** — version 1.0.0 Repaired

Based on the original **[SCP-066 by RisottoMan](https://github.com/RisottoMan/SCP-066)**
Special thanks: **RisottoMan**, **PaRRot**
