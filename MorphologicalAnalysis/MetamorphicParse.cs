using System;
using System.Collections.Generic;
using Dictionary.Dictionary;

namespace MorphologicalAnalysis
{
    public class MetamorphicParse
    {
        private readonly List<string> _metaMorphemeList;
        private readonly Word _root;

        /**
         * metaMorphemes that can be used.
         */
        private static readonly string[] MetaMorphemes =
        {
            "Ar", "Ar", "CA", "CA",
            "CA", "cAsHnA", "CH", "CHk",
            "DA", "DAn", "DH", "DHk",
            "DHkCA", "DHr", "DHr", "DHr",
            "H", "Hl", "Hm", "Hn",
            "Hn", "Hn", "HmHz", "HncH",
            "HnHz", "Hr", "Hr", "Hs",
            "Ht", "Hyor", "Hz", "k",
            "ki", "kü", "lAn", "lAr",
            "lArDHr", "lArH", "lArH'", "lAs",
            "lH", "lHk", "lHm", "m",
            "mA", "mA", "mAcA", "mAdAn",
            "mAk", "mAksHzHn", "mAktA", "mAlH",
            "mAzlHk", "mHs", "n", "n",
            "nA", "ncA", "nDA", "nDAn",
            "nH", "nHn", "nHz", "nlAr",
            "SA", "SAl", "sH", "SH",
            "SH", "SHn", "SHnHz", "SHnlAr",
            "SHz", "ŞAr", "t", "yA",
            "yA", "yAbil", "yAcAk", "yAcAk",
            "yAdur", "yAgel", "yAlH", "yAmA",
            "yAmAdAn", "yAn", "yArAk", "yAsH",
            "yDH", "yH", "yHcH", "yHm",
            "yHn", "yHncA", "yHp", "yHs",
            "yHver", "yHz", "yken", "ylA",
            "ymHs", "ysA", "z", "zsHn",
            "zsHnHz", "zlAr", "yAkal", "yAkoy",
            "yAgor"
        };

