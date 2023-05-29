using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMotion : MonoBehaviour
{
    [Header("Game Objects")]
    public Image img_fade;
    public Image img_title;
    public GameObject camera;
    public List<Button> btn_start;
    public TextMeshProUGUI txt_start;
    public Image img_set;

    [Header("Camera")]
    public Vector3 c_origin;
    public Vector3 c_target;
    public float landTime;
    public AnimationCurve c_curveDown;

    private void Awake()
    {
        btn_start[0].interactable = false;
        btn_start[1].interactable = false;
        camera.transform.position = c_origin;

        StartCoroutine(AnimCoroutine());
    }

    IEnumerator AnimCoroutine()
    {
        float fadeCount = 1f;   // 초깃값
        while (fadeCount > 0.0f)
        {
            fadeCount -= 0.05f;
            yield return new WaitForSeconds(0.01f);
            img_fade.color = new Color(0, 0, 0, fadeCount);
        }

        // fade 끝
        Destroy(img_fade.gameObject);

        // Camera 이동
        Vector3 c_startPos = c_origin;
        Vector3 c_targetPos = c_target;

        float timer = 0.0f;
        while(timer < landTime)
        {
            timer += Time.deltaTime;
            float percentageComplete = timer / landTime;

            camera.transform.position = Vector3.Lerp(c_startPos, c_targetPos, c_curveDown.Evaluate(percentageComplete));

            yield return null;
        }

        // Title fade
        fadeCount = 0f;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.05f;
            img_title.color = new Color(1f, 1f, 1f, fadeCount);
            yield return new WaitForSeconds(0.01f);
        }

        // Btn fade
        fadeCount = 0f;
        while(fadeCount < 1.0f)
        {
            fadeCount += 0.05f;
            yield return new WaitForSeconds(0.01f);

            btn_start[0].image.color = new Color(1f, 1f, 1f, fadeCount);
            btn_start[1].image.color = new Color(1f, 1f, 1f, fadeCount);
            txt_start.color = new Color(txt_start.color.r, txt_start.color.g, txt_start.color.b, fadeCount);
            img_set.color = new Color(img_set.color.r, img_set.color.g, img_set.color.b, fadeCount);
        }

        btn_start[0].interactable = true;
        btn_start[1].interactable = true;

        // 애니메이션 종료 후 해당 오브젝트 삭제
        Destroy(gameObject);
    }
}
