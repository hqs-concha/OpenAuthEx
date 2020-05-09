
using System;

namespace Mvc.Core.Attribute
{
    /// <summary>
    /// 忽略请求验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IgnoreAntiReplayAttribute : System.Attribute
    {
    }
}
