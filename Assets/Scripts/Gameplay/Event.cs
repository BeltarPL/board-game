using UnityEngine;

public class Event : IEvent
{

    public void ChooseEvent()
    {
        int _randomValue = 3; //Random.Range(1, 7);

        switch (_randomValue)
        {
            case 1:
                GameManager.Instance.EnableIgnoringWalls();
                break;
            case 2:
                GameManager.Instance.EnablePlayerMissTurn();
                break;
            case 3:
                GameManager.Instance.EnableDoubleRoll();
                break;
            case 4:
                GameManager.Instance.EnableBackToStart();
                break;
            case 5:
                GameManager.Instance.EnableNextPlayerMax3TileMovement();
                break;
            case 6:
                GameManager.Instance.EnablePlayerMax3TileMovement();
                break;
        }

    }
   }
