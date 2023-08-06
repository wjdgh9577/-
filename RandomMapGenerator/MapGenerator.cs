using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 3중 배열로 3차원 랜덤 맵을 생성하는 클래스
/// 섬의 본체를 우선 생성한 후 BSP 알고리즘으로 8개의 언덕을 생성한다
/// 이후 스무딩 과정을 거쳐 모서리를 깎고 오브젝트를 풀링한다
/// </summary>
public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance { get; private set; }

    public enum Land
    {
        grass,
        snow,
        grave,
        mine,
        jungle
    }

    public float progress;
    private const int neighbourCount3D = 13;
    
    //*******************************************************My Block Town 전용 parameters**************************************************************************************//

    private bool blockTopia;

    //**************************************************************************************************************************************************************************//
    //*******************************************************Dungeon 전용 parameters********************************************************************************************//

    private int neighbourCount2D = 4;

    [BoxGroup("Only for Dungeon")]
    [Tooltip("던전 내 벽 생성 확률")]
    [SerializeField]
    [Range(0, 100)]
    private int dungeonWallRandomPercent;

    //**************************************************************************************************************************************************************************//

    [BoxGroup("Size of Island")]
    [Tooltip("섬의 가로(world x축)")]
    public int width;
    [BoxGroup("Size of Island")]
    [Tooltip("섬의 세로(world z축)")] 
    public int depth;
    [BoxGroup("Size of Island")]
    [Tooltip("섬의 높이(world y축)")]
    public int height;
    
    [BoxGroup("Parameters")]
    [Tooltip("섬 베이스의 층수")]
    public int floor;
    [SerializeField]
    [BoxGroup("Parameters")]
    [Tooltip("스무딩 횟수")]
    private int smoothCount;
    [BoxGroup("Parameters")]
    [Tooltip("언덕 생성 알고리즘의 가동 횟수")]
    [Range(0, 3)]
    public int hillRoutine;
    [BoxGroup("Parameters")]
    [Tooltip("알고리즘 1회 가동시 2^n개의 언덕이 생성됨")]
    [Range(0, 5)]
    public int treeDepth;
    [BoxGroup("Parameters")]
    [Tooltip("체크할 경우 언덕의 높이 고정")]
    public bool fixHeight;
    [BoxGroup("Parameters")]
    [Tooltip("고정할 언덕 높이")]
    public int fixedHeight;

    public Transform partsRoot { get; private set; }
    [SerializeField]
    private Transform partsRootDefaul;

    private Land targetLand;

    [Serializable]
    public struct Materials
    {
        public byte materialID;
        public Color materialColor;
    }

    [BoxGroup("Materials")]
    [BoxGroup("Materials/GrassLand")]
    [SerializeField]
    private Materials GLGrassDefault;
    [BoxGroup("Materials/GrassLand")]
    [SerializeField]
    private Materials[] GLGrassRandom;
    [BoxGroup("Materials/GrassLand")]
    [SerializeField]
    private Materials GLEarthDefault;
    [BoxGroup("Materials/GrassLand")]
    [SerializeField]
    private Materials[] GLEarthRandom;
    
    [BoxGroup("Materials/SnowLand")]
    [SerializeField]
    private Materials SLGrassDefault;
    [BoxGroup("Materials/SnowLand")]
    [SerializeField]
    private Materials[] SLGrassRandom;
    [BoxGroup("Materials/SnowLand")]
    [SerializeField]
    private Materials SLEarthDefault;
    [BoxGroup("Materials/SnowLand")]
    [SerializeField]
    private Materials[] SLEarthRandom;

    [BoxGroup("Materials/GraveLand")]
    [SerializeField]
    private Materials GvLGrassDefault;
    [BoxGroup("Materials/GraveLand")]
    [SerializeField]
    private Materials[] GvLGrassRandom;
    [BoxGroup("Materials/GraveLand")]
    [SerializeField]
    private Materials GvLEarthDefault;
    [BoxGroup("Materials/GraveLand")]
    [SerializeField]
    private Materials[] GvLEarthRandom;

    [BoxGroup("Materials/MineLand")]
    [SerializeField]
    private Materials MLGrassDefault;
    [BoxGroup("Materials/MineLand")]
    [SerializeField]
    private Materials[] MLGrassRandom;
    [BoxGroup("Materials/MineLand")]
    [SerializeField]
    private Materials MLEarthDefault;
    [BoxGroup("Materials/MineLand")]
    [SerializeField]
    private Materials[] MLEarthRandom;

    [BoxGroup("Materials/JungleLand")]
    [SerializeField]
    private Materials JLGrassDefault;
    [BoxGroup("Materials/JungleLand")]
    [SerializeField]
    private Materials[] JLGrassRandom;
    [BoxGroup("Materials/JungleLand")]
    [SerializeField]
    private Materials JLEarthDefault;
    [BoxGroup("Materials/JungleLand")]
    [SerializeField]
    private Materials[] JLEarthRandom;

    private Materials grassDefault;
    private Materials[] grassRandom;
    private Materials earthDefault;
    private Materials[] earthRandom;

    public int[,,] map { get; private set; }

    private List<Dictionary<string, float>> roomDatas;

    /// <summary>
    /// BSP 알고리즘을 수행하는 트리 클래스
    /// </summary>
    private class BSP
    {
        private int nodeDepth; // 최대 노드 깊이
        private int depth = 0; // 노드 깊이
        private bool isLeaf = false;

        // 방(언덕)의 중심 좌표
        private float x;
        private float y;

        // 방(언덕)의 가로, 세로, 높이
        private float row;
        private float col;
        private float height;

        private bool hasFixedHeight;

        private BSP left;
        private BSP right;

        public BSP(float x, float y, float row, float col, float height, int nodeDepth, bool hasFixedHeight)
        {
            this.x = x;
            this.y = y;
            this.row = row;
            this.col = col;
            this.height = height;
            this.nodeDepth = nodeDepth;
            this.hasFixedHeight = hasFixedHeight;
        }

        /// <summary>
        /// 트리를 구현하는 클래스 내부 메소드
        /// leaf node에 방(언덕)의 데이터가 포함된다
        /// </summary>
        /// <param name="depth"></param>
        public void MakeChild(int depth)
        {
            this.depth = depth;

            if (this.depth == this.nodeDepth)
            {
                this.isLeaf = true;
                NewRoom();
                return;
            }

            if (this.depth % 2 == 0)
            {
                float leftRow = UnityEngine.Random.Range(row * 0.3f, row * 0.7f);
                float rightRow = row - leftRow;
                float leftX = x - rightRow * .5f;
                float rightX = x + leftRow * .5f;

                this.left = new BSP(leftX, this.y, leftRow, this.col, this.height, this.nodeDepth, this.hasFixedHeight);
                this.right = new BSP(rightX, this.y, rightRow, this.col, this.height, this.nodeDepth, this.hasFixedHeight);
            }
            else
            {
                float downCol = UnityEngine.Random.Range(col * 0.3f, col * 0.7f);
                float upCol = col - downCol;
                float downY = y - upCol * .5f;
                float upY = y + downCol * .5f;

                this.left = new BSP(this.x, downY, this.row, downCol, this.height, this.nodeDepth, this.hasFixedHeight);
                this.right = new BSP(this.x, upY, this.row, upCol, this.height, this.nodeDepth, this.hasFixedHeight);
            }

            this.left.MakeChild(this.depth + 1);
            this.right.MakeChild(this.depth + 1);
        }

        /// <summary>
        /// 방(언덕)의 데이터를 무작위로 설정하는 클래스 내부 메소드
        /// </summary>
        private void NewRoom()
        {
            float oldRow = this.row;
            float oldCol = this.col;
            this.height = this.hasFixedHeight ? this.height : UnityEngine.Random.Range(MapGenerator.instance.floor, this.height);
            this.row = UnityEngine.Random.Range(this.row * 0.3f, this.row * 0.7f);
            this.col = UnityEngine.Random.Range(this.col * 0.3f, this.col * 0.7f);
            this.x = this.x + UnityEngine.Random.Range(-(oldRow - this.row) * .5f, (oldRow - this.row) * .5f);
            this.y = this.y + UnityEngine.Random.Range(-(oldCol - this.col) * .5f, (oldCol - this.col) * .5f);
        }

        /// <summary>
        /// 모든 leaf node를 리스트로 반환하는 클래스 내부 메소드
        /// </summary>
        /// <returns></returns>
        public List<BSP> GetLeafNode()
        {
            List<BSP> ret = new List<BSP>();
            if (this.isLeaf)
            {
                ret.Add(this);
                return ret;
            }
            List<BSP> leftRet = (this.left).GetLeafNode();
            List<BSP> rightRet = (this.right).GetLeafNode();
            ret.AddRange(leftRet);
            ret.AddRange(rightRet);

            return ret;
        }

        /// <summary>
        /// 방(언덕)의 데이터를 딕셔너리로 반환하는 클래스 내부 메소드
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, float> GetValues()
        {
            Dictionary<string, float> ret = new Dictionary<string, float>();
            ret.Add("x", this.x);
            ret.Add("y", this.y);
            ret.Add("row", this.row);
            ret.Add("col", this.col);
            ret.Add("height", this.height);

            return ret;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// 3중 배열을 정의하는 메소드
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    private void SetMap(int width, int height, int depth)
    {
        map = new int[width, height, depth];
    }

    /// <summary>
    /// 랜덤 맵 생성 실행 메소드
    /// </summary>
    /// <param name="blockTopia"></param>
    /// <param name="_partsRoot"></param>
    public void GenerateMap(bool blockTopia, Land _targetLand, bool dungeon = false, Transform _partsRoot = null)
    {
        this.partsRoot = _partsRoot == null ? partsRootDefaul : _partsRoot;
        this.blockTopia = blockTopia;
        this.targetLand = _targetLand;
        
        // 섬 타입 추가시 변경
        switch (targetLand)
        {
            case Land.grass:
                grassDefault = GLGrassDefault;
                grassRandom = GLGrassRandom;
                earthDefault = GLEarthDefault;
                earthRandom = GLEarthRandom;
                break;
            case Land.snow:
                grassDefault = SLGrassDefault;
                grassRandom = SLGrassRandom;
                earthDefault = SLEarthDefault;
                earthRandom = SLEarthRandom;
                break;
            case Land.grave:
                grassDefault = GvLGrassDefault;
                grassRandom = GvLGrassRandom;
                earthDefault = GvLEarthDefault;
                earthRandom = GvLEarthRandom;
                break;
            case Land.mine:
                grassDefault = MLGrassDefault;
                grassRandom = MLGrassRandom;
                earthDefault = MLEarthDefault;
                earthRandom = MLEarthRandom;
                break;
            case Land.jungle:
                grassDefault = JLGrassDefault;
                grassRandom = JLGrassRandom;
                earthDefault = JLEarthDefault;
                earthRandom = JLEarthRandom;
                break;
            default:
                break;
        }
        
        StopCoroutine(DoGenerateMap1());
        StopCoroutine(DoGenerateMap2());
        if (dungeon)
        {
            StartCoroutine(DoGenerateMap2());
        }
        else
        {
            StartCoroutine(DoGenerateMap1());
        }
    }

    /// <summary>
    /// 랜덤 맵 생성 메소드
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoGenerateMap1()
    {
        progress = 0;

        SetMap(width, height, depth);

        // 섬 베이스 생성
        yield return StartCoroutine(RandomFillMap3D());
        Debug.Log(string.Format("RandomFillMap done : {0}s", Time.time));
        
        fixedHeight = fixedHeight > height ? height : fixedHeight;
        
        if (blockTopia)
        {
            // 낮은 언덕 생성
            yield return StartCoroutine(BSP2Map(4, floor + 1, true));
        }
        for (int i = 0; i < hillRoutine; i++)
        {
            // 높은 언덕 생성
            yield return StartCoroutine(BSP2Map(treeDepth, fixedHeight, fixHeight));
            Debug.Log(string.Format("BSP2Map{0} done : {1}s", i, Time.time));
        }
        progress = .1f;
        
        // 스무딩 작업
        for (int i = 0; i < smoothCount; i++)
        {
            yield  return StartCoroutine(SmoothMap3D());
            Debug.Log(string.Format("Smooth{0} done : {1}s", i+1, Time.time));
        }
        progress = .4f;

        if (blockTopia)
        {
            for (int i = 0; i < roomDatas.Count; i++)
            {
                int centerX = Mathf.RoundToInt(roomDatas[i]["x"]);
                int centerY = Mathf.RoundToInt(roomDatas[i]["y"]);
                int row = Mathf.RoundToInt(roomDatas[i]["row"]);
                int col = Mathf.RoundToInt(roomDatas[i]["col"]);
                int height = Mathf.RoundToInt(roomDatas[i]["height"]);

                int gridX = Mathf.RoundToInt(row * .5f);
                int gridY = Mathf.RoundToInt(col * .5f);
                SetStair(centerX, centerY, height, gridX - smoothCount - 1, gridY - smoothCount - 1);
            }
        }

        yield return StartCoroutine(Draw());
        progress = 1;
    }

    /// <summary>
    /// 던전 랜덤 맵 생성 메소드
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoGenerateMap2()
    {
        progress = 0;

        // 던전은 높이 고정
        height = floor + 4;
        SetMap(width, height, depth);

        // 벽이 생성될 위치 선정
        yield return StartCoroutine(RandomFillMap2D());
        Debug.Log(string.Format("RandomFillMap2D done : {0}s", Time.time));
        progress = .1f;

        // 스무딩 작업
        for (int i = 0; i < smoothCount; i++)
        {
            yield return StartCoroutine(SmoothMap2D());
            Debug.Log(string.Format("Smooth{0} done : {1}s", i + 1, Time.time));
        }
        progress = .2f;

        // 던전 벽 생성
        yield return StartCoroutine(MakeWall());
        Debug.Log(string.Format("MakeWall done : {0}s", Time.time));
        progress = .3f;
        
        // 섬 베이스 생성
        yield return StartCoroutine(RandomFillMap3D());
        Debug.Log(string.Format("RandomFillMap3D done : {0}s", Time.time));
        progress = .4f;
        
        yield return StartCoroutine(Draw());
        progress = 1;
    }

//*******************************************************비트맵 생성 관련**************************************************************************************//

    /// <summary>
    /// 랜덤으로 블록을 생성하는 메소드(3D)
    /// </summary>
    /// <returns></returns>
    private IEnumerator RandomFillMap3D()
    {
        for (int y = 0; y < (floor <= height ? floor : height); y++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    float _x = Mathf.Abs(x - width * .5f) / (float)(width * .5f);
                    float _z = Mathf.Abs(z - depth * .5f) / (float)(depth * .5f);
                    float distance = Mathf.Pow(_x, 2) + Mathf.Pow(_z, 2);
                    float weight;
                    if (distance > 1.3f)
                    {
                        weight = 0.1f;
                    }
                    else if (distance > 1f)
                    {
                        weight = 0.75f;
                    }
                    else
                    {
                        weight = 1f;
                    }
                    map[x, y, z] = (UnityEngine.Random.Range(0, 100) < 100 * weight) ? 1 : 0;
                }
            }
        }
        yield return null;
        if (!this.blockTopia)
        {
            for (int y = floor; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        map[x, y, z] = (map[x, y - 1, z] == 1 && UnityEngine.Random.Range(0, 100) < 100 - y) ? 1 : 0;
                    }
                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// 랜덤으로 블록을 생성하는 메소드(2D)
    /// </summary>
    /// <returns></returns>
    private IEnumerator RandomFillMap2D()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                map[x, 0, z] = (UnityEngine.Random.Range(0, 100) < dungeonWallRandomPercent) ? 1 : 0;
            }
        }
        yield return null;
    }

    /// <summary>
    /// 굴곡을 완만하게 다듬는 메소드(3D)
    /// </summary>
    /// <returns></returns>
    private IEnumerator SmoothMap3D()
    {
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (x > 0 && x < width - 1 && z > 0 && z < depth - 1)
                    {
                        if (map[x, y, z] == 0)
                        {
                            if ((map[x + 1, y, z] == 1 && map[x - 1, y, z] == 1) || (map[x, y, z + 1] == 1 && map[x, y, z - 1] == 1))
                            {
                                map[x, y, z] = 1;
                            }
                        }
                    }

                    int neighbourWallTiles = GetSurroundingWallCount(x, y, z);

                    if (neighbourWallTiles > neighbourCount3D)
                    {
                        map[x, y, z] = 1;
                    }
                    else if (neighbourWallTiles < neighbourCount3D)
                    {
                        map[x, y, z] = 0;
                    }
                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// 굴곡을 완만하게 다듬는 메소드(2D)
    /// </summary>
    /// <returns></returns>
    private IEnumerator SmoothMap2D()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, z);

                if (neighbourWallTiles > neighbourCount2D)
                {
                    map[x, 0, z] = 1;
                }
                else if (neighbourWallTiles < neighbourCount2D)
                {
                    map[x, 0, z] = 0;
                }
            }
        }
        yield return null;
    }

    /// <summary>
    /// 주변의 블록 개수를 반환하는 메소드(3D)
    /// </summary>
    /// <param name="gridX"></param>
    /// <param name="gridY"></param>
    /// <param name="gridZ"></param>
    /// <returns></returns>
    private int GetSurroundingWallCount(int gridX, int gridY, int gridZ)
    {
        int wallCount = 0;
        
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                for (int neighbourZ = gridZ - 1; neighbourZ <= gridZ + 1; neighbourZ++)
                {
                    if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height && neighbourZ >= 0 && neighbourZ < depth)
                    {
                        if (neighbourX != gridX || neighbourY != gridY || neighbourZ != gridZ)
                        {
                            wallCount += map[neighbourX, neighbourY, neighbourZ];
                        }
                    }
                }
            }
        }

        return wallCount;
    }

    /// <summary>
    /// 주변의 블록 개수를 반환하는 메소드(2D)
    /// </summary>
    /// <param name="gridX"></param>
    /// <param name="gridZ"></param>
    /// <returns></returns>
    private int GetSurroundingWallCount(int gridX, int gridZ)
    {
        int wallCount = 0;

        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourZ = gridZ - 1; neighbourZ <= gridZ + 1; neighbourZ++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourZ >= 0 && neighbourZ < depth)
                {
                    if (neighbourX != gridX || neighbourZ != gridZ)
                    {
                        wallCount += map[neighbourX, 0, neighbourZ];
                    }
                }
            }
        }

        return wallCount;
    }

    /// <summary>
    /// 던전 내 벽을 만드는 메소드
    /// </summary>
    /// <returns></returns>
    private IEnumerator MakeWall()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                if (map[x, 0, z] == 1)
                {
                    for (int y = 1; y < height; y++)
                    {
                        map[x, y, z] = 1;
                    }
                }
            }
        }

        yield return null;
    }

    /// <summary>
    /// BSP 알고리즘으로 언덕을 생성한 후 맵에 적용하는 메소드
    /// </summary>
    /// <param name="nodeDepth">
    /// 노드 깊이
    /// </param>
    /// <param name="_height">
    /// 생성할 언덕의 높이 제한
    /// </param>
    /// <param name="hasFixedHeight">
    /// true일 경우 언덕 높이 일정하게 생성
    /// </param>
    /// <returns></returns>
    private IEnumerator BSP2Map(int nodeDepth, int _height = 0, bool hasFixedHeight = false)
    {
        roomDatas = BSPGenerate(width * .5f, depth * .5f, width, depth, (hasFixedHeight ? _height : height), nodeDepth, hasFixedHeight);

        for (int i = 0; i < roomDatas.Count; i++)
        {
            int centerX = Mathf.RoundToInt(roomDatas[i]["x"]);
            int centerY = Mathf.RoundToInt(roomDatas[i]["y"]);
            int row = Mathf.RoundToInt(roomDatas[i]["row"]);
            int col = Mathf.RoundToInt(roomDatas[i]["col"]);
            int height = Mathf.RoundToInt(roomDatas[i]["height"]);

            int gridX = Mathf.RoundToInt(row * .5f);
            int gridY = Mathf.RoundToInt(col * .5f);
            for (int h = 0; h < height; h++)
            {
                for (int x = centerX - gridX; x < centerX + gridX; x++)
                {
                    for (int y = centerY - gridY; y < centerY + gridY; y++)
                    {
                        map[x, h, y] = 1;
                    }
                }
            }
        }
        yield return null;
    }

    /// <summary>
    /// BSP 알고리즘을 수행하고 결과값을 반환하는 메소드
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="depth"></param>
    /// <param name="height"></param>
    /// <param name="nodeDepth"></param>
    /// <param name="hasFixedHeight"></param>
    /// <returns></returns>
    private List<Dictionary<string, float>> BSPGenerate(float x, float y, int width, int depth, int height, int nodeDepth, bool hasFixedHeight)
    {
        BSP bsp = new BSP(x, y, (float)width, (float)depth, (float)height, nodeDepth, hasFixedHeight);
        bsp.MakeChild(0);
        List<BSP> rooms = bsp.GetLeafNode();

        List<Dictionary<string, float>> roomDatas = new List<Dictionary<string, float>>();

        for (int i = 0; i < rooms.Count; i++)
        {
            Dictionary<string, float> values = rooms[i].GetValues();
            roomDatas.Add(values);
        }

        return roomDatas;
    }

    /// <summary>
    /// 언덕에 계단을 추가하는 메소드
    /// </summary>
    /// <param name="centerX"></param>
    /// <param name="centerY"></param>
    /// <param name="height"></param>
    /// <param name="gridX"></param>
    /// <param name="gridY"></param>
    private void SetStair(int centerX, int centerY, int height, int gridX, int gridY)
    {
        if (height <= floor + 2) return;

        bool setRow = gridX > gridY;
        int minX = centerX - gridX;
        int maxX = centerX + gridX;
        int minY = centerY - gridY;
        int maxY = centerY + gridY;
        int x = centerX;//UnityEngine.Random.Range(minX, maxX);
        int z = centerY;//UnityEngine.Random.Range(minY, maxY);
        int y = height - 2;
        int signX = centerX > width * .5f ? -1 : 1;
        int signY = centerY > depth * .5f ? -1 : 1;

        if (setRow)
        {
            while (map[x, y, z] == 1)
            {
                z += signY;
            }
            Debug.Log(map[x, y, z]);
            while (y > 0)
            {
                for (int i = 0; i <= y; i++)
                {
                    map[x, i, z] = 1;
                }
                if (x + signX < minX || x + signX >= maxX)
                {
                    signX *= -1;
                    z += signY;
                    for (int i = 0; i <= y; i++)
                    {
                        map[x, i, z] = 1;
                    }
                }
                x += signX;
                y--;
            }
        }
        else
        {
            while (map[x, y, z] == 1)
            {
                x += signX;
            }
            Debug.Log(map[x, y, z]);
            while (y > 0)
            {
                for (int i = 0; i <= y; i++)
                {
                    map[x, i, z] = 1;
                }
                if (z + signY < minY || z + signY >= maxY)
                {
                    signY *= -1;
                    x += signX;
                    for (int i = 0; i <= y; i++)
                    {
                        map[x, i, z] = 1;
                    }
                }
                z += signY;
                y--;
            }
        }
    }

