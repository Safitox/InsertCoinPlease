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

    private void Awake() {
        ServiceLocator.Instance.RegisterService( this);
        RL = ServiceLocator.Instance.GetService<ResourceLoader>();
        presenter = new DialogPresenter(this,(_)=>ShowText(_));
        presenter.Init();
        Debug.Log("Sistema de diálogo iniciado");
        btnNextDialog.onClick.AddListener(() => presenter.proximaOracion());
        setActivePanel(false);
    }

    public void DisplayDialog(string[] keys)
    {
        setActivePanel(true);
        if (!presenter.ShowText(textDialogPanel, keys))
            setActivePanel(false);
    }

    public void DisplayMessage(string key)
    {
        setActivePanel(true);
        if (!presenter.ShowText(textDialogPanel, new string[] { key }))
            setActivePanel(false);
    }


    public void setActivePanel(bool set) => dialogPanel.SetActive(set);


    private void ShowText(string dialog)
    {
        int fin = 0;
        bool hasdialog = dialog.Contains("¬");
        avatarFrame.gameObject.SetActive(hasdialog);
        if (hasdialog)
        {
            fin = dialog.IndexOf('¬');
            string avatarIndex = "av/" +  dialog.Substring(0,fin);
            textDialogPanel.text = dialog.Substring(fin+1);
            avatarFrame.sprite = RL.GiveMeAResource<Sprite>(avatarIndex,true);
        }
        else
            textDialogPanel.text = dialog;
    }
}
