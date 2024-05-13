namespace ifm.IoTCore.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Variant;
    using Factory;
    using Message;
    using NUnit.Framework;
    using ServiceData.Responses;

    [TestFixture]
    public class SubscriberList_Tests
    {
        [Test, Property("TestCaseKey", "IOTCS-T38")]
        public void SubscriberList_MultipleEvents()
        { 
            using var ioTCore = IoTCoreFactory.Create("ioTCore");
            ioTCore.ElementManager.CreateSimpleDataElement(ioTCore.Root, "data1", 42);

            var eventIDs = new List<string> { "myevent", "myevent2", "myevent3", "myevent4", "myevent5" };
            var randomCids = new List<int>(5);
            var randomCid = 0; // Random sometimes creates same value
            foreach (var id in eventIDs)
            {
                var myevent = ioTCore.ElementManager.CreateEventElement(ioTCore.Root, id, raiseTreeChanged: true);
                var svcaddr = string.Format("/{0}/subscribe", myevent.Identifier);
                var data = new VariantObject
                {
                    { "callback", new VariantValue("http://callback/not/considered/on/subscribe") }, 
                    { "datatosend", new VariantArray { new VariantValue("/data1") } }
                };
                ++randomCid;
                randomCids.Add(randomCid);

                ioTCore.MessageHandler.HandleRequest(new Message(RequestCodes.Request, randomCid, svcaddr, data)); 
            }
            // check subscriptions were appended
            var subscriptions = ioTCore.MessageHandler.HandleRequest(new Message(RequestCodes.Request, 1, "/getsubscriberlist", null));
            var subscriptionsData = Variant.ToObject<GetSubscriberListResponseServiceData>(subscriptions.Data);

            Assert.That(subscriptionsData.Count(), Is.EqualTo(5));
            for (var i = 0; i < 5; i++)
            {
                Assert.That(randomCids[i],Is.EqualTo(subscriptionsData[i].SubscriptionId), "subscribeid should match the subscribe request. iot core compatibility");
            }
        }
    }
}
