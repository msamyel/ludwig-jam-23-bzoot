using Bzoot;
using UnityEngine;

public class GameSceneEnvironment : MonoBehaviour
{
    public static GameSceneEnvironment Instance => _instance;
    static GameSceneEnvironment _instance;

    [Header("Environment")]
    public GameScenePhysicsValues Physics;

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
}
