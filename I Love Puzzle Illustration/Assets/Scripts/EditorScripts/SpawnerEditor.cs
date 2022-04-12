using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

#if UNITY_EDITOR

[CanEditMultipleObjects]
[CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor
{
    List<SerializedProperty> properties = new List<SerializedProperty>();

    static readonly List<string> propertyNames = new List<string>
    {
            "tilesParent",
            "spawnConfig",
            "levelData",
            "offset"
    };

    static string[] methods;
    static string[] ignoredMethods = new string[] {"Awake", "Start", "Update", "Initialize", "InitializeTileTypes", "SpawnPattern", "SpawnTile", "IsEven", "PatternCreationError" };

    static SpawnerEditor()
    {
        methods =
            typeof(Spawner)
            .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) // Instance methods, both public and private/protected
            .Where(x => x.DeclaringType == typeof(Spawner)) // Only list methods defined in our own class
            //.Where(x => x.GetParameters().Length == 0) // Make sure we only get methods with zero argumenrts
            .Where(x => !ignoredMethods.Any(n => n == x.Name)) // Don't list methods in the ignoredMethods array (so we can exclude Unity specific methods, etc.)
            .Select(x => x.Name)
            .ToArray();
    }

    void OnEnable()
    {
        properties.Clear();

        foreach(var propertyName in propertyNames)
        {
            var property = serializedObject.FindProperty(propertyName);

            if(property != null) properties.Add(property);
            else Debug.Log("Property: \"" + propertyName + "\" is not found!");
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        foreach(var property in properties)
        {
            EditorGUILayout.PropertyField(property);
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUI.EndChangeCheck();

        Spawner obj = target as Spawner;

        if(obj != null)
        {
            int index;

            try
            {
                index = methods
                    .Select((v, i) => new { Name = v, Index = i })
                    .First(x => x.Name == obj.MethodToCall)
                    .Index;
            }
            catch
            {
                index = 0;
            }

            obj.MethodToCall = methods[EditorGUILayout.Popup("Pattern Type", index, methods)];
        }
    }
}

#endif