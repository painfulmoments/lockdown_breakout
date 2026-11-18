# Lockdown：Breakout

**A VR horror escape game developed from an experimental research prototype.**

**Unity · Windows PC VR / OpenXR · 3-person game team · 2025**

[Download PC VR demo](https://github.com/painfulmoments/lockdown_breakout/releases/tag/v0.1.0-pcvr) · [PC VR play guide](PLAYING.md) · [Legacy SURF demo](SURF_PC_DEMO.md) · [中文主页](README.zh-CN.md) · [中文试玩说明](PLAYING.zh-CN.md) · [Project case study](https://14mountain.com/engineering-lockdown) · [Research paper](https://doi.org/10.1016/j.cag.2025.104470)

<img src="docs/images/teaser.png" width="100%" alt="Lockdown：Breakout — a first-person VR view inside the containment facility">

**Release status:** Windows PC VR preview. Build and desktop startup checks passed; a complete packaged headset playthrough is **not yet verified**. [Validation record](docs/BUILD_AND_VALIDATION.md).

You are trapped in a biological research facility after a containment failure. Find a route through its two floors, solve environmental puzzles and avoid the creatures moving through the building. Controller-driven hands let you handle objects and operate the environment; a laser weapon can slow enemies when you need a way past them.

*Chinese title: 禁闭：突围. Created by Team XJTLU Joy Over There (西浦彼间乐), with support from the Virtual Engineering Centre at Xi’an Jiaotong-Liverpool University.*

## Choose a demo

| Version | Package | Download and instructions |
| --- | --- | --- |
| **Current Lockdown：Breakout** | Windows PC VR / OpenXR preview | [v0.1.0-pcvr release](https://github.com/painfulmoments/lockdown_breakout/releases/tag/v0.1.0-pcvr) · [PC VR play guide](PLAYING.md) |
| **Legacy SURF 2024** | Historical Windows keyboard-and-mouse build; **1.96 GiB**, single ZIP | [Legacy playable release](https://github.com/painfulmoments/lockdown_breakout/releases/tag/surf2024-legacy-pc-playable-2026-11-19) · [Legacy play guide](SURF_PC_DEMO.md) |
| SURF 2024 recording | 1080p video; **recording only, not a playable build** | [Watch/download the recording](https://github.com/painfulmoments/lockdown_breakout/releases/tag/surf2024-pc-demo-2026-11-18) |

The legacy release preserves the original SURF desktop archive, with `escape the villa/escape the villa.exe` and its accompanying runtime files. It is a historical binary, **not a build from the current Unity source in this repository**. Its instructions and validation are recorded separately in the [legacy guide](SURF_PC_DEMO.md). The award below belongs to Lockdown：Breakout.

## Play the demo

The steps and controls below apply to **the current Lockdown PC VR preview**. For the historical SURF desktop package, use the [legacy play guide](SURF_PC_DEMO.md).

| Item | Current demo |
| --- | --- |
| Platform | Windows x64 PC VR / OpenXR; not a standalone Quest APK |
| Headset and controllers | Quest 3 was the project target; two tracked controllers required |
| Connection | Enter Quest Link before launch; this package has not completed a physical-headset test |
| Free disk space | Allow about **20 GB** while keeping downloads, combined ZIP and extracted game |
| Player setup | No Unity Editor, Git or Git LFS required |
| Release | **v0.1.0-pcvr**, prototype preview |

Download these files from the [same release](https://github.com/painfulmoments/lockdown_breakout/releases/tag/v0.1.0-pcvr):

| File | Size / purpose |
| --- | --- |
| [ZIP part .001](https://github.com/painfulmoments/lockdown_breakout/releases/download/v0.1.0-pcvr/LockdownBreakOut-Windows-x64-v0.1.0.zip.001) | **1.50 GiB**, required |
| [ZIP part .002](https://github.com/painfulmoments/lockdown_breakout/releases/download/v0.1.0-pcvr/LockdownBreakOut-Windows-x64-v0.1.0.zip.002) | **0.97 GiB**, required |
| [Join_Demo.cmd](https://github.com/painfulmoments/lockdown_breakout/releases/download/v0.1.0-pcvr/Join_Demo.cmd) | Checks and combines the parts using Windows' built-in tools |
| [SHA256SUMS.txt](https://github.com/painfulmoments/lockdown_breakout/releases/download/v0.1.0-pcvr/SHA256SUMS.txt) | Optional manual integrity verification |

1. Put **both numbered parts and `Join_Demo.cmd` in one folder**. These are pieces of one ZIP: do not extract them separately.
2. Double-click **`Join_Demo.cmd`**. It verifies both parts and the completed archive. Right-click the resulting `LockdownBreakOut-Windows-x64-v0.1.0.zip` and choose **Extract All**.
3. Connect the headset, make Meta Link the active OpenXR runtime, and **enter Quest Link inside the headset**.
4. Open the extracted `LockdownBreakOut-Windows-x64` folder and run **`LockdownBreakOut.exe`**. Keep its `_Data` folder and runtime libraries together.

**Do not download GitHub's automatic “Source code” archive to play.** No extra extraction software is needed. The current release is a Windows PC VR application, not an Android installation.

See the [complete play guide](PLAYING.md) or [中文试玩说明](PLAYING.zh-CN.md) for Link setup, controls and troubleshooting.

## Controls

| Input | Action |
| --- | --- |
| Left thumbstick | Move |
| Right thumbstick | Continuous turn |
| Grip / Select | Grab supported objects and operate puzzle switches |
| Trigger / Activate | Fire the held slowing weapon; operate key-controlled doors in range |
| Window close / **Alt+F4** | Exit |

Bindings are checked against the supplied input actions and scripts; physical-controller behavior in this package remains unverified. [Detailed controls](PLAYING.md#controls).

## Gameplay

| System | What it adds to the escape |
| --- | --- |
| Two-floor progression | Move between the lower containment spaces and the contrasting domestic rooms above. Objectives, enemy routes and button locations connect the two floors. |
| Environmental puzzles | Search for keys, operate doors and activate switches to open the way forward. The adopted button puzzle was developed collaboratively. |
| Patrol and pursuit | NavMesh-based enemies patrol, detect and chase the player. Hiding spaces and checkpoints provide ways to respond to encounters. |
| Slowing laser weapon | Use a controller-held weapon to reduce an enemy’s movement speed. It was added to make the competition demo more approachable for players less familiar with horror games. |
| Weapon recovery route | A hole back to the basement gives players another opportunity to discover a gun they may have passed earlier, then return using an already-open route. |
| Embodied interaction | Quest controller inputs animate simulated hands and drive object and environmental interaction. |

The game is a **core gameplay prototype**.

## From research prototype to escape game

Lockdown grew from **SURF 2024**, a Unity VR horror research project. The research prototype separated a shared environment into three conditions: Scene 0 (atmospheric), Scene 1 (monster-chase), and Scene 2 (baseline). A 30-participant study compared physiological recordings and subjective responses, leading to a paper published in 2025.

For Lockdown, the game team brought the previously separated elements into a connected escape experience. The focus shifted from controlling experimental stimuli to arranging progression, encounters, puzzles and player responses across two floors.

**Publication:** Tao, Z., et al. (2025). *Correlations between instant and prolonged stimuli with physiological and subjective responses in VR horror*. Computers & Graphics, 133, 104470. [DOI: 10.1016/j.cag.2025.104470](https://doi.org/10.1016/j.cag.2025.104470).

## Award

**Best Entertainment Award · Professional Category**  
2025 Star Shark Cup Global Virtual Reality Content Competition

Awarded to **Lockdown：Breakout (禁闭：突围)** and **Team XJTLU Joy Over There**. The certificate is dated **26 December 2025**.

<table>
  <tr>
    <td align="center"><img src="docs/awards/certificate.png" width="360" alt="Lockdown：Breakout Best Entertainment Award certificate from the 2025 Star Shark Cup, Professional Category"></td>
    <td align="center"><img src="docs/awards/trophies.jpg" width="420" alt="Award trophies, with the Lockdown：Breakout trophy on the left"></td>
  </tr>
  <tr>
    <td align="center">Best Entertainment Award certificate</td>
    <td align="center">Lockdown trophy on the left. The trophy on the right belongs to a separate project.</td>
  </tr>
</table>

## Game team

| Contributor | Responsibilities |
| --- | --- |
| **Binwei Lei — Game Designer & Lead Developer** | Led most mechanics and level design, including chase routes, spawn and respawn points, two-floor progression and button placement. Wrote the main gameplay code and integrated the team’s systems. Proposed the slowing laser weapon and weapon-recovery route, and the original spotlight puzzle concept. |
| **Zeren Tao — Co-design & Programming** | Contributed to planning and programming, including decisions about monsters, the guard character’s appearance and weapon mechanics. Proposed and developed the adopted four-button puzzle. |
| **Yibang Zhao — Environment Art & Scene Building** | Created map art, assembled scenes, and produced and integrated art assets. |

Specific puzzle ideas were discussed together. **Qilei Sun** provided academic guidance. The SURF research stage involved a separate seven-member research team; its authorship and research credits are recorded in the paper.

## Open the Unity project

The repository contains the gameplay scripts, the main escape scene and the selected assets and interaction dependencies needed by that scene. Large binary assets use **Git LFS**.

```bash
git lfs install
git clone https://github.com/painfulmoments/lockdown_breakout.git
cd lockdown_breakout
git lfs pull
```

The selected source content occupies approximately **8.3 GiB** before Unity generates its `Library` folder. Allow additional space for imports, packages and builds. A GitHub source ZIP containing LFS pointer files is not a substitute for the complete asset checkout.

1. Add the cloned folder to Unity Hub and open it with **Unity 2022.3.62f2**.
2. Allow package restoration and asset import to complete.
3. Open **`Assets/QA_HoldingCells/Demo/Demo_2.unity`**, the main escape level selected in Build Settings. A fresh Editor session opens this scene automatically when there is no saved scene.
4. Connect the headset and select an active OpenXR runtime. The saved Standalone configuration uses the OpenXR loader with **Initialize XR on Startup** enabled.
5. Enter Play Mode to test in the Editor, or build the enabled main scene for Windows Standalone.

| Component | Recorded version |
| --- | --- |
| Unity Editor | 2022.3.62f2 |
| OpenXR Plugin | 1.14.3 |
| XR Interaction Toolkit | 2.6.4 |
| XR Hands package | 1.4.3 |
| AI Navigation | 1.1.7 |
| Universal Render Pipeline | 14.0.12 |

Meta Quest 3 was the project’s target headset. An Android/standalone Quest build has not been validated for this repository.

## Validation

The Windows x64 player now builds successfully and has completed a short desktop startup check. **A connected-headset playthrough has not been performed.** See the [current build record](docs/BUILD_AND_VALIDATION.md) for results and the OpenXR behavior observed without a headset.

The existing clean-source check restored packages, imported the selected assets, compiled scripts and opened the main level in Unity 2022.3.62f2. Its approximately 20-second automated Play Mode run reported zero errors and no missing scene scripts. Interaction checks covered eight saved switch positions, an open cabinet door retaining its rotation, and escape feedback. Those checks called gameplay methods; they did not simulate a complete controller-driven escape playthrough.

Some imported collider warnings remain. See the release notes for packaged-build checks, [validation details](docs/VALIDATION.md) for the source checks, and the [asset manifest](docs/ASSET_MANIFEST.txt) for the selected content.

## Help and feedback

See [troubleshooting](PLAYING.md#troubleshooting). In [Issues](https://github.com/painfulmoments/lockdown_breakout/issues), include the release version, headset, connection, Windows version, GPU, active OpenXR runtime and reproduction steps. Review logs for private information before attaching them.

## License and included assets

The team’s original code and authored gameplay configuration included in this repository use the [MIT License](LICENSE).

Included third-party assets are published based on redistribution permissions confirmed by the project owner and retain their original terms and notices. The MIT License does not relicense them. Unity packages and Unity sample-derived portions retain their respective licenses; notices are preserved in [docs/licenses](docs/licenses), including the [Unity Companion License](docs/licenses/Unity-Companion-License.md). Liberation Sans retains the SIL Open Font License. Award photos document the achievement and are excluded from the software license. See [third-party assets and release scope](THIRD_PARTY.md).

## Related project

[REHUB Grounding Demo](https://github.com/painfulmoments/Grounding_demo) explores guided grounding with a conversational digital human in a forest environment.
