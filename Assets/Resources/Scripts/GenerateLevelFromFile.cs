using System.IO;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevelFromFile : MonoBehaviour
{
    public string dataPath;
    // Start is called before the first frame update
    void Start()
    {
        LoadLevelFromFile();
        //SaveLevelToFile(dataPath);
    }

    private GameObject[] resourcesGameObjects;

    public void LoadLevelFromFile()
    {
        string lvlData = dataPath;
        string[] lvlArray = lvlData.Split('\n');
        GameObject currentObject = null;
        int lvlLength = (lvlArray.Length - 1);
        for (int i = 0; i < lvlLength; i++)
        {
            switch (lvlArray[i])
            {
                case string a when a.Contains("prefab_Type"):
                    string variableName = lvlArray[i].Replace("	prefab_Type:", "");
                    Debug.Log("Prefab: " + variableName);
                    string path = "Level_Prefabs/" + variableName.Substring(0, variableName.Length - 2);
                    Debug.Log(path);
                    
                    currentObject = Instantiate(Resources.Load<GameObject>(path));
                    break;
                case string a when a.Contains("parent"):
                    string parentName = a.Replace("	parent:", "");
                    Debug.Log("Parent: " + parentName);
                    currentObject.transform.parent = GameObject.Find("Boxes").transform;
                    break;
                case string a when a.Contains("position"):
                    string positionText = a;
                    positionText = positionText.Replace("	position: (", "");
                    positionText = positionText.Replace(")", "");
                    string[] splitPosition = positionText.Split(',');
                    splitPosition[1] = splitPosition[1].Remove(splitPosition[1].Length - 1, 1);
                    List<float> positionData = new List<float>();
                    for (int j = 0; j < splitPosition.LongLength; j++)
                    {
                        string position = splitPosition[j];
                        positionData.Add(float.Parse(position));
                    }
                    Debug.Log("Position: " + new Vector3(positionData[0], positionData[1], currentObject.transform.position.z));
                    currentObject.transform.position = new Vector3(positionData[0], positionData[1], currentObject.transform.position.z);
                    break;
                case string a when a.Contains("rotation"):
                    string rotationText = a.Replace("	rotation: (", "");
                    rotationText = rotationText.Replace(")", "");
                    string[] splitRotation = rotationText.Split(',');
                    splitRotation[1] = splitRotation[1].Remove(splitRotation[1].Length - 1, 1);
                    splitRotation[2] = splitRotation[2].Remove(splitRotation[2].Length - 1, 1);
                    List<float> rotationData = new List<float>();
                    for (int j = 0; j < splitRotation.LongLength; j++)
                    {
                        string rotation = splitRotation[j];
                        rotationData.Add(float.Parse(rotation));
                    }
                    Debug.Log("Rotation: " + new Vector3(rotationData[0], rotationData[1], rotationData[2]));
                    currentObject.transform.rotation = Quaternion.Euler(rotationData[0], rotationData[1], rotationData[2]);
                    break;
                case string a when a.Contains("scale"):
                    string scaleText = a.Replace("	scale: (", "");
                    scaleText = scaleText.Replace(")", "");
                    string[] splitScale = scaleText.Split(',');
                    splitScale[1] = splitScale[1].Remove(splitScale[1].Length - 1, 1);
                    splitScale[2] = splitScale[2].Remove(splitScale[2].Length - 1, 1);
                    List<float> scaleData = new List<float>();
                    for (int j = 0; j < splitScale.LongLength; j++)
                    {
                        string scale = splitScale[j];
                        scaleData.Add(float.Parse(scale));
                    }
                    Debug.Log("Scale: " + new Vector3(scaleData[0], scaleData[1], scaleData[2]));
                    currentObject.transform.localScale = new Vector3(scaleData[0], scaleData[1], scaleData[2]);
                    break;
            }
        }
    }

    public void SaveLevelToFile()
    {
        string destination = dataPath;
        List<GameObject> children = new List<GameObject>();
        Transform[] childs = FindObjectsOfType<Transform>();
        resourcesGameObjects = Resources.LoadAll("Level_Prefabs", typeof(GameObject)).Cast<GameObject>().ToArray();
        for (int j = 0; j < childs.Length; j++)
        {
            try
            {
                if (childs[j].transform.parent.name == "Boxes")
                {
                    children.Add(childs[j].gameObject);
                }
            }
            catch
            {

            }
        }
        GameObject[] temp = children.ToArray();
        for(int j = 0; j < temp.Length; j++)
        {
            for(int k = 0; k < resourcesGameObjects.Length; k++)
            {
                if (temp[j].name.Contains(resourcesGameObjects[k].name))
                {
                    temp[j].name = resourcesGameObjects[k].name;
                }
            }
        }
        string contents = "";
        for(int i = 0; i < temp.Length; i++)
        {
            contents +=
        "{" + '\n' +
        '\t' + $"prefab_Type:{temp[i].name} " + '\n' +
        '\t' + $"parent:{temp[i].transform.parent.name}" + '\n' +
        '\t' + $"position: ({temp[i].transform.position.x},{temp[i].transform.position.y} )" + '\n' +
        '\t' + $"rotation: ({temp[i].transform.rotation.x},{temp[i].transform.rotation.y} ,{temp[i].transform.rotation.z} )" + '\n' +
        '\t' + $"scale: ({temp[i].transform.localScale.x},{temp[i].transform.localScale.y} ,{temp[i].transform.localScale.z} )" + '\n' +
        "};" + '\n';
        }
        File.WriteAllText(destination, contents);
    }
}
