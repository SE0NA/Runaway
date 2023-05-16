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
            Debug.Log(stageDataFileName + " �ҷ����� ����!");
            Debug.Log(stagedata.ToString());
        }
        else
        {
            // ������ �������� ������ Resources ���� �⺻ ������ ������ ����
            TextAsset resourceData = Resources.Load(stageDataFileName) as TextAsset;
            stagedata = JsonUtility.FromJson<StageData>(resourceData.ToString());
            SaveStageData();
            Debug.Log(stageDataFileName + " ������ ã�� �� ����! ���ο� ������ Resources�κ��� ����");
        }

        // ������Ʈ�� �������� �߰�
        if(stagedata.version != Application.version)
        {
            // �� ���� ���� ���� ��.
            // ���ҽ��� �߰��� �κк��� �ٿ��ֱ�
            // stagedata�� ���̰� ����
        }

        Debug.Log("stagedata: " + stagedata.levellist[1].stagelist[0].blocks.Length);
    }
    public void SaveStageData()
    {
        string toJsonData = JsonUtility.ToJson(stagedata, true);
        string filePath = Application.persistentDataPath + "/" + stageDataFileName;

        File.WriteAllText(filePath, toJsonData);
        Debug.Log(stageDataFileName + "���� ������ ���� �Ϸ�");
    }
}
