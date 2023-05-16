using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Info this stage
    int now_level;
    int now_stage;
    List<GameObject> blocks = new List<GameObject>();
    enum whyover { dead, restblocks }

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
        Debug.Log("awake");
    }

    void Start()
    {
        now_level = DataManager.instance.selectedLevel;
        now_stage = DataManager.instance.selectedStage;

        cam = FindObjectOfType<Camera>();

        Debug.Log("start");
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

        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
            {
                if (blockdesign[i * row + j] == 0) continue;

                // ���� �κ� ������! ��ġ ����
                GameObject newBlock = Instantiate<GameObject>(obj_block, new Vector3(-2.5f + j * 2.5f, -1.5f, 2.5f * i + 2.5f), Quaternion.identity);
             //   newBlock.transform.position = new Vector3(-2.5f + j * 2.5f, -1.5f, 2.5f * i);
                newBlock.gameObject.GetComponent<Block>().InitBlock(i * row + j, blockdesign[i * row + j], this);
                blocks.Add(newBlock);
            }

        // �÷��̾� ������Ʈ ���� �� ��ġ (0,1,0)
    }


    // � ���� restcount�� 0�� �Ǹ� ȣ�� (Block���� ȣ��)
    public void SetZeroBlock(int idx)
    {
        blocks.RemoveAt(idx);
    }

    // ���� ���� ���� ���� �� ���� (Player���� ȣ��)
    public void GameResult()
    {
        if (!player.IsPlayerAlive)
        {
            GameOver(whyover.dead);
            Destroy(player.gameObject);
        }
        else if (blocks.Count > 0) GameOver(whyover.restblocks);
        else GameClear();   // �÷��̾� ����, �� ��� ���� -> ���� Ŭ����
    }

    void GameOver(whyover reason)
    {

    }

    void GameClear()
    {
        // Ŭ���� ���� ����
        DataManager.instance.stagedata.levellist[now_level - 1].stagelist[now_stage - 1].clear = true;
        DataManager.instance.SaveStageData();
    }
}
