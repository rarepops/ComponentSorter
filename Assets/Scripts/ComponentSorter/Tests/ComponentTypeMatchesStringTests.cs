using NUnit.Framework;
using System;

public class ComponentTypeMatchesStringTests
{

    [TestCase(typeof(RandomComponent), "RandomComponent", ExpectedResult = true, TestName = "WhenComponentTypeMatchesString_ReturnsTrue")]
    [TestCase(typeof(RandomComponent), "SomeComponent", ExpectedResult = false, TestName = "WhenComponentTypeMatchesString_DifferentName_ReturnsFalse")]
    [TestCase(typeof(RandomComponent), "Random Component", ExpectedResult = false, TestName = "WhenComponentTypeMatchesString_HasSpaces_ReturnsFalse")]
    public bool TestComponentTypeMatchesStringTests(Type type, string stringToMatch)
    {
        return ComponentSorterUtilities.ComponentTypeNameMatchesString(type, stringToMatch);
    }

    class RandomComponent { }
}
