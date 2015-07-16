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
                        Attributes = list1
                    },
                    new Element()
                    {
                        Attributes = list1
                    },
                    new Element()
                    {
                        Attributes = new List<NewSuperModifyedSelector.Attribute>()
                        {
                            new NewSuperModifyedSelector.Attribute()
                            {
                                Name = "mounth",
                                Value = "january"
                            },
                            new NewSuperModifyedSelector.Attribute()
                            {
                                Name = "link",
                                Value = "stylesheet"
                            },
                            new NewSuperModifyedSelector.Attribute()
                            {
                                Name = "text",
                                Value = "I Am Vasya"
                            }
                        }
                    },
                    new Element()
                    {
                        Children = new List<Element>(),
                        Attributes = new List<NewSuperModifyedSelector.Attribute>()
                        {
                            new NewSuperModifyedSelector.Attribute()
                            {
                                Name = "Mass",
                                Value = "58kg"
                            }
                        }
                    },
                    new Element()
                    {
                        Attributes = new List<NewSuperModifyedSelector.Attribute>()
                        {
                            new NewSuperModifyedSelector.Attribute()
                            {
                                Name = "Height",
                                Value = "230cm"
                            },
                            new NewSuperModifyedSelector.Attribute()
                            {
                                Name = "Height2",
                                Value = "2302cm"
                            }
                        }
                    }
                }
            };
            #endregion




            var ss = new Selector2(root).QuerySelector(new List<Tuple<string, Action<string>>>()
                {
                    new Tuple<string,Action<string>>("[type=tag][href=$result]",w=>Console.WriteLine(w+" - Selector1")),
                    new Tuple<string,Action<string>>("[Height=230cm]",w=>Console.WriteLine(w+" - Selector2"))
                }).ToList();
        }
    }
}
