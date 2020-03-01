using System;
using System.Collections.Generic;
using System.Text;

namespace Volley.Rules.Building
{
    public interface IFactoryBuilder<T>
    {
        IFactory<T> Build();
    }
}