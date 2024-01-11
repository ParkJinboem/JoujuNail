using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RecyclingSystem
{
    public IRecyclableScrollRectDataSource dataSource;
    protected RectTransform viewport;
    protected RectTransform content;
    protected RectTransform prototypeCell;
    protected bool isGrid;

    protected float minPoolCoverage = 1.5f; // The recyclable pool must cover (viewPort * _poolCoverage) area.
    protected int minPoolSize = 10; // Cell pool must have a min size
    protected float RecyclingThreshold = .2f; //Threshold for recycling above and below viewport

    public abstract IEnumerator InitCoroutine(System.Action onInitialized = null);

    public abstract Vector2 OnValueChangedListener(Vector2 direction);
}
