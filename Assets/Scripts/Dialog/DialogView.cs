using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogView : MonoBehaviour
{
    public Button btnNextDialog;
    public GameObject dialogPanel;
    public TextMeshProUGUI textDialogPanel;
    public Image avatarFrame;
    public Action OnEndDialog;
    [SerializeField] private TextMeshProUGUI txtNext;
    ResourceLoader RL;
    AudioSource audiosource=> GetComponent<AudioSource>();

    string nextDialogKey;
    float dialogLineTime;

    private void Awake() {
        ServiceLocator.Instance.RegisterService( this);
        RL = ServiceLocator.Instance.GetService<ResourceLoader>();
        setActivePanel(false);
    }

    private void Start()
    {
        txtNext.text = DialogManager.Instance.GetDialog("txtPresTabNext");
    }

    private void LateUpdate()
    {
        if (!Input.GetKeyDown(KeyCode.Tab))
        {
            return;
        }
        if (GameManager.Instance.gamePaused)
        {
            if (!dialogPanel.activeSelf)
                return;
        }
        if (dialogPanel.activeSelf)
        {
            NextLine();
        }
    }

    public void DisplayDialog(string[] keys)
    {
        setActivePanel(true);
    }

    public void DisplayMessage(string key)
    {
        ShowText(DialogManager.Instance.GetDialog(key.Trim()));

            AudioClip dialogAudio = DialogManager.Instance.GetDialogAudio(key.Trim());
            //Debug.Log("AudioClip obtenido: " + key.Trim());
            if (dialogAudio)
            {
                StopAllCoroutines();
                StartCoroutine(PlayAudioVoice(dialogAudio));
            }
        setActivePanel(true);

    }


    public void setActivePanel(bool set)
    {
        dialogPanel.SetActive(set);
        Time.timeScale = set && dialogLineTime == 0f ? 0 : 1;
    }

    private void ShowText(string dialog)
    {
        bool hasdialog = dialog.Contains("¬");
        //avatarFrame.gameObject.SetActive(hasdialog);
        if (hasdialog)
        {
            string[] strings = dialog.Split('¬');
            string avatarIndex = "av/" + strings[0];
            dialogLineTime = float.TryParse(strings[1], out float t) ? t : 0f;
            textDialogPanel.text = strings[2];

            nextDialogKey = strings.Length >3? strings[3] : "";
            //Debug.Log("siguiente clave: " + nextDialogKey); 
            //avatarFrame.sprite = RL.GiveMeAResource<Sprite>(avatarIndex,true);

            //Si es diálogo automático, avanza solo a la siguiente lìnea despuès de un tiempo
            if (  dialogLineTime > 0f)
                Invoke("NextLine",dialogLineTime);
        }
        else
            textDialogPanel.text = dialog;
    }

    private void NextLine()
    {
        audiosource.Stop();
        if (nextDialogKey.Trim() != "")
        {

            DisplayMessage(nextDialogKey);
            //nextDialogKey = "";
        }
        else
        {
            OnEndDialog?.Invoke();
            setActivePanel(false);
        }
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }


    IEnumerator TypeText(string message)
    {
        textDialogPanel.text = "";
        foreach (char letter in message.ToCharArray())
        {
            textDialogPanel.text += letter;
            yield return new WaitForSeconds(0.2f);

        }
    }

    IEnumerator PlayAudioVoice(AudioClip clip)
    {

        audiosource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
    }


}
