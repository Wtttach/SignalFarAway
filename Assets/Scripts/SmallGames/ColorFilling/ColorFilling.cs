using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ColorFilling : MonoBehaviour
{
    /// ---------------------preset references----------------------
    public Color[] colorRefs = new Color[7];
    const int maxBlockCount = 20;

    public Transform currentBlockView;
    public Transform colorRequestView;

    /// ---------------------prefab assets--------------------------
    public GameObject blockUnit;
    public GameObject gridUnit;

    // ----------------------Audio Assets---------------------------
    public AudioSource colorFill;
    public AudioClip failAudio;

    /// ---------------------data and listener----------------------
    ColorFillingListener listener;
    ColorFillingSetting gameSetting;

    public float unitSize, gap;
    public float preview_unitSize, preview_gap;

    /// ---------------------gameobject instance--------------------
    private List<GameObject> usableColorBlocks = new List<GameObject>();
    private GameObject[,] colorGrid = new GameObject[20, 20];
    /// <summary>
    /// (x,y) for pos, z for color block index
    /// </summary>
    private Stack<Vector3> blockHistory = new Stack<Vector3>(); /// for remove and restore

    private bool[] isUsed = new bool[maxBlockCount];

    public TextMeshProUGUI blockPointerText;
    public TextMeshProUGUI blockCountText;

    private int blockCount;

    private int blockLeft;
    private int blockPointer = 0;
    /// Open for display the block
    public int BlockPointer
    {
        get { return blockPointer + 1; }
    }

    /// ---------------------game runtime data----------------------
    private int[,] canvasColor = new int[20, 20];
    private bool[,] isInvalid = new bool[20, 20];
    private Vector2 currentBlockPos;

    public enum BlockStatePaint { Added = 0, NotUsed = 1};
    BlockStatePaint isBlockFilled = BlockStatePaint.NotUsed;

    /// ---------------------monobehavior call----------------------

    private void Awake()
    {
        listener = GetComponent<ColorFillingListener>();

        if (listener == null)
            Debug.LogError("Color Filling (Small Game) listener couldn't be found!");
    }
    private void Start()
    {
    }

    private void OnEnable()
    {
        InitializeGame();
        InitializeColorRequest();
        InitializeCanvas();
        InitializeColorBlocks();
    }


    /// ---------------------Initialize-----------------------------
    #region Initialize
    private void InitializeGame()
    {
        gameSetting = listener.GetGameSetting() as ColorFillingSetting;
        blockCount = gameSetting.blocksData.Count;
        blockLeft = blockCount;

        if (blockCountText != null) blockCountText.text = blockCount.ToString();
        if (blockPointerText != null) blockPointerText.text = blockLeft.ToString();
    }

    private void InitializeColorRequest()
    {
        int count = 0;
        for (int i = 0; i < 7; i++)
        {
            if (gameSetting.colorRequest[i] == 0) continue;

            var colorRequest = colorRequestView.GetChild(count).gameObject;           
            colorRequest.SetActive(true);
            colorRequest.GetComponent<Image>().color = colorRefs[i];
            colorRequest.GetComponentInChildren<TextMeshPro>().color = colorRefs[i];
            colorRequest.GetComponentInChildren<TextMeshPro>().text = gameSetting.colorRequest[i].ToString();

            count++;
        }
    }

    private void InitializeColorBlocks()
    {
        foreach (var blockdata in gameSetting.blocksData)
        {
            GameObject newBlock = new GameObject();
            newBlock.name = "Color Block";
            newBlock.transform.parent = currentBlockView;
            newBlock.transform.localPosition = Vector3.zero;
            newBlock.transform.localEulerAngles = Vector3.zero;
            newBlock.transform.localScale = new Vector3(1,1,1);
            newBlock.layer = 6;

            for (int i = 0; i < blockdata.sizeY; i++)
            {
                for (int j = 0; j < blockdata.sizeX; j++)
                {
                    if (!blockdata.isBlock[i * 20 + j]) continue;

                    GameObject newUnit = GameObject.Instantiate(blockUnit, newBlock.transform);
                    newUnit.name = "Block Unit";
                    newUnit.transform.localPosition = new Vector3(j * (preview_gap + preview_unitSize), -i * (preview_gap + preview_unitSize), 0);
                    newUnit.GetComponent<SpriteRenderer>().color = colorRefs[(int)blockdata.blockColor];
                }
            }
            newBlock.transform.localPosition = new Vector3(usableColorBlocks.Count * (preview_unitSize + preview_gap) * 4, 0, 0);
            usableColorBlocks.Add(newBlock);
        }
    }

    private void InitializeCanvas()
    {
        for (int i = 0; i < gameSetting.sizeY; i++)
        {
            for (int j = 0; j < gameSetting.sizeX; j++)
            {
                GameObject newGrid = GameObject.Instantiate(gridUnit,transform);
                newGrid.transform.localEulerAngles = Vector3.zero;
                newGrid.transform.localPosition = new Vector3(j * (gap + unitSize), -i * (gap + unitSize), 0);
                newGrid.name = $"Grid({j}, {i})";
                colorGrid[i, j] = newGrid;
                RefreshGridState(newGrid, 0, false);

                if (!gameSetting.isSpace[i * 20 + j])
                {
                    newGrid.GetComponent<SpriteRenderer>().enabled = false;
                    newGrid.transform.GetChild(0).gameObject.SetActive(false);
                    newGrid.transform.GetChild(1).gameObject.SetActive(false);
                    newGrid.transform.GetChild(2).gameObject.SetActive(false);
                }
            }
        }
    }
    #endregion

    #region Dispose
    /// <summary>
    /// Destroy the gameobjects when a small game is done.
    /// </summary>
    public void DisposeGame()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < colorRequestView.childCount; i++)
        {
            colorRequestView.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < currentBlockView.childCount; i++)
        {
            Destroy(currentBlockView.GetChild(i).gameObject);
        }
        usableColorBlocks.Clear();
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                canvasColor[i, j] = 0;
                isInvalid[i, j] = false;
                colorGrid[i, j] = null;
            }
        }
        for (int  i = 0; i < 20; i++)
        {
            isUsed[i] = false;
        }

        currentBlockView.localPosition = new Vector3(-1.4f, 0.3f, 0.8f);

        blockHistory.Clear();

        isBlockFilled = BlockStatePaint.NotUsed;
        currentBlockPos = Vector2.zero;
        blockPointer = 0;
    }

    #endregion


    /// ---------------------APIs for button events--------------------
    #region APIs
    /// <summary>
    /// Change the grid color and animation state. True for flickering.
    /// </summary>
    private void RefreshGridState(GameObject changeBlock, int color = -1, bool flick = false, bool invalid = false)
    {
        if (invalid == true)
        {
            changeBlock.transform.GetChild(2).gameObject.SetActive(true);
            changeBlock.transform.GetChild(2).GetComponent<SpriteRenderer>().color = colorRefs[(int)gameSetting.blocksData[blockPointer].blockColor];
            return;
        }
        changeBlock.transform.GetChild(2).gameObject.SetActive(false);
        //Debug.Log(changeBlock.name);
        // Color change
        SpriteRenderer blockSprite = changeBlock.GetComponent<SpriteRenderer>();
        SpriteRenderer FillSprite = changeBlock.transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer nonFlickSprite = changeBlock.transform.GetChild(1).GetComponent<SpriteRenderer>();

        if (color != -1)
        {
            if (blockSprite != null) blockSprite.color = colorRefs[color];
            if (FillSprite != null) FillSprite.color = new Color(colorRefs[color].r, colorRefs[color].g, colorRefs[color].b, FillSprite.color.a);
            if (nonFlickSprite != null) nonFlickSprite.color = new Color(colorRefs[color].r, colorRefs[color].g, colorRefs[color].b, nonFlickSprite.color.a);
            else Debug.LogError("SpriteRenderer not found");
        }

        // Animation change
        if (color == 0) { FillSprite.enabled = false; nonFlickSprite.enabled = false; return; }

        if (flick == true) { FillSprite.enabled = true; nonFlickSprite.enabled = false; }
        else { FillSprite.enabled = false; nonFlickSprite.enabled = true; }
    }

    enum ChangeMode
    {
        Add = 0, Remove = 1
    }

    private bool IsInBlock(int posX, int posY)
    {
        int tempx = posX - (int)currentBlockPos.x,
            tempy = posY - (int)currentBlockPos.y;

        if (tempx < 0 || tempx >= gameSetting.blocksData[blockPointer].sizeX) return false;
        if (tempy < 0 || tempy >= gameSetting.blocksData[blockPointer].sizeY) return false;
        if (!gameSetting.blocksData[blockPointer].isBlock[tempy * 20 + tempx]) return false;

        return true;
    }

    /// <summary>
    /// Check if the color block fit the position.
    /// </summary>
    private bool CheckPosValid(int posY, int posX)
    {
        for (int p = 0; p < gameSetting.blocksData[blockPointer].sizeY; p++)
        {
            for (int q = 0; q < gameSetting.blocksData[blockPointer].sizeX; q++)
            {
                if (!gameSetting.blocksData[blockPointer].isBlock[p * 20 + q])
                {
                    continue;
                }

                int i = posY + p, j = posX + q;
                if (!gameSetting.isSpace[i * 20 + j]) return false;
                if (isInvalid[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Change grid colors when a block is moved or filled
    /// </summary>
    private void ChangeBlockColor(int blockIndex, Vector2 blockPos)
    {
        for (int i = 0; i < gameSetting.blocksData[blockIndex].sizeY; i++)
        {
            for (int j = 0; j < gameSetting.blocksData[blockIndex].sizeX; j++)
            {
                if (!gameSetting.blocksData[blockIndex].isBlock[i * 20 + j]) continue;
                int tempPosX = j + (int)blockPos.x, tempPosY = i + (int)blockPos.y;
                if (!isInvalid[tempPosY, tempPosX])
                {
                    canvasColor[tempPosY, tempPosX] = ColorDeMix(canvasColor[tempPosY, tempPosX], (int)gameSetting.blocksData[blockIndex].blockColor);
                }
                isInvalid[tempPosY, tempPosX] = false;
                RefreshGridState(colorGrid[tempPosY, tempPosX], canvasColor[tempPosY, tempPosX], false, false);
            }
        }
    }

    public enum MoveDirection
    {
        up = 0, down, left, right
    }

    // Methods opened for button on click event
    public void MoveBlockUp() { if (!MoveBlock(MoveDirection.up)) { colorFill.clip = failAudio; colorFill.Play(); } }
    public void MoveBlockDown() { if (!MoveBlock(MoveDirection.down)) { colorFill.clip = failAudio; colorFill.Play(); } }
    public void MoveBlockLeft() { if (!MoveBlock(MoveDirection.left)) { colorFill.clip = failAudio; colorFill.Play(); } }
    public void MoveBlockRight() { if (!MoveBlock(MoveDirection.right)) { colorFill.clip = failAudio; colorFill.Play(); } }

    /// <summary>
    /// When a color block is on canvas, try moving the color block.
    /// True for success, boolean value can be used for visual effect.
    /// </summary>
    public bool MoveBlock(MoveDirection moveDir)
    {
        if (isBlockFilled == BlockStatePaint.NotUsed) return false;

        int tempPosX = (int)currentBlockPos.x, tempPosY = (int)currentBlockPos.y;

        switch (moveDir)
        {
            case MoveDirection.up: tempPosY--; break;
            case MoveDirection.down: tempPosY++; break;
            case MoveDirection.left: tempPosX--; break;
            case MoveDirection.right: tempPosX++; break;
        }

        if ((tempPosX < 0 || tempPosX + gameSetting.blocksData[blockPointer].sizeX > gameSetting.sizeX) || (tempPosY < 0 || tempPosY + gameSetting.blocksData[blockPointer].sizeY > gameSetting.sizeY))
            return false;


        ChangeBlockColor(blockPointer, currentBlockPos);

        AddBlockToCanvas(tempPosY, tempPosX);

        currentBlockPos.x = tempPosX;
        currentBlockPos.y = tempPosY;
        return true;
    }
    /// <summary>
    /// Add block to canvas and set valid
    /// </summary>
    private void AddBlockToCanvas(int blockY, int blockX)
    {
        for (int i = 0; i < gameSetting.blocksData[blockPointer].sizeY; i++)
        {
            for (int j = 0; j < gameSetting.blocksData[blockPointer].sizeX; j++)
            {
                if (!gameSetting.blocksData[blockPointer].isBlock[i * 20 + j]) continue;
                int posX = blockX + j;
                int posY = blockY + i;

                if (!gameSetting.isSpace[posY * 20 + posX])
                {
                    isInvalid[posY, posX] = true;
                    RefreshGridState(colorGrid[posY, posX], (int)gameSetting.blocksData[blockPointer].blockColor, false, true);
                    continue;
                }
                
                    if ((canvasColor[posY, posX] == (int)gameSetting.blocksData[blockPointer].blockColor) || canvasColor[posY, posX] == (int)ColorName.green ||
                        canvasColor[posY, posX] == (int)ColorName.orange ||
                        canvasColor[posY, posX] == (int)ColorName.purple ||
                    (canvasColor[posY, posX] != 0 && (gameSetting.blocksData[blockPointer].blockColor == ColorName.green ||
                                                   gameSetting.blocksData[blockPointer].blockColor == ColorName.orange ||
                                                   gameSetting.blocksData[blockPointer].blockColor == ColorName.purple)))
                    {
                        isInvalid[posY, posX] = true;
                        RefreshGridState(colorGrid[posY, posX], (int)gameSetting.blocksData[blockPointer].blockColor, false, true);
                        continue;
                    }
                

                
                canvasColor[posY, posX] = ColorMix(canvasColor[posY, posX], (int)gameSetting.blocksData[blockPointer].blockColor);

                RefreshGridState(colorGrid[posY, posX], canvasColor[posY, posX], true, false);
            }
        }
    }

    private void SetInActive(GameObject block)
    {
        foreach (var child in block.GetComponentsInChildren<SpriteRenderer>())
        {
            child.color -= new Color(0,0,0, 0.9f);
        }
    }
    private void SetActive(GameObject block)
    {
        foreach (var child in block.GetComponentsInChildren<SpriteRenderer>())
        {
            child.color += new Color(0, 0,0, 0.9f);
        }
    }

    /// <summary>
    /// When put key pressed, confirm the color change.
    /// </summary>
    public void FillColor()
    {
        if (isUsed[blockPointer]) return;
        if (isBlockFilled == BlockStatePaint.NotUsed)
        {
            AddBlockToCanvas(0, 0);
            isBlockFilled = BlockStatePaint.Added;
            return;
        }

        if (!CheckPosValid((int)currentBlockPos.y, (int)currentBlockPos.x)) { colorFill.clip = failAudio; colorFill.Play(); return; };

        isBlockFilled = BlockStatePaint.NotUsed;
        for (int i = 0; i < gameSetting.blocksData[blockPointer].sizeY; i++)
        {
            for (int j = 0; j < gameSetting.blocksData[blockPointer].sizeX; j++)
            {
                if (!gameSetting.blocksData[blockPointer].isBlock[i * 20 + j]) continue;

                RefreshGridState(colorGrid[i + (int)currentBlockPos.y, j + (int)currentBlockPos.x], canvasColor[i + (int)currentBlockPos.y, j + (int)currentBlockPos.x]);
            }
        }

        blockHistory.Push(new Vector3(currentBlockPos.x, currentBlockPos.y, blockPointer));
        isUsed[blockPointer] = true;
        SetInActive(usableColorBlocks[blockPointer]);
        blockLeft--;
        blockPointerText.text = blockLeft.ToString();

        if (CheckGameSuccess())
        {
            //Debug.Log("Game Finished");
            listener.GiveMessage();
            listener.GameResult(true);
        }
        
        ChangeCurrentBlock(ChangeBlockMode.Fill);
        ShowColorLeft();
        currentBlockPos = Vector2.zero;
    }

    /// <summary>
    /// Mix color on canvas and new color.
    /// Return int (Refering to ColorName enum).
    /// </sumary>
    private int ColorMix(int original, int addColor)
    {
        int color = 0;
        if (original == 0) { color = addColor; return color; }
        color = original + addColor;
        if (color != 6) color /= 2;
        return color;
    }

    /// <summary>
    /// Restore color on canvas and new color.
    /// Return int (Refering to ColorName enum).
    /// </sumary>
    private int ColorDeMix(int original, int removeColor)
    {
        int color = 0;
        if (original == removeColor) return color;
        color = original * 2 - removeColor;
        if (color > 6) color = original - removeColor;
        return color;
    }

    /// <summary>
    /// Press restore key to undo one color fill move.
    /// </summary>
    public void RestoreBlock()
    {
        if (isBlockFilled == BlockStatePaint.Added)
        {
            ChangeBlockColor(blockPointer, currentBlockPos);
            isBlockFilled = BlockStatePaint.NotUsed;
            currentBlockPos = Vector2.zero;
            return;
        }

        if (blockHistory.Count == 0) { colorFill.clip = failAudio; colorFill.Play(); return; }

        Vector3 restoreBlockInfo = blockHistory.Pop();

        ChangeBlockColor((int)restoreBlockInfo.z, new Vector2(restoreBlockInfo.x, restoreBlockInfo.y));

        isUsed[(int)restoreBlockInfo.z] = false;
        blockLeft++;

        ActivateBlock((int)restoreBlockInfo.z);
        ShowColorLeft();
    }

    public void ChangeBlock()
    {
        if (!ChangeCurrentBlock(ChangeBlockMode.Change))
        {
            colorFill.clip = failAudio; colorFill.Play();
        }
    }

    public enum ChangeBlockMode { Change = 0, Fill = 1}
    /// <summary>
    /// Press keys to change currently using color block.
    /// </summary>
    private bool ChangeCurrentBlock(ChangeBlockMode change)
    {
        if (blockLeft == 0) return false;

        int removeIndex = -1;

        for (int i = 1; i < blockCount; i++)
        {
            int tempPointer = blockPointer + i;
            if (tempPointer >= blockCount) tempPointer -= blockCount;

            if (isUsed[tempPointer] == false)
            {
                removeIndex = blockPointer;
                

                MoveToTargetBlock(tempPointer);

                if (change == ChangeBlockMode.Change && isBlockFilled == BlockStatePaint.Added)
                {
                    ChangeBlockColor(removeIndex, currentBlockPos);
                    isBlockFilled = BlockStatePaint.NotUsed;
                }
                blockPointer = tempPointer;
                currentBlockPos = Vector2.zero;

                return true;
            }
        }

        return false;
    }

    IEnumerator MoveBlockView(float targetPos)
    {
        var targetPosition = new Vector3(-1.4f + targetPos, 0.3f, 0.8f);
        while (Mathf.Abs(currentBlockView.localPosition.x - targetPosition.x) > 0.00001f)
        {
            currentBlockView.localPosition = Vector3.MoveTowards(currentBlockView.localPosition, targetPosition, 0.5f);
            yield return null;
        }

    }

    private void MoveToTargetBlock(int target)
    {
        StartCoroutine(MoveBlockView(-(target * 1f)));
    }

    /// <summary>
    /// Change current block to aim block according to blockIndex
    /// </summary>
    private void ActivateBlock(int blockIndex)
    {
        SetActive(usableColorBlocks[blockIndex]);
        MoveToTargetBlock(blockIndex);
        blockPointer = blockIndex;
    }

    #endregion

    /// ---------------------Game Aim check---------------------------
    #region Callbacks
    /// <summary>
    /// Check if the color block meets the requirements
    /// </summary>
    private bool CheckGameSuccess()
    {
        
        int[] checkColor = { 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < gameSetting.sizeY; i++)
        {
            for (int j = 0; j < gameSetting.sizeX; j++)
            {
                checkColor[canvasColor[i, j]]++;
            }
        }

        for (int i = 1; i < 7; i++)
        {
            if (checkColor[i] != gameSetting.colorRequest[i]) return false;
        }
        return true;
    }

    /// <summary>
    /// Set the color left to fill on the game view
    /// </summary>
    private void ShowColorLeft()
    {
        int[] checkColor = { 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < gameSetting.sizeY; i++)
        {
            for (int j = 0; j < gameSetting.sizeX; j++)
            {
                checkColor[canvasColor[i, j]]++;
            }
        }

        int count = 0;
        for (int i = 1; i < 7; i++)
        {
            if (gameSetting.colorRequest[i] == 0) continue;

            colorRequestView.GetChild(count).GetComponentInChildren<TextMeshPro>().text = Mathf.Max((gameSetting.colorRequest[i] - checkColor[i]),0).ToString();
            count++;
        }
    }
    #endregion
}