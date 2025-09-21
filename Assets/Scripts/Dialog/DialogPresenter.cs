using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogPresenter : MonoBehaviour
{
    public DialogManager manager;
    Queue<string> cola = new Queue<string>();

    DialogView view;
    Action<string> OnShowDialog;

    public DialogPresenter(DialogView view,Action<string> OnShowDialog) {
        this.view = view;
        this.OnShowDialog += OnShowDialog;

    }

    public void Init() {
        manager =ServiceLocator.Instance.GetService<DialogManager>();
        manager.Init();
    }

    public string GetDialog(string key) {
        return manager.GetDialog(key);
    }


    public bool ShowText(TextMeshProUGUI textDialogPanel, string[] keys) {
        //hago una cola con las keys pasadas
        cola = new Queue<string>();
        foreach (string key in keys) {
            string dialogo = GetDialog(key);
            if (dialogo == "")
                return false;
            cola.Enqueue(GetDialog(key));
        }

            OnShowDialog(cola.Dequeue());
        return true;
    }

    public void proximaOracion() {
        if(cola.Count <= 0) {
            view.setActivePanel(false);
            return;
        }
        OnShowDialog(cola.Dequeue());
        
    }
}
