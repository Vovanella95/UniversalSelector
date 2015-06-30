using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniParser
{
    public interface IDocument : IElement
    {

    }
    public interface IElement
    {
        string Name { get; set; }
        string Id { get; set; }
        IEnumerable<string> Classes { get; set; }
        IEnumerable<IAttribute> Attributes { get; set; }
        IEnumerable<IElement> Children { get; set; }
    }
    public interface IAttribute
    {
        string Name { get; set; }
        string Value { get; set; }
    }
    public interface ISelector
    {
        IEnumerable<Tuple<string, ConnectionType>> Parts { get; set; }
    }

    public enum ConnectionType
    {
        None = 0,
        Children = 32,
        DirectChildren = 62,
        After = 126,
        ImmideatlyAfter = 43
    }
}
