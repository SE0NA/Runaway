using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataManager.instance.LoadStageData();
    }

}
