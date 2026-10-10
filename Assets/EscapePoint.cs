using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EscapePoint : MonoBehaviour
{
    public Text messageText; // UIÎÄ±¾

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NewPlayerController newplayerController = other.GetComponent<NewPlayerController>();
            if (newplayerController != null)
            {
                newplayerController.StartCoroutine(ShowMessage("Congratulations! You survived!", 2f));
            }
        }
    }

    private IEnumerator ShowMessage(string message, float delay)
    {
        if (messageText != null)
        {
            messageText.text = message;
            yield return new WaitForSeconds(delay);
            messageText.text = "";
        }
        else
        {
            Debug.LogError("MessageText is not assigned!");
        }
    }
}
