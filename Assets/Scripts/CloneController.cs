using System.Collections.Generic;
using UnityEngine;
using System;

public class CloneController : MonoBehaviour
{
    private List<PlayerRecorder.RecordingFrame> _recording = new List<PlayerRecorder.RecordingFrame>();
    public List<PlayerRecorder.RecordingFrame> Recording
    {
        get => _recording;
        set => _recording = value;
    }

    private int currentFrame = 0;
    private bool _isPlaying = true;
    public static event Action TouchedPlayer;

    private void FixedUpdate()
    {
        if (!_isPlaying) return;

        ExecuteFrame();

        // Loop
        currentFrame += 1;
        if (currentFrame >= Recording.Count) currentFrame = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Must be a player because of the way we defined the layer detections
        TouchedPlayer?.Invoke();
    }

    private void ExecuteFrame()
    {
        transform.position = Recording[currentFrame].position;
        transform.rotation = Recording[currentFrame].rotation;
    }
}
