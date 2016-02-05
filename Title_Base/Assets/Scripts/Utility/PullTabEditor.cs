using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct LastTabInfo
{
    public int NonTabChildren;
    public GameObject Tab;
}

public class PullTabEditor : MonoBehaviour
{
    public List<List<GameObject>> TileGrid = new List<List<GameObject>>();
    [HideInInspector]
    public GameObject CurrentTile = null;

    [HideInInspector]
    public bool PoppedUp = true;
    [ExecuteInEditMode]
    void Start ()
    {
	    
	}

	// Update is called once per frame
	void Update ()
    {
        
        //Debug.Log("Update");
    }

    void OnRenderObject()
    {
        //Debug.Log("Render");
    }
}

#if UNITY_EDITOR
namespace CustomInspector
{
    using UnityEditor;
    using System.Reflection;

    public enum TileType
    {
        None,
        SmallBendy,
        LargeBendy,
        NoBendy
    }

    [CanEditMultipleObjects]
    [CustomEditor(typeof(PullTabEditor), true)]
    public class PullTabEditorEditor : Editor
    {
        Material OutlineMatArch;
        Material DefaultMatArch;
        GameObject PanelArch;
        GameObject SmallPanelArch;
        GameObject BaseArch;
        int Row = 0;
        int Column = 0;
        Vector3 CenterPos = new Vector3(0, -0.025f, 1.375f);
        float TileWidth = 1;

        GameObject CurrentPanel
        {
            get
            {
                return Comp.CurrentTile;
            }
            set
            {
                Comp.CurrentTile = value;
            }
        }

        PullTabEditor Comp = null;
        public void OnEnable()
        {
            DefaultMatArch = Resources.Load<Material>("BlankCardBoard");
            OutlineMatArch = Resources.Load<Material>("OutlinedCardboard");
            PanelArch = Resources.Load<GameObject>("TabChild");
            BaseArch = Resources.Load<GameObject>("PanelBase");
            SmallPanelArch = Resources.Load<GameObject>("SmallTabChild");
            Comp = target as PullTabEditor;
            Comp.TileGrid.Clear();
            
            foreach (Transform i in Comp.GetComponentInChildren<Transform>())
            {
                Debug.Log(i.name);
                

                if (i.gameObject.name.Contains("Column"))
                {

                    char number = i.gameObject.name[i.gameObject.name.Length - 1];
                    var index = (int)char.GetNumericValue(number);
                    Comp.TileGrid.Insert(index, new List<GameObject>());
                    foreach (Transform j in i.GetComponentsInChildren<Transform>())
                    {
                        if(j.gameObject.name == "PanelBase" || j.gameObject.name == "TabChild")
                        {
                            Comp.TileGrid[index].Add(j.gameObject);
                        }
                        
                    }
                }
            }

            if(Comp.TileGrid.Count > Column)
            {
                if(Comp.TileGrid[Column].Count > Row)
                {
                    
                }
                else
                {
                    Row = 0;
                }
            }
            else
            {
                Column = 0;
            }
            if(Comp.TileGrid.Count > 0)
            {
                SetCurrentPanel(Comp.TileGrid[Column][Row]);
            }
            

        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (Comp.TileGrid.Count == 0)
            {
                if (GUILayout.Button("Create Popup"))
                {
                    AddColumn();
                }
            }
            else
            {
                if (GUILayout.Button("Clear Tiles"))
                {
                    DestroyTiles();
                }
            }

            if (Comp.PoppedUp)
            {
                if (GUILayout.Button("Flatten Tiles"))
                {
                    Comp.PoppedUp = false;
                    SetPanelFlatState(true);
                }
            }
            else
            {
                if (GUILayout.Button("Popup Tiles"))
                {
                    Comp.PoppedUp = true;
                    SetPanelFlatState(false);
                }
            }
        }

        void OnSceneGUI()
        {
            if(Comp.TileGrid.Count == 0)
            {
                return;
            }
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                    {

                        if (e.character == '=' || e.character == '+')
                        {
                            if(Row != 0)
                            {
                                SetPanelRotation(new Vector3(0, 90, 0));
                            }
                            else
                            {
                                SetPanelRotation(new Vector3(90, 0, 0));
                            }
                            
                        }
                        else if(e.character == '-' || e.character == '_')
                        {
                            if (Row != 0)
                            {
                                SetPanelRotation(new Vector3(0, -90, 0));
                            }
                            else
                            {
                                SetPanelRotation(new Vector3(-90, 0, 0));
                            }
                        }

                        if(e.keyCode == KeyCode.W)
                        {
                            ++Row;
                            if(Comp.TileGrid[Column].Count > Row)
                            {
                                SetCurrentPanel(Comp.TileGrid[Column][Row]);
                            }
                            else
                            {
                                Debug.Log("Add Row Tile" + Row);
                                TileType tileType;
                                if(e.shift)
                                {
                                    tileType = TileType.LargeBendy;
                                }
                                else if (e.control)
                                {
                                    tileType = TileType.SmallBendy;
                                }
                                else
                                {
                                    tileType = TileType.NoBendy;
                                }
                                AddRowTile(FindLastTabChild(Row - 1), tileType);
                                
                            }
                        }
                        else if (e.keyCode == KeyCode.S)
                        {
                            if(Row > 0)
                            {
                                --Row;
                            }
                            SetCurrentPanel(Comp.TileGrid[Column][Row]);
                        }
                    }
                    break;
            }
        }

