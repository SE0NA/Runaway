using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEditor.VersionControl;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Info of this block
    int restcount;
    int blocklist_idx;

    // Content of this block
    Rigidbody rigid_block;
    [SerializeField] List<Color> blockcolors;
    [SerializeField] List<Color> txtcolors;
    TextMeshPro txt_count;

    // Extern
    GameManager gm;

    // �� �ʱ�ȭ. GameManager���� Block Object ���� �� ȣ��
    public void InitBlock(int idx, int rest, GameManager gm)
    {
        blocklist_idx = idx;
        restcount = rest;
        this.gm = gm;

        txt_count = GetComponentInChildren<TextMeshPro>();
        ChangeBlockSet();
    }

    // ���� ���¸� ��, �ؽ�Ʈ�� ǥ��
    void ChangeBlockSet()
    {
     //   gameObject.GetComponent<Material>().color = blockcolors[restcount];
        if (restcount > 0)
        {
            txt_count.text = restcount.ToString();
            txt_count.color = txtcolors[restcount];
        }
        else
        {
            txt_count.text = "";
            gm.SetZeroBlock(blocklist_idx);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ ���� ���� -> Ƚ�� ����
        if (other.tag == "Player")
        {
            restcount--;
            ChangeBlockSet();
        }

        // �߶� �� ���� ������ �浹
        else if (other.tag == "DestroyZone")
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // �÷��̾ ����� ���� -> ���� Ƚ���� 0�̸� �߶�
        if (other.tag == "Player" && restcount <= 0)
        {
            // �߷�
        }
    }
}
