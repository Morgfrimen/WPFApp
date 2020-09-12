using System;
using System.Collections;

namespace ModelsData
{
    public interface IModel
    {
        Type GetType();

        ArrayList GetValueModels();
    }
}