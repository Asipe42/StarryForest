using UnityEditor;
using UnityEngine;
using System.IO;

public class CSVConverter
{
    static string PATH_PROGRESS = "/Resources/Data/Progress/ProgressCSV.csv";

    [MenuItem("Util/Generate Progress Value")]
    public static void GenerateProgressValue()
    {
        string[] lines = File.ReadAllLines(Application.dataPath + PATH_PROGRESS);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] splitData = lines[i].Split(',');

            ProgressData progressValue = ScriptableObject.CreateInstance<ProgressData>();
            progressValue.index = int.Parse(splitData[0]);
            progressValue.value = int.Parse(splitData[1]);

            AssetDatabase.CreateAsset(progressValue, $"Assets//Resources/Data/Progress/Chapter{progressValue.index}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}
