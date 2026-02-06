using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Nodes
{
    internal record struct ErrorNode(string Error) : INode; 
}
