using NUnit.Framework;
using System.Collections.Generic;
using AssertionException = UnityEngine.Assertions.AssertionException;

public class ComponentSorterTests : GameObjectWithComponentsSetup
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
    }

    [TearDown]
    public override void TearDown()
    {
        base.TearDown();
    }

    [Test]
    public void SortUsingBlueprint_WhenBlueprintList_IsNull_Throws()
    {
        Assert.Throws<AssertionException>(() => m_ComponentSorter.SortComponentsUsingBlueprint(null));
    }

    [Test]
    public void SortUsingBlueprint_WhenBlueprintList_IsEmpty_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => m_ComponentSorter.SortComponentsUsingBlueprint(new List<string>()));
    }

    [Test]
    public void SortUsingBlueprint_WhenBlueprintList_IsEmpty_DoesNotChangeOrder()
    {
        var expectedResult = new List<string>
            {
                "ComponentSorter",
                "NonCustomComponentsClass1",
                "NonCustomComponentsClass2",
                "NonCustomComponentsClass2",
                "NonCustomComponentsClass3",
                "NonCustomComponentsClass3",
                "NonCustomComponentsClass3",
                "BoxCollider",
                "Rigidbody",
                "MeshRenderer",
                "CustomComponent1",
                "CustomComponent2",
                "CustomComponent2",
                "CustomComponent3",
                "CustomComponent3",
                "CustomComponent3",
            };

        m_ComponentSorter.SortComponentsUsingBlueprint(new List<string>());

        Assert.IsTrue(ComponentSorterUtilities.ComponentsMatchExpectedNames(m_Components, expectedResult));
    }

    [Test]
    public void SortUsingBlueprint_WhenBlueprintList_IsDefault_Works()
    {
        var expectedResult = new List<string>
            {
                "BoxCollider",
                "MeshRenderer",
                "NonCustomComponentsClass1",
                "NonCustomComponentsClass2",
                "NonCustomComponentsClass2",
                "NonCustomComponentsClass3",
                "NonCustomComponentsClass3",
                "NonCustomComponentsClass3",
                "Rigidbody",
                "ComponentSorter",
                "CustomComponent1",
                "CustomComponent2",
                "CustomComponent2",
                "CustomComponent3",
                "CustomComponent3",
                "CustomComponent3",
            };

        m_ComponentSorter.SortComponents();

        m_Components = m_ComponentSorter.Components;

        Assert.IsTrue(ComponentSorterUtilities.ComponentsMatchExpectedNames(m_Components, expectedResult));
    }

    [Test]
    public void SortUsingBlueprint_WhenBlueprintList_IsDefaultAndSortingTwice_Works()
    {
        var expectedResult = new List<string>
            {
                "BoxCollider",
                "MeshRenderer",
                "NonCustomComponentsClass1",
                "NonCustomComponentsClass2",
                "NonCustomComponentsClass2",
                "NonCustomComponentsClass3",
                "NonCustomComponentsClass3",
                "NonCustomComponentsClass3",
                "Rigidbody",
                "ComponentSorter",
                "CustomComponent1",
                "CustomComponent2",
                "CustomComponent2",
                "CustomComponent3",
                "CustomComponent3",
                "CustomComponent3",
            };

        m_ComponentSorter.SortComponents();

        m_Components = m_ComponentSorter.Components;

        Assert.IsTrue(ComponentSorterUtilities.ComponentsMatchExpectedNames(m_Components, expectedResult));

        m_ComponentSorter.SortComponents();

        m_Components = m_ComponentSorter.Components;

        Assert.IsTrue(ComponentSorterUtilities.ComponentsMatchExpectedNames(m_Components, expectedResult));
    }

    [Test]
    public void ShufflingWorks()
    {
        var expectedResult = new List<string>
            {
                "BoxCollider",
                "MeshRenderer",
                "NonCustomComponentsClass1",
                "NonCustomComponentsClass2",
                "NonCustomComponentsClass2",
                "NonCustomComponentsClass3",
                "NonCustomComponentsClass3",
                "NonCustomComponentsClass3",
                "Rigidbody",
                "ComponentSorter",
                "CustomComponent1",
                "CustomComponent2",
                "CustomComponent2",
                "CustomComponent3",
                "CustomComponent3",
                "CustomComponent3",
            };

        m_ComponentSorter.SortComponents();

        m_Components = m_ComponentSorter.Components;

        Assert.IsTrue(ComponentSorterUtilities.ComponentsMatchExpectedNames(m_Components, expectedResult));

        m_ComponentSorter.ShuffleComponents();
        m_Components = m_ComponentSorter.Components;

        Assert.IsFalse(ComponentSorterUtilities.ComponentsMatchExpectedNames(m_Components, expectedResult));
    }
}

