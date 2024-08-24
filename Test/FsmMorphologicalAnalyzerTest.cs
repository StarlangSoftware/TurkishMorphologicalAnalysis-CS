using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Corpus;
using Dictionary.Dictionary;
using MorphologicalAnalysis;
using NUnit.Framework;

namespace Test
{
    public class FsmMorphologicalAnalyzerTest
    {
        private FsmMorphologicalAnalyzer _fsm;

        [SetUp]
        public void Setup()
        {
            _fsm = new FsmMorphologicalAnalyzer();
        }

        [Test]
        public void TestGenerateAllParses()
        {
            string[] testWords =
            {
                "açıkla", "yıldönümü", "resim", "hal", "cenk", "emlak", "git", "kavur", "ye", "yemek", "göç",
                "ak", "sıska", "yıka", "bul",
                "cevapla", "coş", "böl", "del", "giy", "kaydol", "anla", "çök", "çık", "doldur", "azal", "göster",
                "aksa", "kalp"
            };
            List<FsmParse> parsesGenerated;
            StreamReader streamReader;
            for (var i = 0; i < testWords.Length; i++)
            {
                var word = (TxtWord)_fsm.GetDictionary().GetWord(testWords[i]);
                var parsesExpected = new List<string>();
                streamReader = new StreamReader("../../../parses/" + word.GetName() + ".txt");
                var line = streamReader.ReadLine();
                while (line != null)
                {
                    parsesExpected.Add(line.Split(" ")[1]);
                    line = streamReader.ReadLine();
                }

                streamReader.Close();
                parsesGenerated = _fsm.GenerateAllParses(word, word.GetName().Length + 5);
                Assert.AreEqual(parsesExpected.Count, parsesGenerated.Count);
                foreach (var parseGenerated in parsesGenerated)
                {
                    Assert.True(parsesExpected.Contains(parseGenerated.ToString()));
                }
            }
        }

        [Test]
        public void MorphologicalAnalysisNewWords()
        {
            Assert.True(_fsm.RobustMorphologicalAnalysis("googlecılardan").Size() == 6);
            Assert.True(_fsm.RobustMorphologicalAnalysis("zaptıraplaştırılmayana").Size() == 8);
            Assert.True(_fsm.RobustMorphologicalAnalysis("abzürtleşenmiş").Size() == 5);
            Assert.True(_fsm.RobustMorphologicalAnalysis("vışlığından").Size() == 8);
        }
        
        [Test]
        public void MorphologicalAnalysisSpecialProperNoun()
        {
            Assert.True(_fsm.MorphologicalAnalysis("Won'u").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("Slack'in").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("SPK'ya").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("Stephen'ın").Size() != 0);
        }

        [Test]
        public void MorphologicalAnalysisDataTimeNumber()
        {
            Assert.True(_fsm.MorphologicalAnalysis("3/4").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("3\\/4").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("4/2/1973").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("14/2/1993").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("14/12/1933").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("6/12/1903").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("%34.5").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("%3").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("%56").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("2:3").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("12:3").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("4:23").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("11:56").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("1:2:3").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("3:12:3").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("5:4:23").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("7:11:56").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("12:2:3").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("10:12:3").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("11:4:23").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("22:11:56").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("45").Size() != 0);
            Assert.True(_fsm.MorphologicalAnalysis("34.23").Size() != 0);
        }

        [Test]
        public void MorphologicalAnalysisProperNoun()
        {
            var dictionary = _fsm.GetDictionary();
            for (var i = 0; i < dictionary.Size(); i++)
            {
                var word = (TxtWord)dictionary.GetWord(i);
                if (word.IsProperNoun())
                {
                    Assert.True(_fsm.MorphologicalAnalysis(word.GetName().ToUpper(new CultureInfo("tr"))).Size() != 0);
                }
            }
        }

        [Test]
        public void MorphologicalAnalysisNounSoftenDuringSuffixation()
        {
            var dictionary = _fsm.GetDictionary();
            for (var i = 0; i < dictionary.Size(); i++)
            {
                var word = (TxtWord)dictionary.GetWord(i);
                if (word.IsNominal() && word.NounSoftenDuringSuffixation())
                {
                    var transitionState = new State("Possessive", false, false);
                    var startState = new State("NominalRoot", true, false);
                    var transition = new Transition(transitionState, "yH", "ACC");
                    string surfaceForm = transition.MakeTransition(word, word.GetName(), startState);
                    Assert.True(_fsm.MorphologicalAnalysis(surfaceForm).Size() != 0);
                }
            }
        }

