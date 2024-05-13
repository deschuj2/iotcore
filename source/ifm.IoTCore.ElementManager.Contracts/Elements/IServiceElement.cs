namespace ifm.IoTCore.ElementManager.Contracts.Elements;

using Common.Variant;

/// <summary>
/// Provides functionality to interact with a service element.
/// </summary>
public interface IServiceElement : IBaseElement
{
    /// <summary>
    /// Invokes the service function.
    /// </summary>
    /// <param name="data">The service parameter.</param>
    /// <param name="cid">The context id.</param>
    /// <returns>The service result.</returns>
    Variant Invoke(Variant data = null, int? cid = null);
}

/// <summary>
/// Provides functionality to interact with a action service element.
/// </summary>
public interface IActionServiceElement : IServiceElement
{
    /// <summary>
    /// Invokes the action service function.
    /// </summary>
    /// <param name="cid">The context id.</param>
    void Invoke(int? cid = null);
}

/// <summary>
/// Provides functionality to interact with a getter service element.
/// </summary>
/// <typeparam name="TOut">The data type.</typeparam>
public interface IGetterServiceElement<out TOut> : IServiceElement
{
    /// <summary>
    /// Invokes the getter service function.
    /// </summary>
    /// <param name="cid">The context id.</param>
    /// <returns>The data to get.</returns>
    TOut Invoke(int? cid = null);
}

/// <summary>
/// Provides functionality to interact with a setter service element.
/// </summary>
/// <typeparam name="TIn">The data type.</typeparam>
public interface ISetterServiceElement<in TIn> : IServiceElement
{
    /// <summary>
    /// Invokes the setter service function.
    /// </summary>
    /// <param name="data">The data to set.</param>
    /// <param name="cid">The context id.</param>
    void Invoke(TIn data, int? cid = null);
}

/// <summary>
/// Provides functionality to interact with a setter and getter service element.
/// </summary>
/// <typeparam name="TIn">The parameter data type.</typeparam>
/// <typeparam name="TOut">The return data type.</typeparam>
public interface IServiceElement<in TIn, out TOut> : IServiceElement
{
    TOut Invoke(TIn data, int? cid = null);
}
