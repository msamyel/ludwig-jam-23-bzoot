using System;
using System.Collections;
using Bzoot;
using UnityEngine;

public class GameSceneEnvironment : MonoBehaviour
{
    public static GameSceneEnvironment Instance => _instance;
    static GameSceneEnvironment _instance;

    [Header("Environment")]
    public GameScenePhysicsValues Physics;

    [Header("Coots")]
    public CootsValues Coots;
    
    [Header("Play Area")]
    [SerializeField] BoxCollider2D _playArea;
    public Bounds PlayArea => _playArea.bounds;

    void Awake()
    {
        if (_instance)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    public void StartDelayedFunction(float delaySecs, Action func)
    {
        StartCoroutine(DelayedFunction(delaySecs, func));
    }

    IEnumerator DelayedFunction(float delaySecs, Action func)
    {
        yield return new WaitForSeconds(delaySecs);
        func.Invoke();
    }
}
