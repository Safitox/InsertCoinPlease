using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro; // importo la libreria de TextMeshPro
using System;

public class DialogManager : Singleton<DialogManager>

{
    private const string Path = "Localization/Dialogo";
    Dictionary<string, string> listaDialogos;
    //string oracionActiva;

    // Start is called before the first frame update
    public void Init()
    {
        listaDialogos = new Dictionary<string, string>();
        listaDialogos = ObtenerDialogos();
    }

    public Dictionary<string, string> ObtenerDialogos()
    {
        Dictionary<string, string> diccionario = new Dictionary<string, string>();
        var file = Resources.Load<TextAsset>(Path);
        if (file != null)
        {

            string[] lineas = file.text.Split('\n'); // esta funcion toma las lineas de un archivo y las carga en un array
            foreach (var linea in lineas)
            { // recorro el array
                var valor = linea.Split('|'); //separo los valores y los coloco en un array para consultarlos por separado
                diccionario.Add(valor[0], valor[1]); // cargo en el diccionario
            }
            return diccionario;
        }
        else
        {
            throw new Exception($"Error: el archivo {Path} no se encuentra");
        }
    }

    public string GetDialog(string key)
    {
        if (listaDialogos.Count <= 0)
        {
            listaDialogos = ObtenerDialogos();
        }

        if (listaDialogos.ContainsKey(key))
        {
            return listaDialogos[key];
        }
        else
        {
            return "";
            //throw new Exception("Atencion: la clave " + key + "no se encuentra dentro del Archivo");
        }
    }
}
