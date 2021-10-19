using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelEditor : EditorWindow
{
    string levelId;
    string openPath;

    [SerializeField] Wave sequence;

    Vector2 scrollPosition;

    static LevelEditor window;
    Texture banner;

   [MenuItem("Window/LevelEditor")]
    static void OpenWindow()
    {
        if (window == null)
        {
            window = GetWindow<LevelEditor>();
        }
        window.minSize = new Vector2(600, 600);
        window.Show();
    }

    private void OnEnable()
    {
        banner = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Resources/Editor/Level/Level.png", typeof(Texture));
    
        if (sequence == null)
        {
            sequence = new Wave();
        }
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

        levelId = EditorGUILayout.TextField("Level id", levelId);

        if (GUILayout.Button("Add"))
        {
            sequence.sequences.Add("");
            sequence.percentages.Add(0);
        }
        if (GUILayout.Button("Remove"))
        {
            if (sequence.sequences.Count > 0)
            {
                sequence.sequences.RemoveAt(sequence.sequences.Count - 1);
                sequence.percentages.RemoveAt(sequence.percentages.Count - 1);
            }
        }
        if (GUILayout.Button("Clear"))
        {
            if (sequence.sequences.Count > 0)
            {
                sequence.sequences.Clear();
                sequence.percentages.Clear();
            }
        }

        DrawSequences();

        sequence.amount = sequence.sequences.Count;

        if (levelId == string.Empty || levelId == null)
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

        string path = $"Assets/Resources/Levels/{openPath}";

        FileInfo fi = new FileInfo(path);

        if (openPath == string.Empty || openPath == null)
        {
            GUI.enabled = false;

            EditorGUILayout.HelpBox("No name", MessageType.Warning);
        }
        else if (!fi.Directory.Exists)
        {
            GUI.enabled = false;

            EditorGUILayout.HelpBox("Can't find level with this name", MessageType.Warning);
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
            window = GetWindow<LevelEditor>();
        }

        for (int i = 0; i < sequence.sequences.Count; i++)
        {
            sequence.sequences[i] = EditorGUILayout.TextField($"Wave {i}", sequence.sequences[i]);
            sequence.percentages[i] = EditorGUILayout.FloatField($"Invocate at (%)", sequence.percentages[i]);

            string path = $"Assets/Resources/Waves/{sequence.sequences[i]}";
            
            FileInfo fi = new FileInfo(path);
            
            if (string.IsNullOrEmpty(sequence.sequences[i]) || !fi.Directory.Exists)
            {
                EditorGUILayout.HelpBox("Wave don't exist", MessageType.Warning);
            }
        }
    }

    void FormatToJson()
    {
        string json = JsonUtility.ToJson(sequence);

        string path = $"Assets/Resources/Levels/{levelId}.json";

        FileInfo fi = new FileInfo(path);
        if (!fi.Directory.Exists)
        {
            System.IO.Directory.CreateDirectory(fi.DirectoryName);
        }

        StreamWriter writer = new StreamWriter(path, false);

        writer.WriteLine(json);

        writer.Close();

        AssetDatabase.ImportAsset(path);
    }

    void OpenWave()
    {
        string path = $"Assets/Resources/Levels/{openPath}.json";

        FileInfo fi = new FileInfo(path);
        if (!fi.Directory.Exists)
        {
            return;
        }

        path = $"Levels/{openPath}";

        TextAsset jsonTextFile = Resources.Load<TextAsset>(path);

        sequence = (Wave)JsonUtility.FromJson(jsonTextFile.text, typeof(Wave));

        levelId = openPath;
        openPath = "";
    }
}
