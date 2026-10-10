# Third-party assets and release scope

This repository contains the team's gameplay source, the main escape scene, Unity project settings, package manifests, XR configuration, and the assets selected for those scenes. Third-party source assets are included based on redistribution permissions confirmed by the project owner. Copyright remains with the respective authors, and included third-party content retains its original terms and notices. The root MIT License applies to the team's original software and authored gameplay data; it does not relicense imported assets.

The main escape level is `Assets/QA_HoldingCells/Demo/Demo_2.unity`, which is selected in Build Settings. The release includes its referenced models, prefabs, materials, textures, animations, audio, lighting data, navigation data, and VR interaction dependencies. The required XR Interaction Toolkit Starter Assets and TextMesh Pro resources are included. A clean Unity import, compilation, scene load, and automated desktop Play Mode check passed for this asset-inclusive release. Headset playtesting and builds have not been performed; see [README.md](README.md) for setup and [VALIDATION.md](docs/VALIDATION.md) for the checks and limits.

## Selected imported content

The release includes selected content from these imported asset directories, rather than complete copies of the original packs:

| Source directory | Included content |
| --- | --- |
| `Assets/3DB_RedSwitcher` | Switch/button models, textures, materials, and a referenced prefab |
| `Assets/ArchVizPRO_Beds_vol.1` | Referenced bed and related material/texture dependencies |
| `Assets/Complete Vehicle Pack` | Referenced vehicle/prop content and its dependencies |
| `Assets/Damian Gonzalez/Old Elevator Built-in RP` | Elevator system, referenced geometry, materials, sounds, and documentation |
| `Assets/HandyHandPack` | Referenced hand models, poses, prefabs, material, and textures |
| `Assets/Horror Creatures` | Referenced creature model, textures, and animation data |
| `Assets/Horror_House` | Referenced modular environment, props, materials, textures, and supporting data |
| `Assets/HQ_Modular_RE_Mansion` | Referenced modular environment, props, materials, textures, and supporting data |
| `Assets/Layer Lab/3D Models Prop House 180 Pack` | Referenced household props and their material/texture dependencies |
| `Assets/Lets Make a VR Game` | Referenced hand animation script, particle content, an example-derived material, and source credits |
| `Assets/QA_HoldingCells` | Customized main escape scene, referenced holding-cell assets, and scene lighting/navigation data |
| `Assets/SciFi_Gun_Collection` | Referenced `hellwailer.FBX` model |
| `Assets/Standard Assets` | Referenced particle shader |

Referenced standalone character models, animation files, audio, images, and authored prefabs under `Assets` are also included. Unity `.meta` files are retained so references survive a fresh checkout. Supporting documentation remains with its source content where available, including credits for the elevator system and the source links in `Assets/Lets Make a VR Game/ReadMe.txt`.

Unused models, alternate demo scenes, unused variants, the unused `ANOT_font` pack, original `.unitypackage` archives, and the duplicate `3Djianmo` asset collection are excluded. Unity caches, logs, editor user settings, and IDE-generated files are also excluded. Files omitted from the public repository remain in the local development folder.

The root `.gitignore` uses an allowlist for `Assets` to retain only the selected release content and required notices. Asset additions should preserve their dependencies, metadata, original copyright notices, and applicable terms.

## Unity content and notices

Unity Package Manager dependencies are declared in `Packages/manifest.json` and `Packages/packages-lock.json` and are restored from their original packages. They retain their respective package licenses; package caches are not distributed in this repository.

Imported XR Interaction Toolkit 2.6.4 Starter Assets and Unity-derived rig/interaction scaffolding retain the applicable Unity license. Notices are preserved in [Unity-XR-Interaction-Toolkit-LICENSE.md](docs/licenses/Unity-XR-Interaction-Toolkit-LICENSE.md) and [Unity-XR-Interaction-Toolkit-Third-Party-Notices.md](docs/licenses/Unity-XR-Interaction-Toolkit-Third-Party-Notices.md). Their Unity-dependent use is governed by the accompanying [Unity Companion License](docs/licenses/Unity-Companion-License.md).

Included TextMesh Pro resources and shaders retain the [TextMesh Pro copyright and license notice](docs/licenses/Unity-TextMesh-Pro-LICENSE.md). Liberation Sans and its font atlas data retain the SIL Open Font License in [`Assets/TextMesh Pro/Fonts/LiberationSans - OFL.txt`](Assets/TextMesh%20Pro/Fonts/LiberationSans%20-%20OFL.txt). TextMesh Pro's included sprite attribution is preserved in [`EmojiOne Attribution.txt`](Assets/TextMesh%20Pro/Sprites/EmojiOne%20Attribution.txt).

Unity-derived portions under `Assets/New Folder`, `Assets/公用`, and `Assets/Lets Make a VR Game/UI 3D Examples`, together with the imported Standard Assets shader, are not relicensed as MIT. Referenced package components retain the terms supplied with their original packages.

## Audio attribution

`Assets/104534__skyumori__door_locked_02.wav` corresponds to **Door_locked_02.wav by skyumori**, released under CC0 on [Freesound](https://freesound.org/people/skyumori/sounds/104534/). Other included audio retains its original rights and is included within the owner-confirmed redistribution scope.

## Large assets and Git LFS

Large binary source assets use Git LFS. Install Git LFS before cloning and run `git lfs pull` in the repository to retrieve the complete files. The selected source assets occupy approximately 8.3 GiB before Unity creates import caches. The [README](README.md#getting-started) provides checkout and editor setup instructions.

## Award photos

`docs/awards/certificate.jpg` and `docs/awards/trophies.jpg` are the owner-provided photos supplied for this project's award documentation. They document **Best Entertainment Award, Professional Category, 2025 Star Shark Cup Global VR Content Competition** and are not offered under the MIT software license. The separate project's trophy visible on the right of the second photo is not an award claim for Lockdown: Breakout.
