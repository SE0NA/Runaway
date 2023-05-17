using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // state of player
    bool isAlive = true;
    public bool isMoving = false;
    [SerializeField] PLACE placeWhere = PLACE.START;

    // components
    public float jumpforce = 2.5f;
    Rigidbody rigid;

    // enum
    enum PLACE { START, BLOCK, END };
    public enum DIR { LEFT, RIGHT, FRONT, BACK };

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) Move(DIR.FRONT);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) Move(DIR.BACK);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) Move(DIR.LEFT);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) Move(DIR.RIGHT);
    }

    public bool IsPlayerAlive => isAlive;

    public void Move(DIR dir)
    {
        // 움직이는 중이면 기능 X
        if (isMoving || !isAlive)
            return;
        // start 지점에서 back 사용 불가
        if (placeWhere == PLACE.START && dir == DIR.BACK)
            return;

        isMoving = true;

        int xDir = 0, zDir = 0;
        float yRot = 0f;    // 바라보는 방향

        if (dir == DIR.LEFT) { xDir = -1; zDir = 0; yRot = 270f; }
        else if (dir == DIR.RIGHT) { xDir = 1; zDir = 0; yRot = 90f; }
        else if (dir == DIR.FRONT) { xDir = 0; zDir = 1; yRot = 0f; }
        else if (dir == DIR.BACK) { xDir = 0; zDir = -1; yRot = 180f; }

        // 플레이어 방향 전환
        gameObject.transform.rotation = Quaternion.Euler(0f, yRot, 0f);

        // 플레이어 이동
        rigid.AddForce(transform.forward * jumpforce, ForceMode.Impulse);

    }

    private void OnCollisionEnter(Collision collision)
    {
        // 착륙 모션

        if (collision.gameObject.tag == "startbuilding")
        {
            placeWhere = PLACE.START;
        }
        else if(collision.gameObject.tag == "block")
        {
            placeWhere = PLACE.BLOCK;
        }
        else if(collision.gameObject.tag == "endbuilding")
        {
            placeWhere = PLACE.END;
        }

        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "destroyZone")
        {
            // 게임 오버
            isAlive = false;
            FindObjectOfType<GameManager>().GameResult();
        }
    }
}
