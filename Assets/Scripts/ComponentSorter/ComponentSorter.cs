using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[DisallowMultipleComponent]
public class ComponentSorter : MonoBehaviour
{
    // NonCustomComponents - e.g. Unity;
    // Separator - This script;
    // CustomComponents - Non-specific components.
    // The integer represents the default position.
    private readonly List<(string, int)> k_ReservedNames = new List<(string, int)> {
            ("NonCustomComponents",     0),
            ("Separator",               1),
            ("CustomComponents",        2) };


    // The ordering we get from the ReorderableList.
    public List<string> ComponentOrdering;

    // Cached value if there are duplicate entries or not.
    private bool m_HasDuplicates;
    public bool HasDuplicates => m_HasDuplicates;

    // Cache for components. Needs to be updated when needed.
    private List<Component> m_Components = new List<Component>(16);
    public List<Component> Components
    {
        get
        {
            RefreshComponentsCache();
            return m_Components;
        }
    }

    private void OnValidate()
    {
        if (ComponentOrdering == null)
        {
            ComponentOrdering = new List<string>();
        }

        // Add reserved components strings.
        for (int i = 0; i < k_ReservedNames.Count; ++i)
        {
            AddReservedIfMissing(ComponentOrdering, k_ReservedNames[i].Item1, k_ReservedNames[i].Item2);
        }

        // Remove spaces from strings.
        for (int i = 0; i < ComponentOrdering.Count; ++i)
        {
            ComponentOrdering[i] = ComponentOrdering[i].Replace(" ", "");
        }

        RefreshHasDuplicates(ComponentOrdering);
    }

    /// <summary>
    /// Adds reserved component names if missing at the specified index.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="stringToLookFor"></param>
    /// <param name="index"></param>
    private void AddReservedIfMissing(List<string> source, string stringToLookFor, int index = 0)
    {
        foreach (var val in source)
        {
            if (val == stringToLookFor)
            {
                return;
            }
        }

        // If we haven't found the string, add it.
        source.Insert(index, stringToLookFor);
    }

    private void RefreshHasDuplicates(List<string> source)
    {
        m_HasDuplicates = source.Distinct().ToList().Count < source.Count;
    }

    private void RemoveDuplicates(ref List<string> source)
    {
        source = source.Distinct().ToList();
    }

    public void SortComponents()
    {
        RemoveDuplicates(ref ComponentOrdering);

        RefreshComponentsCache();

        var blueprint = ComponentSorterUtilities.ComputeExpandedBlueprint(m_Components, ComponentOrdering);

        if (!ComponentSorterUtilities.ComponentsMatchExpectedNames(m_Components, blueprint))
        {
            SortComponentsUsingBlueprint(blueprint);
        }
    }

    public void ShuffleComponents()
    {
        RefreshComponentsCache();

        foreach (var comp in m_Components)
        {
            ComponentSorterUtilities.MoveComponent(comp, Random.Range(0, m_Components.Count), Random.Range(0, m_Components.Count));
        }
    }

    public void SortComponentsUsingBlueprint(IReadOnlyList<string> blueprint)
    {
        Assert.IsNotNull(blueprint);

        if (blueprint.Count == 0)
        {
            return;
        }

        var tempComponents = Components;

        for (int i = 0; i < blueprint.Count; i++)
        {
            for (int j = 0; j < tempComponents.Count; j++)
            {
                if (blueprint[i] == tempComponents[j].GetType().Name)
                {
                    // We know everything before here is sorted correctly, that's why we give it 0 as "to". 
                    // We just care where we are relatively to the end of the sorted components.
                    ComponentSorterUtilities.MoveComponent(tempComponents[j], j, 0);

                    tempComponents.RemoveAt(j);
                    break;
                }
            }
        }
    }

    private void RefreshComponentsCache()
    {
        GetComponents(m_Components);

        ComponentSorterUtilities.FilterOutUnmovable(ref m_Components);
    }
}

