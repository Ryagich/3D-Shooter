using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGrid
{
    public int[,] grid;

    public InventoryGrid(int width, int height)
    {
        grid = new int[width, height];
    }
}
