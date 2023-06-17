using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : Singleton<PlayerManager>
{

    [SerializeField]
    PlayerController[] _playerControllers;

    public PlayerController[] PlayerControllers => _playerControllers;

    int _currentPlayerIndex = 0;

    public UnityEvent<Vector3> OnOneTileMovedEvent;

    public UnityEvent OnPathEndedEvent;

    public int FieldsDoneThisTurn => GetCurrentPlayerController().TilesDoneThisTurn;

    // Start is called before the first frame update
    void Start()
    {
        _playerControllers.ForEach(pc => pc.OnOneTileMoved.AddListener(OnPlayerOneTileMoved));
        _playerControllers.ForEach(pc => pc.OnPathEndedEvent.AddListener(OnPlayerPathEnded));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentPlayer(int index)
    {
        _currentPlayerIndex = index;
    }

    public PlayerController GetCurrentPlayerController()
    {
        return _playerControllers[_currentPlayerIndex];
    }

    public void OnPlayerOneTileMoved()
    {
        OnOneTileMovedEvent.Invoke(GetCurrentPlayerController().transform.position);
    }

    public void OnPlayerPathEnded()
    {
        OnPathEndedEvent.Invoke();
    }

    public void SetPathForCurrentPlayer(List<Vector3> path)
    {
        GetCurrentPlayerController().MoveOnPath(path);
    }

    public void SetMovingPlayer(bool state)
    {
        GetCurrentPlayerController().SetPause(state);
    }

    public void PrepareCurrentPlayer()
    {
        _playerControllers.ForEach(pc => pc.StopPulse());
        GetCurrentPlayerController().StartPulse();
    }
}
