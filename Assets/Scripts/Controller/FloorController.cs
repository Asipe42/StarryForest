using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum EDifficultyType
{
    Easy, 
    Normal, 
    Hard
}

public class FloorController : SerializedMonoBehaviour
{
    public static FloorController instance;

    [SerializeField]
    FloorData floorData;

    [SerializeField]
    GameObject floorGroup;

    void Awake()
    {
        instance = this;
    }

    public GameObject SpawnFloor(EChapterType chapterType)
    {
        #region Add Progress Point
        ProgressController.instance.AddPoint();
        #endregion

        EDifficultyType difficultyType = JudgeDifficulty();
        
        int ranIndex = UnityEngine.Random.Range(0, floorData.candidate[chapterType][difficultyType].floors.Length);

        GameObject spawnedFloor = Instantiate(floorData.candidate[chapterType][difficultyType].floors[ranIndex]);

        return spawnedFloor;
    }

    EDifficultyType JudgeDifficulty()
    {
        EDifficultyType result = EDifficultyType.Easy;

        for (int i = Enum.GetValues(typeof(EDifficultyType)).Length - 1; i >= 0; i--)
        {
            if (floorData.difficultyTable[(EDifficultyType)i] < BoosterController.instance.BoosterRatio)
            {
                result = (EDifficultyType)i;
                break;
            }
        }

        return result;
    }

    public void ReplaceFloor(EChapterType type)
    {
        int count = 2;
        List<GameObject> list = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(floorData.defaultFloors[type]);

            go.transform.SetParent(floorGroup.transform);

            list.Add(go);
        }

        ScrollController.instance.InitFloor(list);
    }
}