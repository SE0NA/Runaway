using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // state of player
    bool isAlive = true;
    public bool isMoving = true;
    [SerializeField] PLACE placeWhere = PLACE.START;

    // components
    public float moveforce = 2.5f;
    public float jumpforce = 3f;
    Rigidbody rigid;
    Animator anim;

    // enum
    enum PLACE { START, BLOCK, END };
    public enum DIR { LEFT, RIGHT, FRONT, BACK };

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) Move(DIR.FRONT);
        else if (Input.GetKeyDown(KeyCode.S)) Move(DIR.BACK);
        else if (Input.GetKeyDown(KeyCode.A)) Move(DIR.LEFT);
        else if (Input.GetKeyDown(KeyCode.D)) Move(DIR.RIGHT);
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

        Debug.Log("move: " + dir.ToString());

        isMoving = true;

        float xDir = 0f, zDir = 0f;
        float yRot = 0f;    // 바라보는 방향

        if (dir == DIR.LEFT) { xDir = -1f; yRot = 270f; }
        else if (dir == DIR.RIGHT) { xDir = 1f; yRot = 90f; }
        else if (dir == DIR.FRONT) { zDir = 1f; yRot = 0f; }
        else if (dir == DIR.BACK) { zDir = -1f; yRot = 180f; }

        // 플레이어 방향 전환
        gameObject.transform.rotation = Quaternion.Euler(0f, yRot, 0f);

        // 플레이어 이동
     //   rigid.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        rigid.AddForce(new Vector3(xDir * moveforce, jumpforce, zDir * moveforce), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        isMoving = false;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "destroyzone")
        {
            // 게임 오버
            isAlive = false;
            FindObjectOfType<GameManager>().GameResult();
        }
    }
}
