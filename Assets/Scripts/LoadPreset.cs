using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadPreset : MonoBehaviour
{
    public string PresetPath = Application.persistentDataPath + "/Presets";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenUI()
    {
        
    }
    
    private List<string> ListPresets()
    {
        List<string> files = new List<string>();
        foreach (string f in Directory.GetFiles(PresetPath))
        {
            files.Add(f);
        }
        return files;
    }
}
