namespace ifm.IoTCore.UnitTests.Elements
{
    using Common.Variant;
    using Factory;
    using NUnit.Framework;

    [TestFixture]
    class ReaderServiceElementTests
    {
        [Test, Property("TestCaseKey", "IOTCS-T211")]
        public void ReaderServiceElement_Invoked_OutputsInt()
        {
            // Given
            using var ioTCore = IoTCoreFactory.Create("ioTCore");
            var struct1 = ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, "struct1", raiseTreeChanged: true);
            var serviceInvoked=false;
            Assert.That(serviceInvoked, Is.False);
            var service = ioTCore.ElementManager.CreateGetterServiceElement<int>(struct1, 
                "readerServiceInt",
                (sender, cid) =>
                {
                    serviceInvoked = true;
                    return 42;
                },
                raiseTreeChanged: true);

            // When
            var response = ioTCore.MessageHandler.HandleRequest(0, "/struct1/readerserviceint", null);

            // Then
            Assert.That(serviceInvoked, Is.True);
            Assert.That((int)(VariantValue)response.Data, Is.EqualTo(42));
        }

        [Test, Property("TestCaseKey", "IOTCS-T211")]
        public void ReaderServiceElement_Invoked_OutputsString()
        {
            // Given
            using var ioTCore = IoTCoreFactory.Create("ioTCore");
            var struct1 = ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, "struct1", raiseTreeChanged: true);
            var serviceInvoked=false;
            Assert.That(serviceInvoked, Is.False);
            var service = ioTCore.ElementManager.CreateGetterServiceElement<string>(struct1, 
                "readerServiceString",
                (_, _) =>
                {
                    serviceInvoked = true; 
                    return "Forty Two";
                },
                raiseTreeChanged: true);

            // When
            var response = ioTCore.MessageHandler.HandleRequest(0, "/struct1/readerservicestring");

            // Then
            Assert.That(serviceInvoked, Is.True);
            Assert.That((string)(VariantValue)response.Data, Is.EqualTo("Forty Two"));
        }

        [Test, Property("TestCaseKey", "IOTCS-T211")]
        public void ReaderServiceElement_Invoked_OutputsFloat()
        {
            // Given
            using var ioTCore = IoTCoreFactory.Create("ioTCore");
            var struct1 = ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, "struct1", raiseTreeChanged: true);
            var serviceInvoked=false;
            Assert.That(serviceInvoked, Is.False);
            var service = ioTCore.ElementManager.CreateGetterServiceElement<float>(struct1, 
                "readerServiceFloat",
                (_, _) =>
                {
                    serviceInvoked = true; 
                    return 42f;
                },
                raiseTreeChanged: true);

            // When
            var response = ioTCore.MessageHandler.HandleRequest(0, "/struct1/readerservicefloat");
            
            // Then
            Assert.That(serviceInvoked, Is.True);
            Assert.That((float)(VariantValue)response.Data, Is.EqualTo(42f).Within(double.Epsilon));
        }

        [Test, Property("TestCaseKey", "IOTCS-T211")]
        public void ReaderServiceElement_Invoked_OutputsBool()
        {
            // Given
            using var ioTCore = IoTCoreFactory.Create("ioTCore");
            var struct1 = ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, "struct1", raiseTreeChanged: true);
            var serviceInvoked=false;
            Assert.That(serviceInvoked, Is.False);
            var service = ioTCore.ElementManager.CreateGetterServiceElement<string>(struct1, 
                "readerServiceBool",
                (_, _) =>
                {
                    serviceInvoked = true; 
                    return "Forty Two";
                },
                raiseTreeChanged: true);

            // When
            var response = ioTCore.MessageHandler.HandleRequest(0, "/struct1/readerserviceBool");

            // Then
            Assert.That(serviceInvoked, Is.True);
            Assert.That((string)(VariantValue)response.Data, Is.EqualTo("Forty Two"));
        }


        [Test, Property("TestCaseKey", "IOTCS-T211")]
        public void ReaderServiceElement_Invoked_OutputsUserData()
        {
            // Given
            using var ioTCore = IoTCoreFactory.Create("ioTCore");
            var struct1 = ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, "struct1", raiseTreeChanged: true);
            var serviceInvoked=false;
            Assert.That(serviceInvoked, Is.False);
            var service = ioTCore.ElementManager.CreateGetterServiceElement<complexData>(struct1, "readerServiceUserData",
                (sender, cid) =>
                {
                    serviceInvoked = true;
                    return new complexData();
                },
                raiseTreeChanged: true);

            // When
            var response = ioTCore.MessageHandler.HandleRequest(0, "/struct1/readerserviceUserData");

            // Then
            Assert.That(serviceInvoked, Is.True);

            var data = Variant.ToObject<complexData>(response.Data);
            Assert.That(data, Is.EqualTo(new complexData()));
        }
    }
}
