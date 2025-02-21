using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ScrollController : SerializedMonoBehaviour
{
    public static ScrollController instance;

    [SerializeField]
    ScrollData scrollData;

    [SerializeField] 
    Dictionary<ELayerType, List<GameObject>> layerGroupDic;

    const float WIDTH = 38.4f;
    const float DEADLINE = -35.0f;

    void Awake()
    {
        InitProperty();
    }

    void InitProperty()
    {
        instance = this;

        scrollData.validation = 1f;
        scrollData.boosterModifier = 1f;
        scrollData.superBoosterModifier = 1f;
    }

    void Update()
    {
        Scroll();
        ObservePosition();
    }

    void Scroll()
    {
        for (int i = 0; i < Enum.GetValues(typeof(ELayerType)).Length; i++)
            ScrollLogic(layerGroupDic[(ELayerType)i], (ELayerType)i);
    }

    void ScrollLogic(List<GameObject> layers, ELayerType layerType)
    {
        foreach (var item in layers)
            item?.transform.Translate(Vector2.left * scrollData.scrollSpeedDic[layerType] * scrollData.boosterModifier * scrollData.superBoosterModifier * scrollData.validation * Time.deltaTime);
    }

    void ObservePosition()
    {
        for (int i = 0; i < Enum.GetValues(typeof(ELayerType)).Length; i++)
            ObservePositionLogic(layerGroupDic[(ELayerType)i], (ELayerType)i);
    }

    void ObservePositionLogic(List<GameObject> layers, ELayerType layerType)
    {
        GameObject go = layers[0];

        if (layerType == ELayerType.Floor)
        {
            if (go != null && go.transform.position.x <= DEADLINE)
            {
                Replace(layers);
            }
        }
        else
        {
            if (go != null && go.transform.position.x <= DEADLINE)
            {
                Reposition(layers);
            }
        }
    }

    void Reposition(List<GameObject> list)
    {
        GameObject go = list[0];
        list.RemoveAt(0);

        go.transform.position = list[list.Count - 1].transform.position + new Vector3(WIDTH, 0f, 0f); ;

        list.Add(go);
    }

    void Replace(List<GameObject> list)
    {
        GameObject go = list[0];
        list.RemoveAt(0);
        Destroy(go);

        GameObject spawnedFloor =  FloorController.instance.SpawnFloor(GameManager.instance.gameData.chapterType);
        spawnedFloor.transform.position = list[list.Count - 1].transform.position + new Vector3(WIDTH, 0f, 0f);
        spawnedFloor.transform.SetParent(transform);

        list.Add(spawnedFloor);
    }

    public void StopScrolling(float duration)
    {
        StartCoroutine(StopScrollingByTime(duration));
    }

    IEnumerator StopScrollingByTime(float duration)
    {
        scrollData.validation = 0f;

        yield return new WaitForSeconds(duration);

        scrollData.validation = 1f;
    }

    public void StopScrolling()
    {
        scrollData.validation = 0f;
    }

    public void PlayScrolling()
    {
        scrollData.validation = 1f;
    }

    public void SetScrollModifier(float value)
    {
        scrollData.boosterModifier = Mathf.Max(1, value);
    }

    public void SetSuperBoosterModifier(float value)
    {
        scrollData.superBoosterModifier = Mathf.Max(1, value);
    }

    public void InitFloor(List<GameObject> list, int count = 2)
    {
        #region Clear List
        List<GameObject> temp = layerGroupDic[ELayerType.Floor];

        while (temp.Count > 0)
        {
            var floor = temp[0];

            temp.RemoveAt(0);
            Destroy(floor);
        }
        #endregion

        #region Add Floor
        for (int i = 0; i < count; i++)
        {
            if (i == 0)
                list[i].transform.localPosition = Vector3.zero;
            else
                list[i].transform.localPosition = list[i - 1].transform.position + new Vector3(WIDTH, 0f, 0f);

            layerGroupDic[ELayerType.Floor].Add(list[i]);
        }
        #endregion
    }
}
