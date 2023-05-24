using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using UnityEngine;

public class BGManager : MonoBehaviour
{
    [Header("Manager")]
    public float movingTime = 0.3f;
    public float waitTime = 1.0f;

    [Header("Rabbit")]
    public GameObject rabbit;
    Animator r_anim;
    public float r_jump = 2f;
    public float r_origin = 1.3f;
    public AnimationCurve r_curveJumpUp;
    public AnimationCurve r_curveJumpDown;

    [Header("Blocks")]
    public List<GameObject> b_list;
    public Vector3[] b_start_list = new Vector3[4];
    public Vector3[] b_target_list = new Vector3[4];
    
    public AnimationCurve b_curveMove;

    void Start()
    {
        r_anim = rabbit.GetComponent<Animator>();

        for(int i = 0; i < b_list.Count; i++)
            b_start_list[i] = b_list[i].transform.position;

        StartCoroutine("Play");
    }

    public IEnumerator Play()
    {
        // rabbit
        Vector3 r_startPos = new Vector3(0, r_origin, 0);
        Vector3 r_targetPos = new Vector3(0, r_jump, 0);

        // blocks
        for (int i = 0; i < 4; i++)
        {
            if (b_list[i].transform.position.x < -5)
                b_list[i].transform.position = new Vector3(10f, 0f, 0f);

            b_start_list[i] = b_list[i].transform.position;
            b_target_list[i] = b_list[i].transform.position + new Vector3(-2.5f, 0f, 0f);
        }

        float timer = 0.0f;

        r_anim.SetBool("Jump", true);

        while (timer < movingTime)
        {
            timer += Time.deltaTime;
            float percentageComplete = timer / movingTime;

            rabbit.transform.position = Vector3.Lerp(r_startPos, r_targetPos, r_curveJumpUp.Evaluate(percentageComplete));
            for (int i = 0; i < 4; i++)
                b_list[i].transform.position = Vector3.Lerp(b_start_list[i], b_target_list[i], b_curveMove.Evaluate(percentageComplete));

            yield return null;
        }

        timer = 0.0f;
        r_startPos = rabbit.transform.position;
        r_targetPos = new Vector3(0, r_origin, 0);
        for (int i = 0; i < 4; i++)
        {
            b_start_list[i] = b_list[i].transform.position;
            b_target_list[i] = b_list[i].transform.position + new Vector3(-2.5f, 0f, 0f);
        }

        while(timer < movingTime)
        {
            timer += Time.deltaTime;
            float percentageComplete = timer / movingTime;

            rabbit.transform.position = Vector3.Lerp(r_startPos, r_targetPos, r_curveJumpDown.Evaluate(percentageComplete));
            for (int i = 0; i < 4; i++)
                b_list[i].transform.position = Vector3.Lerp(b_start_list[i], b_target_list[i], b_curveMove.Evaluate(percentageComplete));

            yield return null;
        }

        r_anim.SetBool("Jump", false);

        yield return new WaitForSeconds(waitTime);

        StartCoroutine("Play");
    }
}
