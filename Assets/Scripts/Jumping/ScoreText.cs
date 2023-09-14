using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreText : MonoBehaviour
{
    public Text textScore;
    float speed = 1.0f;

    void Start()
    {
        StartCoroutine(Fadeout());
    }

    void Update()
    {
        float amount = speed * Time.deltaTime;
        transform.Translate(Vector3.up * amount);
    }

    // 텍스트 사라짐 효과
    IEnumerator Fadeout()
    {
        yield return new WaitForSeconds(1f);
        Color color = textScore.color;

        for (float alpha = 1; alpha > 0; alpha -= 0.02f)
        {
            color.a = alpha;
            textScore.color = color;

            yield return null;
        }

        Destroy(gameObject);
    }

    void SetScore (int score)
    {
        textScore.text = score.ToString("+0; -0");

        if (score < 0)
        {
            textScore.color = Color.red;
        }
    }
}
