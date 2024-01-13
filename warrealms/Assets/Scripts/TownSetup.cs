using CityBuilderCore;
using System.Collections.Generic;
using UnityEngine;

namespace CityBuilderTown
{
    /// <summary>
    /// generates a random terrain at the start of the game<br/>
    /// can be tested in the editor using the buttons in the inspector<br/>
    /// theres a save test scene in CityBuilderTown.Tests/Setup/TownSetupTest.unity
    /// </summary>
    public class TownSetup : MonoBehaviour
    {
        public Terrain Terrain;

        [Header("Map")]
        [Tooltip("multiplier for the perlin noise")]
        public float HeightScale = 2f;
        [Tooltip("move down so 0 is middle, 0.4 leaves more mountains than water")]
        public float HeightOffsetDown = 0.4f;
        [Tooltip("middle area that is flattened out")]
        public float HeightFlat = 0.2f;
        [Tooltip("divides noise values, higher value for flatter terrain")]
        public float HeightFactor = 1.2f;
        [Tooltip("moves values up again so they are 0-1")]
        public float HeightOffsetUp = 0.5f;
        [Tooltip("height that is applied to the borders")]
        public float HeightBottom = 0.35f;
        [Tooltip("height at which the ground is green")]
        public float GrassMid = 75f;
        [Tooltip("area for green ground to fade out around")]
        public float GrassArea = 5f;

        [Header("Trees")]
        [Tooltip("index of the terrain tree")]
        public int TreeIndex = 0;
        [Tooltip("how many fields to move between checks(1 checks every field)")]
        public int TreeStep = 5;
        [Tooltip("height and width of the individual objects is 0.8-1.2 times this value")]
        public float TreeSize = 0.5f;
        [Tooltip("multiplier for the perlin noise")]
        public float TreeScale = 5f;
        [Tooltip("value of noise from which a tree is added")]
        public float TreeThreshold = 0.6f;
        [Tooltip("minimum height of the terrain for objects to be added(no trees in the ocean)")]
        public float TreeHeightMin = 74;
        [Tooltip("maximum height of the terrain for objects to be added(no bushes on mountain tops)")]
        public float TreeHeightMax = 85;

        [Header("Bushes")]
        [Tooltip("index of the terrain tree")]
        public int BushIndex = 3;
        [Tooltip("how many fields to move between checks(1 checks every field)")]
        public int BushStep = 3;
        [Tooltip("height and width of the individual objects is 0.8-1.2 times this value")]
        public float BushSize = 0.25f;
        [Tooltip("multiplier for the perlin noise")]
        public float BushScale = 10f;
        [Tooltip("value of noise from which a tree is added")]
        public float BushThreshold = 0.85f;
        [Tooltip("minimum height of the terrain for objects to be added(no trees in the ocean)")]
        public float BushHeightMin = 74;
        [Tooltip("maximum height of the terrain for objects to be added(no bushes on mountain tops)")]
        public float BushHeightMax = 90;

        [Header("Rocks")]
        [Tooltip("index of the terrain tree")]
        public int RockIndex = 1;
        [Tooltip("how many fields to move between checks(1 checks every field)")]
        public int RockStep = 2;
        [Tooltip("height and width of the individual objects is 0.8-1.2 times this value")]
        public float RockSize = 20f;
        [Tooltip("multiplier for the perlin noise")]
        public float RockScale = 200f;
        [Tooltip("value of noise from which a tree is added")]
        public float RockThreshold = 0.88f;
        [Tooltip("minimum height of the terrain for objects to be added(no trees in the ocean)")]
        public float RockHeightMin = 75;
        [Tooltip("maximum height of the terrain for objects to be added(no bushes on mountain tops)")]
        public float RockHeightMax = 999;
        
        // [Header("Gold")]
        // [Tooltip("index of the terrain tree")]
        // public int GoldIndex = 5;
        // [Tooltip("how many fields to move between checks(1 checks every field)")]
        // public int GoldStep = 2;
        // [Tooltip("height and width of the individual objects is 0.8-1.2 times this value")]
        // public float GoldSize = 0.85f;
        // [Tooltip("multiplier for the perlin noise")]
        // public float GoldScale = 20f;
        // [Tooltip("value of noise from which a gold is added")]
        // public float GoldThreshold = 0.88f;
        // [Tooltip("minimum height of the terrain for objects to be added(no trees in the ocean)")]
        // public float GoldHeightMin = 75;
        // [Tooltip("maximum height of the terrain for objects to be added(no bushes on mountain tops)")]
        // public float GoldHeightMax = 999;

