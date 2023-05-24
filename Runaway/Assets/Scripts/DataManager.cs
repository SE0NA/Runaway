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

            //복호화
            fromJsonData = Crypto.AESDecrypt128(fromJsonData);

            stagedata = JsonUtility.FromJson<StageData>(fromJsonData);
            Debug.Log(stageDataFileName + " 불러오기 성공!");
        }
        else
        {
            // 파일이 존재하지 않으면 Resources 에서 기본 파일을 가져와 저장
            TextAsset resourceData = Resources.Load(stageDataFileName) as TextAsset;
            stagedata = JsonUtility.FromJson<StageData>(resourceData.ToString());
            SaveStageData();
            Debug.Log(stageDataFileName + " 파일을 찾을 수 없음! 새로운 파일을 Resources로부터 생성");
        }

        // 업데이트 스테이지
        if(stagedata.version != Application.version)
        {
            stagedata = UpdataStageData();
            SaveStageData();
        }

    }
    StageData UpdataStageData()
    {
        StageData newSD = new StageData();

        // 리소스 파일(업데이트 파일)에 기존 파일 내용 입력
        TextAsset resourceData = Resources.Load(stageDataFileName) as TextAsset;
        newSD = JsonUtility.FromJson<StageData>(resourceData.ToString());

        foreach (Level l in stagedata.levellist)
            foreach (Stage s in stagedata.levellist[l.level - 1].stagelist)
            {
                newSD.levellist[l.level - 1].stagelist[s.stageNo - 1] = s;
            }

        // 이미 모든 스테이지가 클리어된 스테이지
        // 기존 마지막 스테이지의 클리어 여부에 따라 그 다음 스테이지(new)의 unLock 설정
        foreach (Level l in stagedata.levellist)
            if (l.stagelist[l.stagelist.Length - 1].clear)
                newSD.levellist[l.level - 1].stagelist[l.stagelist.Length].unLock = true;

        Debug.Log(stageDataFileName + " 파일 업데이트 완료! >> newSD.length: " + newSD.levellist[0].stagelist.Length);

        
        return newSD;
    }

    public void SaveStageData()
    {
        string toJsonData = JsonUtility.ToJson(stagedata, true);
        string filePath = Application.persistentDataPath + "/" + stageDataFileName;

        // 암호화
        toJsonData = Crypto.AESEncrypt128(toJsonData);

        File.WriteAllText(filePath, toJsonData);
        Debug.Log(stageDataFileName + "파일 데이터 저장 완료");
    }

    public void FileDelete()
    {
        // PlayerPrefs
        PlayerPrefs.DeleteAll();

        string filePath = Application.persistentDataPath + "/" + stageDataFileName;
        if(File.Exists(filePath))
            File.Delete(filePath);

        Debug.Log("게임 데이터 초기화 완료!");
    }
}