        /**
         * morphotacticTags that can be used.
         */
        public static MorphologicalTag[] morphotacticTags =
        {
            /**
             * Aorist Tense : Her hafta sonunda futbol oynarlar.
             */
            MorphologicalTag.AORIST,

            /**
             * Causative Form : Pişirmek
             */
            MorphologicalTag.CAUSATIVE,

            /**
             * As if : Yaşarmışcasına
             */
            MorphologicalTag.ASIF,

            /**
             * -LY : Akıllıca
             */
            MorphologicalTag.LY,

            /**
             * Equative : Öylece
             */
            MorphologicalTag.EQUATIVE,

            /**
             * As if
             */
            MorphologicalTag.ASIF,

            /**
             * Agent : Toplayıcı
             */
            MorphologicalTag.AGENT,

            /**
             * Dimension : Küçücük
             */
            MorphologicalTag.DIMENSION,

            /**
             * Locative : Aşağıda
             */
            MorphologicalTag.LOCATIVE,

            /**
             * Ablative : Okuldan
             */
            MorphologicalTag.ABLATIVE,

            /**
             * Past tense : Yaşandı
             */
            MorphologicalTag.PASTTENSE,

            /**
             * Past participle : Kırılmış
             */
            MorphologicalTag.PASTPARTICIPLE,

            /**
             * As long as : Yaşadıkça
             */
            MorphologicalTag.ASLONGAS,

            /**
             * Copula : Mavidir
             */
            MorphologicalTag.COPULA,

            /**
             * Since : Yıllardır
             */
            MorphologicalTag.SINCE,

            /**
             * Causitive
             */
            MorphologicalTag.CAUSATIVE,

            /**
             * 3rd person singular possessive : Onun
             */
            MorphologicalTag.P3SG,

            /**
             * Passive : Açıldı
             */
            MorphologicalTag.PASSIVE,

            /**
             * 1st person singular possessive : Benim
             */
            MorphologicalTag.P1SG,

            /**
             * Reflexive : Kendi
             */
            MorphologicalTag.REFLEXIVE,

            /**
             * Passive
             */
            MorphologicalTag.PASSIVE,

            /**
             * 2nd person singular possessive :Senin
             */
            MorphologicalTag.P2SG,

            /**
             * 1st person plural possessive :  Bizim
             */
            MorphologicalTag.P1PL,

            /**
             * Ordinal Number : Altıncı
             */
            MorphologicalTag.ORDINAL,

            /**
             * 2nd person plural possessive : Sizin
             */
            MorphologicalTag.P2PL,

            /**
             * Aorist
             */
            MorphologicalTag.AORIST,

            /**
             * Causitive
             */
            MorphologicalTag.CAUSATIVE,

            /**
             * Reciprocal verb : Görüşmek
             */
            MorphologicalTag.RECIPROCAL,

            /**
             * Causitive
             */
            MorphologicalTag.CAUSATIVE,

            /**
             * Progressive1 : Görüyor
             */
            MorphologicalTag.PROGRESSIVE1,

            /**
             * 1st person plural agreement : Biz gideriz
             */
            MorphologicalTag.A1PL,

            /**
             * 1st person plural agreement
             */
            MorphologicalTag.A1PL,

            /**
             * Relative : Gelenin
             */
            MorphologicalTag.RELATIVE,

            /**
             * Relative
             */
            MorphologicalTag.RELATIVE,

            /**
             * Acquire : Kazanılan
             */
            MorphologicalTag.ACQUIRE,

            /**
             * 3rd person plural agreement : Onlar giderler
             */
            MorphologicalTag.A3PL,

            /**
             * Since
             */
            MorphologicalTag.SINCE,

            /**
             * 3rd person plural possessive : Onların
             */
            MorphologicalTag.P3PL,

            /**
             * 3rd person plural possessive
             */
            MorphologicalTag.P3PL,

            /**
             * Become : Abideleş
             */
            MorphologicalTag.BECOME,

            /**
             * With : Kalemle
             */
            MorphologicalTag.WITH,

            /**
             * Ness : Ağırbaşlılık
             */
            MorphologicalTag.NESS,

            /**
             * 1st person plural agreement
             */
            MorphologicalTag.A1PL,

            /**
             * 1st person singular agreement : Ben giderim
             */
            MorphologicalTag.A1SG,

            /**
             * Infinitive2 : Yapma
             */
            MorphologicalTag.INFINITIVE2,

            /**
             * Negative : Yapama
             */
            MorphologicalTag.NEGATIVE,

            /**
             * Act of : Aldatmaca
             */
            MorphologicalTag.ACTOF,

            /**
             * Without having done so : Çaktırmadan
             */
            MorphologicalTag.WITHOUTHAVINGDONESO,

            /**
             * Infinitive : Yapmak
             */
            MorphologicalTag.INFINITIVE,

            /**
             * Without having done so
             */
            MorphologicalTag.WITHOUTHAVINGDONESO,

            /**
             * Progressive2 : Görmekte
             */
            MorphologicalTag.PROGRESSIVE2,

            /**
             * Necessity : Yapmalı
             */
            MorphologicalTag.NECESSITY,

            /**
             * Notable state : Anlaşılmazlık
             */
            MorphologicalTag.NOTABLESTATE,

            /**
             * Narrative Narrative Past Tense : Oluşmuş
             */
            MorphologicalTag.NARRATIVE,

            /**
             * 2nd person singuular agreement : Sen gelirsin
             */
            MorphologicalTag.A2SG,

            /**
             * Passive
             */
            MorphologicalTag.PASSIVE,

            /**
             * Dative case : Bana
             */
            MorphologicalTag.DATIVE,

            /**
             * Equative
             */
            MorphologicalTag.EQUATIVE,

            /**
             * Locative
             */
            MorphologicalTag.LOCATIVE,

            /**
             * Ablative
             */
            MorphologicalTag.ABLATIVE,

            /**
             * Accusatıve : Beni
             */
            MorphologicalTag.ACCUSATIVE,

            /**
             * Genitive : Benim
             */
            MorphologicalTag.GENITIVE,

            /**
             * 2nd person plural agreement : Siz gelirsiniz
             */
            MorphologicalTag.A2PL,

            /**
             * 3rd person plural agreement
             */
            MorphologicalTag.A3PL,

            /**
             * Desire/Past Auxiliary : Çıkarsa
             */
            MorphologicalTag.DESIRE,

            /**
             * Related to : Davranışsal
             */
            MorphologicalTag.RELATED,

            /**
             * 3rd person singular possessive
             */
            MorphologicalTag.P3SG,

            /**
             * Just like : Destansı
             */
            MorphologicalTag.JUSTLIKE,

            /**
             * Almost : Dikensi
             */
            MorphologicalTag.ALMOST,

            /**
             * 2nd person singuular agreement
             */
            MorphologicalTag.A2SG,

            /**
             * 2nd person plural agreement
             */
            MorphologicalTag.A2PL,

            /**
             * 3rd person plural agreement
             */
            MorphologicalTag.A3PL,

            /**
             * Without : Dikişsiz
             */
            MorphologicalTag.WITHOUT,

            /**
             * Distributive : altışar
             */
            MorphologicalTag.DISTRIBUTIVE,

            /**
             * Causitive form
             */
            MorphologicalTag.CAUSATIVE,

            /**
             * Dative case
             */
            MorphologicalTag.DATIVE,

            /**
             * Optative : Doğanaya
             */
            MorphologicalTag.OPTATIVE,

            /**
             * Ability, possibility : Olabilir
             */
            MorphologicalTag.ABLE,

            /**
             * Future participle : Gülecek
             */
            MorphologicalTag.FUTUREPARTICIPLE,

            /**
             * Future : Yağacak
             */
            MorphologicalTag.FUTURE,

            /**
             * Continuous : Yapadur
             */
            MorphologicalTag.REPEAT,

            /**
             * Ever since : Çıkagel
             */
            MorphologicalTag.EVERSINCE,

            /**
             * Since doing so : Amasyalı
             */
            MorphologicalTag.SINCEDOINGSO,

            /**
             * Not able state : Anlaşılmazlık
             */
            MorphologicalTag.NOTABLESTATE,

            /**
             * Wıthout beıng able to have done so: kararlamadan
             */
            MorphologicalTag.WITHOUTBEINGABLETOHAVEDONESO,

            /**
             * Present partıcıple : Sarılan
             */
            MorphologicalTag.PRESENTPARTICIPLE,

            /**
             * By doing so : Zıplayarak
             */
            MorphologicalTag.BYDOINGSO,

            /**
             * Projection : Öpülesi
             */
            MorphologicalTag.FEELLIKE,

            /**
             * Past tense : Gitti
             */
            MorphologicalTag.PASTTENSE,

            /**
             * Accusative
             */
            MorphologicalTag.ACCUSATIVE,

            /**
             * Agent
             */
            MorphologicalTag.AGENT,

            /**
             * 1st person singuular agreement
             */
            MorphologicalTag.A1SG,

            /**
             * 2nd person plural agreement
             */
            MorphologicalTag.A2PL,

            /**
             * When : Okuyunca
             */
            MorphologicalTag.WHEN,

            /**
             * After doing so : Yapıp
             */
            MorphologicalTag.AFTERDOINGSO,

            /**
             * Infinitive : Yapış
             */
            MorphologicalTag.INFINITIVE3,

            /**
             * Hastility : Yapıver
             */
            MorphologicalTag.HASTILY,

            /**
             * 1st person plural agreement
             */
            MorphologicalTag.A1PL,

            /**
             * While : Gelirken
             */
            MorphologicalTag.WHILE,

            /**
             * Instrumental : Kışın, arabayla
             */
            MorphologicalTag.INSTRUMENTAL,

            /**
             * Narrative
             */
            MorphologicalTag.NARRATIVE,

            /**
             * Conditional : Gelirse
             */
            MorphologicalTag.CONDITIONAL,

            /**
             * 3rd person singuular agreement : O gelir
             */
            MorphologicalTag.A3SG,

            /**
             * 2nd person singuular agreement
             */
            MorphologicalTag.A2SG,

            /**
             * 2nd person plural agreement
             */
            MorphologicalTag.A2PL,

            /**
             * 3rd person plural agreement
             */
            MorphologicalTag.A3PL,

            /**
             * Stay : Bakakal
             */
            MorphologicalTag.STAY,

            /**
             * Start : Alıkoy
             */
            MorphologicalTag.START,

            /**
             * Repeat : Yapagör
             */
            MorphologicalTag.REPEAT
        };

