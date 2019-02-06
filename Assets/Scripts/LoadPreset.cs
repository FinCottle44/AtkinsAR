using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Xml.Linq;
using GoogleARCoreInternal;
using UnityEditor.Presets;
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
    
    private List<string> ListPresets()
    {
        List<string> files = new List<string>();
        foreach (string f in Directory.GetFiles(PresetPath))
        {
            files.Add(f);
        }
        return files;
    }

    private void LoadXml(string selectedPresetName)
    {
        XDocument doc = new XDocument();
        string file = PresetPath + "/" + selectedPresetName;
        
        doc = XDocument.Load(file);
    }
}
