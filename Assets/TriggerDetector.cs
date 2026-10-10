using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public System.Action<Collider> OnPlayerEnter;
    public System.Action<Collider> OnPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        OnPlayerEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnPlayerExit?.Invoke(other);
    }
}
