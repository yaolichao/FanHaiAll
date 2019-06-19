using System;

namespace FanHai.Gui.Core
{
    /// <summary>
    /// Interface for classes that can evaluate conditions defined in the addin tree.
    /// </summary>
    public interface IConditionEvaluator
    {
        bool IsValid(object caller, Condition condition);
    }
}
