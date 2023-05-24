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

            //��ȣȭ
            fromJsonData = Crypto.AESDecrypt128(fromJsonData);

            stagedata = JsonUtility.FromJson<StageData>(fromJsonData);
            Debug.Log(stageDataFileName + " �ҷ����� ����!");
        }
        else
        {
            // ������ �������� ������ Resources ���� �⺻ ������ ������ ����
            TextAsset resourceData = Resources.Load(stageDataFileName) as TextAsset;
            stagedata = JsonUtility.FromJson<StageData>(resourceData.ToString());
            SaveStageData();
            Debug.Log(stageDataFileName + " ������ ã�� �� ����! ���ο� ������ Resources�κ��� ����");
        }

        // ������Ʈ ��������
        if(stagedata.version != Application.version)
        {
            stagedata = UpdataStageData();
            SaveStageData();
        }

    }
    StageData UpdataStageData()
    {
        StageData newSD = new StageData();

        // ���ҽ� ����(������Ʈ ����)�� ���� ���� ���� �Է�
        TextAsset resourceData = Resources.Load(stageDataFileName) as TextAsset;
        newSD = JsonUtility.FromJson<StageData>(resourceData.ToString());

        foreach (Level l in stagedata.levellist)
            foreach (Stage s in stagedata.levellist[l.level - 1].stagelist)
            {
                newSD.levellist[l.level - 1].stagelist[s.stageNo - 1] = s;
            }

        // �̹� ��� ���������� Ŭ����� ��������
        // ���� ������ ���������� Ŭ���� ���ο� ���� �� ���� ��������(new)�� unLock ����
        foreach (Level l in stagedata.levellist)
            if (l.stagelist[l.stagelist.Length - 1].clear)
                newSD.levellist[l.level - 1].stagelist[l.stagelist.Length].unLock = true;

        Debug.Log(stageDataFileName + " ���� ������Ʈ �Ϸ�! >> newSD.length: " + newSD.levellist[0].stagelist.Length);

        
        return newSD;
    }

    public void SaveStageData()
    {
        string toJsonData = JsonUtility.ToJson(stagedata, true);
        string filePath = Application.persistentDataPath + "/" + stageDataFileName;

        // ��ȣȭ
        toJsonData = Crypto.AESEncrypt128(toJsonData);

        File.WriteAllText(filePath, toJsonData);
        Debug.Log(stageDataFileName + "���� ������ ���� �Ϸ�");
    }

    public void FileDelete()
    {
        // PlayerPrefs
        PlayerPrefs.DeleteAll();

        string filePath = Application.persistentDataPath + "/" + stageDataFileName;
        if(File.Exists(filePath))
            File.Delete(filePath);

        Debug.Log("���� ������ �ʱ�ȭ �Ϸ�!");
    }
}
