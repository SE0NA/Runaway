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
    [SerializeField] List<Transform> trans_cam; // 레벨에 따라 캠 위치가 다름
    [Header("Building")]
    [SerializeField] GameObject goalbuilding;
    [SerializeField] List<Object> obj_list_backgroundSettings;   // 레벨에 따른 백그라운드 디자인(조명, 캠 등)
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
        // 카에라 높이 설정
     //   cam.transform.position = trans_cam[now_level - 1].position;

        // 블럭 크기 및 종료 빌딩 위치 설정
        int row = 0, col = 0;
        if (now_level == 1) { row = 3; col = 3; goalbuilding.transform.position = new Vector3(0, -2f, 18.2f); }
        else if (now_level == 2) { row = 4; col = 3; goalbuilding.transform.position = new Vector3(0, -2f, 20.7f); }
        else if (now_level == 3) { row = 5; col = 3; goalbuilding.transform.position = new Vector3(0, -2f, 23.2f); }

        // 블럭 설치
        int[] blockdesign = DataManager.instance.stagedata.levellist[now_level - 1].stagelist[now_stage - 1].blocks;

        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
            {
                if (blockdesign[i * row + j] == 0) continue;

                // 생성 부분 생략함! 위치 설정
                GameObject newBlock = Instantiate<GameObject>(obj_block, new Vector3(-2.5f + j * 2.5f, -1.5f, 2.5f * i + 2.5f), Quaternion.identity);
             //   newBlock.transform.position = new Vector3(-2.5f + j * 2.5f, -1.5f, 2.5f * i);
                newBlock.gameObject.GetComponent<Block>().InitBlock(i * row + j, blockdesign[i * row + j], this);
                blocks.Add(newBlock);
            }

        // 플레이어 오브젝트 생성 및 위치 (0,1,0)
    }


    // 어떤 블럭의 restcount가 0이 되면 호출 (Block에서 호출)
    public void SetZeroBlock(int idx)
    {
        blocks.RemoveAt(idx);
    }

    // 게임 종료 조건 만족 시 실행 (Player에서 호출)
    public void GameResult()
    {
        if (!player.IsPlayerAlive)
        {
            GameOver(whyover.dead);
            Destroy(player.gameObject);
        }
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