//*******************************************************그리기 관련**************************************************************************************//

    /// <summary>
    /// 지형 오브젝트를 생성하고 배치하는 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator Draw()
    {
        PartManager.instance.RemoveAllChilds(partsRoot);
        if (map != null)
        {
            Vector3 rootPos = partsRoot.position;
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        if (map[x, y, z] == 1)
                        {
                            Part part = ChoosePart(x, y, z);
                        }
                    }
                    //yield return null;
                }
                Debug.Log(string.Format("Draw {0}layer done : {1}s", y, Time.time));
                yield return null;
                progress += (1 - .4f) * (1 / (float)height);
            }
            Debug.Log(string.Format("Draw All done : {0}s", Time.time));
        }
        yield return null;
    }

    /// <summary>
    /// 적절한 지형 오브젝트를 생성하는 메소드
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private Part ChoosePart(int x, int y, int z)
    {
        Part part = null;
        Quaternion rotateAngle = Quaternion.identity;
        int w = 0, h = 0, d = 0;

        // 6x6x6(1027)
        if (IsCube(x, y, z, 6, 6, 6) && y != 0)
        {
            w = 6;
            h = 6;
            d = 6;
            FillZero(x, y, z, w, h, d);
            rotateAngle = Quaternion.Euler(-90, 0, 180);
            part =  PartManager.instance.CreatePart(Vector3.zero, rotateAngle, 1027, partsRoot);
        }
        // 6x6x1(1025)
        else if (IsCube(x, y, z, 6, 1, 6))
        {
            w = 6;
            h = 1;
            d = 6;
            FillZero(x, y, z, w, h, d);
            rotateAngle = Quaternion.Euler(-90, 0, 180);
            part = PartManager.instance.CreatePart(Vector3.zero, rotateAngle, 1025, partsRoot);
        }
        // 3x3x3(1023)
        else if (IsCube(x, y, z, 3, 3, 3) && y != 0)
        {
            w = 3;
            h = 3;
            d = 3;
            FillZero(x, y, z, w, h, d);
            rotateAngle = Quaternion.Euler(-90, 0, 180);
            part = PartManager.instance.CreatePart(Vector3.zero, rotateAngle, 1023, partsRoot);
        }
        // 3x3x1(1021)
        else if (IsCube(x, y, z, 3, 1, 3))
        {
            w = 3;
            h = 1;
            d = 3;
            FillZero(x, y, z, w, h, d);
            rotateAngle = Quaternion.Euler(-90, 0, 180);
            part = PartManager.instance.CreatePart(Vector3.zero, rotateAngle, 1021, partsRoot);
        }
        // Normal(1001)
        // Half(1002)
        else if (IsCube(x, y, z, 1, 1, 1))
        {
            w = 1;
            h = 1;
            d = 1;
            FillZero(x, y, z, w, h, d);
            rotateAngle = Quaternion.Euler(-90, 0, 180);
            if (IsTop(x, y, z, w, h, d))
            {
                if (y < floor + 1)
                {
                    part = PartManager.instance.CreatePart(Vector3.zero, rotateAngle, 1001, partsRoot);
                }
                else
                {
                    part = PartManager.instance.CreatePart(Vector3.zero, rotateAngle, 1002, partsRoot);
                }
            }
            else
            {
                part = PartManager.instance.CreatePart(Vector3.zero, rotateAngle, 1001, partsRoot);
            }
        }

        if (IsTop(x, y, z, w, h, d))
        {
            if (part.partID == 1001 || part.partID == 1002)
            {
                PartManager.instance.SetPaintOfPart(part, grassDefault.materialID, grassDefault.materialColor);
            }
            else
            {
                if (grassRandom.Length != 0)
                {
                    int index = UnityEngine.Random.Range(0, grassRandom.Length);
                    PartManager.instance.SetPaintOfPart(part, grassRandom[index].materialID, grassRandom[index].materialColor);
                }
                else
                {
                    PartManager.instance.SetPaintOfPart(part, grassDefault.materialID, grassDefault.materialColor);
                }
            }
        }
        else
        {
            PartManager.instance.SetPaintOfPart(part, earthDefault.materialID, earthDefault.materialColor);
        }

        SetPos(part, x, y, z, rotateAngle);

        return part;
    }

    /// <summary>
    /// Cube 생성이 가능한지 판단하는 메소드
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    /// <param name="_w"></param>
    /// <param name="_h"></param>
    /// <param name="_d"></param>
    /// <returns></returns>
    private bool IsCube(int _x, int _y, int _z, int _w, int _h, int _d)
    {
        if (_x + _w > width || _y + _h > height || _z + _d > depth)
        {
            return false;
        }

        for (int y = _y; y < _y + _h; y++)
        {
            for (int x = _x; x < _x + _w; x++)
            {
                for (int z = _z; z < _z + _d; z++)
                {
                    if (map[x, y, z] == 0)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// 맨 위 오브젝트인지 판단하는 메소드
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    /// <param name="_w"></param>
    /// <param name="_h"></param>
    /// <param name="_d"></param>
    /// <returns></returns>
    private bool IsTop(int _x, int _y, int _z, int _w, int _h, int _d)
    {
        if (_y + _h == height)
        {
            return true;
        }

        int total = 0;
        int top = 0;
        for (int x = _x; x < _x + _w; x++)
        {
            for (int z = _z; z < _z + _d; z++)
            {
                if (map[x, _y + _h, z] == 0)
                {
                    top++;
                }
                total++;
            }
        }

        if (top >= total * 0.5f)
            return true;
        return false;
    }

    /// <summary>
    /// Cube를 생성한 부분을 비활성화하는 메소드
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    /// <param name="_w"></param>
    /// <param name="_h"></param>
    /// <param name="_d"></param>
    private void FillZero(int _x, int _y, int _z, int _w, int _h, int _d)
    {
        for (int y = _y; y < _y + _h; y++)
        {
            for (int x = _x; x < _x + _w; x++)
            {
                for (int z = _z; z < _z + _d; z++)
                {
                    map[x, y, z] = 0;
                }
            }
        }
    }

    /// <summary>
    /// 오브젝트가 놓일 위치를 계산하는 메소드
    /// </summary>
    /// <param name="part"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="rotateAngle"></param>
    /// <returns></returns>
    private void SetPos(Part part, int x, int y, int z, Quaternion rotateAngle)
    {
        Vector3 pos = transform.InverseTransformPoint(partsRoot.position) + new Vector3((-width * .5f + x + .5f) * .1f, (y - 1.5f) * .1f, (-depth * .5f + z + .5f) * .1f);

        BoxCollider boxCollider = part.transform.GetComponent<BoxCollider>();
        Vector3 point = boxCollider.bounds.center - boxCollider.bounds.extents + Vector3.one * .05f;
        point = transform.InverseTransformPoint(point);
        part.transform.localPosition -= (point - pos);
    }
}
