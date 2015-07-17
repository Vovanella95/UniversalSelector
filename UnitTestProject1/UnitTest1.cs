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
        public void QuerySelectorForTupleMustWork1()
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

        [TestMethod]
        public void QuerySelectorForTupleMustWork2()
        {
            var list = new List<string>();
            var k = new List<Tuple<string, Action<string>>>()
            {
                new Tuple<string,Action<string>>("[type=tag][href=$result]",w=>list.Add(w + " - Selector1")),
                    new Tuple<string,Action<string>>("[type=$result][href=nothing]",w=>list.Add(w + " - Selector2")),
                    new Tuple<string,Action<string>>("[Height=$result]",w=>list.Add(w + " - Selector3")),
                    new Tuple<string,Action<string>>("*",w=>list.Add(w + " - Selector4"))
            };
            var cells = (new Selector(root)).QuerySelector(k).ToList();

            var isTrue = list.ElementAt(0) == "nothing - Selector1"
                      && list.ElementAt(1) == "tag - Selector2"
                      && list.ElementAt(2) == "nothing - Selector1"
                      && list.ElementAt(3) == "tag - Selector2"
                      && list.ElementAt(4) == "230cm - Selector3";
            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void QuerySelectorMustImplementsAllSelector()
        {
           
            var k = new List<Tuple<string, Action<string>>>()
            {
                    new Tuple<string,Action<string>>("*",w=>{ })
            };
            var cells = (new Selector(root)).QuerySelector(k).ToList();

            var isTrue = cells.ElementAt(0).ToString() == "Empty"
                      && cells.ElementAt(1).ToString() == "[href = nothing][type = tag][text = hello]"
                      && cells.ElementAt(2).ToString() == "[href = nothing][type = tag][text = hello]"
                      && cells.ElementAt(3).ToString() == "[mounth = january][link = stylesheet][text = I Am Vasya]"
                      && cells.ElementAt(4).ToString() == "[Mass = 58kg]"
                      && cells.ElementAt(5).ToString() == "[Height = 230cm][Height2 = 2302cm]";
            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void GetAttributeByNameMustWork()
        {
            var attr = root2.Children.First().GetAttributeValue("href");
            Assert.IsTrue(attr == "nothing");
        }


    }
}
