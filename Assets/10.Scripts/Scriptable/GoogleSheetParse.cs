using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GoogleSheetParse", order = 1)]
public class GoogleSheetParse : ScriptableObject
{
    [SerializeField] private string sheetID;
    [SerializeField] private List<string> sheetName;
    [SerializeField] private UnityEngine.Object saveFolder;

    public async void ParseData()
    {
        string savePath = AssetDatabase.GetAssetPath(saveFolder);
        savePath += "/{0}.json";

        foreach (var item in sheetName)
        {
            string url = $"https://opensheet.elk.sh/{sheetID}/{item}";
            var value = await Request(url);
            Debug.Log(value);
            using (FileStream fs = new FileStream(string.Format(savePath, item), FileMode.Create))
            {
                if (fs == null)
                {
                    return;
                }
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(value);
                }
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Complete");
    }

    private static async Task<string> Request(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"HTTP ERROR {request.error} {url}");
        }
        while (!request.isDone)
        {
            await Task.Delay(10);
        }
        return request.downloadHandler.text;
    }
}

[CustomEditor(typeof(GoogleSheetParse))]
public class ParseScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GoogleSheetParse Parse = (GoogleSheetParse)target;

        GUILayout.Space(20);
        if (GUILayout.Button("Parse Data"))
        {
            Parse.ParseData();
        }
    }
}
#endif