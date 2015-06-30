using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace UniParser
{
    public class Document : IDocument
    {
        public IEnumerable<IElement> Children { get; set; }
    }

    public class Selector : ISelector
    {
        public IEnumerable<Tuple<string, ConnectionType>> Parts { get; set; }
        public Selector(string selector)
        {
            Parts = GetParts(selector);
        }
        public static IEnumerable<Tuple<string, ConnectionType>> GetParts(string selector)
        {
            var delimeters = new char[] { '>', ' ', '~', '+' };
            int i1 = 0;
            for (int i2 = 1; i2 < selector.Length; i2++)
            {
                if (delimeters.Contains(selector[i2]) || i2 == selector.Length - 1)
                {
                    int help = i2 == selector.Length - 1 ? 1 : 0;
                    yield return new Tuple<string, ConnectionType>(selector.Substring(i1, i2 - i1 + help),(ConnectionType)(help==0?selector[i2]:0));
                    i1 = i2 + 1;
                }
            }
        }
    }

    public class Element : IElement
    {
        public IEnumerable<IElement> Children { get; set; }
        public IEnumerable<IAttribute> Attributes { get; set; }
        public IEnumerable<string> Classes { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public override string ToString()
        {
            return "Name = " + Name;
        }
    }

    public class Attribute : IAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool CompareTo(IAttribute attr2)
        {
            return (Name == attr2.Name && Value == attr2.Value);
        }
    }
}

