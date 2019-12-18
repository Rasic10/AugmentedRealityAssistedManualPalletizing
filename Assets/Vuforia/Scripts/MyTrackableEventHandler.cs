using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

public class MyTrackableEventHandler : DefaultTrackableEventHandler
{
    //da li je postavljen target
    static bool[] placed = new bool[7];
    //da li je pogodjena pozicija
    static bool[] matched = new bool[7];
    //targeti
    static GameObject[] targets = new GameObject[7];

    static bool finished = true;

    static bool loaded=false;

    static int toVerify = -1;

    public Dictionary<int, Vector3> positions = new Dictionary<int, Vector3>();
    public Dictionary<int, Vector3> scales = new Dictionary<int, Vector3>();
    public Dictionary<int, String> names = new Dictionary<int, string>();

    //ime baze
    [SerializeField]
    private Text text;

    protected void LoadXML()
    {
        XmlNodeList levelsList;

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(xmlFile.text);

        levelsList = xmlDoc.GetElementsByTagName("ImageTarget");

        string[] tmp = new string[3];
        float[] dimensions = new float[3];
        

        //foreach box
        for (int i = 1; i < 8; i++)
        {
            foreach (XmlNode node in levelsList)
            {
                String name = "k" + i;
                String label = "";

                if (node.Attributes[0].InnerText.Equals(name + ".Left"))
                {
                    string[] helper = node.Attributes[1].InnerText.Split(' ');

                    tmp[0] = helper[0];
                    tmp[1] = helper[1];

                    Debug.Log(name + "( " + tmp[0] + ", " + tmp[1] + ", " + tmp[2] + ")");

                    for (int j = 0; j < 3; j++)
                    {
                        dimensions[j] = float.Parse(tmp[j], System.Globalization.CultureInfo.InvariantCulture);
                    }
                    Vector3 scale = new Vector3(), position = new Vector3();
                    switch (name)
                    {
                        case "k1":
                            {
                                scale = new Vector3(dimensions[2], dimensions[0], dimensions[1]);
                                position = new Vector3(-0.25f + dimensions[2] / 2, dimensions[0] / 2, 0.25f - dimensions[1] / 2);
                                label = "Kozel";
                                break;
                            }
                        case "k2":
                            {
                                scale = new Vector3(dimensions[1], dimensions[0], dimensions[2]);
                                position = new Vector3(-0.1f + dimensions[1] / 2, dimensions[0] / 2, 0.25f - dimensions[2] / 2);
                                label = "Tuborg";
                                break;
                            }
                        case "k3":
                            {
                                scale = new Vector3(dimensions[2], dimensions[0], dimensions[1]);
                                position = new Vector3(-0.16f + dimensions[2] / 2, 0.05f + dimensions[0] / 2, 0.25f - dimensions[1] / 2);
                                label = "Multivita";
                                break;
                            }
                        case "k4":
                            {
                                scale = new Vector3(dimensions[2], dimensions[0], dimensions[1]);
                                position = new Vector3(-0.02f + dimensions[2] / 2, dimensions[0] / 2, 0.13f - dimensions[1] / 2);
                                label = "Bambi";
                                break;
                            }
                        case "k5":
                            {
                                scale = new Vector3(dimensions[2], dimensions[0], dimensions[1]);
                                position = new Vector3(-0.06f + dimensions[2] / 2, 0.04f + dimensions[0] / 2, 0.25f - dimensions[1] / 2);
                                label = "Nesquik";
                                break;
                            }
                        case "k6":
                            {
                                scale = new Vector3(dimensions[2], dimensions[0], dimensions[1]);
                                position = new Vector3(-0.1f + dimensions[2] / 2, dimensions[0] / 2, 0.13f - dimensions[1] / 2);
                                label = "Snickers";
                                break;
                            }
                        case "k7":
                            {
                                scale = new Vector3(dimensions[2], dimensions[0], dimensions[1]);
                                position = new Vector3(0.06f + dimensions[2] / 2, dimensions[0] / 2, 0.25f - dimensions[1] / 2);
                                label = "Guinness";
                                break;
                            }
                    }
                    scales.Add(i, scale);
                    positions.Add(i, position);
                    names.Add(i, label);
                    break;
                }
                if (node.Attributes[0].InnerText.Equals(name + ".Bottom"))
                {
                    tmp[2] = node.Attributes[1].InnerText.Split(' ')[0];
                }
            }
        }
    }

    

