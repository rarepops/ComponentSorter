using NUnit.Framework;
using System;

public class IsSeparatorComponentTests
{
    [TestCase(typeof(ComponentSorter),      ExpectedResult = true,  TestName = "WhenIsSeparatorComponent_ReturnsTrue")]
    [TestCase(typeof(CustomComponent1),     ExpectedResult = false, TestName = "WhenIsCustomComponentonent_ReturnsFalse")]
    [TestCase(typeof(RandomComponent),      ExpectedResult = false, TestName = "WhenIsNotCustomComponentonent_ReturnsFalse")]
    [TestCase(typeof(UnityEngine.Collider), ExpectedResult = false, TestName = "WhenIsUnityComponent_ReturnsFalse")]
    public bool TestIsSeparatorComponent(Type type)
    {
        return ComponentSorterUtilities.IsSeparatorComponent(type);
    }

    class RandomComponent { }
}
