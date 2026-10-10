using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WardrobeDoor : MonoBehaviour
{
    [Header("门设置")]
    public float openAngle = 90f;
    public float openSpeed = 2f;
    
    [Header("交互设置")]
    public XRSimpleInteractable interactable;
    public AudioSource audioSource;
    public AudioClip openDoorClip;
    public AudioClip closeDoorClip;
    
    [Header("状态")]
    public bool isDoorOpen = false;
    
    // 自动检测的组件
    private Transform doorModelTransform;
    private Collider doorCollider;
    
    private Vector3 closedRotation;
    private Vector3 openRotation;
    private Vector3 targetRotation;
    
    void Start()
    {
        // 自动查找模型Transform
        FindDoorModel();
        
        // 自动查找碰撞器
        doorCollider = GetComponent<Collider>();
        if (doorCollider == null)
        {
            Debug.LogError("未找到碰撞器组件!");
        }
        
        // 保存门的关闭状态旋转
        closedRotation = doorModelTransform.localEulerAngles;
        
        // 计算门的打开状态旋转
        openRotation = new Vector3(
            closedRotation.x,
            closedRotation.y + openAngle,
            closedRotation.z
        );
        
        // 设置初始目标旋转
        targetRotation = closedRotation;
        
        // 如果没有指定交互组件，尝试获取
        if (interactable == null)
            interactable = GetComponent<XRSimpleInteractable>();
        
        // 注册交互事件
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnDoorInteracted);
        }
        else
        {
            Debug.LogError("未找到XRSimpleInteractable组件!");
        }
        
        Debug.Log($"门初始化完成: 模型={doorModelTransform.name}, 碰撞器={doorCollider?.name}");
    }
    
    // 自动查找门模型
    private void FindDoorModel()
    {
        // 首先检查自己是否有渲染器
        Renderer selfRenderer = GetComponent<Renderer>();
        if (selfRenderer != null)
        {
            doorModelTransform = transform;
            Debug.Log("使用自身作为门模型");
            return;
        }
        
        // 查找包含渲染器的子对象
        foreach (Transform child in transform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                doorModelTransform = child;
                Debug.Log($"找到门模型: {child.name}");
                return;
            }
        }
        
        // 如果还是没找到，使用第一个子对象
        if (transform.childCount > 0)
        {
            doorModelTransform = transform.GetChild(0);
            Debug.LogWarning($"未找到明确的门模型，使用第一个子对象: {doorModelTransform.name}");
        }
        else
        {
            // 最后使用自身
            doorModelTransform = transform;
            Debug.LogWarning("未找到子对象，使用自身作为门模型");
        }
    }
    
    void Update()
    {
        // 平滑旋转到目标角度
        doorModelTransform.localRotation = Quaternion.Slerp(
            doorModelTransform.localRotation,
            Quaternion.Euler(targetRotation),
            Time.deltaTime * openSpeed
        );
        
        // 同步更新碰撞器的旋转（如果需要）
        if (doorCollider != null && doorCollider.transform != doorModelTransform)
        {
            doorCollider.transform.rotation = doorModelTransform.rotation;
        }
    }
    
    // 当门被交互时调用
    private void OnDoorInteracted(SelectEnterEventArgs args)
    {
        ToggleDoor();
        
        // 添加触觉反馈
        XRBaseController controller = args.interactorObject.transform.GetComponent<XRBaseController>();
        if (controller != null)
        {
            controller.SendHapticImpulse(0.3f, 0.1f);
        }
    }
    
    // 切换门的状态
    public void ToggleDoor()
    {
        if (isDoorOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }
    
    // 开门
    public void OpenDoor()
    {
        if (isDoorOpen) return;
        
        targetRotation = openRotation;
        isDoorOpen = true;
        
        // 播放开门音效
        PlaySound(openDoorClip);
        
        Debug.Log("衣柜门打开");
    }
    
    // 关门
    public void CloseDoor()
    {
        if (!isDoorOpen) return;
        
        targetRotation = closedRotation;
        isDoorOpen = false;
        
        // 播放关门音效
        PlaySound(closeDoorClip);
        
        Debug.Log("衣柜门关闭");
    }
    
    // 播放音效
    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    
    // 强制设置门为关闭状态
    public void SetDoorClosedState()
    {
        doorModelTransform.localEulerAngles = closedRotation;
        targetRotation = closedRotation;
        isDoorOpen = false;
    }
    
    // 强制设置门为开启状态
    public void SetDoorOpenState()
    {
        doorModelTransform.localEulerAngles = openRotation;
        targetRotation = openRotation;
        isDoorOpen = true;
    }
    
    // 在编辑器中调试开门
    [ContextMenu("调试开门")]
    private void DebugOpenDoor()
    {
        OpenDoor();
    }
    
    // 在编辑器中调试关门
    [ContextMenu("调试关门")]
    private void DebugCloseDoor()
    {
        CloseDoor();
    }
    
    // 在Scene视图中绘制调试信息
    private void OnDrawGizmosSelected()
    {
        if (doorModelTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(doorModelTransform.position, Vector3.one * 0.1f);
            Gizmos.DrawLine(doorModelTransform.position, doorModelTransform.position + doorModelTransform.forward * 0.5f);
        }
    }
}