namespace ifm.IoTCore.UnitTests.Elements
{
    using Common.Variant;
    using Factory;
    using Common;
    using Message;
    using NUnit.Framework;

    [TestFixture]
    class ServiceElementTests
    {
        [Test, Property("TestCaseKey", "IOTCS-T211")]
        public void ServiceElement_Invoked_InputOutput_UserData1UserData2()
        {
            // Given
            using var ioTCore = IoTCoreFactory.Create("ioTCore");
            var struct1 = ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, "struct1",raiseTreeChanged: true);
            object argtest =null;
            var service = ioTCore.ElementManager.CreateServiceElement<complexData, complexData>(struct1, 
                "ServiceElement_InOut_UserData1UserData2",
                (_, inputarg, _) =>
                {
                    argtest = inputarg; 
                    return new complexData();
                }, 
                raiseTreeChanged: true);

            // When
            var response = ioTCore.MessageHandler.HandleRequest(new Message(RequestCodes.Request, 1, "/struct1/ServiceElement_InOut_UserData1UserData2", Variant.FromObject(new complexData())));
            
            // Then
            Assert.That(argtest, Is.EqualTo(new complexData()));
            Assert.That(Variant.ToObject<complexData>(response.Data), Is.EqualTo(new complexData()));
        }

        [Test, Property("TestCaseKey", "IOTCS-T211")]
        public void ServiceElement_Invoked_InputOutput_BoolString()
        {
            // Given
            using var ioTCore = IoTCoreFactory.Create("ioTCore");
            var struct1 = ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, "struct1",raiseTreeChanged: true);
            object argtest =null;
            var service = ioTCore.ElementManager.CreateServiceElement<bool,string>(struct1, "ServiceElement_InOut_BoolString",
                (sender, inputarg, cid) => { argtest = inputarg; return "Forty Two!"; },raiseTreeChanged: true);

            // When
            var response = ioTCore.MessageHandler.HandleRequest(new Message(RequestCodes.Request, 1, "/struct1/ServiceElement_InOut_BoolString", Variant.FromObject(true)));
            
            // Then
            Assert.That((bool)argtest, Is.EqualTo(true));
            Assert.That((string)(VariantValue)response.Data, Is.EqualTo("Forty Two!"));
        }

        [Test, Property("TestCaseKey", "IOTCS-T211")]
        public void ServiceElement_Invoked_InputOutput_IntInt()
        {
            // Given
            using var ioTCore = IoTCoreFactory.Create("ioTCore");
            var struct1 = ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, "struct1",raiseTreeChanged: true);
            object argtest =null;
            var service = ioTCore.ElementManager.CreateServiceElement<int,int>(struct1, 
                "ServiceElement_InOut_IntInt",
                (_, inputarg, _) =>
                {
                    argtest = inputarg; 
                    return 43;
                },
                raiseTreeChanged: true);

            // When
            var response = ioTCore.MessageHandler.HandleRequest(new Message(RequestCodes.Request, 1, "/struct1/ServiceElement_InOut_IntInt", Variant.FromObject(42)));
            
            // Then
            Assert.That((int)argtest, Is.EqualTo(42));
            Assert.That((int)(VariantValue)response.Data, Is.EqualTo(43));
        }

        [Test, Property("TestCaseKey", "IOTCS-T211")]
        public void ActionServiceElement_Invoked_NoInput_NoOutput()
        {
            // Given
            using var ioTCore = IoTCoreFactory.Create("ioTCore");
            var struct1 = ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, "struct1",raiseTreeChanged: true);
            var serviceInvoked =false;
            Assert.That(serviceInvoked, Is.False);
            var service = ioTCore.ElementManager.CreateActionServiceElement(struct1, 
                "actionService",
                (_, _) =>
                {
                    serviceInvoked = true;
                },
                raiseTreeChanged: true);

            // When
            var response = ioTCore.MessageHandler.HandleRequest(0, "/struct1/actionService");
            
            // Then
            Assert.That(serviceInvoked, Is.True);
            Assert.That(response.Data, Is.Null);
        }
    }
}
