namespace ifm.IoTCore.UnitTests
{
    using System.Linq;
    using Common;
    using ElementManager.Contracts.Elements;
    using Factory;
    using NUnit.Framework;
    using ServiceData.Requests;
    using ServiceData.Responses;

    [TestFixture]
    public class SubTreeTests
    {
        [Test]
        public void TestSubTreeLevel4()
        {
            using var ioTCore = IoTCoreFactory.Create("id0");
            var structureElementLevel1 = ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, "level1",raiseTreeChanged: true);
            var structureElementLevel2 = ioTCore.ElementManager.CreateStructureElement(structureElementLevel1, "level2",raiseTreeChanged: true); 
            var structureElementLevel3 = ioTCore.ElementManager.CreateStructureElement(structureElementLevel2, "level3",raiseTreeChanged: true);
            var structureElementLevel4 = ioTCore.ElementManager.CreateStructureElement(structureElementLevel3, "level4",raiseTreeChanged: true);

            var getTreeService =
                (ioTCore.Root.Subs.Single(x => x.Identifier.ToLower() == Identifiers.GetTree.ToLowerInvariant()) as
                    IServiceElement<GetTreeRequestServiceData, GetTreeResponseServiceData>);

            var result = getTreeService.Invoke(new GetTreeRequestServiceData("id0", null));

            var element4 = result.Subs.First(x => x.Identifier == "level1").Subs.First(x => x.Identifier == "level2").Subs
                .First(x => x.Identifier == "level3").Subs.First(x => x.Identifier == "level4");
            
            Assert.That(element4,Is.Not.Null);
            Assert.That(structureElementLevel4.Address, Is.EqualTo(element4.Address));
        }

        [Test]
        public void TestSubTreeLevel3()
        {
            using var ioTCore = IoTCoreFactory.Create("id0");
            var structureElementLevel1 = ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, "level1", raiseTreeChanged: true);
            var structureElementLevel2 = ioTCore.ElementManager.CreateStructureElement(structureElementLevel1, "level2", raiseTreeChanged: true);
            var structureElementLevel3 = ioTCore.ElementManager.CreateStructureElement(structureElementLevel2, "level3", raiseTreeChanged: true);
            var structureElementLevel4 = ioTCore.ElementManager.CreateStructureElement(structureElementLevel3, "level4", raiseTreeChanged: true);

            var getTreeService =
                (ioTCore.Root.Subs.Single(x => x.Identifier.ToLower() == Identifiers.GetTree.ToLowerInvariant()) as
                    IServiceElement<GetTreeRequestServiceData, GetTreeResponseServiceData>);

            var result = getTreeService.Invoke(new GetTreeRequestServiceData("id0", 3));

            var element3 = result.Subs.First(x => x.Identifier == "level1").Subs.First(x => x.Identifier == "level2")
                .Subs
                .FirstOrDefault(x => x.Identifier == "level3");
            Assert.That(element3,Is.Not.Null);

            var element4 = element3.Subs?.FirstOrDefault(x => x.Identifier == "level4");
            Assert.That(element4,Is.Null);
            Assert.That(structureElementLevel3.Address, Is.EqualTo(element3.Address));
        }

        [Test]
        public void TestSubTreeLevel2()
        {
            using var ioTCore = IoTCoreFactory.Create("id0");
            var structureElementLevel1 = ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, "level1", raiseTreeChanged: true);
            var structureElementLevel2 = ioTCore.ElementManager.CreateStructureElement(structureElementLevel1, "level2", raiseTreeChanged: true);
            var structureElementLevel3 = ioTCore.ElementManager.CreateStructureElement(structureElementLevel2, "level3", raiseTreeChanged: true);
            var structureElementLevel4 = ioTCore.ElementManager.CreateStructureElement(structureElementLevel3, "level4", raiseTreeChanged: true);

            var getTreeService =
                (ioTCore.Root.Subs.Single(x => x.Identifier.ToLower() == Identifiers.GetTree.ToLowerInvariant()) as
                    IServiceElement<GetTreeRequestServiceData, GetTreeResponseServiceData>);

            var result = getTreeService.Invoke(new GetTreeRequestServiceData("id0", 2));

            var element2 = result.Subs.First(x => x.Identifier == "level1").Subs.First(x => x.Identifier == "level2");
            Assert.That(element2,Is.Not.Null);
            if (element2.Subs != null)
            {
                Assert.That(element2.Subs.FirstOrDefault(x=>x.Identifier == "level3"),Is.Null);
            }
            Assert.That(structureElementLevel2.Address, Is.EqualTo(element2.Address));
        }
    }
}
