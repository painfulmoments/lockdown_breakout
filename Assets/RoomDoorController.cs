using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class RoomDoorController : MonoBehaviour
{
    [Header("Door Animators")]
    public Animator leftDoorAnimator;
    public Animator rightDoorAnimator;

    [Header("Animation Names")]
    public string leftDoorOpenAnimationName = "LeftOpen";
    public string leftDoorCloseAnimationName = "LeftClose";
    public string rightDoorOpenAnimationName = "RightOpen";
    public string rightDoorCloseAnimationName = "RightClose";

    [Header("Trigger & UI")]
    public Collider RoomDoorTrigger;
    public Text messageText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openDoorClip;
    public AudioClip closeDoorClip;
    public AudioClip lockDoorClip;

    [Header("Input")]
    public InputActionReference leftTriggerButton;
    public InputActionReference rightTriggerButton;

    public bool isDoorOpen = false;
    public bool doorUnlocked = false;  // 门是否解锁
    private bool handInRange = false;   // 玩家手是否在触发范围
    private GameObject currentRoomKey = null; // 当前触发器内的钥匙
    private bool playerHoldingKey = false;    // 玩家是否抓着钥匙

    public void Start()
    {
        SetDoorClosedState();

        // 添加触发器检测组件
        TriggerDetector triggerDetector = RoomDoorTrigger.gameObject.AddComponent<TriggerDetector>();
        triggerDetector.OnPlayerEnter = OnTriggerEnter;
        triggerDetector.OnPlayerExit = OnTriggerExit;
    }

    public void Update()
    {
        // 持钥匙在门口按下扳机 → 解锁 + 开门
        bool triggerPressed = leftTriggerButton.action.WasPerformedThisFrame() 
                              || rightTriggerButton.action.WasPerformedThisFrame();

        if (!doorUnlocked && (currentRoomKey != null || playerHoldingKey) && triggerPressed)
        {
            Debug.Log("钥匙解锁并开门");
            if (currentRoomKey != null)
            {
                currentRoomKey.SetActive(false);
                currentRoomKey = null;
            }
            doorUnlocked = true;
            ToggleDoor();
            HideMessage();
            return;
        }

        if (doorUnlocked && handInRange && triggerPressed)
        {
            ToggleDoor();
            HideMessage();
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        // 钥匙进入触发器
        if (other.CompareTag("RoomKey") && !doorUnlocked)
        {
            currentRoomKey = other.gameObject;
            ShowMessage("按下扳机开门");
        }
        // 已解锁 → 检测手进入
        else if (doorUnlocked && (other.CompareTag("Left Hand") || other.CompareTag("Right Hand")))
        {
            handInRange = true;
            ShowMessage("按下扳机开门/关门");
        }
        // 检测手抓着钥匙（比如 VR 抓取组件有 Hand tag）
        else if (!doorUnlocked && (other.CompareTag("Left Hand") || other.CompareTag("Right Hand")))
        {
            handInRange = true;
            if (playerHoldingKey)
                ShowMessage("按下扳机开门");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentRoomKey)
        {
            currentRoomKey = null;
            HideMessage();
        }
        else if (other.CompareTag("Left Hand") || other.CompareTag("Right Hand"))
        {
            handInRange = false;
            HideMessage();
        }
    }

    public void ToggleDoor()
    {
        if (isDoorOpen)
        {
            leftDoorAnimator.Play(leftDoorCloseAnimationName);
            rightDoorAnimator.Play(rightDoorCloseAnimationName);
            PlaySound(closeDoorClip);
        }
        else
        {
            leftDoorAnimator.Play(leftDoorOpenAnimationName);
            rightDoorAnimator.Play(rightDoorOpenAnimationName);
            PlaySound(openDoorClip);
        }
        isDoorOpen = !isDoorOpen;
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void SetDoorClosedState()
    {
        leftDoorAnimator.Play(leftDoorCloseAnimationName);
        rightDoorAnimator.Play(rightDoorCloseAnimationName);
        isDoorOpen = false;
    }

    private void ShowMessage(string msg)
    {
        if (messageText != null)
            messageText.text = msg;
    }

    private void HideMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }

    // 供 VR 抓取系统调用（比如 XR Grab Interactable 的事件）
    public void SetHoldingKey(bool holding)
    {
        playerHoldingKey = holding;
        if (holding && handInRange && !doorUnlocked)
        {
            ShowMessage("按下扳机开门");
        }
        else if (!holding && !doorUnlocked)
        {
            HideMessage();
        }
    }
}
