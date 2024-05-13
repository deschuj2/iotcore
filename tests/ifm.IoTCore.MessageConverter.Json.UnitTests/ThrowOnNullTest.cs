namespace ifm.IoTCore.MessageConverter.Json.UnitTests
{
    using Common;
    using Common.Exceptions;
    using NUnit.Framework;

    [TestFixture]
    internal class ThrowOnNullTest
    {
        [Test]
        public void TestDontThrowOnNull()
        {
            var messageConverter = new MessageConverter();

            Assert.DoesNotThrow(() =>
            {
                var result = messageConverter.Deserialize("{\"code\" : 10, \"cid\" : 10, \"adr\": \"asdf\", \"data\": {\"something\" : null}}");
            });
        }

        [Test]
        public void TestThrowOnNull()
        {
            var messageConverter = new MessageConverter(MessageDeserializationMode.ThrowOnNull);

            var exception = Assert.Throws<DataInvalidException>(() =>
            {
                var result = messageConverter.Deserialize("{\"code\" : 10, \"cid\" : 10, \"adr\": \"asdf\", \"data\": {\"something\" : null}}");
            });

            Assert.That(exception.ResponseCode, Is.EqualTo(ResponseCodes.DataInvalid));
        }

        [Test]
        public void TestThrowsDataInvalidOnDuplicateKey()
        {
            var messageConverter = new MessageConverter();

            Assert.Throws<DataInvalidException>(() =>
            {
                try
                {

                    var result = messageConverter.Deserialize(
                        "{\"code\" : 10, \"cid\" : 10, \"adr\": \"asdf\", \"data\": {\"something\" : null, \"something\" : null}}");
                }
                catch (IoTCoreException e)
                {
                    Assert.That(e.ResponseCode, Is.EqualTo(ResponseCodes.DataInvalid));
                    throw;
                }
            });
        }
    }
}
