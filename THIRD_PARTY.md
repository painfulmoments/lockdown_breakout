# Third-party assets and release scope

This repository publishes the team's original gameplay source code, selected authored interaction and animation data, Unity project settings, package manifests, and XR configuration. It excludes imported third-party content whose source redistribution rights were not established.

The local development folder is larger than the public repository. Excluded files remain in the local project; they were not deleted or replaced during publication.

## Content excluded from the public repository

The following imported asset directories and their Unity metadata are excluded:

- `Assets/3DB_RedSwitcher`
- `Assets/ANOT_font` and its demo scene
- `Assets/ArchVizPRO_Beds_vol.1`
- `Assets/Complete Vehicle Pack`
- `Assets/Damian Gonzalez`
- `Assets/HandyHandPack`
- `Assets/Horror Creatures`
- `Assets/Horror_House`
- `Assets/HQ_Modular_RE_Mansion`
- `Assets/Layer Lab`
- `Assets/Lets Make a VR Game`
- `Assets/QA_HoldingCells`
- `Assets/SciFi_Gun_Collection`
- `Assets/Skyboxes`
- `Assets/Standard Assets`

Unverified standalone models, character textures, imported creature prefabs, images, audio, fonts, and baked lighting are also excluded. Unity package sample folders, TextMesh Pro resources, and other imported sample content are omitted so that users can obtain them through their original packages with the applicable license notices.

`3Djianmo` contains additional copies of imported assets and generated files and is excluded in full. Exported `.unitypackage` archives, Unity caches, logs, editor user settings, and IDE-generated files are not published.

The root `.gitignore` uses an allowlist for `Assets`. New imported content is excluded by default. Review its provenance and license before changing the allowlist.

## Reconstructing a playable project

Acquire needed assets from their original publishers under the appropriate licenses. An asset's presence in the development folder does not grant a new license to redistribute its source files.

The complete escape level referenced by the original Build Settings is `Assets/QA_HoldingCells/Demo/Demo_2.unity`, which is not included. Importing a stock version of the asset pack alone may not reproduce the team's customized level. Build your own scene using the published gameplay systems, restore or replace omitted prefab references, and update Build Settings accordingly.

Unity package dependencies are declared in `Packages/manifest.json` and `Packages/packages-lock.json`; they retain their own licenses. Restore package samples and TextMesh Pro resources using Unity's package tools when needed. The selected saved XR rig prefabs may depend on those omitted samples.

Import **XR Interaction Toolkit 2.6.4 > Samples > Starter Assets** in Package Manager before compiling: `EnemyChase.cs` and `RoomDoor.cs` reference its namespace. Saved rig prefabs additionally reference omitted hand models and hand animation scripts; supply or replace those dependencies in your scene.

The saved `GameStart.unity` menu layout depends on an omitted third-party loading script, images, and a font. `Scenes/SampleScene.unity` contains only legacy settings and references omitted lighting and skybox assets. Neither scene is a complete runnable game.

## Unity sample-derived scaffolding

Unity-provided portions of the saved XR rig and interaction scaffolding, including sample-derived data under `Assets/New Folder` and `Assets/公用`, retain their applicable Unity license and are not relicensed as MIT. The original XR Interaction Toolkit 2.6.4 copyright and license notice is preserved in [docs/licenses/Unity-XR-Interaction-Toolkit-LICENSE.md](docs/licenses/Unity-XR-Interaction-Toolkit-LICENSE.md). Its referenced [Unity Companion License](https://unity.com/legal/licenses/unity-companion-license) governs Unity-dependent use of those portions. The team's original gameplay scripts remain under the MIT License.

## Software license and award photos

The repository's MIT License applies to the team's original software and authored gameplay data included in the release. It does not relicense Unity packages, excluded third-party content, or award photos.

`docs/awards/certificate.jpg` and `docs/awards/trophies.jpg` are the owner-provided photos supplied for this project's award documentation. They are included to document the Best Entertainment Award and are not offered under the MIT software license. The separate project's trophy visible on the right of the second photo is not an award claim for Lockdown: Breakout.
