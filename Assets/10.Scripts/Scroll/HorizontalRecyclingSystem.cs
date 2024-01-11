using System;
using System.Collections;
using System.Collections.Generic;
using PolyAndCode.UI;
using UnityEngine;

public class HorizontalRecyclingSystem : RecyclingSystem
{
    private readonly int rows;
    private float cellWidth;
    private float cellHeight;

    private List<RectTransform> cellPool;
    private List<ICell> cachedCells;
    private Bounds recyclableViewBounds;


    //Temps, Flags
    private readonly Vector3[] corners = new Vector3[4];
    private bool recycling;

    //Trackers
    private int currentItemCount;
    private int leftMostCellIndex;
    private int rightMostCellIndex;
    private int leftMostCellRow;
    private int RightMostCellRow;

    //Cached zero vector 
    private Vector2 zeroVector = Vector2.zero;

    public HorizontalRecyclingSystem(RectTransform prototypeCell, RectTransform viewport, RectTransform content, IRecyclableScrollRectDataSource dataSource, bool isGrid, int rows)
    {
        base.prototypeCell = prototypeCell;
        base.viewport = viewport;
        base.content = content;
        base.dataSource = dataSource;
        base.isGrid = isGrid;
        this.rows = isGrid ? rows : 1;
        recyclableViewBounds = new Bounds();
    }

    public override IEnumerator InitCoroutine(Action onInitialized)
    {
        //Setting up container and bounds
        SetLeftAnchor(content);
        content.anchoredPosition = Vector3.zero;
        yield return null;
        SetRecyclingBounds();

        //Cell Poool
        CreateCellPool();
        currentItemCount = cellPool.Count;
        leftMostCellIndex = 0;
        rightMostCellIndex = cellPool.Count - 1;

        //Set content width according to no of coloums
        int coloums = Mathf.CeilToInt((float)cellPool.Count / rows);
        float contentXSize = coloums * cellWidth;
        content.sizeDelta = new Vector2(contentXSize, content.sizeDelta.y);
        SetLeftAnchor(content);

        if (onInitialized != null) onInitialized();
    }

    private void SetRecyclingBounds()
    {
        viewport.GetWorldCorners(corners);
        float threshHold = RecyclingThreshold * (corners[2].x - corners[0].x);
        recyclableViewBounds.min = new Vector3(corners[0].x - threshHold, corners[0].y);
        recyclableViewBounds.max = new Vector3(corners[2].x + threshHold, corners[2].y);
    }
    /// <summary>
    /// 매니큐어 프리팹생성
    /// </summary>
    private void CreateCellPool()
    {
        //Reseting Pool
        if (cellPool != null)
        {
            cellPool.ForEach((RectTransform item) => UnityEngine.Object.Destroy(item.gameObject));
            cellPool.Clear();
            cachedCells.Clear();
        }
        else
        {
            cachedCells = new List<ICell>();
            cellPool = new List<RectTransform>();
        }

        //Set the prototype cell active and set cell anchor as top 
        prototypeCell.gameObject.SetActive(true);
        SetLeftAnchor(prototypeCell);

        //set new cell size according to its aspect ratio
        //매니큐어 아이템 크기 지정
        //cellHeight = content.rect.height / rows;      //원본
        //cellWidth = prototypeCell.sizeDelta.x / prototypeCell.sizeDelta.y * cellHeight;       //원본
        cellHeight = prototypeCell.sizeDelta.y / 2;
        cellWidth = prototypeCell.sizeDelta.x / 2;
        

        //Reset
        leftMostCellRow = RightMostCellRow = 0;

        //Temps
        float currentPoolCoverage = 0;
        int poolSize = 0;
        float posX = 0;
        float posY = 0;

        //Get the required pool coverage and mininum size for the Cell pool
        float requriedCoverage = minPoolCoverage * viewport.rect.width;
        int minPoolSizeVloue = Math.Min(minPoolSize, dataSource.GetItemCount());

        //create cells untill the Pool area is covered and pool size is the minimum required
        while ((poolSize < minPoolSizeVloue || currentPoolCoverage < requriedCoverage) && poolSize < dataSource.GetItemCount())
        {
            //Instantiate and add to Pool
            RectTransform item = (UnityEngine.Object.Instantiate(prototypeCell.gameObject)).GetComponent<RectTransform>();
            //item.name = "Cell";
            item.sizeDelta = new Vector2(cellWidth, cellHeight);
            cellPool.Add(item);
            item.SetParent(content, false);

            if (isGrid)
            {
                posY = -RightMostCellRow * cellHeight;
                item.anchoredPosition = new Vector2(posX, posY);
                if (++RightMostCellRow >= rows)
                {
                    RightMostCellRow = 0;
                    posX += cellWidth;
                    currentPoolCoverage += item.rect.width;
                }
            }
            else
            {
                item.anchoredPosition = new Vector2(posX, 0);
                posX = item.anchoredPosition.x + item.rect.width;
                currentPoolCoverage += item.rect.width;
            }

            //Setting data for Cell
            cachedCells.Add(item.GetComponent<ICell>());
            dataSource.SetCell(cachedCells[cachedCells.Count - 1], poolSize);

            //Update the Pool size
            poolSize++;
        }

        if (isGrid)
        {
            RightMostCellRow = (RightMostCellRow - 1 + rows) % rows;
        }

        //Deactivate prototype cell if it is not a prefab(i.e it's present in scene)
        if (prototypeCell.gameObject.scene.IsValid())
        {
            prototypeCell.gameObject.SetActive(false);
        }
    }

