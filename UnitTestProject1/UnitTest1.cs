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
        public void IsMatchMustReturnRightResult()
        {
            Element house = new Element()
            {
                Name = "House",
                Id = "22",
                Classes = new List<string> { "Big", "Nice", "Wonderful" },
                Attributes = new List<UniParser.Attribute> {
                    new UniParser.Attribute() {Name = "Yard", Value = "Yes"},
                    new UniParser.Attribute() {Name = "Stairs", Value = "Good"},
                    new UniParser.Attribute() {Name = "Levels", Value = "2"}
                }
            };
            Assert.IsTrue(ElementSelector.IsMatch("House#22.Big.Nice.Wonderful[Yard=Yes]", house));
            Assert.IsTrue(ElementSelector.IsMatch("[Yard=Yes]", house));
            Assert.IsTrue(ElementSelector.IsMatch(".Big.Nice.Wonderful", house));
        }

        [TestMethod]
        public void QuerySelectorAllMustSelectChildElements()
        {
            var getedTokens = ElementSelector.QuerySelectorAll(doc, new Selector("House *")).Select(w => w.ToString()).ToList();
            List<string> tokens = new List<string>()
            {
                "Name = Koridor",
                "Name = Citchen",
                "Name = Bigroom",
                "Name = Table",
                "Name = Chear",
                "Name = Gas",
                "Name = Citchen",
                "Name = TVSet",
                "Name = Lol",
                "Name = Lol",
                "Name = foof",
                "Name = Citchen"
            };
            Assert.IsTrue(tokens.All(w=>getedTokens.Contains(w)) && getedTokens.All(w=>tokens.Contains(w)));
        }

        [TestMethod]
        public void QuerySelectorAllMustSelectDirectChildElements()
        {
            var getedTokens = ElementSelector.QuerySelectorAll(doc, new Selector("House>*")).Select(w => w.ToString()).ToList();
            List<string> tokens = new List<string>()
            {
                "Name = Koridor",
                "Name = Citchen",
                "Name = Bigroom",
            };
            Assert.IsTrue(tokens.All(w => getedTokens.Contains(w)) && getedTokens.All(w => tokens.Contains(w)));
        }

        [TestMethod]
        public void QuerySelectorAllMustSelectAfterElements()
        {
            var getedTokens = ElementSelector.QuerySelectorAll(doc, new Selector("Lol~*")).Select(w => w.ToString()).ToList();
            List<string> tokens = new List<string>()
            {
                "Name = Lol",
                "Name = foof",
                "Name = Citchen"
            };
            Assert.IsTrue(tokens.All(w => getedTokens.Contains(w)) && getedTokens.All(w => tokens.Contains(w)));
        }

        [TestMethod]
        public void QuerySelectorAllMustSelectImmediatlyAfterElements()
        {
            var getedTokens = ElementSelector.QuerySelectorAll(doc, new Selector("Lol+*")).Select(w => w.ToString()).ToList();
            List<string> tokens = new List<string>()
            {
                "Name = Lol",
                "Name = foof"
            };
            Assert.IsTrue(tokens.All(w => getedTokens.Contains(w)) && getedTokens.All(w => tokens.Contains(w)));
        }

        [TestMethod]
        public void QuerySelectorAllMustSelectCombinedElements()
        {
            var getedTokens = ElementSelector.QuerySelectorAll(doc, new Selector("House>Citchen~*")).Select(w => w.ToString()).ToList();
            List<string> tokens = new List<string>()
            {
                "Name = Bigroom",
            };
            Assert.IsTrue(tokens.All(w => getedTokens.Contains(w)) && getedTokens.All(w => tokens.Contains(w)));
        }

        [TestMethod]
        public void QuerySelectorAllMustSelectVeryCombinedWithAttribsElements()
        {
            var getedTokens = ElementSelector.QuerySelectorAll(doc, new Selector("House .nice [ScreenResolution=1920x1080]")).Select(w => w.ToString()).ToList();
            List<string> tokens = new List<string>()
            {
                "Name = TVSet",
            };
            Assert.IsTrue(tokens.All(w => getedTokens.Contains(w)) && getedTokens.All(w => tokens.Contains(w)));
        }
    }
    }

