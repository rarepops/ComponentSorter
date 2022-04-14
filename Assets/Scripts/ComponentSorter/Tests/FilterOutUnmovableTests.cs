using NUnit.Framework;
using System.Linq;
using UnityEngine;

public class FilterOutUnmovableTests : GameObjectWithComponentsSetup
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
    public void FilterOutUnmovable_Works()
    {
        m_Components = m_GO.GetComponents<Component>().ToList();

        Assert.That(m_Components.Exists(x => x.GetType() == typeof(Transform)));
        Assert.AreEqual(17, m_Components.Count);

        ComponentSorterUtilities.FilterOutUnmovable(ref m_Components);

        Assert.That(!m_Components.Exists(x => x.GetType() == typeof(Transform)));
        Assert.AreEqual(16, m_Components.Count);
    }
}
