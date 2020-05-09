using System;

namespace Mvc.Core.Attribute
{
    /// <summary>
    /// 忽略模型验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IgnoreVerifyAttribute : System.Attribute
    {

    }
}
