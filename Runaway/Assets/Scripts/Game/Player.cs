using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // state of player
    bool isAlive = true;
    PLACE placeWhere = PLACE.START;
    // 
    enum PLACE { START, BLOCK, END };

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool IsPlayerAlive => isAlive;

    public void PlayerObjectMove()
    {
        // back���� PLACE.START �̸� �����ϵ���
    }

    private void OnCollisionEnter(Collision collision)
    {
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
        if(other.tag == "destroyZone")
        {
            // ���� ����
            isAlive = false;
            FindObjectOfType<GameManager>().GameResult();
        }
    }
}
