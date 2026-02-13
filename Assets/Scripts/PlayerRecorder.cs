using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerRecorder : MonoBehaviour
{
    public struct RecordingFrame
    {
        public Vector3 position;
        public Quaternion rotation;
        public RecordingFrame(Vector3 _position, Quaternion _rotation)
        {
            position = _position;
            rotation = _rotation;
        }
    }

    private List<RecordingFrame> _fullRecording = new List<RecordingFrame>();
    private void SetRecording(Vector3 position, Quaternion rotation)
    {
        RecordingFrame newFrame = new RecordingFrame(position, rotation);
        _fullRecording.Add(newFrame);
    }

    public static event Action<List<RecordingFrame>> FinishedRecording;

    private bool _isRecording = true;
    public bool IsRecording => _isRecording;

    private void OnEnable()
    {
        PlayerSpawnsManager.StartingNextPlayer += EmitRecording;
    }

    private void OnDisable()
    {
        PlayerSpawnsManager.StartingNextPlayer -= EmitRecording;
    }

    private void FixedUpdate()
    {
        if (!IsRecording) return;

        SetRecording(transform.position, transform.rotation);
    }

    private void EmitRecording()
    {
        Debug.Log("emitting recording");
        FinishedRecording?.Invoke(_fullRecording);
    }
}