        [Header("Details")]
        [Tooltip("how many fields to move between checks(1 checks every field)")]
        public int DetailStep = 3;
        [Tooltip("multiplier for the perlin noise")]
        public float DetailScale = 2f;
        [Tooltip("value of noise from which a tree is added")]
        public float DetailThreshold = 0.7f;
        [Tooltip("height at which details are added")]
        public float DetailHeight = 75;

        public void Setup()
        {
            var data = Terrain.terrainData;

            var parameters = Dependencies.GetOptional<IMissionManager>()?.MissionParameters;

            var seed = parameters?.RandomSeed ?? System.DateTime.Now.Millisecond;

            //HEIGHT

            var h = data.GetHeights(0, 0, data.heightmapResolution, data.heightmapResolution);

            var heightScale = data.heightmapResolution / HeightScale;
            var heightFlat = HeightFlat;
            var heightSeed = seed;

            for (int x = 0; x < data.heightmapResolution; x++)
            {
                for (int y = 0; y < data.heightmapResolution; y++)
                {
                    var value = Mathf.PerlinNoise(x / heightScale + heightSeed, y / heightScale + heightSeed);

                    value -= HeightOffsetDown;//move down so 0 is middle, 0.4 leaves more mountains than water

                    //completely flatten out middle area
                    if (value < -heightFlat)
                        value += heightFlat;
                    else if (value > heightFlat)
                        value -= heightFlat;
                    else
                        value = 0f;

                    value /= HeightFactor;//smooth out all
                    value += HeightOffsetUp;//move up into 0-1

                    h[x, y] = value;
                }
            }

            var heightBottom = HeightBottom;

            for (int i = 0; i < data.heightmapResolution; i++)
            {
                h[i, 0] = heightBottom;
                h[0, i] = heightBottom;
                h[data.heightmapResolution - 1, i] = heightBottom;
                h[i, data.heightmapResolution - 1] = heightBottom;
            }

            data.SetHeights(0, 0, h);

            //SPLAT

            var s = new float[data.alphamapWidth, data.alphamapHeight, data.alphamapLayers];

            var splatMid = GrassMid;
            var splatArea = GrassArea;

            for (int y = 0; y < data.alphamapWidth; y++)
            {
                for (int x = 0; x < data.alphamapHeight; x++)
                {
                    var h_x = x / (float)data.alphamapWidth;
                    var h_y = y / (float)data.alphamapHeight;

                    var height = data.GetHeight(Mathf.RoundToInt(h_y * data.heightmapResolution), Mathf.RoundToInt(h_x * data.heightmapResolution));

                    float floor;
                    float sand;

                    if (height > splatMid + splatArea || height < splatMid - splatArea)
                    {
                        floor = 0f;
                        sand = 1f;
                    }
                    else
                    {
                        var delta = Mathf.Abs(height - splatMid) / splatArea;

                        floor = 1 - delta;
                        sand = delta;
                    }

                    s[x, y, 0] = floor;
                    s[x, y, 1] = sand;
                }
            }

            data.SetAlphamaps(0, 0, s);

            //TREES

            var t = new List<TreeInstance>();

            var treeScale = data.size.x / TreeScale;
            var treeThreshold = TreeThreshold;
            var treeSeed = seed * 2;

            var bushScale = data.size.x / BushScale;
            var bushThreshold = BushThreshold;
            var bushSeed = seed * 5;

            var rockScale = data.size.x / RockScale;
            var rockThreshold = RockThreshold;
            var rockSeed = seed + 50;   
            
            // var goldScale = data.size.x / GoldScale;
            // var goldThreshold = GoldThreshold;
            // var goldSeed = seed + 50;

            addTree(data, t,
                TreeIndex, TreeStep, TreeSize,
                treeScale, treeSeed, treeThreshold,
                TreeHeightMin, TreeHeightMax);

            addTree(data, t,
                BushIndex, BushStep, BushSize,
                bushScale, bushSeed, bushThreshold,
                BushHeightMin, BushHeightMax,
                (int x, int y) => !hasTree(x, y, treeScale, treeSeed, treeThreshold));

            addTree(data, t,
                RockIndex, RockStep, RockSize,
                rockScale, rockSeed, rockThreshold,
                RockHeightMin, RockHeightMax,
                (int x, int y) => !hasTree(x, y, treeScale, treeSeed, treeThreshold) && !hasTree(x, y, bushScale, bushSeed, bushThreshold));
            
            // addTree(data, t,
            //     GoldIndex, GoldStep, GoldSize,
            //     goldScale, goldSeed, goldThreshold,
            //     GoldHeightMin, GoldHeightMax,
            //     (int x, int y) => !hasTree(x, y, treeScale, treeSeed, treeThreshold) && !hasTree(x, y, bushScale, bushSeed, bushThreshold));

            data.SetTreeInstances(t.ToArray(), true);

            //DETAILS

            var detailStep = DetailStep;
            var detailScale = data.detailResolution / DetailScale;

            var d = new int[data.detailWidth, data.detailHeight];

            for (int y = 0; y < data.detailWidth - detailStep; y += detailStep)
            {
                for (int x = 0; x < data.detailHeight - detailStep; x += detailStep)
                {
                    var h_x = x / (float)data.detailWidth;
                    var h_y = y / (float)data.detailHeight;

                    var height = data.GetHeight(Mathf.RoundToInt(h_y * data.heightmapResolution), Mathf.RoundToInt(h_x * data.heightmapResolution));

                    if (height != DetailHeight)
                        continue;

                    var value = Mathf.PerlinNoise(x / detailScale + heightSeed, y / detailScale + heightSeed);

                    if (Random.Range(0f, DetailThreshold) > value)
                        d[x + Random.Range(0, detailStep), y + Random.Range(0, detailStep)] = 1;
                }
            }

            data.SetDetailLayer(0, 0, 0, d);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(data);
#endif
        }

