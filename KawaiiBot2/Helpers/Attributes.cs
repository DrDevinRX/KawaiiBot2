using System;
using System.Collections.Generic;
using System.Text;

namespace KawaiiBot2
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class DevOnlyCmdAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HiddenCmdAttribute : Attribute
    {

    }
}
