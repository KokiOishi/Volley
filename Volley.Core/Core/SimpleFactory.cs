using System;
using System.Collections.Generic;
using System.Text;

namespace Volley
{
    public sealed class SimpleFactory<T> : IFactory<T>
    {
        public SimpleFactory(Func<T> createFunc)
        {
            CreateFunc = createFunc ?? throw new ArgumentNullException(nameof(createFunc));
        }

        public Func<T> CreateFunc { get; }

        public T Create() => CreateFunc.Invoke();
    }

    public sealed class SimpleParametrizedFactory<T, TParam> : IParametricFactory<T, TParam>
    {
        public SimpleParametrizedFactory(Func<TParam, T> createFunc)
        {
            CreateFunc = createFunc ?? throw new ArgumentNullException(nameof(createFunc));
        }

        public Func<TParam, T> CreateFunc { get; }

        public T Create(TParam param) => CreateFunc.Invoke(param);
    }
}
