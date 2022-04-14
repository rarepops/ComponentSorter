using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public static class ComponentSorterUtilities
{
    // The root assembly name where components are. Used to determine if it is a custom component or not.
    private static readonly string k_CustomComponentsAssemblyName = "CustomComponents";

    public static object ComputeExpandedBlueprint(List<Component> m_Components)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Given a source of components and the layout string, compute the expanded future layout (blueprint).
    /// If the layout can't find any strings (if given) it will return an empty array.
    /// </summary>
    /// <param name="componentSource"></param>
    /// <param name="componentsOrder"></param>
    /// <returns></returns>
    public static List<string> ComputeExpandedBlueprint(IReadOnlyList<Component> componentSource, IReadOnlyList<string> componentsOrder)
    {
        Assert.IsNotNull(componentSource);
        Assert.IsTrue(componentSource.Count > 0);
        Assert.IsNotNull(componentsOrder);
        Assert.IsTrue(componentsOrder.Count > 0);

        // The expanded blueprint based on the components ordering we want.
        var computedBlueprint = new List<string>(componentSource.Count);

        // Need a copy that we can modify by removing alreadty found components from. Makes searches faster and need less checks.
        var componentSourceCopy = new List<Component>(componentSource);
        // Index to be used later to specify where "CustomComponents" components should be placed at.
        int customComponentsMedIndex = -1;

        for (int i = 0; i < componentsOrder.Count && componentSourceCopy.Count != 0; ++i)
        {
            switch (componentsOrder[i])
            {
                case "Separator":
                    {
                        for (int j = componentSourceCopy.Count - 1; j >= 0; --j)
                        {
                            var componentSourceCopyType = componentSourceCopy[j].GetType();
                            if (IsSeparatorComponent(componentSourceCopyType))
                            {
                                computedBlueprint.Add(typeof(ComponentSorter).Name);
                                componentSourceCopy.RemoveAt(j);
                                break;
                            }
                        }
                        break;
                    }
                case "NonCustomComponents":
                    {
                        var NonCustomComponents = new List<string>(componentSourceCopy.Count);
                        for (int j = componentSourceCopy.Count - 1; j >= 0; --j)
                        {
                            var componentSourceCopyType = componentSourceCopy[j].GetType();
                            if (!IsCustomComponentonent(componentSourceCopyType) &&
                                !IsSeparatorComponent(componentSourceCopyType))
                            {
                                NonCustomComponents.Add(componentSourceCopy[j].GetType().Name);
                                componentSourceCopy.RemoveAt(j);
                            }
                        }
                        NonCustomComponents.Sort();

                        computedBlueprint.AddRange(NonCustomComponents);

                        break;
                    }
                case "CustomComponents":
                    {
                        // We mark the index in order to use it at a later point.
                        customComponentsMedIndex = computedBlueprint.Count;
                        break;
                    }
                default:
                    {
                        // Checks if it is a CustomComponent, Not a separator, and if the type name matches the component 
                        // in the ordering we want to achieve.
                        for (int j = componentSourceCopy.Count - 1; j >= 0; --j)
                        {
                            var componentSourceCopyType = componentSourceCopy[j].GetType();

                            // If we want all components to be sub-categorized, remove the IsCustomComponentonent check.
                            if (ComponentTypeNameMatchesString(componentSourceCopyType, componentsOrder[i]) &&
                                IsCustomComponentonent(componentSourceCopyType) &&
                                !IsSeparatorComponent(componentSourceCopyType))
                            {
                                computedBlueprint.Add(componentSourceCopy[j].GetType().Name);
                                componentSourceCopy.RemoveAt(j);
                            }
                        }
                        break;
                    }
            }
        }

        // If we are here it means all that should be left are other Custom components that weren't specified.
        if (componentSourceCopy.Count > 0 && customComponentsMedIndex != -1)
        {
            var CustomComponents = new List<string>(componentSourceCopy.Count);
            foreach (var comp in componentSourceCopy)
            {
                CustomComponents.Add(comp.GetType().Name);
            }
            CustomComponents.Sort();

            computedBlueprint.InsertRange(customComponentsMedIndex, CustomComponents);
        }

        return computedBlueprint;
    }

    public static void FilterOutUnmovable(ref List<Component> source)
    {
        Assert.IsNotNull(source);

        for (int i = source.Count - 1; i >= 0; --i)
        {
            if (source[i].GetType() == typeof(Transform))
            {
                source.RemoveAt(i);
            }
        }
    }

    public static bool ComponentsMatchExpectedNames(IReadOnlyList<Component> components, IReadOnlyList<string> componentNames)
    {
        Assert.IsNotNull(components);
        Assert.IsNotNull(componentNames);

        if (components.Count != componentNames.Count)
        {
            return false;
        }

        for (int i = 0; i < components.Count; ++i)
        {
            if (components[i].GetType().Name != componentNames[i])
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsCustomComponentonent(Type type)
    {
        Assert.IsNotNull(type);
        // Could use TypeCache here if we know all our components inherit from some type for way better performance than reflection.
        return type.Assembly.GetName().Name == k_CustomComponentsAssemblyName;
    }

    public static bool IsSeparatorComponent(Type type)
    {
        return ComponentTypeNameMatchesString(type, typeof(ComponentSorter).Name);
    }

    public static bool ComponentTypeNameMatchesString(Type type, string stringToMatch)
    {
        Assert.IsNotNull(type);
        Assert.IsFalse(string.IsNullOrEmpty(stringToMatch));

        return type.Name == stringToMatch;
    }

    public static void MoveComponent(Component component, int from, int to)
    {
        if (to == from)
        {
            return;
        }

        var delta = to - from;

        while (delta != 0)
        {
            if (delta < 0)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentUp(component);
                delta++;
            }
            else
            {
                UnityEditorInternal.ComponentUtility.MoveComponentDown(component);
                delta--;
            }
        }
    }
}
