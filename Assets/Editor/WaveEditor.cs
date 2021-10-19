using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class WaveEditor : EditorWindow
{
    Texture2D timeTexture;
    Texture2D nextTexture;

    Texture banner;

    string waveName;
    string openPath;

    List<WaveSequence> sequence = new List<WaveSequence>();
    WaveSequence wavePref;

    static WaveEditor window;

    Vector2 scrollPosition;

    Rect timeRect;
    Rect nextRect;

    [MenuItem("Window/WaveEditor")]
    static void OpenWindow()
    {
        if (window == null)
        {
            window = GetWindow<WaveEditor>();
        }
        window.minSize = new Vector2(600, 600);
        window.Show();
    }

    void InitTextures()
    {
        banner = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Editor/Wave/Wave.png", typeof(Texture));

        timeTexture = Resources.Load<Texture2D>("Editor/Wave/Icons/Time");
        timeRect.x = 0;
        timeRect.y = 0;
        timeRect.width = 20;
        timeRect.height = 20;

        nextTexture = Resources.Load<Texture2D>("Editor/Wave/Icons/Next");
        nextRect.x = 0;
        nextRect.y = 0;
        nextRect.width = 12;
        nextRect.height = 12;
    }

    private void OnEnable()
    {
        InitTextures();
    }

    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        Rect r = new Rect();

        r.x = 0;
        r.y = 0;
        r.width = 600;
        r.height = 300;

        GUI.DrawTexture(r, banner, ScaleMode.ScaleToFit, true);

        GUILayout.Space(r.height);

        waveName = EditorGUILayout.TextField("Name", waveName);

        if (GUILayout.Button("Add"))
        {
            WaveSequence seq = new WaveSequence();

            sequence.Add(seq);
        }
        if (GUILayout.Button("Remove"))
        {
            if (sequence.Count > 0)
            {
                sequence.RemoveAt(sequence.Count - 1);
            }
        }

        DrawSequences();

        if (waveName == string.Empty || waveName == null)
        {
            GUI.enabled = false;

            EditorGUILayout.HelpBox("No name", MessageType.Warning);
        }

        if (GUILayout.Button("Save"))
        {
            FormatToJson();
        }

        GUI.enabled = true;

        openPath = EditorGUILayout.TextField("Open name", openPath);

        string path = $"Assets/Resources/Waves/{openPath}/";

        FileInfo fi = new FileInfo(path);

        if (openPath == string.Empty || openPath == null)
        {
            GUI.enabled = false;

            EditorGUILayout.HelpBox("No name", MessageType.Warning);
        }
        else if (!fi.Directory.Exists)
        {
            GUI.enabled = false;

            EditorGUILayout.HelpBox("Can't find wave with this name", MessageType.Warning);
        }

        if (GUILayout.Button("Open"))
        {
            OpenWave();
        }

        GUI.enabled = true;

        GUILayout.EndScrollView();
    }

    void DrawSequences()
    {
        if (window == null)
        {
            window = GetWindow<WaveEditor>();
        }

        int i = 0;
        foreach (WaveSequence s in sequence)
        {
            GUILayout.Label($"Sequence {i}");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                s.Group.Add(null);
                s.Offset.Add(Vector2.zero);
            }
            if (GUILayout.Button("Remove"))
            {
                if (s.amount > 0)
                {
                    s.Group.RemoveAt(s.Group.Count - 1);
                    s.Offset.RemoveAt(s.Offset.Count - 1);
                }
            }
            GUI.enabled = false;
            s.amount = EditorGUILayout.IntField("Amount", s.amount);
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            s.amount = s.Group.Count;

            GUILayout.BeginHorizontal();
            GUILayout.Label("Groups");
            GUILayout.Label("Offsets");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            for (int a = 0; a < s.Group.Count; a++)
            {
                s.Group[a] = EditorGUILayout.ObjectField(s.Group[a], typeof(GameObject), false) as GameObject;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            for (int a = 0; a < s.Offset.Count; a++)
            {
                s.Offset[a] = EditorGUILayout.Vector2Field("", s.Offset[a]);
            }
            EditorGUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            s.time = EditorGUILayout.FloatField("Time", s.time);

            GUILayout.Space(40);

            i++;
        }
    }

    void FormatToJson()
    {
        int i = 0;

        foreach (WaveSequence s in sequence)
        {
            string json = JsonUtility.ToJson(s);

            string path = $"Assets/Resources/Waves/{waveName}/sequence{i}.json";

            FileInfo fi = new FileInfo(path);
            if (!fi.Directory.Exists)
            {
                System.IO.Directory.CreateDirectory(fi.DirectoryName);
            } 

            StreamWriter writer = new StreamWriter(path, false);

            writer.WriteLine(json);

            writer.Close();

            i++;

            AssetDatabase.ImportAsset(path);
        }
    }

    void OpenWave()
    {
        string path = $"Assets/Resources/Waves/{openPath}/";

        FileInfo fi = new FileInfo(path);
        if (!fi.Directory.Exists)
        {
            return;
        }

        path = $"Waves/{openPath}/";

        TextAsset[] jsonTextFile = Resources.LoadAll<TextAsset>(path);

        sequence.Clear();

        foreach (TextAsset txt in jsonTextFile)
        {
            sequence.Add((WaveSequence)JsonUtility.FromJson(txt.text, typeof(WaveSequence)));
        }

        waveName = openPath;
        openPath = "";
    }
}
