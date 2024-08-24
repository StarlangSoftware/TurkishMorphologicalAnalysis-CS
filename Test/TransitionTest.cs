using MorphologicalAnalysis;
using NUnit.Framework;

namespace Test
{
    public class TransitionTest
    {
        FsmMorphologicalAnalyzer fsm;

        [SetUp]
        public void Setup()
        {
            fsm = new FsmMorphologicalAnalyzer();
        }

        [Test]
        public void TestNumberWithAccusative()
        {
            Assert.True(fsm.MorphologicalAnalysis("2'yi").Size() != 0);
            Assert.AreEqual(0, fsm.MorphologicalAnalysis("2'i").Size());
            Assert.True(fsm.MorphologicalAnalysis("5'i").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("9'u").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("10'u").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("30'u").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("3'ü").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("4'ü").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("100'ü").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("6'yı").Size() != 0);
            Assert.AreEqual(0, fsm.MorphologicalAnalysis("6'ı").Size());
            Assert.True(fsm.MorphologicalAnalysis("40'ı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("60'ı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("90'ı").Size() != 0);
        }

        [Test]
        public void TestNumberWithDative()
        {
            Assert.True(fsm.MorphologicalAnalysis("6'ya").Size() != 0);
            Assert.AreEqual(0, fsm.MorphologicalAnalysis("6'a").Size());
            Assert.True(fsm.MorphologicalAnalysis("9'a").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("10'a").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("30'a").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("40'a").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("60'a").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("90'a").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("2'ye").Size() != 0);
            Assert.AreEqual(0, fsm.MorphologicalAnalysis("2'e").Size());
            Assert.True(fsm.MorphologicalAnalysis("8'e").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("5'e").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("4'e").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("1'e").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("3'e").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("7'ye").Size() != 0);
            Assert.AreEqual(0, fsm.MorphologicalAnalysis("7'e").Size());
        }

        [Test]
        public void TestPresentTense()
        {
            Assert.True(fsm.MorphologicalAnalysis("büyülüyor").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("bölümlüyor").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("buğuluyor").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("bulguluyor").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("açıklıyor").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("çalkalıyor").Size() != 0);
        }

        [Test]
        public void TestA()
        {
            Assert.True(fsm.MorphologicalAnalysis("saatinizi").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("alkole").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("anormale").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("sakala").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("kabala").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("faika").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("halika").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("kediye").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("eve").Size() != 0);
        }

        [Test]
        public void TestC()
        {
            Assert.True(fsm.MorphologicalAnalysis("gripçi").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("güllaççı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("gülütçü").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("gülükçü").Size() != 0);
        }

        [Test]
        public void TestSH()
        {
            Assert.True(fsm.MorphologicalAnalysis("altışar").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("yedişer").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("üçer").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("beşer").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("dörder").Size() != 0);
        }

        [Test]
        public void TestNumberWithD()
        {
            Assert.True(fsm.MorphologicalAnalysis("1'di").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("2'ydi").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("3'tü").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("4'tü").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("5'ti").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("6'ydı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("7'ydi").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("8'di").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("9'du").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("30'du").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("40'tı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("60'tı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("70'ti").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("50'ydi").Size() != 0);
        }

        [Test]
        public void TestD()
        {
            Assert.True(fsm.MorphologicalAnalysis("koştu").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("kitaptı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("kaçtı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("evdi").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("fraktı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("sattı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("aftı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("kesti").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("ahtı").Size() != 0);
        }

        [Test]
        public void TestExceptions()
        {
            Assert.True(fsm.MorphologicalAnalysis("yiyip").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("sana").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("bununla").Size() != 0);
            Assert.AreEqual(0, fsm.MorphologicalAnalysis("buyla").Size());
            Assert.True(fsm.MorphologicalAnalysis("onunla").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("şununla").Size() != 0);
            Assert.AreEqual(0, fsm.MorphologicalAnalysis("şuyla").Size());
            Assert.True(fsm.MorphologicalAnalysis("bana").Size() != 0);
        }

        [Test]
        public void TestVowelEChangesToIDuringYSuffixation()
        {
            Assert.True(fsm.MorphologicalAnalysis("diyor").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("yiyor").Size() != 0);
        }

        [Test]
        public void TestLastIdropsDuringPassiveSuffixation()
        {
            Assert.True(fsm.MorphologicalAnalysis("yoğruldu").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("buyruldu").Size() != 0);
        }

        [Test]
        public void TestShowsSuRegularities()
        {
            Assert.True(fsm.MorphologicalAnalysis("karasuyu").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("suyu").Size() != 0);
        }

        [Test]
        public void TestDuplicatesDuringSuffixation()
        {
            Assert.True(fsm.MorphologicalAnalysis("tıbbı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("ceddi").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("zıddı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("serhaddi").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("fenni").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("haddi").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("hazzı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("şakkı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("şakı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("halli").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("hali").Size() != 0);
        }

        [Test]
        public void TestLastIdropsDuringSuffixation()
        {
            Assert.True(fsm.MorphologicalAnalysis("hizbi").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("kaybı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("ahdi").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("nesci").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("zehri").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("zikri").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("metni").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("metini").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("katli").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("katili").Size() != 0);
        }

        [Test]
        public void TestNounSoftenDuringSuffixation()
        {
            Assert.True(fsm.MorphologicalAnalysis("adabı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("amibi").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("armudu").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("ağacı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("akacı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("arkeoloğu").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("filoloğu").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("ahengi").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("küngü").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("kitaplığı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("küllüğü").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("adedi").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("adeti").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("ağıdı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("ağıtı").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("anotu").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("anodu").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("Kuzguncuk'u").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("Leylak'ı").Size() != 0);
        }

        [Test]
        public void TestVerbSoftenDuringSuffixation()
        {
            Assert.True(fsm.MorphologicalAnalysis("cezbediyor").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("ediyor").Size() != 0);
            Assert.True(fsm.MorphologicalAnalysis("bahsediyor").Size() != 0);
        }
    }
}