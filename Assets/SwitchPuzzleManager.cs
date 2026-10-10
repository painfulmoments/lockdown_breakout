using System.Collections.Generic;
using UnityEngine;

public class SwitchPuzzleManager : MonoBehaviour
{
    [Header("开关管理")]
    public List<Switch> switches = new List<Switch>(); // 所有开关的列表
    [Header("门对象")]
    public SpecialDoor targetDoor; // 需要解锁的特殊门

    [Header("调试信息")]
    [SerializeField] private int activatedSwitchesCount = 0; // 已激活开关数量
    private bool puzzleSolved = false; // 新增：谜题是否已解决

    void Start()
    {
        // 初始化所有开关
        foreach (Switch switchObj in switches)
        {
            switchObj.OnSwitchStateChanged += HandleSwitchStateChange;
            switchObj.Initialize();
        }
        
        // 初始化门状态
        if (targetDoor != null)
        {
            targetDoor.SetLocked(true);
        }
    }

    // 处理开关状态变化
    private void HandleSwitchStateChange(bool isOn)
    {
        // 如果谜题已经解决，不再处理开关变化
        if (puzzleSolved) return;
        
        activatedSwitchesCount = 0;
        
        // 计算当前激活的开关数量
        foreach (Switch switchObj in switches)
        {
            if (switchObj.IsOn)
            {
                activatedSwitchesCount++;
            }
        }
        
        Debug.Log($"激活开关数量: {activatedSwitchesCount}/{switches.Count}");
        
        // 更新门锁状态
        if (targetDoor != null)
        {
            bool allSwitchesOn = (activatedSwitchesCount == switches.Count);
            
            if (allSwitchesOn)
            {
                Debug.Log("所有开关已激活，门已解锁!");
                targetDoor.SetLocked(false); // 解锁门
                puzzleSolved = true; // 标记谜题已解决
                
                // 可选：禁用所有开关，防止玩家再次操作
                foreach (Switch switchObj in switches)
                {
                    // 如果Switch脚本有禁用交互的方法，可以在这里调用
                    // switchObj.DisableInteraction();
                }
            }
            else
            {
                targetDoor.SetLocked(true); // 确保门保持锁定状态
            }
        }
    }
    
    // 在编辑器中添加开关
    [ContextMenu("添加所有开关")]
    private void FindAllSwitches()
    {
        switches.Clear();
        Switch[] foundSwitches = FindObjectsOfType<Switch>();
        switches.AddRange(foundSwitches);
        Debug.Log($"已找到 {switches.Count} 个开关");
    }
    
    // 重置谜题（用于测试或游戏机制）
    public void ResetPuzzle()
    {
        puzzleSolved = false;
        foreach (Switch switchObj in switches)
        {
            switchObj.SetSwitchState(false, true);
        }
        if (targetDoor != null)
        {
            targetDoor.SetLocked(true);
            targetDoor.CloseDoor();
        }
    }
}