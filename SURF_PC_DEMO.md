# SURF 2024 — Legacy Windows PC demo

**A preserved keyboard-and-mouse exploration build from the earlier SURF project, titled Escape the Villa.** It runs on a Windows x64 PC. No VR headset, tracked controllers, Unity Editor, Git or Git LFS is required to play this packaged build.

[Download the legacy demo](https://github.com/painfulmoments/lockdown_breakout/releases/tag/surf2024-legacy-pc-playable-2026-11-19) · [中文说明](SURF_PC_DEMO.zh-CN.md) · [Current Lockdown PC VR demo](PLAYING.md) · [Project home](README.md)

This is a historical binary supplied by the project owner. It was built with **Unity 2023.2.20f1c1**, and has not been rebuilt from the current Lockdown source repository. The archived demo starts with an introduction and loads one `demo` scene; it should not be treated as a distribution of all three experimental conditions from the SURF research.

## Download and first launch

| Release attachment | Size / role |
| --- | --- |
| [SURF-2024-Legacy-PC-Demo.zip](https://github.com/painfulmoments/lockdown_breakout/releases/download/surf2024-legacy-pc-playable-2026-11-19/SURF-2024-Legacy-PC-Demo.zip) | **1.96 GiB** (2,101,800,847 bytes); the complete original archive under a download-friendly attachment name |
| `SURF-PC-PLAYING.txt` | English/Chinese quick-start and known limitations |
| `SHA256SUMS.txt` | SHA-256 checksums for the ZIP and quick-start file |

1. Download **`SURF-2024-Legacy-PC-Demo.zip`** from the linked release. It is a **single complete ZIP**, so no split-file join helper is needed. GitHub's automatic **Source code** archives are not this playable build.
2. Use Windows **Extract All**. Keep at least **10 GB of free disk space** while retaining the download and extracted files. The archive expands to approximately **4.75 GiB** (5,105,106,810 bytes).
3. Open the extracted **`escape the villa`** folder. Keep its contents together, especially `UnityPlayer.dll`, `escape the villa_Data` and `MonoBleedingEdge`.
4. Double-click **`escape the villa.exe`**. The window uses the original internal title **`3Djianmo`**.
5. At the **Escape the Villa / START GAME** screen, press **any key** or **left-click**. The configured two-second fade leads into the `demo` scene; first loading can take longer. Use the keyboard and mouse to explore.

## Controls

| Action | Input |
| --- | --- |
| Move | **WASD** or **arrow keys** |
| Look | **Mouse** |
| Run | Hold **Left Shift** |
| Crouch | Hold **Left Ctrl** |
| Pick up a key / operate a nearby door | **E**, when in interaction range |
| Toggle the flashlight | **F** |
| Pause / resume | **Esc** |
| Quit from pause | **Esc**, then **Enter** |
| Close the Windows game | **Alt+F4**, if needed |

These bindings were checked against this archived build's managed code and enabled scene components. The intro-to-scene transition was also exercised with the keyboard. The pause/quit bindings were reviewed statically; do not rely on clickable pause-menu buttons in this historical build. The scene's Jump component is disabled, so Space is not documented as a jump control.

## Checks and known limitations

- **Archive:** all 197 files passed CRC validation and were safely extracted. Required x64 Unity Player, Mono runtime and game-data files are present. The ZIP contents were not repacked or modified.
- **Desktop startup:** a 20.12-second windowed check initialized Unity, Direct3D 11, Mono and input. A separate interactive check displayed the intro, accepted the start key and rendered the hallway scene without a headset.
- **Historical defects observed after entering the scene:** `PauseMenu.Start()` reports a `NullReferenceException`; one object named `Am` has a missing script; an agent reports no valid NavMesh. Collider and Animator warnings also occur. The scene still renders, but these issues can affect the menu or enemy behavior. No binary patch has been applied.
- **Scope:** a full escape playthrough, every interaction and enemy encounter have **not** been verified. This is a historical preview, not an error-free or production-ready release. It is separate from the current Lockdown PC VR build and its validation record.

For the exact checks and preserved-artifact details, see [Legacy validation](docs/SURF_PC_VALIDATION.md).

## Integrity and troubleshooting

The ZIP SHA-256 is:

```text
d39811e9c1aec47a8c139dddbc08683f80297fae769ec88a3fcaf3ee0cffe550
```

Use `SHA256SUMS.txt` to check a completed download. If extraction fails, compare the ZIP's byte count and SHA-256, then download it again if they differ. A missing `UnityPlayer.dll` or `_Data` error usually means the executable was moved away from its accompanying files or the archive was not fully extracted.

If you expected the later VR escape game, use [Lockdown's PC VR guide](PLAYING.md). This legacy build does not use that release's split ZIPs, join helper or Quest Link launch procedure. The [SURF recording](https://github.com/painfulmoments/lockdown_breakout/releases/tag/surf2024-pc-demo-2026-11-18) remains available as a separate video.

The archived application and its bundled third-party content retain their original rights. The repository's MIT license does not automatically relicense the whole legacy binary package; see [release scope and notices](THIRD_PARTY.md).
