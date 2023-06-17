using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CursorTile : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _numberLabel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNumber(int number)
    {
        _numberLabel.text = number.ToString();
    }
}
