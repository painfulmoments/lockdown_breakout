using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Switch : MonoBehaviour
{
    [Header("开关设置")]
    public bool isOn = false; // 初始状态
    public Transform buttonTransform; // Button_C的Transform组件
    public float pressedOffset = -0.05f; // 按钮按下的Z轴偏移量
    public float animationDuration = 0.3f; // 动画持续时间

    [Header("事件")]
    public UnityEvent onSwitchOn; // 开启事件
    public UnityEvent onSwitchOff; // 关闭事件
    
    // 状态变化委托
    public delegate void SwitchStateChangedHandler(bool isOn);
    public event SwitchStateChangedHandler OnSwitchStateChanged;
    
    // 属性：获取当前状态
    public bool IsOn { get; private set; }
    
    private Vector3 buttonOriginalPosition;
    private XRSimpleInteractable interactable;
    
    private void Awake()
    {
        // 确保设置了按钮变换组件
        if (buttonTransform == null)
        {
            // 尝试自动查找Button_C
            buttonTransform = transform.Find("Button_C");
            if (buttonTransform == null)
            {
                Debug.LogError("未找到按钮变换组件，请手动分配Button_C变换组件");
                return;
            }
        }
        
        buttonOriginalPosition = buttonTransform.localPosition;

    }

    private void Start()
    {
        if (buttonTransform == null) return;
        
        // 添加或获取XR交互组件
        interactable = buttonTransform.gameObject.GetComponent<XRSimpleInteractable>();
        if (interactable == null)
        {
            interactable = buttonTransform.gameObject.AddComponent<XRSimpleInteractable>();
        }
        
        // 注册交互事件
        interactable.selectEntered.AddListener(OnButtonPressed);
        
        Initialize();
    }
    
    // 初始化开关状态
    public void Initialize()
    {
        SetSwitchState(isOn, false);
    }
    
    // 设置开关状态
    public void SetSwitchState(bool state, bool animate = true)
    {
        IsOn = state;
        
        // 设置按钮位置
        Vector3 targetPosition = buttonOriginalPosition;
        if (state)
        {
            targetPosition.z += pressedOffset;
        }
        
        if (animate)
        {
            StartCoroutine(AnimateButton(targetPosition));
        }
        else
        {
            buttonTransform.localPosition = targetPosition;
        }
        
        // 触发事件
        if (state)
        {
            onSwitchOn.Invoke();
        }
        else
        {
            onSwitchOff.Invoke();
        }
        
        // 通知状态变化
        OnSwitchStateChanged?.Invoke(state);
    }
    
    // 切换开关状态
    public void ToggleSwitch()
    {
        SetSwitchState(!IsOn);
    }
    
    // 按钮按下事件处理
    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        ToggleSwitch();
    }
    
    // 按钮动画协程
    private System.Collections.IEnumerator AnimateButton(Vector3 targetPosition)
    {
        Vector3 startPos = buttonTransform.localPosition;
        float elapsed = 0;
        
        while (elapsed < animationDuration)
        {
            buttonTransform.localPosition = Vector3.Lerp(startPos, targetPosition, elapsed / animationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        buttonTransform.localPosition = targetPosition;
    }
    
    // 清理事件监听
    private void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnButtonPressed);
        }
    }
}