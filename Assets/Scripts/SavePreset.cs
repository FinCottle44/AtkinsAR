using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Linq;
using DG.Tweening.Plugins.Core.PathCore;
using FantomLib;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SavePreset : MonoBehaviour
{
    public GameObject Dialog;
    public GameObject DebugSnack;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        //CreateXml();
    
        //DebugSnack.GetComponent<Text>().text = Dialog.name;
    } 

    private List<PointObject> GatherObjects()
    {
        List<GameObject> points = GameObject.FindGameObjectsWithTag("point").ToList();
        List<PointObject> objects = new List<PointObject>();
        
        foreach (GameObject point in points)
        {
            int objType = GrabType(point);
            PointObject obj = new PointObject() {ObjectType = objType, Position = point.transform.position};
            objects.Add(obj);
        }
        return objects;
    }

    //Checks if directory is alrady made, if not, makes it
    private void MakeDirectory()
    {
        string path = Application.persistentDataPath + "/Presets";
        if (!Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);    
        }
        else
        {
            print("Directory already exists!");
        }
    }

    public class PointObject
    {
        public int ObjectType { get; set; }
        public Vector3 Position { get; set; }
    }
    
    public void CreateXml(string filename)
    {
        MakeDirectory(); //create directory to save items
        
        List<PointObject> objects = GatherObjects();
        XDocument positions = new XDocument(
            new XComment("Positions of preset values"),
            new XElement("objects")
        );
        
        int x = 1;
        foreach (PointObject obj in objects)
        {
            XElement el = new XElement("object" + x, new XAttribute("Type", obj.ObjectType.ToString()), obj.Position.ToString());//(1,2,3),(1) as position unique unlike objtype
            positions.Root.Add(el); //adds el as child of "objects" element defined above (root of positions)
            x = x + 1;
        }
        
        positions.Save(Application.persistentDataPath + "/Presets/" + filename + ".xml");
        AndroidPlugin.ShowToast("Preset successfully saved as '" + filename + "'", true);
    }

    private int GrabType(GameObject go)
    {
        int output;
        if (go.name.Contains("Cone"))
        {
            output = 1; //CONE
        }
        else if (go.name.Contains("Barrier"))
        {
            output = 2; //barrier
        }
        else if (go.name.Contains("marker") || go.name.Contains("ring"))
        {
            output = 3; //marker
        }
        else
        {
            output = -1;
        }

        return output;
    }

    public void SaveName()
    {
        AndroidPlugin.ShowSingleLineTextDialog("Enter filename", "Please enter a name for this preset:","filename", 15, "SavePreset", "CreateXml", "Save");
    }
}
