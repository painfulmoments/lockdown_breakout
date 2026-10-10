# Validation of the playable source release

Validated on 2026-10-11 (Asia/Shanghai) with Unity 2022.3.62f2 in an isolated project folder copied from the selected public files, without the original Library or package cache. Unity restored the packages from the committed manifest and lock file.

## Results

| Check | Result |
| --- | --- |
| Package restoration and script compilation | Passed |
| Main scene | `Assets/QA_HoldingCells/Demo/Demo_2.unity` opened successfully |
| Scene objects | 4,762 |
| Missing scripts in the loaded scene | 0 |
| Unresolved original gameplay script classes | 0 |
| Unity-reported asset dependencies absent from the release selection | 0 |
| Standalone XR settings registered | Yes |
| Initialize XR on Startup | Enabled |
| Configured XR loader | `UnityEngine.XR.OpenXR.OpenXRLoader` |
| Final automated Play Mode duration | Approximately 20 seconds |
| Errors and interaction assertion failures | 0 |
| Active switch initial positions | All eight preserved their configured positions |
| Cabinet door opening | Remained at its open rotation after two seconds |
| Escape feedback | Displayed `Congratulations! You survived!` for a Player-tagged collider |

The interaction checks called the existing gameplay methods in the isolated copy. They did not simulate physical controller input or a complete escape-game playthrough. No headset was active during the check. An environment preview rendered with the camera temporarily raised only for the image, then restored; production XR camera settings were not altered.

## Release preparation

The selected resource set includes the main scene, required models, materials, textures, animations, audio, lighting and navigation data, XR Starter Assets, TextMesh Pro resources, and their Unity metadata. Unity's native dependency list was compared with the release inventory; original FBX material dependencies and their textures were included. See [ASSET_MANIFEST.txt](ASSET_MANIFEST.txt).

The preparation corrected three gameplay script filenames to match their classes while preserving GUIDs, restored a known prefab script reference, removed empty orphaned scene components, assigned existing hand/UI targets, and registered the existing Standalone XR settings. Small gameplay fixes preserve the door's open rotation, cache switch positions before Start calls, and allow escape feedback from the current XR player. Unused stale Oculus and AR simulation settings and unreferenced OpenXR subobjects were excluded.

## Remaining warnings and untested paths

The final check recorded eight imported collider warnings: four vehicle WheelColliders without Rigidbody components, two complex meshes using partial convex hulls, and two negatively scaled cell-glass BoxColliders. They did not cause errors during the automated run.

A connected-headset test of tracking, controller input, locomotion, and the complete escape sequence has not been performed. Desktop executable and standalone Android/Quest builds have not been validated. These checks establish that the selected source project imports, compiles, opens, and runs the tested desktop scene interactions.
