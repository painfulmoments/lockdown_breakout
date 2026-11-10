# Windows PC VR build and validation

This record describes the **v0.1.0 Windows x64 PC VR preview** distributed through [GitHub Releases](https://github.com/painfulmoments/lockdown_breakout/releases/tag/v0.1.0-pcvr). For installation and controller instructions, read the [play guide](../PLAYING.md).

## Build

| Item | Result |
| --- | --- |
| Unity Editor | 2022.3.62f2 |
| Target | Windows Standalone x64 |
| Enabled scene | `Assets/QA_HoldingCells/Demo/Demo_2.unity` |
| Player executable | `LockdownBreakOut.exe` |
| Build result | **Success**, exit code 0 |
| Build time | 344.177 seconds |
| C# compiler errors | 0 |
| Unique C# compiler warnings | 2 unused-field warnings |
| Complete player output | 168 files; 6,724,355,363 bytes before documentation and compression |
| Runtime files | Matching `_Data` folder, `UnityPlayer.dll`, Mono runtime and OpenXR native plugins included |

The build used the maintainer's existing local project and Library cache at source commit `7ca1a3dad744c1d9d17f382cc64df7a1d9faebcb`. It was not a fresh clone/import test. The local project also contains untracked or ignored sample files, so this record does not claim a byte-for-byte reproducible clean-checkout build. The earlier [clean-source validation](VALIDATION.md) is a separate check.

The Windows player was generated with the enabled scene and saved Standalone OpenXR configuration. No texture-quality reduction or gameplay change was introduced for this release. Original tracked configuration and script checksums, together with the pre-existing working-copy status, were preserved after the build.

## Startup check

The generated executable was launched for **14.02 seconds without a connected headset**. Unity and Direct3D 11 initialized, and the scene reached its gameplay update loop. No common managed game exceptions were observed during that interval.

OpenXR returned **`XR_ERROR_FORM_FACTOR_UNAVAILABLE`** because no headset was connected. This is not evidence of a successful VR session. Headset tracking, physical controller interaction, visual comfort and a complete escape playthrough remain unverified for this packaged preview.

The checked scene's XR rig uses **left-thumbstick movement** and **right-thumbstick continuous turning**. Other interactions were checked against the input actions and gameplay scripts; see [controls](../PLAYING.md#controls).

## Download integrity and packaging

The complete Unity player is compressed without removing its runtime dependencies. GitHub limits each release attachment to [less than 2 GiB](https://docs.github.com/en/repositories/releasing-projects-on-github/about-releases), so the ZIP is provided as numbered parts. `Join_Demo.cmd` combines those parts using Windows' built-in binary-copy command; Windows **Extract All** can then extract the complete ZIP.

The release includes `SHA256SUMS.txt` for the numbered parts and combined archive. The complete ZIP is checked for CRC errors before publication. Keep the executable, its `_Data` directory, native libraries and runtime folders together.

This is a **PC VR preview**, not an Android APK or a validated standalone Quest build. Meta Quest 3 is the project's target headset; the play guide explains the Quest Link / active OpenXR setup.
