using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameEnd = false;

    // Info this stage
    int now_level;
    int now_stage;
    List<GameObject> blocks = new List<GameObject>();
    int blockCount = 0;
    public enum Result { dead, restblocks, clear }

    // Setting Values
    [SerializeField] List<Transform> trans_cam; // ������ ���� ķ ��ġ�� �ٸ�
    [Header("Building")]
    [SerializeField] GameObject goalbuilding;
    [SerializeField] List<Object> obj_list_backgroundSettings;   // ������ ���� ��׶��� ������(����, ķ ��)
    [Header("Block")]
    [SerializeField] GameObject obj_block;
    [Header("Player")]
    [SerializeField] GameObject obj_player;

    // Extern
    Camera cam;
    Player player;


    private void Awake()
    {
        DataManager.instance.LoadStageData();
    }

    void Start()
    {
        now_level = DataManager.instance.selectedLevel;
        now_stage = DataManager.instance.selectedStage;

        cam = FindObjectOfType<Camera>();
        player = FindObjectOfType<Player>();

        SettingStage();
    }

    void SettingStage()
    {
        // ī���� ���� ����
     //   cam.transform.position = trans_cam[now_level - 1].position;

        // �� ũ�� �� ���� ���� ��ġ ����
        int row = 0, col = 0;
        if (now_level == 1) { row = 3; col = 3; goalbuilding.transform.position = new Vector3(0, -2f, 18.2f); }
        else if (now_level == 2) { row = 4; col = 3; goalbuilding.transform.position = new Vector3(0, -2f, 20.7f); }
        else if (now_level == 3) { row = 5; col = 3; goalbuilding.transform.position = new Vector3(0, -2f, 23.2f); }

        // �� ��ġ
        int[] blockdesign = DataManager.instance.stagedata.levellist[now_level - 1].stagelist[now_stage - 1].blocks;
        int indexblock = 0;
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
            {
                if (blockdesign[i * col + j] == 0) continue;

                // ���� �κ� ������! ��ġ ����
                GameObject newBlock = Instantiate<GameObject>(obj_block, new Vector3(-2.5f + j * 2.5f, -1.5f, 2.5f * i + 2.5f), Quaternion.identity, transform.GetChild(0));
                
                newBlock.gameObject.GetComponent<Block>().InitBlock(indexblock++, blockdesign[i * col + j], this);
                blocks.Add(newBlock);
            }

        blockCount = blocks.Count;
    }


    // � ���� restcount�� 0�� �Ǹ� ȣ�� (Block���� ȣ��)
    public void SetZeroBlock(int idx)
    {
        blockCount--;
    }

    // ���� ���� ���� ���� �� ���� (Player���� ȣ��)
    public void GameResult()
    {
        gameEnd = true;

        Result res;

        if (!player.IsPlayerAlive)
        {
            // �÷��̾� ĳ���� ����
            Destroy(player.gameObject);
            res = Result.dead;
        }
        else if (blockCount > 0)
        {
            // ���� �� ����
            res = Result.restblocks;
        }
        else
        {
            // �÷��̾� ����, �� ��� ���� -> ���� Ŭ����
            DataManager.instance.stagedata.levellist[now_level - 1].stagelist[now_stage - 1].clear = true;
            DataManager.instance.SaveStageData();

            res = Result.clear;
        }

        // ��� ǥ��
        UIManager uimanager = FindObjectOfType<UIManager>();
        uimanager.ActiveResult(res);
    }
}
