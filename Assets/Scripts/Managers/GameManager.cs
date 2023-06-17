using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    int _currentPlayerIndex = 0;

    [SerializeField]
    MouseBehaviour _mouseBehaviour;

    bool[] _playersIgnoreWalls = new bool[2] { false, false };

    bool[] _playersDoubleRoll = new bool[2] { false, false };

    bool[] _playersMax3Move = new bool[2] { false, false };

    bool[] _playersMissTurn = new bool[2] { false, false };

    bool[] _playerBackToStartingPosition = new bool[2] { false, false };

    public UnityEvent<int> OnRolledDice;

    public UnityEvent OnActivateRoute;

    public UnityEvent OnNewTurnStart;

    public UnityEvent OnContinueMove;

    public UnityEvent<int> OnGameEnded;

    UnityEvent[] PendingEventsForPlayers = new UnityEvent[2];
    // Start is called before the first frame update
    void Start()
    {
        KeysManager.Instance.OnKeyPicked.AddListener(PlayerPickKey);
        PlayerManager.Instance.OnOneTileMovedEvent.AddListener(AfterOneTileMoved);
        PlayerManager.Instance.OnPathEndedEvent.AddListener(AfterPlayerPathEnded);
        _mouseBehaviour.SetStartPos(PlayerManager.Instance.PlayerControllers[0].transform.position);
        PlayerManager.Instance.PrepareCurrentPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePlayerTurn()
    {
        _currentPlayerIndex = (_currentPlayerIndex + 1) % 2;
        PlayerManager.Instance.SetCurrentPlayer(_currentPlayerIndex);
        PendingEventsForPlayers[_currentPlayerIndex]?.Invoke();
    }

    public void MoveActivePlayerOnRoute()
    {
        List<Vector3> routeToMove = _mouseBehaviour.FieldsCurrentlySelected;
        if (routeToMove.Count == 0) return;

        PlayerManager.Instance.SetPathForCurrentPlayer(routeToMove);
        OnActivateRoute.Invoke();
    }

    public void RollDice()
    {
        int maxRoll = 6;
        if (_playersMax3Move[_currentPlayerIndex]) maxRoll = 3;

        int rollResult = Random.Range(1, maxRoll+1);
        OnRolledDice.Invoke(rollResult);
        _mouseBehaviour.SetMaxCellsThisTurn(rollResult);
        _mouseBehaviour.SetCanTraverseWalls(_playersIgnoreWalls[_currentPlayerIndex]);
        _mouseBehaviour.SetStartPos(PlayerManager.Instance.PlayerControllers[_currentPlayerIndex].transform.position);
        _mouseBehaviour.SetCursorInteraction(true);
        
    }

    public void AfterOneTileMoved(Vector3 positionToCheck)
    {
        if(InteractionManager.Instance.CheckInteractable(positionToCheck))
        {

        }
    }

    public void AfterPlayerPathEnded()
    {
        if (PlayerManager.Instance.FieldsDoneThisTurn == _mouseBehaviour.MaxCellsDoneThisTurn)
        {
            _mouseBehaviour.SetCursorInteraction(false);
            EventsManager.Instance.CheckIfAnyEventsToInvoke();
            if (_playersDoubleRoll[_currentPlayerIndex])
            {
                _playersDoubleRoll[_currentPlayerIndex] = false;
                PrepareForTurn(false);
            }
            else if (_playerBackToStartingPosition[_currentPlayerIndex])
            {
                PrepareForTurn(true);
            }
            else
            {
                PrepareForTurn(true);
            }
            PlayerManager.Instance.PrepareCurrentPlayer();
        }
        else
        {
            _mouseBehaviour.SetCursorInteraction(true);
            _mouseBehaviour.SetMaxCellsThisTurn(_mouseBehaviour.MaxCellsDoneThisTurn - PlayerManager.Instance.FieldsDoneThisTurn);
            _mouseBehaviour.ClearPath();
            _mouseBehaviour.SetStartPos(PlayerManager.Instance.PlayerControllers[_currentPlayerIndex].transform.position);
            OnContinueMove.Invoke();
            PlayerManager.Instance.PrepareCurrentPlayer();
        }
    }

    public void PrepareForTurn(bool ShouldChangePlayer)
    {
        if (ShouldChangePlayer)
        {
            _playersDoubleRoll[_currentPlayerIndex] = false;
            _playersIgnoreWalls[_currentPlayerIndex] = false;
            _playersMax3Move[_currentPlayerIndex] = false;
            _playersMissTurn[_currentPlayerIndex] = false;
            _playerBackToStartingPosition[_currentPlayerIndex] = false;
            ChangePlayerTurn();
        }

        /// Write some code to prepare next turn
        if (_playersMissTurn[_currentPlayerIndex])
        {
            PrepareForTurn(true);
            return;
        }

        _mouseBehaviour.ClearPath();

        OnNewTurnStart.Invoke();
    }

    public void PlayerPickKey()
    {
        PlayerManager.Instance.PlayerControllers[_currentPlayerIndex].pickedKeys++;
        if (PlayerManager.Instance.PlayerControllers[_currentPlayerIndex].pickedKeys == 4)
        {
            Debug.Log("The winner is Player"+_currentPlayerIndex+1);
            OnGameEnded.Invoke(_currentPlayerIndex);
        }
    }

    public void EnableIgnoringWalls()
    {
        _playersIgnoreWalls[_currentPlayerIndex] = true;
        Debug.Log("Enabled ignoring walls for player: " + _currentPlayerIndex);
    }

    public void EnableDoubleRoll()
    {
        _playersIgnoreWalls[_currentPlayerIndex] = true;
        Debug.Log("Enabled double roll for player: " + _currentPlayerIndex);
    }

    public void EnablePlayerMax3TileMovement()
    {
        _playersMax3Move[_currentPlayerIndex] = true;
        Debug.Log("Enabled max 3 tile movement for player: " + _currentPlayerIndex);
    }

    public void EnablePlayerMissTurn()
    {
        _playersMissTurn[_currentPlayerIndex == 0 ? 1 : 0] = true;
        Debug.Log("Enabled missing turn for player: " + _currentPlayerIndex);
    }

    public void EnableBackToStart()
    {
        _playerBackToStartingPosition[_currentPlayerIndex] = true;
        Debug.Log("Enabled back to start for player: " + _currentPlayerIndex);
    }

    public void EnableNextPlayerMax3TileMovement()
    {
        _playersMax3Move[_currentPlayerIndex == 0 ? 1 : 0] = true;
        Debug.Log("Enabled max 3 tile movement for player: " + (_currentPlayerIndex == 0 ? 1 : 0));
    }
}
