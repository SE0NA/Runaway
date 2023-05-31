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
    public bool isNumbering = true;
    public int restPlay = 1;


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
            newLD.levellist[l.level - 1].clear = l.clear;
        }

        Debug.Log(levelDataFileName + " ���� ������Ʈ �Ϸ�! >> newSD.length: " + newLD.levellist.Length);

        // �������� ������Ʈ
        foreach(Level l in newLD.levellist)
        {
            UpdateStageData(stageDataFileName + l.level.ToString());
        }

        SaveLevelData();

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
                newSD.stagelist[s.stageNo - 1].clear = s.clear;
            }


            Debug.Log(fileName + " ���� ������Ʈ �Ϸ�! >>  �������� ��: " + newSD.stagelist.Length);
        }
        stagedata = newSD;
        SaveStageData();
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

    public void ReduceRestPlay()
    {
        restPlay--;
        PlayerPrefs.SetInt("restPlay", restPlay);
    }
    public void ChargeRestPlay()
    {
        Debug.Log("DataManager:ChargeRestPlay - Ads");

        restPlay = 3;
        PlayerPrefs.SetInt("restPlay", restPlay);
    }
}
