using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class ScanTrackableEventHandler : DefaultTrackableEventHandler
{
    //da li je postavljen target
    static bool[] placed = new bool[7];
    //da li je pogodjena pozicija
    static bool[] matched = new bool[7];
    //targeti
    static GameObject[] targets = new GameObject[7];
    //da li je prethodna kutija pozicionirana
    static bool finished = true;
    //da li je loadovan XML
    static bool loaded = false;
    //da li je zavrseno skeniranje
    static bool finishedScanning = false;
    //poslednji
    static int last = 0;
    //static bool hasMore = true;

    static int toVerify = -1;

    public Dictionary<int, Vector3> positions = new Dictionary<int, Vector3>();
    public Dictionary<int, Vector3> scales = new Dictionary<int, Vector3>();
    public Dictionary<int, String> names = new Dictionary<int, string>();

    //ime baze
    [SerializeField]
    private Text text;

    void Update()
    {
        
        if (finishedScanning && finished)
        {
            for (int i=last; i < 7; i++, last++) {
                if (placed[i] && !matched[i]) {
                    Vector3 scale = scales[i + 1];
                    Vector3 position = positions[i + 1];

                    //smesta 0. nivo
                    if (Math.Abs(position.y - scale.y / 2) < 0.00001)
                    {
                        finished = false;
                        targets[i].GetComponent<Renderer>().material.color = new Color(0.94f, 0.06f, 0.06f, 0.8f);
                        targets[i].GetComponent<Collider>().enabled = true;
                        GameObject statusGO = GameObject.Find("StatusBar");

                        statusGO.GetComponent<Text>().text = "Postavite " + names[i+1] + " na obelezenu poziciju";
                        last++;
                        return;
                    }

                }
            }

            last = 0;

            for (int i = last; i < 7; i++, last++)
            {
                if (placed[i] && !matched[i])
                {
                    Vector3 scale = scales[i + 1];
                    Vector3 position = positions[i + 1];

                    //smesta 1. nivo
                    if (Math.Abs(Math.Abs(position.y - scale.y / 2) - 0.05f) < 0.00001)
                    {
                        finished = false;
                        //!!!!!!!!!!!!!!!!!
                        targets[i].GetComponent<Renderer>().material.color = new Color(0.94f, 0.06f, 0.06f, 0.8f);
                        targets[i].GetComponent<Collider>().enabled = true;
                        GameObject statusGO = GameObject.Find("StatusBar");
                        statusGO.GetComponent<Text>().text = "Postavite " + names[i + 1] + " na obelezenu poziciju";
                        last++;
                        return;
                    }

                }
            }

            last = 0;

            for (int i = last; i < 7; i++, last++)
            {
                if (placed[i] && !matched[i])
                {
                    Vector3 scale = scales[i + 1];
                    Vector3 position = positions[i + 1];

                    //smesta 1. nivo
                    if (Math.Abs(Math.Abs(position.y - scale.y / 2) - 0.15f) < 0.00001)
                    {
                        finished = false;
                        targets[i].GetComponent<Renderer>().material.color = new Color(0.94f, 0.06f, 0.06f, 0.8f);
                        targets[i].GetComponent<Collider>().enabled = true;
                        GameObject statusGO = GameObject.Find("StatusBar");
                        statusGO.GetComponent<Text>().text = "Postavite " + names[i + 1] + " na obelezenu poziciju";
                        last++;
                        return;
                    }

                }
            }
            //finishedScanning = false;
        }
    }

    public void finishScanning() {

        finishedScanning = true;
        
        GameObject doneGO = GameObject.Find("Done");
        Button done = doneGO.GetComponent<Button>();
       
        done.enabled = false;
        GameObject verifyGO = GameObject.Find("Verify");
        Button verify = verifyGO.GetComponent<Button>();
        verify.enabled=true;

        //prikazuje optimalan raspored kutija
        for (int i = 0; i < 7; i++) {

            if (placed[i]) {
                targets[i].GetComponent<Renderer>().material.color = new Color(0.94f, 0.06f, 0.06f, 0.8f);
            }
        }

        ListUnmatchedBoxes();
        }


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
                                //position = new Vector3(-0.16f + dimensions[2] / 2, 0.05f + dimensions[0] / 2, 0.25f - dimensions[1] / 2);
                                position = new Vector3(-0.25f + dimensions[2] / 2, dimensions[0] / 2 + 0.05f, 0.25f - dimensions[1] / 2);
                                label = "Multivita";
                                break;
                            }
                        case "k4":
                            {
                                scale = new Vector3(dimensions[2], dimensions[0], dimensions[1]);
                                position = new Vector3(-0.25f + dimensions[2] / 2, dimensions[0] / 2 + 0.15f, 0.15f - dimensions[1] / 2);
                                label = "Bambi";
                                break;
                            }
                        case "k5":
                            {
                                scale = new Vector3(dimensions[2], dimensions[0], dimensions[1]);
                                //position = new Vector3(-0.06f + dimensions[2] / 2, 0.04f + dimensions[0] / 2, 0.25f - dimensions[1] / 2);
                                position = new Vector3(-0.25f + dimensions[2] / 2, dimensions[0] / 2 + 0.05f, 0.15f - dimensions[1] / 2);
                                label = "Nesquik";
                                break;
                            }
                        case "k6":
                            {
                                scale = new Vector3(dimensions[2], dimensions[0], dimensions[1]);
                                position = new Vector3(0.14f + dimensions[2] / 2, dimensions[0] / 2, 0.25f - dimensions[1] / 2);
                                label = "Snickers";
                                break;
                            }
                        case "k7":
                            {
                                scale = new Vector3(dimensions[2], dimensions[0], dimensions[1]);
                                position = new Vector3(-0.25f + dimensions[2] / 2, dimensions[0] / 2, 0.02f - dimensions[1] / 2);
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
        if (!loaded)
        {
           
            LoadXML();
            GameObject verifyGO = GameObject.Find("Verify");
            Button verify = verifyGO.GetComponent<Button>();
            verify.enabled=false;
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
                GameObject statusGO = GameObject.Find("StatusBar");
                statusGO.GetComponent<Text>().text = "Verifikujte poziciju";
                toVerify = num;
                //matched[num] = true;
                //Task.Delay(3000);
                //a.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
                a.GetComponent<Renderer>().material.color = new Color(0, 0.8f, 0.2f, 0.8f);
                //finished = true;

                //for (int i = 0; i < 7; i++)
                //{
                //    if (placed[i] && !matched[i])
                //    {
                        
                //        statusGO.GetComponent<Text>().text = "OK: " + names[num + 1] + "! Nastavite dalje.";
                        
                //        return;
                //    }
                //}
                //last = 0;
                //finishedScanning = false;
                ////nije ovde zavrsio nego kad verifikuje poslednju!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //statusGO.GetComponent<Text>().text = "Zavrsili ste! Mozete nastaviti dalje";


            }
        }
    }

    //postavi sve targete ali providne
    protected void CreateTarget(int num, Vector3 scale, Vector3 position) {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        GameObject groundPlane = GameObject.Find("Ground Plane Stage Random (1)");
        Vector3 groundPlanePosition = groundPlane.transform.localPosition;

        cube.transform.parent = groundPlane.transform;

        Material materialColored;
        materialColored = new Material(Shader.Find("Transparent/Diffuse"));
        materialColored.color = new Color(0.96f, 0.82f, 0.6f, 0);
        cube.GetComponent<Renderer>().material = materialColored;
        cube.transform.rotation = new Quaternion(0, 0, 0, 0);

        targets[num] = cube;

        cube.transform.localScale = scale;
        cube.transform.localPosition = position;

        
        GameObject statusGO = GameObject.Find("StatusBar");
        statusGO.GetComponent<Text>().text = "DETEKTOVANO: " + names[num + 1] + "! Nastavite dalje.";

        ((BoxCollider)cube.GetComponent<Collider>()).size = new Vector3(1.2f, 3.0f, 1.2f);
        ((BoxCollider)cube.GetComponent<Collider>()).enabled = false;
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
            if (!placed[num] && finishedScanning==false)
            {
                
                Vector3 scale = scales[num + 1]; 
                Vector3 position = positions[num + 1];
                CreateTarget(num, scale, position);
                    
                    
            }
        }
    }

    public void Verify()
    {

        if (toVerify != -1)
        {
            targets[toVerify].GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
            matched[toVerify] = true;
            finished = true;
            GameObject statusGO = GameObject.Find("StatusBar");
            ListUnmatchedBoxes();



            for (int i = 0; i < 7; i++)
            {
                if (placed[i] && !matched[i])
                {

                    statusGO.GetComponent<Text>().text = "OK: " + names[toVerify + 1] + "! Nastavite dalje.";
                    toVerify = -1;
                    return;
                }
            }
            last = 0;
            finishedScanning = false;
            toVerify = -1;
            // DODATO !!!!
            GameObject doneGO = GameObject.Find("Done");
            Button done = doneGO.GetComponent<Button>();
          //  doneGO.SetActive(true);
            done.enabled = true;
            statusGO.GetComponent<Text>().text = "Zavrsili ste! Mozete nastaviti dalje";
            GameObject verifyGO = GameObject.Find("Verify");
            Button verify = verifyGO.GetComponent<Button>();
            verify.enabled=false;
        }

    }

    public void ListUnmatchedBoxes() {

        GameObject dropDownGO = GameObject.Find("Dropdown");
        Dropdown dropDown = dropDownGO.GetComponent<Dropdown>();

        dropDown.ClearOptions();

        List<string> m_DropOptions = new List<string>();

        for (int i = 0; i < 7; i++) {
            if (placed[i] && !matched[i]) {
                m_DropOptions.Add(names[i + 1]);
            }
        }

        dropDown.AddOptions(m_DropOptions);
    }


}

