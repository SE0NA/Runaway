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
        // �����̴� ���̸� ��� X
        if (isMoving || !isAlive)
            return;
        // start �������� back ��� �Ұ�
        if (placeWhere == PLACE.START && dir == DIR.BACK)
            return;

        isMoving = true;

        int xDir = 0, zDir = 0;
        float yRot = 0f;    // �ٶ󺸴� ����

        if (dir == DIR.LEFT) { xDir = -1; zDir = 0; yRot = 270f; }
        else if (dir == DIR.RIGHT) { xDir = 1; zDir = 0; yRot = 90f; }
        else if (dir == DIR.FRONT) { xDir = 0; zDir = 1; yRot = 0f; }
        else if (dir == DIR.BACK) { xDir = 0; zDir = -1; yRot = 180f; }

        // �÷��̾� ���� ��ȯ
        gameObject.transform.rotation = Quaternion.Euler(0f, yRot, 0f);

        // �÷��̾� �̵�
        rigid.AddForce(transform.forward * jumpforce, ForceMode.Impulse);

    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���� ���

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
            // ���� ����
            isAlive = false;
            FindObjectOfType<GameManager>().GameResult();
        }
    }
}