        [Test]
        public void MorphologicalAnalysisVowelAChangesToIDuringYSuffixation()
        {
            var dictionary = _fsm.GetDictionary();
            for (var i = 0; i < dictionary.Size(); i++)
            {
                var word = (TxtWord)dictionary.GetWord(i);
                if (word.IsVerb() && word.VowelAChangesToIDuringYSuffixation())
                {
                    var transitionState = new State("VerbalStem", false, false);
                    var startState = new State("VerbalRoot", true, false);
                    var transition = new Transition(transitionState, "Hyor", "PROG1");
                    string surfaceForm = transition.MakeTransition(word, word.GetName(), startState);
                    Assert.True(_fsm.MorphologicalAnalysis(surfaceForm).Size() != 0);
                }
            }
        }

        [Test]
        public void MorphologicalAnalysisIsPortmanteau()
        {
            TxtDictionary dictionary = _fsm.GetDictionary();
            for (var i = 0; i < dictionary.Size(); i++)
            {
                var word = (TxtWord)dictionary.GetWord(i);
                if (word.IsNominal() && word.IsPortmanteau() && !word.IsPlural() &&
                    !word.IsPortmanteauFacedVowelEllipsis())
                {
                    var transitionState = new State("CompoundNounRoot", true, false);
                    var startState = new State("CompoundNounRoot", true, false);
                    var transition = new Transition(transitionState, "lArH", "A3PL+P3PL");
                    string rootForm, surfaceForm, exceptLast2, exceptLast;
                    exceptLast2 = word.GetName().Substring(0, word.GetName().Length - 2);
                    exceptLast = word.GetName().Substring(0, word.GetName().Length - 1);
                    if (word.IsPortmanteauFacedSoftening())
                    {
                        switch (word.GetName()[word.GetName().Length - 2])
                        {
                            case 'b':
                                rootForm = exceptLast2 + 'p';
                                break;
                            case 'c':
                                rootForm = exceptLast2 + 'ç';
                                break;
                            case 'd':
                                rootForm = exceptLast2 + 't';
                                break;
                            case 'ğ':
                                rootForm = exceptLast2 + 'k';
                                break;
                            default:
                                rootForm = exceptLast;
                                break;
                        }
                    }
                    else
                    {
                        if (word.IsPortmanteauEndingWithSI())
                        {
                            rootForm = exceptLast2;
                        }
                        else
                        {
                            rootForm = exceptLast;
                        }
                    }

                    surfaceForm = transition.MakeTransition(word, rootForm, startState);
                    Assert.True(_fsm.MorphologicalAnalysis(surfaceForm).Size() != 0);
                }
            }
        }

        [Test]
        public void MorphologicalAnalysisNotObeysVowelHarmonyDuringAgglutination()
        {
            TxtDictionary dictionary = _fsm.GetDictionary();
            for (var i = 0; i < dictionary.Size(); i++)
            {
                var word = (TxtWord)dictionary.GetWord(i);
                if (word.IsNominal() && word.NotObeysVowelHarmonyDuringAgglutination())
                {
                    var transitionState = new State("Possessive", false, false);
                    var startState = new State("NominalRoot", true, false);
                    var transition = new Transition(transitionState, "yH", "ACC");
                    string surfaceForm = transition.MakeTransition(word, word.GetName(), startState);
                    Assert.True(_fsm.MorphologicalAnalysis(surfaceForm).Size() != 0);
                }
            }
        }

        [Test]
        public void MorphologicalAnalysisLastIdropsDuringSuffixation()
        {
            TxtDictionary dictionary = _fsm.GetDictionary();
            for (var i = 0; i < dictionary.Size(); i++)
            {
                var word = (TxtWord)dictionary.GetWord(i);
                if (word.IsNominal() && word.LastIdropsDuringSuffixation())
                {
                    var transitionState = new State("Possessive", false, false);
                    var startState = new State("NominalRoot", true, false);
                    var transition = new Transition(transitionState, "yH", "ACC");
                    string surfaceForm = transition.MakeTransition(word, word.GetName(), startState);
                    Assert.True(_fsm.MorphologicalAnalysis(surfaceForm).Size() != 0);
                }
            }
        }

        [Test]
        public void MorphologicalAnalysisVerbSoftenDuringSuffixation()
        {
            TxtDictionary dictionary = _fsm.GetDictionary();
            for (var i = 0; i < dictionary.Size(); i++)
            {
                var word = (TxtWord)dictionary.GetWord(i);
                if (word.IsVerb() && word.VerbSoftenDuringSuffixation())
                {
                    var transitionState = new State("VerbalStem", false, false);
                    var startState = new State("VerbalRoot", true, false);
                    var transition = new Transition(transitionState, "Hyor", "PROG1");
                    string surfaceForm = transition.MakeTransition(word, word.GetName(), startState);
                    Assert.True(_fsm.MorphologicalAnalysis(surfaceForm).Size() != 0);
                }
            }
        }

