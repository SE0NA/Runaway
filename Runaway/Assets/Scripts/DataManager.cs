using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;
using Unity.VisualScripting;
using UnityEditor;

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

    // Gamedata ///////////
    public bool isHaptic = true;


    // LevelData ////////////////////////////////////////////////////////////

    string levelDataFileName = "leveldata";
    string stageDataFileName = "stagedata";
    public LevelData leveldata = new LevelData();
    public StageData stagedata = new StageData();

    public void LoadLevelData()
    {
        string filePath = Application.persistentDataPath + "/" + levelDataFileName;

        if (File.Exists(filePath))
        {
            string fromJsonData = File.ReadAllText(filePath);
            fromJsonData = Crypto.AESDecrypt128(fromJsonData);

            leveldata = JsonUtility.FromJson<LevelData>(fromJsonData);
            Debug.Log(levelDataFileName + " 불러오기 성공! -> total level: " + leveldata.levellist.Length);
        }
        else
        {
            // 파일이 존재하지 않으면 Resources 에서 기본 파일을 가져와 저장
            TextAsset resourceData = Resources.Load(levelDataFileName) as TextAsset;
            leveldata = JsonUtility.FromJson<LevelData>(resourceData.ToString());
            SaveLevelData();
            Debug.Log(levelDataFileName + " 파일을 찾을 수 없음! 새로운 파일을 Resources로부터 생성");
        }

        // 업데이트 스테이지
        if (leveldata.version != Application.version)
        {
            leveldata = UpdateLevelData();
            SaveLevelData();
        }
    }

    public void SaveLevelData()
    {
        
        string toJsonData = JsonUtility.ToJson(leveldata, true);
        string filePath = Application.persistentDataPath + "/" + levelDataFileName;

        // 암호화
        toJsonData = Crypto.AESEncrypt128(toJsonData);

        File.WriteAllText(filePath, toJsonData);
        Debug.Log(levelDataFileName + "파일 데이터 저장 완료");
    }

    LevelData UpdateLevelData()
    {
        LevelData newLD = new LevelData();

        // 리소스 파일(업데이트 파일)에 기존 파일 내용 입력
        TextAsset resourceData = Resources.Load(levelDataFileName) as TextAsset;
        newLD = JsonUtility.FromJson<LevelData>(resourceData.ToString());

        foreach(Level l in leveldata.levellist)
        {
            newLD.levellist[l.level - 1] = l;
        }

        Debug.Log(levelDataFileName + " 파일 업데이트 완료! >> newSD.length: " + newLD.levellist.Length);

        // 스테이지 업데이트
        foreach(Level l in newLD.levellist)
        {
            UpdateStageData(stageDataFileName + l.level.ToString());
        }

        return newLD;
    }

    public void LoadStageData()
    {
        string filePath = Application.persistentDataPath + "/" + stageDataFileName + selectedLevel.ToString();

        if (File.Exists(filePath))
        {
            string fromJsonData = File.ReadAllText(filePath);

            //복호화
            fromJsonData = Crypto.AESDecrypt128(fromJsonData);

            stagedata = JsonUtility.FromJson<StageData>(fromJsonData);

            Debug.Log(fromJsonData);
            Debug.Log(stagedata.level + " 불러오기 성공!");
        }
        else
        {
            // 파일이 존재하지 않으면 Resources 에서 기본 파일을 가져와 저장
            TextAsset resourceData = Resources.Load(stageDataFileName + selectedLevel) as TextAsset;
            stagedata = JsonUtility.FromJson<StageData>(resourceData.ToString());
            SaveStageData();
            Debug.Log(stageDataFileName + " 파일을 찾을 수 없음! 새로운 파일을 Resources로부터 생성");
        }
        GetRestHeart();

    }


    void UpdateStageData(string fileName)
    {
        StageData newSD = new StageData();
        StageData restSD = new StageData();

        // 리소스 파일(업데이트 파일)에 기존 파일 내용 입력
        TextAsset resourceData = Resources.Load(fileName) as TextAsset;
        newSD = JsonUtility.FromJson<StageData>(resourceData.ToString());

        // 기존 데이터
        string filePath = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(filePath))
        {
            string restData = File.ReadAllText(filePath);
            restData = Crypto.AESDecrypt128(restData);
            restSD = JsonUtility.FromJson<StageData>(restData);

            foreach (Stage s in restSD.stagelist)
            {
                newSD.stagelist[s.stageNo - 1] = s;
            }

            Debug.Log(fileName + " 파일 업데이트 완료! >>  스테이지 수: " + newSD.stagelist.Length);
        }
    }

    public void SaveStageData()
    {
        string toJsonData = JsonUtility.ToJson(stagedata, true);
        string filePath = Application.persistentDataPath + "/" + stageDataFileName + stagedata.level;

        // 암호화
        toJsonData = Crypto.AESEncrypt128(toJsonData);

        File.WriteAllText(filePath, toJsonData);
        Debug.Log(stageDataFileName + "파일 데이터 저장 완료");
    }

    public void FileDelete()
    {
        // PlayerPrefs
        PlayerPrefs.DeleteAll();

        string filePath;

        foreach (Level l in leveldata.levellist)
        {
            filePath = Application.persistentDataPath + "/" + stageDataFileName + l.level;
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        filePath = Application.persistentDataPath + "/" + levelDataFileName;
        if (File.Exists(filePath))
            File.Delete(filePath);

        Debug.Log("게임 데이터 초기화 완료!");
    }


    public void GetRestHeart()
    {
        string data = PlayerPrefs.GetString("restdata","0");
        if (data.Equals("0"))   // 파일 없었음
        {
            SetRestHeart(3);
            ReplayBtn.rest = 3;
        }
        else
        {
            int rest = Int32.Parse(Crypto.AESDecrypt128(data));
            Debug.Log("남은 replay: " + rest);

            if (rest > 3) rest = 3;

            ReplayBtn.rest = rest;
        }
    }
    public void SetRestHeart(int rest)
    {
        string data = Crypto.AESEncrypt128(rest.ToString());
        PlayerPrefs.SetString("restdata", data);
    }
}