        public void Clear()
        {
            var data = Terrain.terrainData;

            var h = data.GetHeights(0, 0, data.heightmapResolution, data.heightmapResolution);

            for (int i = 0; i < data.heightmapResolution; i++)
            {
                for (int j = 0; j < data.heightmapResolution; j++)
                {
                    h[i, j] = 0.5f;
                }
            }

            data.SetHeights(0, 0, h);

            var s = new float[data.alphamapWidth, data.alphamapHeight, data.alphamapLayers];

            for (int y = 0; y < data.alphamapWidth; y++)
            {
                for (int x = 0; x < data.alphamapHeight; x++)
                {
                    s[x, y, 0] = 1;
                }
            }

            data.SetAlphamaps(0, 0, s);

            data.SetTreeInstances(new TreeInstance[] { }, false);

            data.SetDetailLayer(0, 0, 0, new int[data.detailWidth, data.detailHeight]);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(data);
#endif
        }

        private bool hasTree(int x, int y, float scale, float seed, float threshold)
        {
            return Mathf.PerlinNoise(x / scale + seed, y / scale + seed) > threshold;
        }

        private void addTree(TerrainData data, List<TreeInstance> t,
            int index, int step, float heightScale,
            float scale, float seed, float threshold,
            float minHeight, float maxHeight,
            System.Func<int, int, bool> prerequisite = null)
        {
            for (int x = 0; x < data.size.x - step; x += step)
            {
                for (int y = 0; y < data.size.z - step; y += step)
                {
                    if (prerequisite != null && !prerequisite(x, y))
                        continue;

                    if (hasTree(x, y, scale, seed, threshold))
                    {
                        var position = new Vector3(
                            x + Random.Range(0.5f, step - 0.5f),
                            0,
                            y + Random.Range(0.5f, step - 0.5f));

                        var height = Terrain.SampleHeight(new Vector3(x, 0, y));
                        if (height > maxHeight || height < minHeight)
                            continue;

                        var color = 1f - Random.Range(0, 0.4f);

                        t.Add(new TreeInstance()
                        {
                            prototypeIndex = index,
                            position = new Vector3(position.x / data.size.x, position.y / data.size.y, position.z / data.size.z),
                            heightScale = Random.Range(0.8f * heightScale, 1.2f * heightScale),
                            widthScale = Random.Range(0.8f * heightScale, 1.2f * heightScale),
                            color = new Color(color, color, color),
                            lightmapColor = Color.white
                        });
                    }
                }
            }
        }
    }
}
