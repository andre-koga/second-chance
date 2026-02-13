using UnityEngine;
using System;

public class TargetManager : MonoBehaviour
{
    private int _targetIndex = -1;
    public int TargetIndex
    {
        get => _targetIndex;
        set => _targetIndex = value;
    }

    public static event Action<int> ReachedTarget;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger by: " + other.name);
        if (TargetIndex == -1)
        {
            Debug.LogWarning("This Target hasn't been properly indexed!");
            return;
        }

        Debug.Log("invoking this right?");
        ReachedTarget?.Invoke(TargetIndex);
    }
}
