using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public int selectedLevel = 1;
    public int selectedStage = 1;

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

    string stageDataFileName = "stagedata";

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
            // 파일이 존재하지 않으면 Resources 에서 기본 파일을 가져와 저장
            TextAsset resourceData = Resources.Load(stageDataFileName) as TextAsset;
            stagedata = JsonUtility.FromJson<StageData>(resourceData.ToString());
            SaveStageData();
            Debug.Log(stageDataFileName + " 파일을 찾을 수 없음! 새로운 파일을 Resources로부터 생성");
        }

        // 업데이트된 스테이지 추가
        if(stagedata.version != Application.version)
        {
            // 각 레벨 별로 길이 비교.
            // 리소스의 추가된 부분부터 붙여넣기
            // stagedata에 붙이고 저장
        }

        Debug.Log("stagedata: " + stagedata.levellist[1].stagelist[0].blocks.Length);
    }
    public void SaveStageData()
    {
        string toJsonData = JsonUtility.ToJson(stagedata, true);
        string filePath = Application.persistentDataPath + "/" + stageDataFileName;

        File.WriteAllText(filePath, toJsonData);
        Debug.Log(stageDataFileName + "파일 데이터 저장 완료");
    }
}
