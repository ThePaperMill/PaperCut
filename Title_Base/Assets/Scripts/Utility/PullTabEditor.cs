using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

struct LastTabInfo
{
    public int NonTabChildren;
    public GameObject Tab;
}

public class PullTabEditor : MonoBehaviour
{
    public bool PopUp = true;
    
    [HideInInspector]
    public bool FoldIn = false;
    [HideInInspector]
    public float RotationAmount = 90;
    [HideInInspector]
    public GameObject LargeBendyArch;
    [HideInInspector]
    public GameObject SmallBendyArch;
    [HideInInspector]
    public GameObject PanelBaseArch;

    public List<List<GameObject>> TileGrid = new List<List<GameObject>>();
    [HideInInspector]
    public GameObject CurrentTile = null;

    [HideInInspector]
    public bool PoppedUp = true;
    
    void Start ()
    {
	    if(!PopUp)
        {
            var comps = GetComponentsInChildren<PullTabChild>();
            foreach(PullTabChild i in comps)
            {
                if(!i.gameObject.name.Contains("Column"))
                {
                    if(FoldIn)
                    {
                        i.BendVector = i.transform.localEulerAngles + i.BendVector;
                    }
                    else
                    {
                        i.BendVector = i.transform.localEulerAngles - i.BendVector;
                    }
                    
                }
            }
        }
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
        
        Material LastSelectedMat;
        Material OutlineMatArch;
        Material DefaultMatArch;
        GameObject PanelArch { get { return Comp.LargeBendyArch; } set { Comp.LargeBendyArch = value; } }
        GameObject SmallPanelArch { get { return Comp.SmallBendyArch; } set { Comp.SmallBendyArch = value; } }
        GameObject BaseArch { get { return Comp.PanelBaseArch; } set { Comp.PanelBaseArch = value; } }
        int Row = 0;
        int Column = 0;
        Vector3 CenterPos = new Vector3(0, -0.025f, 1.375f);
        //float TileWidth = 1;
        SerializedProperty FoldInProp;
        bool Confirmation = false;

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
            Comp = target as PullTabEditor;
            FoldInProp = serializedObject.FindProperty("FoldIn");
            DefaultMatArch = Resources.Load<Material>("BlankCardBoard");
            OutlineMatArch = Resources.Load<Material>("OutlinedCardboard");
            if (!PanelArch)
            {
                PanelArch = Resources.Load<GameObject>("TabChild");
            }
            if(!BaseArch)
            {
                BaseArch = Resources.Load<GameObject>("PanelBase");
            }
            if(!SmallPanelArch)
            {
                SmallPanelArch = Resources.Load<GameObject>("SmallTabChild");
            }
            
            Comp.TileGrid.Clear();

            foreach (Transform i in Comp.GetComponentInChildren<Transform>())
            {
                //Debug.Log(i.name);
                SetCurrentPanel(null);
                //Column
                if (i.gameObject.name[0] == 'C')
                {

                    char number = i.gameObject.name[i.gameObject.name.Length - 1];
                    var index = (int)char.GetNumericValue(number);
                    if(index >= Comp.TileGrid.Count)
                    {
                        for(int j = Comp.TileGrid.Count; j <= index; ++j)
                        {
                            Comp.TileGrid.Add(new List<GameObject>());
                        }
                    }
                    
                    foreach (Transform j in i.GetComponentsInChildren<Transform>())
                    {
                        //PanleBase and TabChild
                        if(j.gameObject.name[0] == 'P' || j.gameObject.name[0] == 'T')
                        {
                            if(Comp.TileGrid[index].Count == 0)
                            {
                                Comp.TileGrid[index].Add(j.gameObject.transform.parent.gameObject);
                                
                            }
                            else
                            {
                                Comp.TileGrid[index].Add(j.gameObject);
                                
                            }
                            
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
            if(Comp.TileGrid.Count > 0 && !Application.isPlaying)
            {
                SetCurrentPanel(Comp.TileGrid[Column][Row]);
            }
            

        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (!Comp.PopUp)
            {
                EditorGUILayout.PropertyField(FoldInProp);
                serializedObject.ApplyModifiedProperties();
            }
            GUILayout.Label("Use WASD to navigate and add tiles." + 
                            "\nHold SHIFT to add a tile that should be bent towards you." + 
                            "\nHold CTRL to add a tile that should bent away from you." +
                            "\nUse + and - to rotate a bendable tile forward or backward." +
                            "\nUse R to remove a tile. \nWARNING: This will remove ALL of its children.");
            

            if (GUILayout.Button("Clear Selection"))
            {
                SetCurrentPanel(null);
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

            if (Comp.TileGrid.Count == 0)
            {
                if (GUILayout.Button("Create Popup"))
                {
                    AddColumn(CenterPos);
                }
            }
            else
            {
                if (!Confirmation)
                {
                    if (GUILayout.Button("Clear Tiles"))
                    {
                        Confirmation = true;
                    }
                }

                if (Confirmation)
                {
                    GUILayout.Label(" ");
                    GUILayout.Label("ARE YOU SURE YOU WANT TO CLEAR?");


                    if (GUILayout.Button("NO"))
                    {
                        Confirmation = false;
                    }
                    if (GUILayout.Button("YES"))
                    {
                        Confirmation = false;
                        DestroyTiles();
                    }
                }
            }
            bool allowSceneObjects = !EditorUtility.IsPersistent(target);
            Comp.RotationAmount = EditorGUILayout.FloatField("Rotation Amount", Comp.RotationAmount);

            Comp.LargeBendyArch = (GameObject)EditorGUILayout.ObjectField("Large Bendy", Comp.LargeBendyArch, typeof(GameObject), allowSceneObjects);
            Comp.SmallBendyArch = (GameObject)EditorGUILayout.ObjectField("Small Bendy", Comp.SmallBendyArch, typeof(GameObject), allowSceneObjects);
            Comp.PanelBaseArch = (GameObject)EditorGUILayout.ObjectField("Panel Base", Comp.PanelBaseArch, typeof(GameObject), allowSceneObjects);
            
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
                                SetPanelRotation(new Vector3(0, Comp.RotationAmount, 0));
                            }
                            else
                            {
                                SetPanelRotation(new Vector3(Comp.RotationAmount, 0, 0));
                            }
                            
                        }
                        else if(e.character == '-' || e.character == '_')
                        {
                            if (Row != 0)
                            {
                                SetPanelRotation(new Vector3(0, -Comp.RotationAmount, 0));
                            }
                            else
                            {
                                SetPanelRotation(new Vector3(-Comp.RotationAmount, 0, 0));
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
                        else if (e.keyCode == KeyCode.A)
                        {
                            if (Column == 0)
                            {
                                var leftColumn = Comp.TileGrid[Column][0].transform;

                                for (int i = 0; i < Comp.TileGrid.Count; ++i)
                                {
                                    Comp.TileGrid[i][0].name = "Column" + (i + 1);
                                }
                                AddColumn(leftColumn.localPosition - new Vector3(1, 0, 0));
                            }
                            else
                            {
                                --Column;
                                if (Row >= Comp.TileGrid[Column].Count)
                                {
                                    Row = Comp.TileGrid[Column].Count - 1;
                                }

                            }
                            SetCurrentPanel(Comp.TileGrid[Column][Row]);
                        }
                        else if (e.keyCode == KeyCode.D)
                        {
                            ++Column;
                            if (Column == Comp.TileGrid.Count)
                            {
                                Row = 0;
                                var rightColumn = Comp.TileGrid[Comp.TileGrid.Count - 1][0].transform;

                                AddColumn(rightColumn.localPosition + new Vector3(1, 0, 0));
                            }
                            else
                            {
                                if(Row >= Comp.TileGrid[Column].Count)
                                {
                                    Row = Comp.TileGrid[Column].Count - 1;
                                }
                            }
                            SetCurrentPanel(Comp.TileGrid[Column][Row]);
                        }
                        else if (e.keyCode == KeyCode.Delete || e.keyCode == KeyCode.R)
                        {
                            DeleteCurrentTile();
                        }
                    }
                    break;
            }

            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                SetCurrentPanel(null);
            }
        }

        void ShiftLeft()
        {
            for (int i = Column; i < Comp.TileGrid.Count; ++i)
            {
                Comp.TileGrid[i][0].transform.localPosition -= new Vector3(1, 0, 0);
                Comp.TileGrid[i][0].name = "Column" + (i);
            }
        }

        void ShiftRight()
        {
            for (int i = 0; i < Column; ++i)
            {
                Comp.TileGrid[i][0].transform.localPosition += new Vector3(1, 0, 0);
            }
            for (int i = Column; i < Comp.TileGrid.Count; ++i)
            {
                Comp.TileGrid[i][0].name = "Column" + (i);
            }
        }

        void DeleteCurrentTile()
        {
            var currentTile = Comp.TileGrid[Column][Row];
            if (currentTile.name == "PanelBase")
            {
                var info = FindLastTabChild(Row);
                List<Transform> Results = new List<Transform>(info.NonTabChildren + 1);
                info.Tab.transform.GetComponentsInChildren<Transform>(Results);
                bool destroy = false;
                for(var i = 1; i < Results.Count; ++i)
                {
                    if(Results[i].gameObject == currentTile)
                    {
                        destroy = true;
                        continue;
                    }
                    if(!destroy)
                    {
                        continue;
                    }
                    if (Results[i].name != "PanelBase")
                    {
                        DestroyImmediate(Results[i].gameObject);
                        break;
                    }
                    DestroyImmediate(Results[i].gameObject);
                }
            }

            if (Row == 0)
            {
                Comp.TileGrid.RemoveAt(Column);
                if(currentTile.transform.localPosition.x >= 0)
                {
                    ShiftLeft();
                }
                else
                {
                    ShiftRight();
                }
            }
            else
            {
                Comp.TileGrid[Column].RemoveRange(Row, Comp.TileGrid[Column].Count - Row);
            }
            DestroyImmediate(currentTile);

            if (Column >= Comp.TileGrid.Count)
            {
                Column = Comp.TileGrid.Count - 1;
                if (Column < 0)
                {
                    Column = 0;
                }
            }
            if(Comp.TileGrid. Count > 0 && Row >= Comp.TileGrid[Column].Count)
            {
                Row = Comp.TileGrid[Column].Count - 1;
                if(Row < 0)
                {
                    Row = 0;
                }
            }
            if(Comp.TileGrid.Count > 0)
            {
                SetCurrentPanel(Comp.TileGrid[Column][Row]);
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

        void AddColumn(Vector3 localPos)
        {
            var column = new List<GameObject>();
            if(Column == Comp.TileGrid.Count)
            {
                Comp.TileGrid.Add(column);
            }
            else
            {
                Comp.TileGrid.Insert(Column, column);
            }
            
            var panel = Instantiate<GameObject>(PanelArch);
            panel.transform.SetParent(Comp.transform, false);
            panel.transform.localPosition = localPos;
            panel.name = "Column" + Column;
            
            column.Add(panel);
            SetCurrentPanel(panel);
            SetPanelRotation(new Vector3());

        }

        LastTabInfo FindLastTabChild(int row)
        {
            int nonTabChildren = 0;
            LastTabInfo info = new LastTabInfo();
            
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
                if(!LastSelectedMat)
                {
                    LastSelectedMat = DefaultMatArch;
                }
                oldRenderer.material = LastSelectedMat;
            }
            if (!panel)
            {
                CurrentPanel = panel;
                return;
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
            LastSelectedMat = renderer.sharedMaterial;
            
            renderer.material = OutlineMatArch;
            
            CurrentPanel = panel;
        }

        void SetPanelFlatState(bool setFlat)
        {
            foreach(var i in Comp.TileGrid)
            {
                bool firstDone = false;
                foreach(var j in i[0].transform.GetComponentsInChildren<PullTabChild>())
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
            var remaining = Comp.GetComponentsInChildren<PullTabChild>();
            foreach (var i in remaining)
            {
                DestroyImmediate(i.gameObject);
            }
            Comp.TileGrid.Clear();
            Row = 0;
            Column = 0;
            Comp.PoppedUp = true;
        }

        void OnDisable()
        {
            SetCurrentPanel(null);
        }

        void OnDestroy()
        {
            SetCurrentPanel(null);
        }

    }
}
#endif

