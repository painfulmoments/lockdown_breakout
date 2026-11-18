# Lockdown：Breakout 中文试玩说明

[项目主页](README.zh-CN.md) · [试玩下载](https://github.com/painfulmoments/lockdown_breakout/releases/tag/v0.1.0-pcvr) · [English](PLAYING.md)

Lockdown 是 **Windows PC VR 恐怖原型**。游戏运行在 Windows 电脑上，需要 VR 头显、两个跟踪手柄及可运行 PC VR 的电脑；Windows EXE 不会直接安装到 Quest 头显中。

## 1. 准备设备与空间

- 满足所用头显 PC VR 软件要求的 Windows 电脑，以及可运行 VR 的显卡。当前没有经过测定的最低硬件配置。
- VR 头显和两个跟踪手柄。**Meta Quest 3** 是项目目标设备；本包尚未完成连接物理头显后的全流程测试，其他 OpenXR 头显也未确认。
- 已安装并设为活动状态的 OpenXR Runtime。Quest 使用电脑端 **Meta Horizon Link / Meta Quest Link**，名称可能因已安装版本而异。
- 可用的电脑与头显连接：合适的 USB 线，或已经配置完成的无线 Link 连接。连接方式在本包中的实机表现仍待验证。
- 约 **20 GB 空闲磁盘空间**，用于同时保留分片、拼接后的 ZIP 和解压后的游戏。
- 完整解压出的 Windows 游戏文件夹，包含 EXE、对应 `_Data` 文件夹、`UnityPlayer.dll` 及其他随包文件。

运行试玩包不需要 Unity Editor、Git 或 Git LFS；这些工具只供处理源项目时使用。

## 2. 下载、拼接与解压

打开 [v0.1.0-pcvr 试玩 Release](https://github.com/painfulmoments/lockdown_breakout/releases/tag/v0.1.0-pcvr)，在 **Assets** 中下载以下文件到同一个文件夹：

- **`LockdownBreakOut-Windows-x64-v0.1.0.zip.001`**
- **`LockdownBreakOut-Windows-x64-v0.1.0.zip.002`**
- **`Join_Demo.cmd`**

建议同时下载该版本的 **`SHA256SUMS.txt`** 作为校验记录。GitHub 自动生成的 **“Source code (zip)”／“Source code (tar.gz)”不是可执行试玩包**。

1. 双击 **`Join_Demo.cmd`**。助手会核对两个分片的 SHA-256，再将它们拼接为完整 ZIP 并核对结果。已有的完整 ZIP 也会校验，不会仅因文件存在就跳过检查。
2. 等待成功提示。拼接过程使用临时 `.pending` 文件，通过校验后才形成 `LockdownBreakOut-Windows-x64-v0.1.0.zip`。校验不通过时按提示处理，不要继续解压损坏文件。
3. 右键完整 ZIP，选择 Windows **“全部解压缩 / Extract All”**。无需额外安装解压软件。
4. 打开解压后的 **`LockdownBreakOut-Windows-x64`** 文件夹，确认 **`LockdownBreakOut.exe`** 及配套文件仍在一起。先完成下一节的 Link 连接，再运行游戏。

**`.001` 和 `.002` 是同一个 ZIP 的编号分片，不是两个独立 ZIP。必须先拼接，再解压完整 ZIP。** 不要分别解压分片，不要在压缩包里直接运行 EXE，也不要只复制 EXE 而丢下资源文件夹和运行库。

## 3. 首次通过 Quest Link 启动

1. 在电脑打开 Meta Link 应用，添加或选择头显，建立电脑与头显的连接。
2. 在电脑端打开 **Settings → General → OpenXR Runtime**，将 Meta Link 设为活动 OpenXR Runtime。如果它已经处于活动状态，按钮可能呈灰色，无需重复设置。
3. 戴上头显，启用 **Quest Link** 并进入 Link 会话。先确认头显已连接、两个手柄均能被跟踪。
4. 在电脑桌面打开解压后的游戏文件夹，双击 **`LockdownBreakOut.exe`**。也可以在头显里的 Link 桌面视图中启动。
5. 返回头显画面，使用手柄探索关卡。

顺序是：**先进入可用的 Link 会话，再启动游戏。** 如果游戏在头显或 OpenXR 尚不可用时启动，请关闭游戏，完成 Link 连接后重新运行。

连接方式和运行时术语可参考 [Meta 官方 Link 开发连接说明](https://developers.meta.com/vr/documentation/unity/unity-link/)。这里同样使用电脑端 VR 连接，运行已打包的 Windows 游戏无需 Unity 编辑器。

## 4. 手柄操作

游戏使用带动画的虚拟手，输入来自跟踪手柄。

| 操作 | 手柄交互 |
| --- | --- |
| 观察环境 | 转动头部，视角跟随头显跟踪。 |
| 移动 | 使用 **左摇杆**。 |
| 转向 | 使用 **右摇杆**连续转向。 |
| 拾取并握住可交互物品 | 将手柄靠近物体，使用 **握持键 / Grip / Select**。 |
| 使用减速武器 | 拿起武器并瞄准，按住 **扳机 / Trigger / Activate**；松开扳机停止发射。 |
| 操作钥匙控制的门 | 将正确钥匙带入门的交互区域，再按手柄 **扳机**。门解锁后，手处于交互范围内即可切换开关。 |
| 操作谜题开关 | 将手柄靠近开关，使用 **握持键 / Grip / Select**。 |
| 退出 | 返回电脑桌面，关闭游戏窗口，或使用 **Alt+F4**。 |

操作说明依据当前输入动作资源与玩法脚本核对；本包的完整物理手柄验证仍待完成。

## 5. 试玩目标

探索下层收容空间和上层生活区域，解决环境谜题，寻找逃脱路线。留意敌人的巡逻与追逐；藏身点和减速枪提供应对方式。返回地下层的路线可以让你补取先前错过的武器。

部分场景提示为中文。钥匙门上的 **“按下扳机开门”** 表示按扳机开门；**“按下扳机开门/关门”** 表示按扳机切换门的状态。

## 6. 常见问题

| 现象 | 处理方法 |
| --- | --- |
| 助手提示缺少分片或校验失败 | 确认两个分片与助手来自同一 Release、文件名未改变且位于同一文件夹；重新下载提示损坏或缺失的文件后再试。 |
| 拼接中断或留下 `.pending` 文件 | 检查磁盘空间，按助手提示处理临时文件后重试；不要把未完成文件当成完整 ZIP 解压。 |
| 游戏在电脑显示器上打开，但头显里没有正确画面 | 关闭游戏，确认头显已进入活动 Link 会话、Meta Link 是活动 OpenXR Runtime、手柄可跟踪，再重新启动。桌面窗口可以只是 VR 画面的镜像。 |
| 手柄丢失或没有反应 | 先在 Link 环境确认手柄跟踪正常。恢复连接后再重新启动游戏。 |
| 缺少 `_Data` 文件夹或 `UnityPlayer.dll` | 完整解压整个 ZIP，保留所有随包文件，不要只运行复制出来的 EXE。 |
| 下载后是 Unity 工程而不是游戏程序 | 你选择了 GitHub 的 Source code 归档。请下载 Release 中的编号游戏分片。 |
| 门无法打开 | 检查是否锁定，将匹配钥匙带入门的交互区域，保持跟踪手在范围内，再按扳机。谜题控制的门需要先完成关联开关。 |
| 墙上开关不响应扳机 | 该开关使用 XR **Select**。用手柄指向或靠近开关后，尝试握持键。 |
| 连接卡顿 | 先检查电脑是否满足 Link 要求，并单独确认 Link 环境是否流畅。连接本身稳定后，再判断游戏性能。 |

反馈问题时请附上 Release 版本、头显型号、Windows 版本、显卡、当前活动的 OpenXR Runtime 和复现步骤。当前构建保留原项目标识，Unity 玩家日志位于：

```text
%USERPROFILE%\AppData\LocalLow\DefaultCompany\Ghost2.1\Player.log
```

## 7. 验证范围

源项目此前通过了包恢复、脚本编译、主场景加载及部分自动化 Play Mode 交互检查。Windows x64 试玩包已构建成功并完成短时桌面启动检查。

**这些检查均不代表已完成连接头显后的全流程试玩。** 头显跟踪、物理手柄操作、舒适度和完整逃脱过程仍待验证。构建成功、桌面启动和完整 VR 试玩是不同的检查，请分别参阅[本包构建记录](docs/BUILD_AND_VALIDATION.md)、[历史源项目验证](docs/VALIDATION.md)与 Release 说明。

原型包含黑暗环境、追逐、突然的声响和人工移动。感到不适时请停止体验。
