using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniParser;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        #region Document
        Document doc = new Document()
        {
            Children = new List<Element>()
                {
                    new Element()
                    {
                        Name = "House",
                        Children = new List<Element>()
                        {
                            new Element()
                            {
                                Name = "Koridor"
                            },
                            new Element()
                            {
                                Name = "Citchen",
                                Children = new List<Element>()
                                {
                                    new Element()
                                    {
                                        Name="Table"
                                    },
                                    new Element()
                                    {
                                        Name="Chear"
                                    },
                                    new Element()
                                    {
                                        Name="Gas"
                                    },
                                    new Element()
                                    {
                                        Name = "Citchen",
                                        Children = new List<Element>()
                                        {
                                            new Element()
                                            {
                                                Name = "Lol",
                                                Id = "id1"
                                            },
                                            new Element()
                                            {
                                                Name = "Lol",
                                                Id = "id2"
                                            },
                                            new Element()
                                            {
                                                Name = "foof",
                                                Id = "id"
                                            },
                                            new Element()
                                            {
                                                Name = "Citchen",
                                                Id = "id1"
                                            }
                                        }
                                    }
                                }
                            },
                            new Element()
                            {
                                Name = "Bigroom",
                                Classes = new List<string> {"big","nice"},
                                Children = new List<Element>()
                                {
                                    new Element()
                                    {
                                        Name = "TVSet",
                                        Attributes = new List<UniParser.Attribute>()
                                        {
                                            new UniParser.Attribute()
                                            {
                                                Name = "ChannelCount",
                                                Value = "12"
                                            },
                                            new UniParser.Attribute()
                                            {
                                                Name = "ScreenResolution",
                                                Value = "1920x1080"
                                            },
                                            new UniParser.Attribute()
                                            {
                                                Name = "Size",
                                                Value = "180x100"
                                            }
                                        }
                                    }
                                }
                                
                            }
                        }
                    }
                }
        };
        #endregion

        [TestMethod]
        public void QuerySelectorMustDoSimpleSelects()
        {
            List<string> elements = new List<string>()
            {
                "Name = Chear",
                "Name = Gas",
                "Name = Citchen"
            };
            Match(elements, "Table~*");
        }

        [TestMethod]
        public void QuerySelectorMustDoCombinedSelects()
        {
            List<string> elements = new List<string>()
            {
                "Name = Citchen",
                "Name = Lol",
                "Name = Lol",
                "Name = foof",
                "Name = Citchen"
            };
            Match(elements, "Citchen Gas~*");
        }

        public void Match(IEnumerable<string> elements, string selector)
        {
            var token = (Selector2.Create(selector)).QuerySelector(doc.Children).Select(w => w.ToString());
            bool temp = true;

            foreach (var item in Enumerable.Concat(elements, token))
            {
                if (!elements.Contains(item) || !token.Contains(item))
                {
                    temp = false;
                    break;
                }
            }
            Assert.IsTrue(temp);
        }
    }
}