        /**
         * <summary>The getMetaMorphemeTag method takes a String tag as an input and takes the first char of the tag. If first char
         * is a punctuation it gets a substring from the tag. And gets the meta morphemes of this tag then adds to the
         * result {@link ArrayList}.</summary>
         *
         * <param name="tag">String to get meta morphemes from.</param>
         * <returns>ArrayList type result which holds meta morphemes.</returns>
         */
        public static List<MorphologicalTag> GetMetaMorphemeTag(String tag)
        {
            var result = new List<MorphologicalTag>();

            var s = "" + tag[0];
            if (Word.IsPunctuation(s))
            {
                tag = tag.Substring(1);
            }

            for (var j = 0; j < MetaMorphemes.Length; j++)
            {
                if (tag == MetaMorphemes[j])
                {
                    result.Add(morphotacticTags[j]);
                }
            }

            return result;
        }

        /**
         * <summary>Another getMetaMorphemeTag method which also takes parse as an input. It also checks the morphotactic tags.</summary>
         *
         * <param name="parse">MorphologicalParse type input.</param>
         * <param name="tag">  String to get meta morphemes from.</param>
         * <returns>ArrayList type result which holds meta morphemes.</returns>
         */
        public static List<MorphologicalTag> GetMetaMorphemeTag(MorphologicalParse parse, string tag)
        {
            var result = new List<MorphologicalTag>();

            var s = "" + tag[0];
            if (Word.IsPunctuation(s))
            {
                tag = tag.Substring(1);
            }

            for (var j = 0; j < MetaMorphemes.Length; j++)
            {
                if (tag == MetaMorphemes[j] && parse.ContainsTag(morphotacticTags[j]))
                {
                    result.Add(morphotacticTags[j]);
                }
            }

            return result;
        }

