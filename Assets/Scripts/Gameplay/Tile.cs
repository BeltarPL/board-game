using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Connected tiles")]
    [SerializeField] Tile leftTile;
    [SerializeField] Tile rightTile;
    [SerializeField] Tile upTile;
    [SerializeField] Tile downTile;

    [Header("Direction indicators")]
    [SerializeField] GameObject leftIndicator;
    [SerializeField] GameObject rightIndicator;
    [SerializeField] GameObject upIndicator;
    [SerializeField] GameObject downIndicator;

    private void Start()
    {
        leftIndicator.SetActive(false);
        rightIndicator.SetActive(false);
        upIndicator.SetActive(false);
        downIndicator.SetActive(false);
    }

    void CheckMovementAvailability()
    {
        if (leftTile != null)
        {
            EnableDirectionIndicator("left");
        }

        if(rightTile != null)
        {
            EnableDirectionIndicator("right");
        }

        if(upTile != null)
        {
            EnableDirectionIndicator("up");
        }

        if(downTile != null)
        {
            EnableDirectionIndicator("down");
        }
    }

    void EnableDirectionIndicator(string direction)
    {
        switch (direction)
        {
            case "left":
                leftIndicator.SetActive(true);
                break;
            case "right":
                rightIndicator.SetActive(true);
                break;
            case "up":
                upIndicator.SetActive(true);
                break;
            case "down":
                downIndicator.SetActive(true);
                break;
        }
    }

    public void OnTileSelected()
    {
        CheckMovementAvailability();
    }
}
