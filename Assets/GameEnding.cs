using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    // 游戏胜利时图片淡入淡出的时间
    public float fadeDuration = 1f;
    // 胜利图片显示的时间
    public float displayDuration = 1f;
    // 游戏人物
    public GameObject Player;
    // 胜利画布背景
    public CanvasGroup ExitBK;
    // 失败画布背景
    public CanvasGroup FailBK;

    public NewPlayerController newplayerController; // 引用PlayerController

   //public EnemyChase enemyChase; // 引用EnemyChase

    public Transform player; // 玩家Transform

    // 游戏胜利时的bool值
    public static bool isExit = false;
    // 游戏失败时的bool值
    public static bool isFail = false;

    // 定义计时器用于图片的渐变完全显示
    public float timer = 0f;

    void Start()
    {
        // Reset game state variables when scene is loaded
        isExit = false;
        isFail = false;

        // Ensure the timer is reset
        timer = 0f;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            // 检测到玩家,游戏胜利
            isExit = true;
        }
    }

    // 游戏失败时的控制函数
    public void Caught()
    {
        isFail = true;
        Debug.Log("Player caught!");

        // 直接调用PlayerController中的Die方法
        newplayerController.Die();
    }

    // Update is called once per frame
    void Update()
    {
        // 游戏胜利
        if (isExit == true)
        {
            EndLevel(ExitBK, false);
        }
        // 游戏失败
        else if (isFail == true)
        {
            EndLevel(FailBK, true);
        }
    }

    // 游戏胜利或失败时的方法
    void EndLevel(CanvasGroup igCanvasGroup, bool doRestart)
    {
        // 玩家碰到触发器，计时器开始计时
        timer += Time.deltaTime;
        // 控制CanvasGroup的不透明度
        igCanvasGroup.alpha = timer / fadeDuration; // 0-1s是处于渐变

        // 游戏失败复活
        if (doRestart && timer >= fadeDuration + displayDuration)
        {
            isFail = false;
            timer = 0f;
            igCanvasGroup.alpha = 0f;
        }
        // 游戏胜利退出
        else if (!doRestart && timer >= fadeDuration + displayDuration)
        {
            Application.Quit();
        }
    }
}
