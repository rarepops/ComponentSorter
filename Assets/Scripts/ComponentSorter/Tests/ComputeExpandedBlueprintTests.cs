using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AssertionException = UnityEngine.Assertions.AssertionException;

public class ComputeExpandedBlueprintTests : GameObjectWithComponentsSetup
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
    public void ComputeExpandedBlueprint_WhenOrderingList_IsEmpty_Throws()
    {
        Assert.Throws<AssertionException>(() => ComponentSorterUtilities.ComputeExpandedBlueprint(m_Components, new List<string>()));
    }

    [Test]
    public void ComputeExpandedBlueprint_WhenOrderingList_IsNull_Throws()
    {
        Assert.Throws<AssertionException>(() => ComponentSorterUtilities.ComputeExpandedBlueprint(m_Components, null));
    }

    [Test]
    public void ComputeExpandedBlueprint_WhenComponentsList_IsEmpty_Throws()
    {
        Assert.Throws<AssertionException>(() => ComponentSorterUtilities.ComputeExpandedBlueprint(new List<Component>(), new List<string> { "AAA" }));
    }

    [Test]
    public void ComputeExpandedBlueprint_WhenComponentsList_IsNull_Throws()
    {
        Assert.Throws<AssertionException>(() => ComponentSorterUtilities.ComputeExpandedBlueprint(null, new List<string> { "AAA" }));
    }

    [Test]
    public void ComputeExpandedBlueprint_WhenOrderingList_IsNotMatchingAny_WillBeEmpty()
    {
        List<string> orderingList = new List<string> { "AAA" };

        var blueprint = ComponentSorterUtilities.ComputeExpandedBlueprint(m_Components, orderingList);

        Assert.That(blueprint.Count, Is.EqualTo(0));
    }

    [Test]
    public void ComputeExpandedBlueprint_WhenOrderingList_IsDefault_Works()
    {
        List<string> orderingList = new List<string>
            {
                "NonCustomComponents",
                "Separator",
                "CustomComponents"
            };

        var blueprint = ComponentSorterUtilities.ComputeExpandedBlueprint(m_Components, orderingList);

        var blueprintResult = new List<string>
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

        Assert.True(blueprint.SequenceEqual(blueprintResult));

        Assert.That(blueprint.Count, Is.EqualTo(16));
    }

    [Test]
    public void ComputeExpandedBlueprint_WhenOrderingList_HasSpecificCustomElements_Works()
    {
        List<string> orderingList = new List<string>
            {
                "CustomComponent3",
                "NonCustomComponents",
                "Separator",
                "CustomComponents"
            };

        var blueprint = ComponentSorterUtilities.ComputeExpandedBlueprint(m_Components, orderingList);

        var blueprintResult = new List<string>
            {
                "CustomComponent3",
                "CustomComponent3",
                "CustomComponent3",
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
            };

        Assert.True(blueprint.SequenceEqual(blueprintResult));

        Assert.That(blueprint.Count, Is.EqualTo(16));
    }

    [Test]
    public void ComputeExpandedBlueprint_WhenOrderingList_HasSpecificCustomElementsAndSeparatorIsFirst_Works()
    {
        List<string> orderingList = new List<string>
            {
                "Separator",
                "CustomComponent3",
                "NonCustomComponents",
                "CustomComponents"
            };

        var blueprint = ComponentSorterUtilities.ComputeExpandedBlueprint(m_Components, orderingList);

        var blueprintResult = new List<string>
            {
                "ComponentSorter",
                "CustomComponent3",
                "CustomComponent3",
                "CustomComponent3",
                "BoxCollider",
                "MeshRenderer",
                "NonCustomComponentsClass1",
                "NonCustomComponentsClass2",
                "NonCustomComponentsClass2",
                "NonCustomComponentsClass3",
                "NonCustomComponentsClass3",
                "NonCustomComponentsClass3",
                "Rigidbody",
                "CustomComponent1",
                "CustomComponent2",
                "CustomComponent2",
            };

        Assert.True(blueprint.SequenceEqual(blueprintResult));

        Assert.That(blueprint.Count, Is.EqualTo(16));
    }

    [Test]
    public void ComputeExpandedBlueprint_WhenOrderingList_HasSpaces_IgnoreElement()
    {
        List<string> orderingList = new List<string>
            {
                "Custom Comp3",
                "NonCustomComponents",
                "Separator",
                "CustomComponents"
            };

        var blueprint = ComponentSorterUtilities.ComputeExpandedBlueprint(m_Components, orderingList);

        var blueprintResult = new List<string>
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

        Assert.True(blueprint.SequenceEqual(blueprintResult));

        Assert.That(blueprint.Count, Is.EqualTo(16));
    }

    [Test]
    public void ComputeExpandedBlueprint_WhenOrderingList_HasSpecificNonCustomComponentsElements_IgnoresElement()
    {
        List<string> orderingList = new List<string>
            {
                "Rigidbody",
                "NonCustomComponents",
                "Separator",
                "CustomComponents"
            };

        var blueprint = ComponentSorterUtilities.ComputeExpandedBlueprint(m_Components, orderingList);

        var blueprintResult = new List<string>
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

        Assert.True(blueprint.SequenceEqual(blueprintResult));

        Assert.That(blueprint.Count, Is.EqualTo(16));
    }
}