        [Test]
        public void MorphologicalAnalysisDuplicatesDuringSuffixation()
        {
            TxtDictionary dictionary = _fsm.GetDictionary();
            for (var i = 0; i < dictionary.Size(); i++)
            {
                var word = (TxtWord)dictionary.GetWord(i);
                if (word.IsNominal() && word.DuplicatesDuringSuffixation())
                {
                    var transitionState = new State("Possessive", false, false);
                    var startState = new State("NominalRoot", true, false);
                    var transition = new Transition(transitionState, "yH", "ACC");
                    string surfaceForm = transition.MakeTransition(word, word.GetName(), startState);
                    Assert.True(_fsm.MorphologicalAnalysis(surfaceForm).Size() != 0);
                }
            }
        }

        [Test]
        public void MorphologicalAnalysisEndingKChangesIntoG()
        {
            TxtDictionary dictionary = _fsm.GetDictionary();
            for (var i = 0; i < dictionary.Size(); i++)
            {
                var word = (TxtWord)dictionary.GetWord(i);
                if (word.IsNominal() && word.EndingKChangesIntoG())
                {
                    var transitionState = new State("Possessive", false, false);
                    var startState = new State("NominalRoot", true, false);
                    var transition = new Transition(transitionState, "yH", "ACC");
                    string surfaceForm = transition.MakeTransition(word, word.GetName(), startState);
                    Assert.True(_fsm.MorphologicalAnalysis(surfaceForm).Size() != 0);
                }
            }
        }

        [Test]
        public void MorphologicalAnalysisLastIdropsDuringPassiveSuffixation()
        {
            TxtDictionary dictionary = _fsm.GetDictionary();
            for (var i = 0; i < dictionary.Size(); i++)
            {
                var word = (TxtWord)dictionary.GetWord(i);
                if (word.IsVerb() && word.LastIdropsDuringPassiveSuffixation())
                {
                    var transitionState = new State("VerbalStem", false, false);
                    var startState = new State("VerbalRoot", true, false);
                    var transition = new Transition(transitionState, "Hl", "^DB+VERB+PASS");
                    string surfaceForm = transition.MakeTransition(word, word.GetName(), startState);
                    Assert.True(_fsm.MorphologicalAnalysis(surfaceForm).Size() != 0);
                }
            }
        }

        [Test]
        public void TestReplaceWord()
        {
            Assert.AreEqual("Şvesterine söyle kazağı güzelmiş",
                _fsm.ReplaceWord(new Sentence("Hemşirene söyle kazağı güzelmiş"), "hemşire", "şvester").ToString());
            Assert.AreEqual("Burada çok abartma var",
                _fsm.ReplaceWord(new Sentence("Burada çok mübalağa var"), "mübalağa", "abartma").ToString());
            Assert.AreEqual("Bu bina çok kötü şekilsizleştirildi",
                _fsm.ReplaceWord(new Sentence("Bu bina çok kötü biçimsizleştirildi"), "biçimsizleş", "şekilsizleş")
                    .ToString());
            Assert.AreEqual("Abim geçen yıl ölmüştü gibi",
                _fsm.ReplaceWord(new Sentence("Abim geçen yıl son yolculuğa çıkmıştı gibi"), "son yolculuğa çık", "öl")
                    .ToString());
            Assert.AreEqual("Hemşirenle evlendim",
                _fsm.ReplaceWord(new Sentence("Kız kardeşinle evlendim"), "kız kardeş", "hemşire").ToString());
            Assert.AreEqual("Dün yaptığı güreş maçında yenildi",
                _fsm.ReplaceWord(new Sentence("Dün yaptığı güreş maçında mağlup oldu"), "mağlup ol", "yenil")
                    .ToString());
            Assert.AreEqual("Abim geçen yıl son yolculuğa çıkmıştı gibi",
                _fsm.ReplaceWord(new Sentence("Abim geçen yıl ölmüştü gibi"), "öl", "son yolculuğa çık").ToString());
            Assert.AreEqual("Kız kardeşinle evlendim",
                _fsm.ReplaceWord(new Sentence("Hemşirenle evlendim"), "hemşire", "kız kardeş").ToString());
            Assert.AreEqual("Dün yaptığı güreş maçında mağlup oldu",
                _fsm.ReplaceWord(new Sentence("Dün yaptığı güreş maçında yenildi"), "yenil", "mağlup ol").ToString());
            Assert.AreEqual("Dün yaptığı güreş maçında alt oldu sanki",
                _fsm.ReplaceWord(new Sentence("Dün yaptığı güreş maçında mağlup oldu sanki"), "mağlup ol", "alt ol")
                    .ToString());
            Assert.AreEqual("Yemin billah vermişlerdi vazoyu kırmadığına",
                _fsm.ReplaceWord(new Sentence("Yemin etmişlerdi vazoyu kırmadığına"), "yemin et", "yemin billah ver")
                    .ToString());
            Assert.AreEqual("Yemin etmişlerdi vazoyu kırmadığına",
                _fsm.ReplaceWord(new Sentence("Yemin billah vermişlerdi vazoyu kırmadığına"), "yemin billah ver",
                    "yemin et").ToString());
        }
    }
}