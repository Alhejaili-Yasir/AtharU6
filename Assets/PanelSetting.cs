using UnityEngine;
using UnityEngine.UI;

public class ShopPanelSetup : MonoBehaviour
{
    public Vector2 itemSize = new Vector2(200f, 200f);
    public Vector2 spacing = new Vector2(20f, 20f);
    public int maxItemsPerRow = 3;

    void Start()
    {
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        if (grid == null)
            grid = gameObject.AddComponent<GridLayoutGroup>();

        grid.cellSize = itemSize;
        grid.spacing = spacing;
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = maxItemsPerRow;
        grid.childAlignment = TextAnchor.UpperLeft;

        ContentSizeFitter fitter = GetComponent<ContentSizeFitter>();
        if (fitter == null)
            fitter = gameObject.AddComponent<ContentSizeFitter>();

        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
    }
}
