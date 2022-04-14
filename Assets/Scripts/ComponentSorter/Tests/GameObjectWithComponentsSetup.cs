using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using UnityEditor.SceneManagement;
using UnityEngine;

public abstract class GameObjectWithComponentsSetup
{
    protected GameObject m_GO;
    protected ComponentSorter m_ComponentSorter;
    protected List<Component> m_Components;

    [SetUp]
    public virtual void Setup()
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

        m_GO = new GameObject("TestGO");

        m_ComponentSorter = m_GO.AddComponent<ComponentSorter>();

        // Add Non-Custom components.
        m_GO.AddComponent<NonCustomComponentsClass1>();
        m_GO.AddComponent<NonCustomComponentsClass2>();
        m_GO.AddComponent<NonCustomComponentsClass2>();
        m_GO.AddComponent<NonCustomComponentsClass3>();
        m_GO.AddComponent<NonCustomComponentsClass3>();
        m_GO.AddComponent<NonCustomComponentsClass3>();

        // Add some Unity components.
        m_GO.AddComponent<BoxCollider>();
        m_GO.AddComponent<Rigidbody>();
        m_GO.AddComponent<MeshRenderer>();

        // Add Custom components.
        m_GO.AddComponent<CustomComponent1>();
        m_GO.AddComponent<CustomComponent2>();
        m_GO.AddComponent<CustomComponent2>();
        m_GO.AddComponent<CustomComponent3>();
        m_GO.AddComponent<CustomComponent3>();
        m_GO.AddComponent<CustomComponent3>();

        m_Components = m_ComponentSorter.Components;
    }

    [TearDown]
    public virtual void TearDown()
    {
        GameObject.DestroyImmediate(m_GO);
        m_Components.Clear();
        GameObject.DestroyImmediate(m_ComponentSorter);
    }

    // Use this to create expected results faster.
    public void DebugIEnumerableToListConstructor(IEnumerable<string> source)
    {
        StringBuilder result = new StringBuilder();
        foreach (var s in source)
        {
            result.AppendLine($"\"{s}\",");
        }
        Debug.Log(result.ToString());
    }

    // Use this to create expected results faster.
    public void DebugIEnumerableToListConstructor(IEnumerable<Component> source)
    {
        StringBuilder result = new StringBuilder();
        foreach (var s in source)
        {
            result.AppendLine($"\"{s.GetType().Name}\",");
        }
        Debug.Log(result.ToString());
    }
}
