using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel
{
    public ItemData ItemData { get; private set; }
    public bool IsRotated { get; private set; }
    public int Height => IsRotated ? ItemData.Width : ItemData.Height;
    public int Width => IsRotated ? ItemData.Height : ItemData.Width;

}