    public override Vector2 OnValueChangedListener(Vector2 direction)
    {
        if (recycling || cellPool == null || cellPool.Count == 0) return zeroVector;

        //Updating Recyclable view bounds since it can change with resolution changes.
        SetRecyclingBounds();

        if (direction.x < 0 && cellPool[rightMostCellIndex].MinX() < recyclableViewBounds.max.x)
        {
            return RecycleLeftToRight();
        }
        else if (direction.x > 0 && cellPool[leftMostCellIndex].MaxX() > recyclableViewBounds.min.x)
        {
            return RecycleRightToleft();
        }
        return zeroVector;
    }

    private Vector2 RecycleLeftToRight()
    {
        recycling = true;

        int n = 0;
        float posX = isGrid ? cellPool[rightMostCellIndex].anchoredPosition.x : 0;
        float posY = 0;

        //to determine if content size needs to be updated
        int additionalColoums = 0;

        //Recycle until cell at left is avaiable and current item count smaller than datasource
        while (cellPool[leftMostCellIndex].MaxX() < recyclableViewBounds.min.x && currentItemCount < dataSource.GetItemCount())
        {
            if (isGrid)
            {
                if (++RightMostCellRow >= rows)
                {
                    n++;
                    RightMostCellRow = 0;
                    posX = cellPool[rightMostCellIndex].anchoredPosition.x + cellWidth;
                    additionalColoums++;
                }

                //Move Left most cell to right
                posY = -RightMostCellRow * cellHeight;
                cellPool[leftMostCellIndex].anchoredPosition = new Vector2(posX, posY);

                if (++leftMostCellRow >= rows)
                {
                    leftMostCellRow = 0;
                    additionalColoums--;
                }
            }
            else
            {
                //Move Left most cell to right
                posX = cellPool[rightMostCellIndex].anchoredPosition.x + cellPool[rightMostCellIndex].sizeDelta.x;
                cellPool[leftMostCellIndex].anchoredPosition = new Vector2(posX, cellPool[leftMostCellIndex].anchoredPosition.y);
            }

            //Cell for row at
            dataSource.SetCell(cachedCells[leftMostCellIndex], currentItemCount);

            //set new indices
            rightMostCellIndex = leftMostCellIndex;
            leftMostCellIndex = (leftMostCellIndex + 1) % cellPool.Count;

            currentItemCount++;
            if (!isGrid) n++;
        }

        //Content size adjustment 
        if (isGrid)
        {
            content.sizeDelta += additionalColoums * Vector2.right * cellWidth;
            if (additionalColoums > 0)
            {
                n -= additionalColoums;
            }
        }

        //Content anchor position adjustment.
        cellPool.ForEach((RectTransform cell) => cell.anchoredPosition -= n * Vector2.right * cellPool[leftMostCellIndex].sizeDelta.x);
        content.anchoredPosition += n * Vector2.right * cellPool[leftMostCellIndex].sizeDelta.x;
        recycling = false;
        return n * Vector2.right * cellPool[leftMostCellIndex].sizeDelta.x;

    }