    protected override void Start()
    {
        base.Start();

        //GameObject MainMenuGO = GameObject.Find("Commands");
        //MainMenuScript MainMenu = MainMenuGO.GetComponent<MainMenuScript>();    
        //if (!loaded && MainMenu.again)
        if (!loaded)
        {
            //placed = new bool[7];
            //matched = new bool[7];
            //targets = new GameObject[7];

            //finished = true;

            //toVerify = -1;

            //positions = new Dictionary<int, Vector3>();
            //scales = new Dictionary<int, Vector3>();
            //names = new Dictionary<int, string>();
            //loaded = true;
            //MainMenu.again = false;
            
            LoadXML();
            //loaded = true;
        }
    }



    void OnCollisionStay(Collision collisionInfo)
    {
        string name = mTrackableBehaviour.TrackableName;
        int num = name[1] - '0';
        num--;

        if (placed[num] && !matched[num])
        {
            Collider a = targets[num].GetComponent<Collider>();
            Collider b = mTrackableBehaviour.gameObject.GetComponent<Collider>();

            

            if (a.bounds.Contains(b.bounds.min) && a.bounds.Contains(b.bounds.max) && !matched[num])
            {
                toVerify = num;
                GameObject statusGO = GameObject.Find("StatusBar");
                //statusGO.GetComponent<Text>().text = "OK: " + names[num + 1] + "! Nastavite dalje.";
                statusGO.GetComponent<Text>().text = "Verifikujte poziciju";
                //matched[num] = true;
                a.GetComponent<Renderer>().material.color = new Color(0, 0.8f, 0.2f, 0.8f);
                
                //a.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
                //finished = true;
            }
        }
    }

  

    protected void CreateTarget(int num, Vector3 scale, Vector3 position) {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        GameObject groundPlane = GameObject.Find("Ground Plane Stage");
        Vector3 groundPlanePosition = groundPlane.transform.localPosition;

        cube.transform.parent = groundPlane.transform;

        Material materialColored;
        materialColored = new Material(Shader.Find("Transparent/Diffuse"));
        materialColored.color = new Color(0.94f, 0.06f, 0.06f, 0.8f);
        cube.GetComponent<Renderer>().material = materialColored;
        cube.transform.rotation = new Quaternion(0, 0, 0, 0);

        targets[num] = cube;

        cube.transform.localScale = scale;
        cube.transform.localPosition = position;

        //Debug.Log("Target" + scale);
        GameObject statusGO = GameObject.Find("StatusBar");
        statusGO.GetComponent<Text>().text = "Postavite " + names[num+1] + " na obelezenu poziciju";


        ((BoxCollider)cube.GetComponent<Collider>()).size = new Vector3(1.2f, 3.0f, 1.2f);

        placed[num] = true;
    }

    protected override void OnTrackingFound()
    {
        if (mTrackableBehaviour)
        {
            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

            // Enable rendering:
            foreach (var component in rendererComponents)
                component.enabled = true;

            // Enable colliders:
            foreach (var component in colliderComponents)
                component.enabled = true;

            // Enable canvas':
            foreach (var component in canvasComponents)
                component.enabled = true;

            // Print size:
            string name = mTrackableBehaviour.TrackableName;
            int num = name[1] - '0';
            num--;
            
            //ako nije vec instanciran taj target i ako je prethodno detektovana kutija matchovana
            if (!placed[num] && finished)
            {
                
                Vector3 scale = scales[num + 1]; 
                Vector3 position = positions[num + 1];
                
                if(boxAllowed(scale, position, num + 1))
                {
                    CreateTarget(num, scale, position);
                    finished = false;
                }
                    
            }
        }
    }

    private bool boxAllowed(Vector3 scale, Vector3 position, int num)
    {
        Debug.Log("boxAllowed="+ Math.Abs(position.y - scale.y / 2));
        if (Math.Abs(position.y - scale.y / 2) < 0.00001)
            return true;

        return firstLevelPlaced(num);
    }

    private bool firstLevelPlaced(int num)
    {
        Debug.Log("firstLevel=");
        
        for (int i = 1; i < positions.Count + 1; i++)
        {
            Debug.Log("firstLevel=" + Math.Abs(positions[i].y - scales[i].y / 2));
            Debug.Log("firstLevel="+matched[i-1]);
            if (Math.Abs(positions[i].y - (scales[i].y)/ 2) < 0.00001 && matched[i-1] == false)
            {
                GameObject statusGO = GameObject.Find("StatusBar");
                statusGO.GetComponent<Text>().text = "ERROR: Nije popunjen prvi nivo!";
      
                return false;
            }
                
        }

        return true;
    }

    public void Verify() {

        if (toVerify != -1) {
            targets[toVerify].GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
            matched[toVerify] = true;
            finished = true;
            GameObject statusGO = GameObject.Find("StatusBar");
            statusGO.GetComponent<Text>().text = "OK: " + names[toVerify+1] + "! Nastavite dalje.";
            toVerify = -1;
        }

    }
}

