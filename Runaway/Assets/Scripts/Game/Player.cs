using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class Player : MonoBehaviour
{
    // state of player
    bool isAlive = true;
    public bool isMoving = true;
    [SerializeField] PLACE placeWhere = PLACE.START;
    float xDir = 0f, zDir = 0f;

    // constraint
    public int now_x = 0, now_z = 0;
    int x_range = 1, z_range = 3;

    // components
    public float moveforce = 0.1f;
    public float jumpforce = 3f;
    Rigidbody rigid;
    GameManager gm;
    Animator anim;

    public AnimationCurve curveJumpUp;
    public AnimationCurve curveJumpDown;
    public float jumpHeight = 3f;
    public float jumpTime = 0.2f;

    // enum
    enum PLACE { START, BLOCK, END };
    public enum DIR { LEFT, RIGHT, FRONT, BACK };

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if(DataManager.instance.selectedLevel == 1)
        {
            x_range = 3 / 2;    z_range = 3;
        }
        else if(DataManager.instance.selectedLevel == 2)
        {
            x_range = 3 / 2;    z_range = 4;
        }
        else if(DataManager.instance.selectedLevel == 3)
        {
            x_range = 3 / 2;    z_range = 5;
        }
    }

    void Update()
    {
        if (!isAlive)
            return;

        if (Input.GetKeyDown(KeyCode.W)) Move(DIR.FRONT);
        else if (Input.GetKeyDown(KeyCode.S)) Move(DIR.BACK);
        else if (Input.GetKeyDown(KeyCode.A)) Move(DIR.LEFT);
        else if (Input.GetKeyDown(KeyCode.D)) Move(DIR.RIGHT);
        /*
        if (isMoving) {
            transform.position = Vector3.Lerp(gameObject.transform.position, target, moveforce);
        }
        */
    }

    public bool IsPlayerAlive => isAlive;

    public IEnumerator Jump(DIR dir)
    {
        float yRot = 0f;
        if (dir == DIR.LEFT) { xDir = -1.25f; zDir = 0f; yRot = 270f; now_x--; }
        else if (dir == DIR.RIGHT) { xDir = 1.25f; zDir = 0f; yRot = 90f; now_x++; }
        else if (dir == DIR.FRONT) { xDir = 0f; zDir = 1.25f; yRot = 0f; now_z++; }
        else if (dir == DIR.BACK) { xDir = 0f; zDir = -1.25f; yRot = 180f; now_z--; }

        gameObject.transform.rotation = Quaternion.Euler(0f, yRot, 0f);

        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = startPos + new Vector3(xDir, jumpHeight, zDir);

        float timer = 0.0f;

        anim.SetBool("Jump", true);

        while(timer < jumpTime)
        {
            timer += Time.deltaTime;
            float percentageComplete = timer / jumpTime;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, curveJumpUp.Evaluate(percentageComplete));
            yield return null;
        }
        timer = 0.0f;
        startPos = transform.localPosition;
        targetPos = startPos + new Vector3(xDir, jumpHeight, zDir);
        while (timer < jumpTime)
        {
            timer += Time.deltaTime;
            float percentageComplete = timer / jumpTime;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, curveJumpDown.Evaluate(percentageComplete));
            yield return null;
        }
    }

    public void Move(DIR dir)
    {
        if (!isAlive || isMoving) return;
        // 움직이는 중이면 기능 X

        // start 지점에서 back 사용 불가
        if (placeWhere == PLACE.START && dir == DIR.BACK)
            return;

        // 이동 제한
        if (dir == DIR.LEFT && now_x <= -x_range)
            return;
        else if (dir == DIR.RIGHT && now_x >= x_range)
            return;
        else if (dir == DIR.BACK && now_z <= 0)
            return;


        isMoving = true;

        StartCoroutine("Jump", dir);

        Debug.Log("move");



        /*
        float yRot = 0f;    // 바라보는 방향

        if (dir == DIR.LEFT) { xDir = -2.5f; zDir = 0f; yRot = 270f; }
        else if (dir == DIR.RIGHT) { xDir = 2.5f; zDir = 0f; yRot = 90f; }
        else if (dir == DIR.FRONT) { xDir = 0f; zDir = 2.5f; yRot = 0f; }
        else if (dir == DIR.BACK) { xDir = 0f; zDir = -2.5f; yRot = 180f; }

        // 플레이어 방향 전환
        gameObject.transform.rotation = Quaternion.Euler(0f, yRot, 0f);

        // 플레이어 이동
        //    rigid.AddForce(Vector3.up * jumpforce, ForceMode.VelocityChange);
        target = gameObject.transform.position + new Vector3(xDir, 0f, zDir);
        
        Debug.Log("move: " + dir.ToString());
        */
    }

    private void OnCollisionEnter(Collision collision)
    {
        float newX = Mathf.Round(transform.position.x * 10) * 0.1f;
        float newZ = Mathf.Round(transform.position.z * 10) * 0.1f;
        gameObject.transform.position = new Vector3(newX, gameObject.transform.position.y, newZ);

        isMoving = false;
        anim.SetBool("Jump", false);

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
