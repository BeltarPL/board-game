using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    int _objectPoolCount;

    [SerializeField]
    GameObject _prefab;

    GameObject[] _pool;

    int _currentObjIndex = -1;

    [SerializeField]
    bool _wrapAround = false;

    public int CurrentObjectCount => _currentObjIndex + 1;

    // Start is called before the first frame update
    void Awake()
    {
        _pool = new GameObject[_objectPoolCount];
        for(int i = 0; i<_objectPoolCount; i++)
        {
            _pool[i] = Instantiate(_prefab, transform);
            _pool[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Instantiate(Vector3 position)
    {
        if (!_wrapAround && _currentObjIndex + 1 == _objectPoolCount) return null;
        _currentObjIndex = (_currentObjIndex + 1) % _objectPoolCount;
        _pool[_currentObjIndex].transform.position = position;
        _pool[_currentObjIndex].SetActive(true);
        return _pool[_currentObjIndex];
    }

    public void DestroyAll()
    {
        ResetIndex();
        _pool.ForEach(elem => elem.SetActive(false));
    }

    public bool IsAtMax()
    {
        return _currentObjIndex + 1 == _objectPoolCount;
    }

    public GameObject GetCurrentPoolObject()
    {
        return _pool[_currentObjIndex];
    }



    public void ResetIndex()
    {
        _currentObjIndex = -1;
    }
}
