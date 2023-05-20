using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // state of player
    bool isAlive = true;
    public bool isMoving = true;
    [SerializeField] PLACE placeWhere = PLACE.START;
    float xDir = 0f, zDir = 0f;
    Vector3 target;

    // components
    public float moveforce = 2.5f;
    public float jumpforce = 3f;
    Rigidbody rigid;
    GameManager gm;

    // enum
    enum PLACE { START, BLOCK, END };
    public enum DIR { LEFT, RIGHT, FRONT, BACK };

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isAlive)
            return;

        if (Input.GetKeyDown(KeyCode.W)) Move(DIR.FRONT);
        else if (Input.GetKeyDown(KeyCode.S)) Move(DIR.BACK);
        else if (Input.GetKeyDown(KeyCode.A)) Move(DIR.LEFT);
        else if (Input.GetKeyDown(KeyCode.D)) Move(DIR.RIGHT);

        if (isMoving) {
            transform.position = Vector3.Slerp(gameObject.transform.position, target, moveforce);
        }
    }

    public bool IsPlayerAlive => isAlive;

    public void Move(DIR dir)
    {
        if (!isAlive || isMoving) return;
        // 움직이는 중이면 기능 X

        // start 지점에서 back 사용 불가
        if (placeWhere == PLACE.START && dir == DIR.BACK)
            return;

        isMoving = true;

        float yRot = 0f;    // 바라보는 방향

        if (dir == DIR.LEFT) { xDir = -2.5f; zDir = 0f; yRot = 270f; }
        else if (dir == DIR.RIGHT) { xDir = 2.5f; zDir = 0f; yRot = 90f; }
        else if (dir == DIR.FRONT) { xDir = 0f; zDir = 2.5f; yRot = 0f; }
        else if (dir == DIR.BACK) { xDir = 0f; zDir = -2.5f; yRot = 180f; }

        // 플레이어 방향 전환
        gameObject.transform.rotation = Quaternion.Euler(0f, yRot, 0f);

        // 플레이어 이동
        rigid.AddForce(Vector3.up * jumpforce, ForceMode.VelocityChange);
        target = gameObject.transform.position + new Vector3(xDir, 3f, zDir);
        
        Debug.Log("move: " + dir.ToString());
    }

    private void OnCollisionEnter(Collision collision)
    {
        float newX = Mathf.Round(transform.position.x * 10) * 0.1f;
        float newZ = Mathf.Round(transform.position.z * 10) * 0.1f;
        gameObject.transform.position = new Vector3(newX, gameObject.transform.position.y, newZ);

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
            gm.GameResult();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "destroyzone")
        {
            gm.GameResult();
        }
        else if(other.tag == "freezezone")
        {
            isAlive = false;
        }
    }
}
