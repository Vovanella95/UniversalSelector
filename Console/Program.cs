using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewSuperModifyedSelector;
using System.Diagnostics;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Attributes
            var list1 = new List<NewSuperModifyedSelector.Attribute>()
            {
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "href",
                    Value = "nothing"
                },
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "type",
                    Value = "tag"
                },
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "text",
                    Value = "hello"
                }
            };
            var list2 = new List<NewSuperModifyedSelector.Attribute>()
            {
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "ne4w",
                    Value = "nerws"
                },
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "santa",
                    Value = "claus"
                },
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "href",
                    Value = "nothing"
                },
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "new",
                    Value = "news"
                },
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "type",
                    Value = "tag"
                },
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "text",
                    Value = "hello"
                }
            };
            #endregion

            #region Elements
            var root = new Element()
            {
                Children = new List<Element>()
                {
                    new Element()
                    {
                        Id = "idshnick",
                        Name = "TheName",
                        Attributes = list1
                    },
                    new Element()
                    {
                        Id = "idshnick",
                        Name = "TheName",
                        Attributes = list2
                    },
                    new Element()
                    {
                        Attributes = list2,
                        Children = new List<Element>()
                    },
                    new Element()
                    {
                        Id = "idshnick",
                        Attributes = list1
                    }
                }

            };
            #endregion

            #region Template
            var template = new TemplateElement()
            {
                Name = "TheName",
                Attributes = list1
            };
            #endregion



            var k = Selector.QuerySelector(template, root);




        }
    }
}
