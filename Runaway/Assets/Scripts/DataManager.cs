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
            Debug.Log(levelDataFileName + " �ҷ����� ����! -> total level: " + leveldata.levellist.Length);
        }
        else
        {
            // ������ �������� ������ Resources ���� �⺻ ������ ������ ����
            TextAsset resourceData = Resources.Load(levelDataFileName) as TextAsset;
            leveldata = JsonUtility.FromJson<LevelData>(resourceData.ToString());
            SaveLevelData();
            Debug.Log(levelDataFileName + " ������ ã�� �� ����! ���ο� ������ Resources�κ��� ����");
        }

        // ������Ʈ ��������
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

        // ��ȣȭ
        toJsonData = Crypto.AESEncrypt128(toJsonData);

        File.WriteAllText(filePath, toJsonData);
        Debug.Log(levelDataFileName + "���� ������ ���� �Ϸ�");
    }

    LevelData UpdateLevelData()
    {
        LevelData newLD = new LevelData();

        // ���ҽ� ����(������Ʈ ����)�� ���� ���� ���� �Է�
        TextAsset resourceData = Resources.Load(levelDataFileName) as TextAsset;
        newLD = JsonUtility.FromJson<LevelData>(resourceData.ToString());

        foreach(Level l in leveldata.levellist)
        {
            newLD.levellist[l.level - 1] = l;
        }

        Debug.Log(levelDataFileName + " ���� ������Ʈ �Ϸ�! >> newSD.length: " + newLD.levellist.Length);

        // �������� ������Ʈ
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

            //��ȣȭ
            fromJsonData = Crypto.AESDecrypt128(fromJsonData);

            stagedata = JsonUtility.FromJson<StageData>(fromJsonData);

            Debug.Log(fromJsonData);
            Debug.Log(stagedata.level + " �ҷ����� ����!");
        }
        else
        {
            // ������ �������� ������ Resources ���� �⺻ ������ ������ ����
            TextAsset resourceData = Resources.Load(stageDataFileName + selectedLevel) as TextAsset;
            stagedata = JsonUtility.FromJson<StageData>(resourceData.ToString());
            SaveStageData();
            Debug.Log(stageDataFileName + " ������ ã�� �� ����! ���ο� ������ Resources�κ��� ����");
        }
        GetRestHeart();

    }


    void UpdateStageData(string fileName)
    {
        StageData newSD = new StageData();
        StageData restSD = new StageData();

        // ���ҽ� ����(������Ʈ ����)�� ���� ���� ���� �Է�
        TextAsset resourceData = Resources.Load(fileName) as TextAsset;
        newSD = JsonUtility.FromJson<StageData>(resourceData.ToString());

        // ���� ������
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

            Debug.Log(fileName + " ���� ������Ʈ �Ϸ�! >>  �������� ��: " + newSD.stagelist.Length);
        }
    }

    public void SaveStageData()
    {
        string toJsonData = JsonUtility.ToJson(stagedata, true);
        string filePath = Application.persistentDataPath + "/" + stageDataFileName + stagedata.level;

        // ��ȣȭ
        toJsonData = Crypto.AESEncrypt128(toJsonData);

        File.WriteAllText(filePath, toJsonData);
        Debug.Log(stageDataFileName + "���� ������ ���� �Ϸ�");
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

        Debug.Log("���� ������ �ʱ�ȭ �Ϸ�!");
    }


    public void GetRestHeart()
    {
        string data = PlayerPrefs.GetString("restdata","0");
        if (data.Equals("0"))   // ���� ������
        {
            SetRestHeart(3);
            ReplayBtn.rest = 3;
        }
        else
        {
            int rest = Int32.Parse(Crypto.AESDecrypt128(data));
            Debug.Log("���� replay: " + rest);

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
