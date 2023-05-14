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
    [SerializeField] List<Transform> trans_cam; // 레벨에 따라 캠 위치가 다름
    [Header("Building")]
    [SerializeField] List<Object> obj_list_buildings;    // 빌딩 디자인
    [SerializeField] List<Transform> trans_list_finishbuildings; // 종료 빌딩 위치 - 레벨에 따라 다름
    [SerializeField] List<Object> obj_list_backgroundSettings;   // 레벨에 따른 백그라운드 디자인(조명, 캠 등)
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

        // 백그라운드, 캠 불러오기


        // 빌딩, 블럭, 플레이어 생성

    }

    void FileReadForStageSetting()
    {
        // 파일에서 해당 스크립트의 
    }

    // 어떤 블럭의 restcount가 0이 되면 호출 (Block에서 호출)
    public void SetZeroBlock(int idx)
    {
        blocks.RemoveAt(idx);
    }

    // 게임 종료 조건 만족 시 실행 (Player에서 호출)
    public void GameResult()
    {
        if (!player.IsPlayerAlive) GameOver(whyover.dead);
        else if (blocks.Count > 0) GameOver(whyover.restblocks);
        else GameClear();   // 플레이어 생존, 블럭 모두 제거 -> 게임 클리어
    }

    void GameOver(whyover reason)
    {

    }

    void GameClear()
    {
        // 클리어 내용 저장
        DataManager.instance.stagedata.levellist[now_level - 1].stagelist[now_stage - 1].clear = true;
        DataManager.instance.SaveStageData();
    }
}
