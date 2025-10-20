using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogManager : Singleton<DialogManager>

{
    private const string Path = "Localization/Dialogo";
    Dictionary<string, string> listaDialogos;
    Dictionary<string,AudioClip> listaAudios=new Dictionary<string, AudioClip>();  


    // Start is called before the first frame update

    public void Start()
    {
        listaDialogos = new Dictionary<string, string>();
        listaDialogos = ObtenerDialogos();
        Debug.Log("Dialogos cargados: " + listaDialogos.Count);
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
                if (string.IsNullOrWhiteSpace(linea)) continue; // si la linea esta vacia, paso a la siguiente
                var valor = linea.Split('|'); //separo los valores y los coloco en un array para consultarlos por separado
                diccionario.Add(valor[0], valor[1]); // cargo en el diccionario
                var audioFile = Resources.Load<AudioClip>("Localization/Voices/" + valor[0]);
                if (audioFile != null)
                {
                    listaAudios.Add(valor[0], audioFile);
                }


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

        if (listaDialogos.TryGetValue(key.Trim(), out string d))
        {
            //Debug.Log("Dialogo encontrado: " + key + " - " + listaDialogos[key]);
            return d;
        }
        else
        {
            //return "";
            throw new Exception("Atencion: la clave " + key + "no se encuentra dentro del Archivo");
        }
    }

    public AudioClip GetDialogAudio(string key)
    {
        if (listaAudios.TryGetValue(key.Trim(), out AudioClip a))
        {
            //Debug.Log("Audio de dialogo encontrado: " + key );
            return a;
        }
        return null;
    }
}