        /**
         * <summary>The getter method for Private Word root.</summary>
         *
         * <returns>Word type root.</returns>
         */
        public Word GetWord()
        {
            return _root;
        }

        /**
         * <summary>A constructor of {@link MetamorphicParse} class which creates an {@link ArrayList} metaMorphemeList which has split words
         * according to +.</summary>
         *
         * <param name="parse">String to parse.</param>
         */
        public MetamorphicParse(string parse)
        {
            int i;
            _metaMorphemeList = new List<string>();
            if (parse == "+")
            {
                _root = new Word("+");
            }
            else
            {
                var words = parse.Split("\\+");
                _root = new Word(words[0]);
                for (i = 1; i < words.Length; i++)
                    _metaMorphemeList.Add(words[i]);
            }
        }

        /**
         * <summary>The size method returns the size of the metaMorphemeList.</summary>
         *
         * <returns>the size of the metaMorphemeList.</returns>
         */
        public int Size()
        {
            return _metaMorphemeList.Count + 1;
        }

        /**
         * <summary>The addMetaMorphemeList method splits input String by + and add to the metaMorphemeList.</summary>
         *
         * <param name="newTacticSet">String to add the metaMorphemeList.</param>
         */
        public void AddMetaMorphemeList(string newTacticSet)
        {
            var tactics = newTacticSet.Split("\\+");
            foreach (var tactic in tactics)
            {
                _metaMorphemeList.Add(tactic);
            }
        }

        /**
         * <summary>The removeMetaMorphemeFromIndex method removes the meta morpheme at given index from metaMorphemeList.</summary>
         *
         * <param name="index">to remove from metaMorphemeList.</param>
         */
        public void RemoveMetaMorphemeFromIndex(int index)
        {
            var i = index - 1;
            while (i < _metaMorphemeList.Count)
            {
                _metaMorphemeList.RemoveAt(i);
            }
        }

        /**
         * <summary>The getMetaMorpheme method gets the meta morpheme at given index.</summary>
         *
         * <param name="index">is used to get the meta morpheme.</param>
         * <returns>metaMorphemeList's corresponding meta morpheme.</returns>
         */
        public string GetMetaMorpheme(int index)
        {
            if (index == 0)
            {
                return _root.GetName();
            }

            return _metaMorphemeList[index - 1];
        }

        /**
         * <summary>Overridden toString method to return resulting meta morphemes in metaMorphemeList.</summary>
         *
         * <returns>String result.</returns>
         */
        public override string ToString()
        {
            var result = _root.GetName();
            foreach (var metaMorpheme in _metaMorphemeList)
            {
                result = result + "+" + metaMorpheme;
            }

            return result;
        }
    }
}