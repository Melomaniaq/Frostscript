using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Internal
{
    internal record ValidationVariable(IDataType DataType, bool Mutable);
}
