namespace ifm.IoTCore.Common.UnitTests;

using System;
using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class CollectionExtensionsTests
{
    [Test]
    public void DictionaryRemoveAllInvalidParameter_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => ((IDictionary<int, int>)null).RemoveAll(null));

        var dic = new Dictionary<int, int>();
        Assert.Throws<ArgumentNullException>(() => dic.RemoveAll(null));
    }

    [Test]
    public void DictionaryRemoveAll_Success()
    {
        var dic = new Dictionary<int, int> { { 1, 1 }, { 2, 1 }, { 3, 3 }, { 4, 4 } };

        dic.RemoveAll(x => x == 1);

        Assert.That(dic.Count == 2);
    }

    [Test]
    public void ListAddIfNotNullInvalidParameter_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => ((IList<int?>)null).AddIfNotNull(null));
    }

    [Test]
    public void ListAddIfNotNull_Success()
    {
        var list = new List<int?>();

        list.AddIfNotNull(1);
        Assert.That(list.Count == 1);

        list.AddIfNotNull(null);
        Assert.That(list.Count == 1);
    }
}