# Legacy SURF PC demo — archive and runtime checks

This record applies only to [surf2024-legacy-pc-playable-2026-11-19](https://github.com/painfulmoments/lockdown_breakout/releases/tag/surf2024-legacy-pc-playable-2026-11-19), the historical Windows keyboard-and-mouse build supplied by the project owner. It is separate from the [current Lockdown PC VR build record](BUILD_AND_VALIDATION.md). Checks were performed on 2026-11-19, Asia/Shanghai.

## Preserved artifact

| Item | Result |
| --- | --- |
| Original archive name | `escape the villa.zip` |
| Release attachment | `SURF-2024-Legacy-PC-Demo.zip` |
| ZIP size | 2,101,800,847 bytes |
| ZIP SHA-256 | `d39811e9c1aec47a8c139dddbc08683f80297fae769ec88a3fcaf3ee0cffe550` |
| Archive entries | 219: 197 files and 22 directories |
| Uncompressed file bytes | 5,105,106,810 |
| CRC validation | All 197 files passed during full extraction |
| Path checks | No absolute/traversing paths, symlinks, encrypted members, case collisions or Windows reserved names |
| Main executable | `escape the villa/escape the villa.exe`, Windows x64 |
| Runtime | Unity Player 2023.2.20f1c1 (881fe566f400), Mono included |
| Original window title | `3Djianmo` |
| Distribution | Original ZIP bytes retained, including original debug-information sidecar; no repack or binary patch |

The archive does not contain its own readme or license document. A separate bilingual quick-start and checksum file accompany the download. Existing rights remain with their respective owners; the repository's root MIT does not relicense the entire archived binary package.

## Code and scene inspection

The packaged BuildSettings lists `Assets/GameStart.unity`, followed by `Assets/HQ_Modular_RE_Mansion/Scenes/demo.unity`. The enabled start component accepts a key or left click and calls `LoadScene("demo")` after its fade. The active scene uses first-person keyboard-and-mouse movement and look; the bindings in [English](../SURF_PC_DEMO.md) and [Chinese](../SURF_PC_DEMO.zh-CN.md) were checked against compiled IL, input settings and enabled scene components. The Jump component is disabled. The keyboard pause/quit route exists independently of the missing button bindings.

This preserved executable was not rebuilt from the current Unity source repository. No claim is made that it contains every research condition, matches the later VR game's mechanics or reproduces the original study.

## Runtime observations

1. A windowed Direct3D launch was observed for 20.12 seconds and then stopped. Unity, Direct3D 11, Mono and input initialized; no managed exception was logged during that initial intro-only interval.
2. A separate interactive desktop launch displayed the original Escape the Villa intro. A Return key triggered the transition; the `demo` hallway rendered successfully. No VR headset was needed for this observed desktop route.
3. The interactive scene log reported `NullReferenceException` in `PauseMenu.Start()`, a missing script on `Am`, `Failed to create agent because there is no valid NavMesh`, plus collider and Animator warnings. These are preserved historical defects. Scene rendering continued.

The intro-to-scene transition and scene rendering were observed. The pause/quit keyboard bindings and the remaining controls were inspected statically, not exhaustively exercised. No complete escape playthrough, full enemy-navigation check, network/offline audit or physical-headset test is claimed. The existing Lockdown PC VR validation is not evidence for this older binary.
