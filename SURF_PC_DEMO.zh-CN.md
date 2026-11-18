# SURF 2024 旧版 Windows PC 试玩

**这是早期 SURF 项目的键鼠探索版本，程序标题为 Escape the Villa。** 它在 Windows x64 电脑上运行，试玩无需 VR 头显、跟踪手柄、Unity Editor、Git 或 Git LFS。

[下载旧版试玩](https://github.com/painfulmoments/lockdown_breakout/releases/tag/surf2024-legacy-pc-playable-2026-11-19) · [English guide](SURF_PC_DEMO.md) · [当前 Lockdown PCVR 说明](PLAYING.zh-CN.md) · [项目主页](README.zh-CN.md)

本包是项目所有者提供的历史二进制，使用 **Unity 2023.2.20f1c1** 构建，本次没有从当前 Lockdown 源码重新打包。它从开场进入一个 `demo` 场景，不能据此认为包内提供了 SURF 研究中全部三个实验条件。

## 下载与首次启动

| 发布附件 | 大小／用途 |
| --- | --- |
| [SURF-2024-Legacy-PC-Demo.zip](https://github.com/painfulmoments/lockdown_breakout/releases/download/surf2024-legacy-pc-playable-2026-11-19/SURF-2024-Legacy-PC-Demo.zip) | **1.96 GiB**（2,101,800,847 bytes）；原 ZIP 内容保持不变，仅使用便于下载识别的附件名称 |
| `SURF-PC-PLAYING.txt` | 中英文启动说明与已知限制 |
| `SHA256SUMS.txt` | ZIP 与启动说明的 SHA-256 校验值 |

1. 从上述发布页下载 **`SURF-2024-Legacy-PC-Demo.zip`**。它是**单个完整 ZIP**，无需分卷拼接；GitHub 自动生成的 **Source code** 归档不是这个可执行试玩包。
2. 右键 ZIP，选择 Windows **“全部解压缩 / Extract All”**。保留下载包和解压文件期间，建议准备至少 **10 GB 空闲空间**。解压后约 **4.75 GiB**（5,105,106,810 bytes）。
3. 打开解压后的 **`escape the villa`** 文件夹。保留其全部内容，尤其是 `UnityPlayer.dll`、`escape the villa_Data` 和 `MonoBleedingEdge`。
4. 双击 **`escape the villa.exe`**。窗口会保留旧程序内部标题 **`3Djianmo`**。
5. 在 **Escape the Villa / START GAME** 开场画面，按**任意键**或**鼠标左键**。程序配置了两秒淡出后进入 `demo` 场景；首次加载可能更久。进入后用键盘和鼠标探索。

## 操作

| 操作 | 按键 |
| --- | --- |
| 移动 | **WASD** 或**方向键** |
| 观察 | **鼠标** |
| 奔跑 | 按住**左 Shift** |
| 蹲下 | 按住**左 Ctrl** |
| 拾取钥匙／操作附近的门 | 在交互范围内按 **E** |
| 开关手电 | **F** |
| 暂停／继续 | **Esc** |
| 从暂停状态退出 | 先 **Esc**，再 **Enter** |
| 关闭 Windows 游戏窗口 | 必要时使用 **Alt+F4** |

按键已结合旧包程序集和实际启用的场景组件核对，并已用键盘验证从开场进入关卡。暂停与退出按键来自静态核查；不要依赖此历史包的鼠标暂停菜单按钮。场景中的 Jump 组件未启用，因此没有将空格列为跳跃键。

## 已完成检查与已知限制

- **归档：** 197 个文件全部通过 CRC，并完成安全解压；Windows x64 Unity Player、Mono 运行时与游戏数据齐全。没有重压或修改原 ZIP 的内容。
- **桌面启动：** 20.12 秒窗口化检查成功初始化 Unity、Direct3D 11、Mono 与输入系统；另一次交互检查显示开场、接受启动按键并渲染走廊场景，未使用头显。
- **进入关卡后观察到的旧版问题：** `PauseMenu.Start()` 报 `NullReferenceException`，名为 `Am` 的对象有缺失脚本，一个 agent 报告没有有效 NavMesh，另有碰撞体和 Animator 警告。场景仍能显示，但菜单或敌人行为可能受影响。本次未修补历史二进制。
- **验证范围：** 尚未验证完整逃脱通关、每一项交互或全部敌人遭遇。本包作为历史预览保留，不宣称零错误或成品质量；当前 Lockdown PCVR 包及其验证记录另行维护。

详细记录见[旧版验证说明](docs/SURF_PC_VALIDATION.md)。

## 完整性与常见问题

ZIP 的 SHA-256：

```text
d39811e9c1aec47a8c139dddbc08683f80297fae769ec88a3fcaf3ee0cffe550
```

下载完成后可用 `SHA256SUMS.txt` 核对。若解压失败，先比较 ZIP 字节数与 SHA-256，不一致时重新下载。若提示缺少 `UnityPlayer.dll` 或 `_Data`，请确认完整解压，且未把 EXE 从配套文件夹中单独移走。

若要体验后续的 VR 逃脱游戏，请使用 [Lockdown PCVR 中文指南](PLAYING.zh-CN.md)。旧 SURF 包不使用那个版本的分卷 ZIP、拼接助手或 Quest Link 启动流程。[SURF 录屏](https://github.com/painfulmoments/lockdown_breakout/releases/tag/surf2024-pc-demo-2026-11-18)仍保留为单独的视频入口。

历史程序及其捆绑第三方内容保留原有权利；仓库根目录 MIT 许可不会自动重新授权整个旧版运行包，详见[发布范围与声明](THIRD_PARTY.md)。
