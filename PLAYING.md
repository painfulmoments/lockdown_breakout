# Playing Lockdown：Breakout

Lockdown is a **Windows PC VR horror prototype**. You need a VR headset, tracked controllers and a PC capable of running both VR software and the game. The Windows executable does not install directly on a Quest headset.

[Back to the project](https://github.com/painfulmoments/lockdown_breakout) · [Download the Windows demo](https://github.com/painfulmoments/lockdown_breakout/releases/tag/v0.1.0-pcvr)

## What you need

- A Windows PC supported by your headset’s PC VR software, with a VR-capable GPU.
- A VR headset and two tracked controllers. **Meta Quest 3** was the project’s target headset. Other OpenXR headsets require their own runtime and have not been confirmed by a connected-headset test for this release.
- An installed, active OpenXR runtime. For Quest over Link, use the Meta Link PC app, called **Meta Horizon Link** or **Meta Quest Link** depending on the installed version.
- A working PC-to-headset connection: a suitable USB cable, or an already configured wireless Link connection.
- The **entire extracted Windows demo folder**, including the `.exe`, its matching `_Data` folder, `UnityPlayer.dll` and any other files supplied in the archive.

Unity Editor, Git and Git LFS are only needed for working with the source project. They are not needed to run a complete packaged build.

## First launch with Quest Link

1. Open the [Windows PC VR release](https://github.com/painfulmoments/lockdown_breakout/releases/tag/v0.1.0-pcvr). Download **all** files named `LockdownBreakOut-Windows-x64-v0.1.0.zip.001`, `.002`, and any later numbered parts, plus **`Join_Demo.cmd`**, into one folder. Do not choose GitHub’s automatically generated **Source code** archive.
2. Double-click **`Join_Demo.cmd`**. It combines the numbered parts into `LockdownBreakOut-Windows-x64-v0.1.0.zip` using Windows' built-in file-copy command. Right-click the complete ZIP and select **Extract All**. Allow about **20 GB of free space** while retaining the parts, combined ZIP and extracted game. Do not run the `.exe` from inside the ZIP or move it away from its accompanying files.
3. Open the Meta Link PC app. Add or select your headset and establish its connection to the PC.
4. In the PC app, open **Settings → General → OpenXR Runtime** and make Meta Link the active OpenXR runtime. If the activation option is grayed out because Meta Link is already active, leave it as it is.
5. Put on the headset, enable **Quest Link**, and enter the Link session. Confirm that the headset is connected and the controllers are tracked before starting the game.
6. From the PC desktop, open the extracted `LockdownBreakOut-Windows-x64` folder and double-click **`LockdownBreakOut.exe`**. You may use Link’s desktop view to launch it while wearing the headset.
7. Return to the headset view and use the controllers to explore the level.

The order matters: **enter the PC VR Link session first, then launch the game**. If you start the game before OpenXR and the headset are available, close it, connect to Link and launch it again.

For the current Link setup and runtime terminology, see Meta’s official [Use Link for App Development](https://developers.meta.com/vr/documentation/unity/unity-link/) guide. The same PC VR connection is used when running a standalone Windows app; the game does not require the Unity Editor.

## Controls

The game uses **tracked controllers with animated virtual hands**.

| Action | Controller interaction |
| --- | --- |
| Look around | Turn your head; the view follows headset tracking. |
| Move through the level | Use the **left thumbstick**. |
| Turn | Use the **right thumbstick** for continuous turning. |
| Pick up and hold an interactable object | Bring a controller near the object and use the **grip / Select** input. |
| Use the slowing weapon | Pick up the weapon, aim it and hold the **trigger / Activate** input. Release the trigger to stop firing. |
| Operate a key-controlled door | Bring the appropriate key into the door’s interaction area, then press a controller **trigger**. An unlocked door can be toggled when your hand is within range. |
| Operate a puzzle switch | Bring the controller to the switch and use the **grip / Select** input. |
| Exit the application | Return to the Windows desktop and close the game window, or use **Alt + F4**. |

These controls are based on the supplied input action asset and gameplay scripts. The release notes record the validation performed for this build.

## The objective

Explore the lower containment spaces and the living area above, solve environmental puzzles and find an escape route. Watch for patrols and pursuit. Hiding spaces and the slowing gun offer ways to respond to enemies. A return route to the basement can let you recover a gun you passed earlier.

Some scene messages may be in Chinese. On a key-controlled door, **“按下扳机开门”** means “Press the trigger to open the door”; **“按下扳机开门/关门”** means “Press the trigger to open/close the door.”

## Troubleshooting

| What happens | What to check |
| --- | --- |
| The game opens on the monitor but does not appear correctly in the headset | Close the game. Confirm that the headset is inside an active Link session, Meta Link is the active OpenXR runtime, and the controllers are tracked; then relaunch. The desktop window can be a mirror of the VR view. |
| Controllers are missing or do not respond | Check controller tracking in the Link environment before launching the game. Reconnect the controllers and restart the game after the connection is working. |
| Unity reports a missing `_Data` folder or `UnityPlayer.dll` | Extract the entire archive again. Keep all supplied files together rather than copying only the `.exe`. |
| The download is a Unity project rather than an executable | The GitHub **Source code** download is for developers. Use the attached Windows game package in Releases. |
| A door does not open | Check whether it is locked, bring the matching key into the door’s interaction area, keep a tracked hand near the door and press the trigger. Puzzle-controlled doors require their linked switches. |
| A wall switch does not react to the trigger | The switch uses XR **Select**. With the supplied standard bindings, try the grip input while the controller is targeting the switch. |
| The headset connection stutters | Verify that the PC meets the Link requirements and test the Link connection itself. A working connection in the PC VR environment is required before assessing game performance. |

To report a problem, include the release version, headset model, Windows version, GPU, active OpenXR runtime and what happened. The current build retains the original project identity, so its Unity player log is at `%USERPROFILE%\AppData\LocalLow\DefaultCompany\Ghost2.1\Player.log`.

## Validation scope

The source project previously passed package restoration, script compilation, main-scene loading and selected automated Play Mode interaction checks. Those checks did not use an active headset. Build success, desktop startup and a complete headset playthrough are separate checks; consult the actual release notes and [validation record](docs/VALIDATION.md) for what has been completed.

The prototype includes darkness, pursuit, sudden sounds and artificial locomotion. Stop the session if you feel unwell.
