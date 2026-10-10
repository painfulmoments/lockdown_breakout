using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GateDoor : MonoBehaviour
{
    public Animator leftDoorAnimator;
    public Animator rightDoorAnimator;

    public string leftDoorOpenAnimationName = "LeftOpen";
    public string leftDoorCloseAnimationName = "LeftClose";
    public string rightDoorOpenAnimationName = "RightOpen";
    public string rightDoorCloseAnimationName = "RightClose";

    public Collider RoomDoorTrigger;
    public Text messageText;
    public AudioSource audioSource;
    public AudioClip openDoorClip;
    public AudioClip closeDoorClip;
    public AudioClip lockDoorClip;

    public bool isDoorOpen = false;
    public bool playerInRange = false;

    [Header("Input")]
    public InputActionReference leftTriggerButton;   // 左手扳机
    public InputActionReference rightTriggerButton;  // 右手扳机

    private bool doorUnlocked = false;
    private GameObject currentRoomKey = null;

    public void Start()
    {
        // 暂停动画
        leftDoorAnimator.speed = 0f;
        rightDoorAnimator.speed = 0f;
        isDoorOpen = false;

        // 触发器检测
        TriggerDetector triggerDetector = RoomDoorTrigger.gameObject.AddComponent<TriggerDetector>();
        triggerDetector.OnPlayerEnter = OnTriggerEnter;
        triggerDetector.OnPlayerExit = OnTriggerExit;
    }

    public void Update()
    {
        // 检测左右手任意扳机是否按下
        bool triggerPressed = leftTriggerButton.action.WasPerformedThisFrame()
                           || rightTriggerButton.action.WasPerformedThisFrame();

        // 有钥匙 + 扳机按下 -> 首次解锁
        if (currentRoomKey != null && triggerPressed)
        {
            currentRoomKey.SetActive(false);
            currentRoomKey = null;
            doorUnlocked = true;

            leftDoorAnimator.speed = 1f;
            rightDoorAnimator.speed = 1f;

            ToggleDoor();
        }
        // 已解锁 -> 正常开关门
        else if (doorUnlocked && playerInRange && triggerPressed)
        {
            ToggleDoor();
        }
        // 没解锁 + 按扳机 -> 播放锁门音效
        else if (!doorUnlocked && playerInRange && triggerPressed)
        {
            PlaySound(lockDoorClip);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GateKey") && !doorUnlocked)
        {
            currentRoomKey = other.gameObject;
        }
        else if (other.CompareTag("Left Hand") || other.CompareTag("Right Hand"))
        {
            playerInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentRoomKey)
        {
            currentRoomKey = null;
        }
        else if (other.CompareTag("Left Hand") || other.CompareTag("Right Hand"))
        {
            playerInRange = false;
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
        leftDoorAnimator.Play(leftDoorCloseAnimationName, 0, 1f);
        rightDoorAnimator.Play(rightDoorCloseAnimationName, 0, 1f);
        leftDoorAnimator.Update(0);
        rightDoorAnimator.Update(0);
        isDoorOpen = false;
    }
}