        void SetPanelRotation(Vector3 rot)
        {
            if (!CurrentPanel || CurrentPanel.name == "PanelBase")
            {
                return;
            }
            CurrentPanel.transform.localEulerAngles += rot;
            CurrentPanel.GetComponent<PullTabChild>().BendVector = CurrentPanel.transform.localEulerAngles;
        }

        void AddColumn()
        {
            var column = new List<GameObject>();
            Comp.TileGrid.Insert(Column, column);
            var panel = Instantiate<GameObject>(PanelArch);
            panel.transform.position = CenterPos;
            panel.transform.SetParent(Comp.transform, false);
            panel.name = "Column" + Column;
            
            column.Add(panel);
            SetCurrentPanel(panel);
            SetPanelRotation(new Vector3());

        }

        LastTabInfo FindLastTabChild(int row)
        {
            int nonTabChildren = 0;
            LastTabInfo info = new LastTabInfo();
            Debug.Log(row);
            var column = Comp.TileGrid[Column];
            while(row > 0)
            {
                if (column[row].name == "TabChild")
                {
                    info.Tab = column[row];
                    info.NonTabChildren = nonTabChildren;
                    return info;
                }
                ++nonTabChildren;
                --row;
            }
            info.NonTabChildren = nonTabChildren;
            info.Tab = column[0];
            return info;
        }

        void AddRowTile(LastTabInfo parent, TileType tileType)
        {
            
            var column = Comp.TileGrid[Column];
            if(column.Count > Row && column[Row])
            {
                return;
            }
            GameObject panel;
            switch (tileType)
            {
                case TileType.NoBendy:
                {
                    panel = Instantiate<GameObject>(BaseArch);
                    panel.name = "PanelBase";
                }
                break;
                case TileType.SmallBendy:
                {
                    panel = Instantiate<GameObject>(SmallPanelArch);
                    panel.name = "TabChild";
                }
                break;
                case TileType.LargeBendy:
                {
                    panel = Instantiate<GameObject>(PanelArch);
                    panel.name = "TabChild";
                }
                break;
                default:
                {
                    panel = Instantiate<GameObject>(BaseArch);
                    panel.name = "PanelBase";
                }
                break;
            };





            
            panel.transform.eulerAngles = new Vector3(0, -270, 90);
            panel.transform.SetParent(parent.Tab.transform, true);
            panel.transform.localPosition = new Vector3(0, 0, 10);
            panel.transform.localEulerAngles = new Vector3(0, 0, 0);
            var steps = parent.Tab.transform.childCount;
            if(parent.Tab.transform.FindChild("Cylinder"))
            {
                --steps;
            }
            panel.transform.localPosition += new Vector3(0, 0, -10 * steps);
            if(tileType == TileType.NoBendy)
            {
                panel.transform.localPosition += new Vector3(0, 0, -5);
            }
            
            Debug.Log("Column " + column.Count);
            Debug.Log("Row " + Row);
            if(column.Count > Row)
            {
                if(!column[Row])
                {
                    column[Row] = panel;
                }
            }
            else
            {
                Debug.Log("Before Insert");
                column.Insert(Row, panel);
                Debug.Log("After Insert");
            }
            
            SetCurrentPanel(panel);

        }

        void SetCurrentPanel(GameObject panel)
        {
            Debug.Log(panel);
            if(CurrentPanel)
            {
                MeshRenderer oldRenderer;
                if (CurrentPanel.name != "PanelBase")
                {
                    oldRenderer = CurrentPanel.transform.FindChild("PanelBase").GetComponent<MeshRenderer>();
                }
                else
                {
                    oldRenderer = CurrentPanel.GetComponent<MeshRenderer>();
                }
                oldRenderer.material = DefaultMatArch;
            }
            MeshRenderer renderer;
            if (panel.name != "PanelBase")
            {
                renderer = panel.transform.FindChild("PanelBase").GetComponent<MeshRenderer>();
            }
            else
            {
                renderer = panel.GetComponent<MeshRenderer>();
            }
            renderer.material = OutlineMatArch;
            
            CurrentPanel = panel;
        }

        void SetPanelFlatState(bool setFlat)
        {
            foreach(var i in Comp.TileGrid)
            {
                bool firstDone = false;
                foreach(var j in Comp.transform.GetComponentsInChildren<PullTabChild>())
                {
                    if (!firstDone)
                    {
                        firstDone = true;
                        continue;
                    }
                        
                    if(setFlat)
                    {
                        j.transform.localEulerAngles -= j.BendVector;
                    }
                    else
                    {
                        j.transform.localEulerAngles = j.BendVector;
                    }
                    
                }
            }
        }

        void DestroyTiles()
        {
            foreach(var i in Comp.TileGrid)
            {
                foreach(var j in i)
                {
                    if(j != null)
                    {
                        DestroyImmediate(j);
                    }
                }
            }
            Comp.TileGrid.Clear();
            Row = 0;
            Column = 0;
            Comp.PoppedUp = true;
        }
    }

    
}
#endif

