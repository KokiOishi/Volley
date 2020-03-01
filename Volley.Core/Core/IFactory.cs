using System;

namespace Volley
{
    /// <summary>
    /// Defines a base infrastructure of a simple Factory patterned object.
    /// </summary>
    public interface IParametricFactory<out T, in TParam>
    {
        /// <summary>
        /// Create the <typeparamref name="T"/> instance using specified param.
        /// </summary>
        /// <param name="param">Parameter.</param>
        T Create(TParam param);
    }

    public interface IFactory<out T>
    {
        T Create();
    }
}