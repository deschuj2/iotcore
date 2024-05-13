namespace ifm.IoTCore.UnitTests
{
    using Common.Variant;
    using Newtonsoft.Json.Linq;

    internal static class VariantExtensions
    {
        internal static JToken ToJToken(this Variant variant)
        {
            return VariantConverter.ToJToken(variant);
        }
    }
}
