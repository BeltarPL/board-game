using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseBehaviour : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer _cursorIndicator;

    [SerializeField]
    ObjectPool _objectPool;

    Vector3 _lastPositionCalculated = Vector3.one * float.PositiveInfinity;

    [SerializeField]
    List<Vector3> _fieldsCurrentlySelected = new List<Vector3>();
    List<Vector3> _reversedPath = new List<Vector3>();

    public List<Vector3> FieldsCurrentlySelected => _fieldsCurrentlySelected;

    public List<Vector3> FieldsCurrentlySelectedReversed => _reversedPath;

    bool _isCursorActive = false;

    int _maxCellsThisTurn = 6;

    bool _canTraverseWalls = false;

    Vector3 _startPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isCursorActive)
        {
            _cursorIndicator.enabled = false;
            return;
        }

        _cursorIndicator.enabled = true;

        Vector3 clampedWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clampedWorldPos = new Vector3(
            Mathf.RoundToInt(clampedWorldPos.x),
            Mathf.RoundToInt(clampedWorldPos.y),
            0
        );
        _cursorIndicator.transform.position = clampedWorldPos;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
            ClearPath();
        }
        else if(Input.GetMouseButton(0))
        {
            if (_lastPositionCalculated == clampedWorldPos) return;
            if (_objectPool.IsAtMax() || _objectPool.CurrentObjectCount == _maxCellsThisTurn) return;
            if (_fieldsCurrentlySelected.Contains(clampedWorldPos)) return;

            if (_objectPool.CurrentObjectCount == 0 && !((Vector2)_startPos).IsVectorNeighbour(clampedWorldPos)) return;

            if (_fieldsCurrentlySelected.Count != 0)
            {
                Vector3 lastAddedPos = _fieldsCurrentlySelected.GetLastItem();
                if(lastAddedPos.x != clampedWorldPos.x && lastAddedPos.y != clampedWorldPos.y) return;

                if(!_canTraverseWalls)
                {
                    Vector3 direction = clampedWorldPos - lastAddedPos;
                    RaycastHit2D hitResult = Physics2D.Raycast(_fieldsCurrentlySelected.GetLastItem(), direction, 1f);
                    if (hitResult.collider != null) return;
                }
            }
            else
            {
                if (!_canTraverseWalls)
                {
                    Vector3 direction = clampedWorldPos - _startPos;
                    RaycastHit2D hitResult = Physics2D.Raycast(_startPos, direction, 1f);
                    if (hitResult.collider != null) return;
                }
            }

            GameObject go = _objectPool.Instantiate(clampedWorldPos);
            _lastPositionCalculated = go.transform.position;
            _fieldsCurrentlySelected.Add(clampedWorldPos);
            _reversedPath.Add(clampedWorldPos);
            go.GetComponent<CursorTile>().SetNumber(_fieldsCurrentlySelected.Count);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            _reversedPath.Reverse();
        }
    }

    

    public void SetCursorInteraction(bool state)
    {
        _isCursorActive = state;
    }

    public void SetMaxCellsThisTurn(int count)
    {
        _maxCellsThisTurn = count;
    }

    public void SetCanTraverseWalls(bool state)
    {
        _canTraverseWalls = state;
    }

    public void ClearPath()
    {
        _objectPool.DestroyAll();
        _lastPositionCalculated = Vector3.one * float.PositiveInfinity;
        _fieldsCurrentlySelected.Clear();
        _reversedPath.Clear();
    }

    public void SetStartPos(Vector3 startPos)
    {
        _startPos = startPos;
    }

    public Vector3 GetStartPos()
    {
        return _startPos;
    }
}
