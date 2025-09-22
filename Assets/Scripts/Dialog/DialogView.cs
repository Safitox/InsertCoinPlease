using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogView : MonoBehaviour
{
    DialogPresenter presenter;
    public Button btnNextDialog;
    public GameObject dialogPanel;
    public TextMeshProUGUI textDialogPanel;
    public Image avatarFrame;
    ResourceLoader RL;


    ThirdPersonController playerController;
    string nextDialogKey;


    private void Awake() {
        ServiceLocator.Instance.RegisterService( this);
        RL = ServiceLocator.Instance.GetService<ResourceLoader>();
        //presenter = new DialogPresenter(this,(_)=>ShowText(_));
        //presenter.Init();
        Debug.Log("Sistema de diálogo iniciado");
        //btnNextDialog.onClick.AddListener(() => presenter.proximaOracion());
        setActivePanel(false);
    }

    private void LateUpdate()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;
        if (dialogPanel.activeSelf)
        {
            NextLine();
        }
    }

    public void DisplayDialog(string[] keys)
    {
        setActivePanel(true);
        //if (!presenter.ShowText(textDialogPanel, keys))
        //    setActivePanel(false);
    }

    public void DisplayMessage(string key)
    {
        ShowText(DialogManager.Instance.GetDialog(key.Trim()));
        setActivePanel(true);
        //if (!presenter.ShowText(textDialogPanel, new string[] { key }))
        //    setActivePanel(false);
    }


    public void setActivePanel(bool set)
    {
        dialogPanel.SetActive(set);
        Time.timeScale = set ? 0 : 1;
    }

    private void ShowText(string dialog)
    {
        bool hasdialog = dialog.Contains("¬");
        avatarFrame.gameObject.SetActive(hasdialog);
        if (hasdialog)
        {
            string[] strings = dialog.Split('¬');
            string avatarIndex = "av/" + strings[0];
            textDialogPanel.text = strings[1];
            nextDialogKey= strings.Length >2? strings[2] : "";
           
            avatarFrame.sprite = RL.GiveMeAResource<Sprite>(avatarIndex,true);
        }
        else
            textDialogPanel.text = dialog;
    }

    public void NextLine()
    {
        if (nextDialogKey.Trim() != "")
        {

            DisplayMessage(nextDialogKey);
            nextDialogKey = "";
        }
        else
        {
           
            setActivePanel(false);
        }
    }



}
