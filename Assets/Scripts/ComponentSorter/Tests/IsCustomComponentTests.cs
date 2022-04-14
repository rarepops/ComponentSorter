using NUnit.Framework;
using System;

public class IsCustomComponentonentTests
{
    [TestCase(typeof(CustomComponent1),     ExpectedResult = true,  TestName = "WhenIsCustomComponentonent_ReturnsTrue")]
    [TestCase(typeof(ComponentSorter),      ExpectedResult = false, TestName = "WhenIsSeparatorComponent_ReturnsFalse")]
    [TestCase(typeof(RandomComponent),      ExpectedResult = false, TestName = "WhenIsNotCustomComponentonent_ReturnsFalse")]
    [TestCase(typeof(UnityEngine.Collider), ExpectedResult = false, TestName = "WhenIsUnityComponent_ReturnsFalse")]
    public bool TestIsCustomComponentonent(Type type)
    {
        return ComponentSorterUtilities.IsCustomComponentonent(type);
    }

    class RandomComponent { }
}

