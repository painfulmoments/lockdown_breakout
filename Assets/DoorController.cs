using UnityEngine;
using System.Collections.Generic;

public class DoorController : MonoBehaviour
{
    [Header("开关设置")]
    public List<Switch> switches; // 按顺序分配的4个开关
    
    [Header("门设置")]
    public Transform doorTransform; // 门的Transform组件
    public Vector3 openRotation = new Vector3(0, 90, 0); // 门打开时的旋转角度
    public float openSpeed = 2.0f; // 门打开的速度
    
    private bool isOpen = false;
    private Quaternion targetRotation;
    
    private void Start()
    {
        // 确保所有开关都已分配
        if (switches == null || switches.Count != 4)
        {
            Debug.LogError("请分配4个开关到DoorController");
            return;
        }
        
        // 为每个开关注册状态变化事件
        for (int i = 0; i < switches.Count; i++)
        {
            // 使用匿名方法捕获索引
            int index = i;
            switches[i].OnSwitchStateChanged += (state) => OnAnySwitchStateChanged(index, state);
        }
        
        // 初始化门的初始旋转
        targetRotation = doorTransform.rotation;
    }
    
    private void Update()
    {
        // 平滑旋转门
        if (doorTransform.rotation != targetRotation)
        {
            doorTransform.rotation = Quaternion.Lerp(doorTransform.rotation, targetRotation, openSpeed * Time.deltaTime);
        }
    }
    
    // 任何开关状态变化时的回调
    private void OnAnySwitchStateChanged(int switchIndex, bool state)
    {
        // 如果门已经打开，不再检查
        if (isOpen) return;
        
        // 检查所有开关的状态是否符合要求
        CheckSwitches();
    }
    
    // 检查所有开关的状态
    private void CheckSwitches()
    {
        // 检查开关状态是否符合 on on off on 的顺序
        if (switches[0].IsOn && 
            switches[1].IsOn && 
            !switches[2].IsOn && 
            switches[3].IsOn)
        {
            OpenDoor();
        }
    }
    
    // 打开门
    private void OpenDoor()
    {
        isOpen = true;
        // 直接设置门的旋转，不使用动画
        doorTransform.rotation = Quaternion.Euler(openRotation);
        
        // 取消所有开关的事件订阅
        foreach (var sw in switches)
        {
            sw.OnSwitchStateChanged -= (state) => { }; // 使用空委托取消订阅
        }
        
        Debug.Log("密码正确！门已打开");
    }
    
    // 清理事件订阅
    private void OnDestroy()
    {
        if (switches != null)
        {
            foreach (var sw in switches)
            {
                if (sw != null)
                {
                    // 由于我们使用匿名方法订阅，这里需要特殊处理
                    // 或者可以在Switch类中添加一个取消所有订阅的方法
                }
            }
        }
    }
}