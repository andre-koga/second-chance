using System.Collections.Generic;
using UnityEngine;

public class ClonesManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject clonePrefab;

    private List<GameObject> clones = new List<GameObject>();

    private void OnEnable()
    {
        PlayerRecorder.FinishedRecording += StartNewClone;
    }

    private void OnDisable()
    {
        PlayerRecorder.FinishedRecording -= StartNewClone;
    }

    private void StartNewClone(List<PlayerRecorder.RecordingFrame> recording)
    {
        Debug.Log("yo");

        GameObject cloneInstance = Instantiate(clonePrefab, recording[0].position, recording[0].rotation, transform);
        CloneController cc = cloneInstance.GetComponent<CloneController>();
        cc.Recording = recording;

        clones.Add(cloneInstance);
    }
}
