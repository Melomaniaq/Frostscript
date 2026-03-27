using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Internal
{
    internal record VariableData(IDataType DataType, bool Mutable);
}
