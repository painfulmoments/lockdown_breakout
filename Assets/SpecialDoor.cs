using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class SpecialDoor : MonoBehaviour
{
    [Header("门设置")]
    public bool isLocked = true;
    public float openAngle = -90f;
    public float openSpeed = 2f;
    public bool isOpen = false;
    public bool autoOpenWhenUnlocked = true; // 新增：解锁时自动开门
    
    [Header("事件")]
    public UnityEvent onDoorUnlocked;
    public UnityEvent onDoorOpened;
    public UnityEvent onDoorClosed;
    
    private Vector3 defaultRotation;
    private Quaternion targetRotation;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    
    // 调试信息
    private string debugStatus = "Initial";
    
    private void Start()
    {
        defaultRotation = transform.localEulerAngles;
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
        targetRotation = closedRotation;
        
        Debug.Log($"门初始化: 锁定={isLocked}, 初始旋转={defaultRotation}");
    }
    
    private void Update()
    {
        // 平滑旋转到目标角度
        if (Quaternion.Angle(transform.localRotation, targetRotation) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation, 
                targetRotation, 
                Time.deltaTime * openSpeed
            );
            debugStatus = "正在旋转";
        }
        else
        {
            transform.localRotation = targetRotation;
            if (isOpen && targetRotation == openRotation)
            {
                debugStatus = "已打开";
            }
            else if (!isOpen && targetRotation == closedRotation)
            {
                debugStatus = "已关闭";
            }
        }
    }
    
    // 设置门锁状态
    public void SetLocked(bool locked)
    {
        bool previousState = isLocked;
        isLocked = locked;
        
        Debug.Log($"门锁状态改变: {previousState} -> {isLocked}");
        
        if (!isLocked && previousState)
        {
            onDoorUnlocked.Invoke();
            Debug.Log("门已解锁!");
            
            // 新增：如果设置为解锁时自动开门，则调用开门方法
            if (autoOpenWhenUnlocked && !isOpen)
            {
                OpenDoor();
            }
        }
    }
    
    // 尝试开门
    public void TryOpenDoor()
    {
        Debug.Log($"尝试开门: 锁定={isLocked}, 已开={isOpen}");
        
        if (!isLocked)
        {
            if (!isOpen)
            {
                OpenDoor();
            }
            else
            {
                CloseDoor();
            }
        }
        else
        {
            Debug.Log("门被锁定，无法打开");
        }
    }
    
    // 开门
    public void OpenDoor()
    {
        if (isOpen) return;
        
        targetRotation = openRotation;
        isOpen = true;
        onDoorOpened.Invoke();
        
        Debug.Log("开门中...");
    }
    
    // 关门
    public void CloseDoor()
    {
        if (!isOpen) return;
        
        targetRotation = closedRotation;
        isOpen = false;
        onDoorClosed.Invoke();
        
        Debug.Log("关门中...");
    }
    
    // VR交互方法
    public void OnVRInteract()
    {
        Debug.Log($"门被交互 - 状态: {debugStatus}");
        TryOpenDoor();
    }
    
    // 在Inspector中显示调试信息
    private void OnGUI()
    {
        if (Time.time < 10) // 只在前10秒显示
        {
            GUI.Label(new Rect(10, 100, 300, 20), $"门状态: 锁定={isLocked}, 开启={isOpen}");
            GUI.Label(new Rect(10, 120, 300, 20), $"旋转状态: {debugStatus}");
        }
    }
}