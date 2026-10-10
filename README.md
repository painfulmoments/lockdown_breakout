# Lockdown: Breakout

**An award-winning VR horror escape game built with Unity and OpenXR.**

*Chinese title: 禁闭：突围*

Lockdown: Breakout places you inside a biological research facility controlled by a rogue scientist. After a containment failure, hostile experimental creatures roam the building. Search for keys, solve environmental puzzles, operate doors and switches, and evade pursuing enemies to find a way out.

The project was created by **Team XJTLU Joy Over There (西浦彼间乐)** with support from the Virtual Engineering Centre at Xi'an Jiaotong-Liverpool University. Its design combines controller-driven virtual hands, interactive escape puzzles, and custom NavMesh enemy behavior to create a tense, embodied VR experience.

This repository includes the original gameplay code, the main escape scene, and its selected models, textures, audio, animation, lighting, and VR interaction dependencies. Asset packs are trimmed to the content needed by the shipped scenes. The team's original code uses the MIT License; included third-party content retains its own terms and notices, with redistribution permissions confirmed by the project owner. See [Third-party assets and release scope](THIRD_PARTY.md).

## Award

**Best Entertainment Award, Professional Category, 2025 Star Shark Cup Global VR Content Competition**

Awarded to **Lockdown: Breakout (禁闭：突围)** and **Team XJTLU Joy Over There**.

<table>
  <tr>
    <td align="center"><img src="docs/awards/certificate.jpg" width="420" alt="Certificate awarding Lockdown: Breakout the Best Entertainment Award in the Professional Category of the 2025 Star Shark Cup Global VR Content Competition"></td>
    <td align="center"><img src="docs/awards/trophies.jpg" width="420" alt="Award trophies, with the Lockdown: Breakout Best Entertainment Award trophy on the left"></td>
  </tr>
  <tr>
    <td align="center">Best Entertainment Award certificate</td>
    <td align="center">Lockdown: Breakout trophy on the left; the trophy on the right belongs to a separate project.</td>
  </tr>
</table>

## Gameplay

- **VR interaction:** animated virtual hands and controller input for grabbing objects and operating interactive elements.
- **Escape puzzles:** keys, locked doors, switches, and environmental triggers that support progression through the facility.
- **Enemy behavior:** custom NavMesh patrol and pursuit logic, including several creature behaviors.
- **Survival systems:** safe zones, death and restart handling, and an escape condition.
- **Environmental storytelling:** contrasting residential and laboratory spaces establish the setting of the original game.

The released project includes door and key interactions, switch puzzle management, enemy AI, safe zones, player handling, and game state logic, together with the saved escape level and its required assets.

## Technology

| Component | Version recorded in the project |
| --- | --- |
| Unity Editor | 2022.3.62f2 |
| OpenXR Plugin | 1.14.3 |
| XR Interaction Toolkit | 2.6.4 |
| XR Hands | 1.4.3 |
| AI Navigation | 1.1.7 |
| Universal Render Pipeline | 14.0.12 |

**VR / OpenXR:** the saved XR configuration uses the OpenXR loader for the Standalone target, with automatic XR initialization enabled. Use an OpenXR-capable headset and an active OpenXR runtime for PC VR testing. Meta Quest 3 was the target headset described in the original project materials; a standalone Android/Quest build has not been validated for this release.

## Getting started

1. Install [Git LFS](https://git-lfs.com/) alongside Git, then clone the repository and retrieve its large binary assets:

   ```bash
   git lfs install
   git clone https://github.com/painfulmoments/lockdown_breakout.git
   cd lockdown_breakout
   git lfs pull
   ```

   Git LFS is required for the full models, textures, audio, and other large assets. Use the Git LFS checkout above to obtain the playable project; a source archive containing LFS pointer files is not sufficient. The selected source content occupies approximately **8.3 GiB** before Unity generates its `Library` folder. Allow additional disk space for package downloads, imports, and builds.

2. Add the cloned folder to Unity Hub and open it with **Unity 2022.3.62f2**. Allow Unity to restore the packages listed in `Packages/manifest.json` and finish importing the assets. URP is set to **14.0.12** for this Unity version.
3. Open **`Assets/QA_HoldingCells/Demo/Demo_2.unity`**, the main escape level already selected in Build Settings. The required XR Interaction Toolkit Starter Assets and TextMesh Pro resources are included in the repository.
4. Connect your VR headset and select its active OpenXR runtime. In **Project Settings > XR Plug-in Management**, review the Standalone OpenXR configuration for your headset. **Initialize XR on Startup** is already enabled in the saved project.
5. With the main scene open and imports complete, enter **Play Mode** for PC VR. To create a desktop build, use the Standalone target and the main scene listed in Build Settings.

The supported starting scene for this release is `Demo_2.unity`. The editor opens this scene automatically when a fresh project has no saved scene. The exact selected asset paths are listed in [ASSET_MANIFEST.txt](docs/ASSET_MANIFEST.txt).

**Validation status:** a clean copy successfully restored packages, imported assets, compiled scripts, and opened the main level in Unity 2022.3.62f2. The final automated check ran approximately 20 seconds in Play Mode with zero errors and no missing scene scripts. It also checked eight switch positions, a cabinet door remaining open, and the escape message. Standalone OpenXR settings and initialization on startup were verified. Some imported collider warnings remain. A connected-headset playtest and a desktop or Quest build have not been performed; see [Validation details](docs/VALIDATION.md).

## Team

**Team XJTLU Joy Over There (西浦彼间乐)** brings together programming, environment art, and interaction design at Xi'an Jiaotong-Liverpool University's Virtual Engineering Centre.

| Contributor | Role |
| --- | --- |
| Binwei Lei (雷滨玮) | Technical lead, core gameplay and enemy AI, testing and optimization |
| Yibang Zhao (赵翊邦) | Art lead, environment planning and scene construction |
| Zeren Tao (陶择仁) | Experience design, level flow, puzzles and interaction design |
| Qilei Sun (孙齐蕾) | Academic guidance |

## License

The team's original code and authored gameplay configuration included in this repository are available under the [MIT License](LICENSE).

Included third-party assets are published based on redistribution permissions confirmed by the project owner and retain their original terms and notices. The MIT License does not relicense them. Unity packages and Unity sample-derived portions retain their respective licenses; notices are preserved in [docs/licenses](docs/licenses), including the [Unity Companion License](docs/licenses/Unity-Companion-License.md). Liberation Sans retains the SIL Open Font License. Award photos document the achievement and are excluded from the software license. See [THIRD_PARTY.md](THIRD_PARTY.md) for the release boundaries.
