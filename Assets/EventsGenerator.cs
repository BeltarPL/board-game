using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsGenerator : MonoBehaviour
{
    static bool[,] _mapFields = new bool[10,10];

    [SerializeField] GameObject _eventPrefab;
    [SerializeField] Transform _eventsHolder;
    static int _mapOffsetX = -4;
    static int _mapOffsetY = -5;
    // Start is called before the first frame update

    private static Dictionary<Vector2, GameObject> eventWithPosition = new Dictionary<Vector2, GameObject>();

    void Start()
    {
        StartCoroutine(MapInit());
    }

    public void PlaceEventsForQuarter(int startingX, int startingY)
    {
        int _tempCount = 4;
        for(int i = 0; i<_tempCount; i++)
        {
            int _tempPositionX = Random.Range(startingX, startingX + 5);
            int _tempPositionY = Random.Range(startingY, startingY + 5);

            if (KeysManager.Instance.CheckIfPositionReserved(new Vector2(_tempPositionX, _tempPositionY)) || _mapFields[_tempPositionX, _tempPositionY] == true)
            {
                _tempCount++;
            }
            else
            {
                _mapFields[_tempPositionX, _tempPositionY] = true;
                SpawnEvent(_tempPositionX, _tempPositionY);                
            }
        }
    }

    public void SpawnEvent(int positionX, int positionY)
    {
        Vector3 _calculatedPosition = new Vector2(positionX + _mapOffsetX, positionY + _mapOffsetY);
        GameObject temp = Instantiate(_eventPrefab, _calculatedPosition, Quaternion.identity, _eventsHolder);
        eventWithPosition.Add(_calculatedPosition, temp);
    }

    public static bool CheckForEvent(int xPos, int yPos)
    {
        int calcX = xPos - _mapOffsetX;
        int calcY = yPos - _mapOffsetY;

        if(_mapFields[calcX, calcY])
        {
            if(eventWithPosition.ContainsKey(new Vector2(xPos, yPos)))
            {
                _mapFields[calcX, calcY] = false;
                eventWithPosition[new Vector2(xPos, yPos)].SetActive(false);
                eventWithPosition.Remove(new Vector2(xPos, yPos));
                //PlayerManager.Instance.GetCurrentPlayerController().SetPause(true);
                return true;
            }
        };

        return false;
    }

    public static void SetPositionState(int X, int Y, bool state)
    {
        _mapFields[X - _mapOffsetX, Y - _mapOffsetY] = state;
    }

    IEnumerator MapInit()
    {
        //yield return new WaitForSeconds(0.5f);

        PlaceEventsForQuarter(0, 0);
        PlaceEventsForQuarter(5, 0);
        PlaceEventsForQuarter(0, 5);
        PlaceEventsForQuarter(5, 5);
        yield return null;
    }
}
