using UnityEngine;

public interface IItemContainerView
{
    public IItemContainerModel GetModel();
    public RectTransform Rect { get; }
    public void UpdateItem(ItemView item);
    public void UpdateView();
    public void SetModel(IItemContainerModel containerM);
    public Vector2Int GetGridPosition(Vector2 mousePos);
}
