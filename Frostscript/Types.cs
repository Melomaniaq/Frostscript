using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript
{
    internal struct Void();
    internal record struct FrostFunction(INode Body, Closure Closure);

}
