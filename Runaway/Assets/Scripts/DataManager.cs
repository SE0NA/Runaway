using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public int selectedLevel;
    public int selectedStage;

    static GameObject _container;

    static DataManager _instance;
    public static DataManager instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataManager";
                _instance = _container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }

    string stageDataFileName = "Stages.json";

    public StageData stagedata = new StageData();

    public void LoadStageData()
    {
        string filePath = Application.persistentDataPath + "/" + stageDataFileName;
        if (File.Exists(filePath))
        {
            string fromJsonData = File.ReadAllText(filePath);
            stagedata = JsonUtility.FromJson<StageData>(fromJsonData);
            Debug.Log(stageDataFileName + " 불러오기 성공!");
        }
        else
        {
            Debug.Log(stageDataFileName + " 파일을 찾을 수 없음!");
        }
    }
    public void SaveStageData()
    {
        string toJsonData = JsonUtility.ToJson(stagedata, true);
        string filePath = Application.persistentDataPath + "/" + stageDataFileName;

        File.WriteAllText(filePath, toJsonData);
        Debug.Log(stageDataFileName + "파일 데이터 저장 완료");
    }
}
