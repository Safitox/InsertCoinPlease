using System.Collections;
using UnityEngine;

public class PanelHelp : SceneSingleton<PanelHelp>
{
    [SerializeField] private TMPro.TextMeshProUGUI txtHelp;
    [SerializeField] private float displayTime = 2f;

    public void ShowHelp(string helpText)
    {
        txtHelp.text = helpText;
        StopAllCoroutines();
        StartCoroutine(Fade());
    }


    public void HideHelp()
    {
        StopAllCoroutines();
        txtHelp.alpha = 0f;
    }

    IEnumerator Fade()
    {
            txtHelp.alpha = 1f;
            yield return new WaitForSeconds(displayTime);
            float elapsed = 0f;
            float duration = 1f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                txtHelp.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                yield return null;
            }
            txtHelp.alpha = 0f;
            yield return new WaitForSeconds(0.5f);
    }

}
