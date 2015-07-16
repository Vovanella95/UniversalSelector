using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using NewSuperModifyedSelector;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        #region Attributes
        static List<NewSuperModifyedSelector.Attribute> list1 = new List<NewSuperModifyedSelector.Attribute>()
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
        static List<NewSuperModifyedSelector.Attribute> list2 = new List<NewSuperModifyedSelector.Attribute>()
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
        Element root = new Element()
        {
            Children = new List<Element>()
                {
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
                                Value = "I Am Vasya"
                            }
                        }
                    },
                    new Element()
                    {
                        Attributes = new List<NewSuperModifyedSelector.Attribute>()
                        {
                            new NewSuperModifyedSelector.Attribute()
                            {
                                Name = "NewAttr1",
                                Value = "NewValue1"
                            },
                            new NewSuperModifyedSelector.Attribute()
                            {
                                Name = "NewAttr2",
                                Value = "NewValue2"
                            }
                        }
                    },
                    new Element()
                    {
                        Attributes = list2,
                        Children = new List<Element>()
                    },
                    new Element()
                    {
                        Attributes = list1
                    }
                }
        };
        #endregion

        #region Elements2
        Element root2 = new Element()
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

        [TestMethod]
        public void SelectorMustDoSimpleSelects()
        {
            var list = new List<string>()
            {
                "hello", "I Am Vasya", "hello"
            };
            Match("#idshnick[href=nothing][type=tag][text=$result]", list);
        }

        [TestMethod]
        public void SelectorMustDoSimpleSelects2()
        {
            var list = new List<string>()
            {
                "hello", "I Am Vasya", "hello"
            };
            Match("TheName[href=nothing][text=$result]", list);
        }

        [TestMethod]
        public void QuerySelectorForTupleMustWork()
        {

            var list = new List<string>();
            var k = new List<Tuple<string, Action<string>>>()
            {
                new Tuple<string,Action<string>>("[href=nothing][type=$result]",w => list.Add(w + " - Selector1")),
                new Tuple<string,Action<string>>("[Mass=$result]",w => list.Add(w + " - Selector2")),
                new Tuple<string,Action<string>>("[Height=$result]",w => list.Add(w + " - Selector3")),
                new Tuple<string,Action<string>>("[href=$result]",w => list.Add(w + " - Selector4")),
            };
            var cells = (new Selector(root2)).QuerySelector(k).ToList();

            var isTrue = list.ElementAt(0) == "tag - Selector1"
                      && list.ElementAt(1) == "nothing - Selector4"
                      && list.ElementAt(2) == "tag - Selector1"
                      && list.ElementAt(3) == "nothing - Selector4"
                      && list.ElementAt(4) == "tag - Selector1"
                      && list.ElementAt(5) == "nothing - Selector4"
                      && list.ElementAt(6) == "58kg - Selector2"
                      && list.ElementAt(7) == "230cm - Selector3";
            Assert.IsTrue(isTrue);
        }

        public void Match(string selector, IEnumerable<string> results)
        {
            var list = new List<string>();
            var k = (new Selector(root)).QuerySelector(selector, w => list.Add(w)).ToList();
            Assert.IsTrue(results.All(w => list.Contains(w)) && list.All(w => results.Contains(w)));
        }
    }
}
