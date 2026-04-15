using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Domain.Internal
{
    public record VariableData(IDataType DataType, bool Mutable);
}
