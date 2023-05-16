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
            Debug.Log(stagedata.ToString());
        }
        else
        {
            Debug.Log(stageDataFileName + " 파일을 찾을 수 없음!");
            
            // 임의 데이터
            selectedLevel = 1;
            selectedStage = 1;

            stagedata = new StageData();
            Stage stage = new Stage();
            stage.stageNo = 1;
            stage.clear = false;
            stage.blocks = new int[] { 0, 1, 0, 0, 1, 1, 0, 0, 1 };
            Level level = new Level();
            level.level = 1;
            level.stagelist = new Stage[] { stage };
            stagedata.levellist = new Level[] { level };
            Debug.Log("임시 데이터 stagedata 저장");
            Debug.Log(stagedata.levellist[0].stagelist[0].blocks.Length);
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