    /// <summary>
    /// Recycles cells from Right to Left in the List heirarchy
    /// </summary>
    private Vector2 RecycleRightToleft()
    {
        recycling = true;

        int n = 0;
        float posX = isGrid ? cellPool[leftMostCellIndex].anchoredPosition.x : 0;
        float posY = 0;

        //to determine if content size needs to be updated
        int additionalColoums = 0;
        //Recycle until cell at Right end is avaiable and current item count is greater than cellpool size
        while (cellPool[rightMostCellIndex].MinX() > recyclableViewBounds.max.x && currentItemCount > cellPool.Count)
        {
            if (isGrid)
            {
                if (--leftMostCellRow < 0)
                {
                    n++;
                    leftMostCellRow = rows - 1;
                    posX = cellPool[leftMostCellIndex].anchoredPosition.x - cellWidth;
                    additionalColoums++;
                }

                //Move Right most cell to left
                posY = -leftMostCellRow * cellHeight;
                cellPool[rightMostCellIndex].anchoredPosition = new Vector2(posX, posY);

                if (--RightMostCellRow < 0)
                {
                    RightMostCellRow = rows - 1;
                    additionalColoums--;
                }
            }
            else
            {
                //Move Right most cell to left
                posX = cellPool[leftMostCellIndex].anchoredPosition.x - cellPool[leftMostCellIndex].sizeDelta.x;
                cellPool[rightMostCellIndex].anchoredPosition = new Vector2(posX, cellPool[rightMostCellIndex].anchoredPosition.y);
                n++;
            }

            currentItemCount--;
            //Cell for row at
            dataSource.SetCell(cachedCells[rightMostCellIndex], currentItemCount - cellPool.Count);

            //set new indices
            leftMostCellIndex = rightMostCellIndex;
            rightMostCellIndex = (rightMostCellIndex - 1 + cellPool.Count) % cellPool.Count;
        }

        //Content size adjustment
        if (isGrid)
        {
            content.sizeDelta += additionalColoums * Vector2.right * cellWidth;
            if (additionalColoums > 0)
            {
                n -= additionalColoums;
            }
        }

        //Content anchor position adjustment.
        cellPool.ForEach((RectTransform cell) => cell.anchoredPosition += n * Vector2.right * cellPool[leftMostCellIndex].sizeDelta.x);
        content.anchoredPosition -= n * Vector2.right * cellPool[leftMostCellIndex].sizeDelta.x;
        recycling = false;
        return -n * Vector2.right * cellPool[leftMostCellIndex].sizeDelta.x;
    }

    private void SetLeftAnchor(RectTransform rectTransform)
    {
        //Saving to reapply after anchoring. Width and height changes if anchoring is change. 
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        Vector2 pos = isGrid ? new Vector2(0, 1) : new Vector2(0, 0.5f);

        //Setting top anchor 
        rectTransform.anchorMin = pos;
        rectTransform.anchorMax = pos;
        rectTransform.pivot = pos;

        //Reapply size
        rectTransform.sizeDelta = new Vector2(width, height);
    }
}
