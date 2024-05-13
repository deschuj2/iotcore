namespace ifm.IoTCore.UnitTests
{
    using System;
    using Common;
    using Common.Variant;
    using Common.Exceptions;
    using Factory;
    using NUnit.Framework;

    [TestFixture]
    public class ServiceExecutionFailedTests
    {
        [Test]
        public void IoTCoreErrorResponseTest_HasDataMessage()
        {
            using var ioTCore = IoTCoreFactory.Create("id0");

            var response = ioTCore.MessageHandler.HandleRequest(0, "/non_existing_service", null);

            Assert.That(response.Code, Is.Not.Null, "response should contain code.");
            Assert.That(response.Data.AsVariantObject().ContainsKey("msg"), "responseData should contain msg tag.");
        }

        [Test]
        public void ServiceErrorResponseTest_InternalError()
        {
            using var ioTCore = IoTCoreFactory.Create("id0");

            ioTCore.ElementManager.CreateActionServiceElement(ioTCore.Root, "failing", (element, i) =>
            {
                throw new Exception("Service Failed");
            },raiseTreeChanged: true);

            var response = ioTCore.MessageHandler.HandleRequest(0, "/failing", null);

            Assert.That(response.Code, Is.EqualTo(ResponseCodes.InternalError));

            Assert.That(response.Code,Is.Not.Null);
            Assert.That(response.Data.AsVariantObject().ContainsKey("msg"));
        }

        [Test]
        public void ServiceErrorResponseTest_ServiceExecutionFailed_withInternalErrorcodeAndMessage()
        {
            using var ioTCore = IoTCoreFactory.Create("id0");

            var errorMessage = "Cannot make coffee. Please empty water tank.";

            ioTCore.ElementManager.CreateActionServiceElement(ioTCore.Root, "failing", (element, i) =>
            {
                throw new IoTCoreException(ResponseCodes.ServiceFailed, errorMessage, 10032);
            },raiseTreeChanged: true);

            var response = ioTCore.MessageHandler.HandleRequest(0, "/failing", null);

            Assert.That(response.Code, Is.EqualTo(ResponseCodes.ServiceFailed));

            Assert.That(response.Code,Is.Not.Null);
            Assert.That(response.Data.AsVariantObject().ContainsKey("msg"));
            Assert.That(response.Data.AsVariantObject().ContainsKey("code"));

            Assert.That((int)response.Data.AsVariantObject()["code"].AsVariantValue(), Is.EqualTo(10032));


        }
    }
}
