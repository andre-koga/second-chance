using UnityEngine;
using System;

public class PlayerSpawnsManager : MonoBehaviour
{
    [Serializable]
    public struct SpawnTargetPair
    {
        public GameObject Spawn;
        public GameObject Target;
        public readonly TargetManager TargetManager => Target.GetComponent<TargetManager>();
    }

    [Header("Spawn / Target Pairs")]
    [SerializeField] private SpawnTargetPair[] spawnTargetPairs;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;

    public static event Action StartingNextPlayer;
    public static event Action WonLevel;

    private int _currentPair = 0;
    public int CurrentPairIndex
    {
        get => _currentPair;
        set
        {
            _currentPair += 1;
            if (_currentPair >= spawnTargetPairs.Length)
            {
                // Reached the end. For now we just loop back endlessly.
                Debug.Log("This is the end!");

                _currentPair = 0;
                WonLevel?.Invoke();
            }
        }
    }
    public GameObject GetCurrentSpawn => spawnTargetPairs[_currentPair].Spawn;
    public GameObject GetCurrentTarget => spawnTargetPairs[_currentPair].Target;

    private GameObject currentInstance;


    private void Start()
    {
        // Literally only gets called once so I am putting it here
        for (int i = 0; i < spawnTargetPairs.Length; i++)
        {
            spawnTargetPairs[i].TargetManager.TargetIndex = i;
        }

        StartNewPlayer();
    }

    private void OnEnable()
    {
        TargetManager.ReachedTarget += ReachedTarget;
        CloneController.TouchedPlayer += StartNewPlayer;
    }

    private void OnDisable()
    {
        TargetManager.ReachedTarget -= ReachedTarget;
        CloneController.TouchedPlayer -= StartNewPlayer;
    }

    private void ReachedTarget(int targetIndex)
    {
        if (targetIndex == CurrentPairIndex)
        {
            // Reached the right target. Going to the next one.
            // Also the += 1 is useless
            StartingNextPlayer?.Invoke();

            CurrentPairIndex += 1;
            StartNewPlayer();
        }
    }

    private void StartNewPlayer()
    {
        // Check previous player is still there
        if (currentInstance != null)
        {
            Destroy(currentInstance);
        }

        // Debug.Log(GetCurrentSpawn.transform.position);
        currentInstance = Instantiate(playerPrefab, GetCurrentSpawn.transform.position, Quaternion.identity, transform);
        currentInstance.transform.position = GetCurrentSpawn.transform.position;
    }

    // ==========================================================
    private void OnDrawGizmos()
    {
        for (int i = 0; i < spawnTargetPairs.Length; i++)
        {
            // Ignore malformed pairs
            if (spawnTargetPairs[i].Spawn == null || spawnTargetPairs[i].Target == null) continue;

            Color randomColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
            Gizmos.color = randomColor;
            Gizmos.DrawLine(spawnTargetPairs[i].Spawn.transform.position, spawnTargetPairs[i].Target.transform.position);
        }
    }
}
