using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Info this stage
    int now_level;
    int now_stage;
    List<Block> blocks = new List<Block>();
    enum whyover { dead, restblocks }

    // Setting Values
    [SerializeField] List<Transform> trans_cam; // ������ ���� ķ ��ġ�� �ٸ�
    [Header("Building")]
    [SerializeField] List<Object> obj_list_buildings;    // ���� ������
    [SerializeField] List<Transform> trans_list_finishbuildings; // ���� ���� ��ġ - ������ ���� �ٸ�
    [SerializeField] List<Object> obj_list_backgroundSettings;   // ������ ���� ��׶��� ������(����, ķ ��)
    [Header("Block")]
    [SerializeField] Object obj_block;
    [SerializeField] List<Transform> trans_list_block;
    [Header("Player")]
    [SerializeField] Object obj_player;
    [SerializeField] Transform trans_player_start;

    // Extern
    Camera cam;
    Player player;

    void Awake()
    {
        FileReadForStageSetting();

        // ��׶���, ķ �ҷ�����


        // ����, ��, �÷��̾� ����

    }

    void FileReadForStageSetting()
    {
        // ���Ͽ��� �ش� ��ũ��Ʈ�� 
    }

    // � ���� restcount�� 0�� �Ǹ� ȣ�� (Block���� ȣ��)
    public void SetZeroBlock(int idx)
    {
        blocks.RemoveAt(idx);
    }

    // ���� ���� ���� ���� �� ���� (Player���� ȣ��)
    public void GameResult()
    {
        if (!player.IsPlayerAlive) GameOver(whyover.dead);
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
