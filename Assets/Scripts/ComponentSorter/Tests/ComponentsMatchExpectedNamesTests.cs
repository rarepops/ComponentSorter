using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using AssertionException = UnityEngine.Assertions.AssertionException;

public class ComponentsMatchExpectedNamesTests : GameObjectWithComponentsSetup
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
    public void ComponentsMatchExpectedNames_WhenComponentNamesIsNull_Throws()
    {
        Assert.Throws<AssertionException>(() => ComponentSorterUtilities.ComponentsMatchExpectedNames(new List<Component>(), null));
    }

    [Test]
    public void ComponentsMatchExpectedNames_WhenComponentsIsNull_Throws()
    {
        Assert.Throws<AssertionException>(() => ComponentSorterUtilities.ComponentsMatchExpectedNames(null, new List<string>()));
    }

    [Test]
    public void ComponentsMatchExpectedNames_WhenLengthsDontMatch_ReturnsFalse()
    {
        List<string> componentNames = new List<string>
            {
                "AAAA",
            };

        Assert.IsFalse(ComponentSorterUtilities.ComponentsMatchExpectedNames(m_Components, componentNames));
    }

    [Test]
    public void ComponentsMatchExpectedNames_WhenNotSorted_ReturnsFalse()
    {
        List<string> orderingList = new List<string>
            {
                "NonCustomComponents",
                "Separator",
                "CustomComponents"
            };

        var blueprint = ComponentSorterUtilities.ComputeExpandedBlueprint(m_Components, orderingList);

        Assert.IsFalse(ComponentSorterUtilities.ComponentsMatchExpectedNames(m_Components, blueprint));
    }

    // Throwing a ICall error, Unity's fault most likely ;)
    [Test]
    public void ComponentsMatchExpectedNames_WhenSorted_ReturnsTrue()
    {
        List<string> orderingList = new List<string>
            {
                "NonCustomComponents",
                "Separator",
                "CustomComponents"
            };

        var blueprint = ComponentSorterUtilities.ComputeExpandedBlueprint(m_Components, orderingList);

        m_ComponentSorter.SortComponentsUsingBlueprint(blueprint);
        m_Components = m_ComponentSorter.Components;

        Assert.IsTrue(ComponentSorterUtilities.ComponentsMatchExpectedNames(m_Components, blueprint));
    }
}

