using System.Collections;
using UnityEngine;
using Unit;

public class HourGlassController : MonoBehaviour
{
    public static HourGlassController instance;
    public static HourGlass spawnedHourGlass;

    [SerializeField] 
    GameObject hourGlass;

    [SerializeField]
    Vector3 spawnPosTop, spawnPosBottom;

    const float SPAWN_MIN_TIME = 10f;
    const float SPAWN_MAX_TIME = 20f;

    bool onSpawn;
    public bool OnSpawn
    {
        get => onSpawn;
        set => onSpawn = value;
    }

    private void Awake()
    {
        instance = this;
    }

    public void ForcedSpawn()
    {
        Vector3 spawnPos = new Vector3(spawnPosTop.x, Random.Range(spawnPosBottom.y, spawnPosTop.y), spawnPosTop.z);

        spawnedHourGlass = Instantiate(hourGlass, spawnPos, Quaternion.identity).GetComponent<HourGlass>();
    }

    public void SpawnHourGlass()
    {
        if (onSpawn)
            return;

        if (FeverController.instance.OnFever)
            return;

        onSpawn = true;

        StartCoroutine(SpawnHourGlassCoroutine());
    }

    IEnumerator SpawnHourGlassCoroutine()
    {
        while (OnSpawn)
        {
            float cooltime = Random.Range(SPAWN_MIN_TIME, SPAWN_MAX_TIME);

            yield return new WaitForSeconds(cooltime);

            Vector3 spawnPos = new Vector3(spawnPosTop.x, Random.Range(spawnPosBottom.y, spawnPosTop.y), spawnPosTop.z);

            spawnedHourGlass = Instantiate(hourGlass, spawnPos, Quaternion.identity).GetComponent<HourGlass>();
        }
    }
}
