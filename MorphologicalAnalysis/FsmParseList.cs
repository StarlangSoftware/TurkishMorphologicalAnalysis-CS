using System;
using System.Collections.Generic;
using Dictionary.Dictionary;

namespace MorphologicalAnalysis
{
    public class FsmParseList
    {
        private readonly List<FsmParse> _fsmParses;

        private static string[] longestRootExceptions =
        {
            "acağı acak NOUN VERB", "acağım acak NOUN VERB", "acağımı acak NOUN VERB", "acağımız acak NOUN VERB",
            "acağın acak NOUN VERB",
            "acağına acak NOUN VERB", "acağını acak NOUN VERB", "acağının acak NOUN VERB", "acağınız acak NOUN VERB",
            "acağınıza acak NOUN VERB",
            "acağınızdır acak NOUN VERB", "acağınızı acak NOUN VERB", "acağınızın acak NOUN VERB",
            "acağız acak NOUN VERB", "acakları acak NOUN VERB",
            "acaklarını acak NOUN VERB", "acaksa acak NOUN VERB", "acaktır acak NOUN VERB", "ardım ar NOUN VERB",
            "arız ar NOUN VERB",
            "arken ar NOUN VERB", "arsa ar NOUN VERB", "arsak ar NOUN VERB", "arsanız ar NOUN VERB",
            "arsınız ar NOUN VERB",
            "eceği ecek NOUN VERB", "eceğim ecek NOUN VERB", "eceğimi ecek NOUN VERB", "eceğimiz ecek NOUN VERB",
            "eceğin ecek NOUN VERB",
            "eceğine ecek NOUN VERB", "eceğini ecek NOUN VERB", "eceğinin ecek NOUN VERB", "eceğiniz ecek NOUN VERB",
            "eceğinizdir ecek NOUN VERB",
            "eceğinize ecek NOUN VERB", "eceğinizi ecek NOUN VERB", "eceğinizin ecek NOUN VERB",
            "eceğiz ecek NOUN VERB", "ecekleri ecek NOUN VERB",
            "eceklerini ecek NOUN VERB", "ecekse ecek NOUN VERB", "ecektir ecek NOUN VERB", "erdim er NOUN VERB",
            "eriz er NOUN VERB",
            "erken er NOUN VERB", "erse er NOUN VERB", "ersek er NOUN VERB", "erseniz er NOUN VERB",
            "ersiniz er NOUN VERB",
            "ilen i VERB VERB", "ilene i VERB VERB", "ilin i VERB VERB", "ilince i VERB VERB", "imiz i ADJ NOUN",
            "in i ADJ NOUN", "inde i ADJ NOUN", "ine i ADJ NOUN", "ini i ADJ NOUN", "inin i ADJ NOUN",
            "ılan ı NOUN VERB", "ılana ı NOUN VERB", "ılın ı NOUN VERB", "ılınca ı NOUN VERB", "la la VERB NOUN",
            "lar la VERB NOUN", "lardan la VERB NOUN", "lardandır la VERB NOUN", "lardır la VERB NOUN",
            "ları la VERB NOUN",
            "larıdır la VERB NOUN", "larım la VERB NOUN", "larımdan la VERB NOUN", "larımız la VERB NOUN",
            "larımıza la VERB NOUN",
            "larımızda la VERB NOUN", "larımızdan la VERB NOUN", "larımızdaydı la VERB NOUN", "larımızı la VERB NOUN",
            "larımızın la VERB NOUN",
            "larımızla la VERB NOUN", "ların la VERB NOUN", "larına la VERB NOUN", "larında la VERB NOUN",
            "larındaki la VERB NOUN",
            "larındakiler la VERB NOUN", "larındakilere la VERB NOUN", "larındakileri la VERB NOUN",
            "larındakilerin la VERB NOUN", "larından la VERB NOUN",
            "larındandır la VERB NOUN", "larındaysa la VERB NOUN", "larını la VERB NOUN", "larının la VERB NOUN",
            "larınız la VERB NOUN",
            "larınıza la VERB NOUN", "larınızda la VERB NOUN", "larınızdaki la VERB NOUN", "larınızdan la VERB NOUN",
            "larınızı la VERB NOUN",
            "larınızın la VERB NOUN", "larınızla la VERB NOUN", "larıyla la VERB NOUN", "le le VERB NOUN",
            "ler le VERB NOUN",
            "lerden le VERB NOUN", "lerdendir le VERB NOUN", "lerdir le VERB NOUN", "leri le VERB NOUN",
            "leridir le VERB NOUN",
            "lerim le VERB NOUN", "lerimden le VERB NOUN", "lerimiz le VERB NOUN", "lerimizde le VERB NOUN",
            "lerimizden le VERB NOUN",
            "lerimizdeydi le VERB NOUN", "lerimize le VERB NOUN", "lerimizi le VERB NOUN", "lerimizin le VERB NOUN",
            "lerimizle le VERB NOUN",
            "lerin le VERB NOUN", "lerinde le VERB NOUN", "lerindeki le VERB NOUN", "lerindekiler le VERB NOUN",
            "lerindekilere le VERB NOUN",
            "lerindekileri le VERB NOUN", "lerindekilerin le VERB NOUN", "lerinden le VERB NOUN",
            "lerindendir le VERB NOUN", "lerindeyse le VERB NOUN",
            "lerine le VERB NOUN", "lerini le VERB NOUN", "lerinin le VERB NOUN", "leriniz le VERB NOUN",
            "lerinizde le VERB NOUN",
            "lerinizdeki le VERB NOUN", "lerinizden le VERB NOUN", "lerinize le VERB NOUN", "lerinizi le VERB NOUN",
            "lerinizin le VERB NOUN",
            "lerinizle le VERB NOUN", "leriyle le VERB NOUN", "madan ma NOUN VERB",
            "malı ma NOUN VERB",
            "malıdır ma NOUN VERB", "malıdırlar ma NOUN VERB", "malılar ma NOUN VERB", "malısınız ma NOUN VERB",
            "malıyım ma NOUN VERB",
            "malıyız ma NOUN VERB", "mam ma NOUN VERB", "mama ma NOUN VERB", "mamız ma NOUN VERB",
            "mamıza ma NOUN VERB",
            "mamızı ma NOUN VERB", "manız ma NOUN VERB", "manızda ma NOUN VERB", "manızdır ma NOUN VERB",
            "manızı ma NOUN VERB",
            "manızla ma NOUN VERB", "ması ma NOUN VERB", "masıdır ma NOUN VERB", "masın ma NOUN VERB",
            "masına ma NOUN VERB",
            "masında ma NOUN VERB", "masındaki ma NOUN VERB", "masını ma NOUN VERB", "masıyla ma NOUN VERB",
            "mdan m NOUN NOUN",
            "meden me NOUN VERB", "meli me NOUN VERB", "melidir me NOUN VERB", "melidirler me NOUN VERB",
            "meliler me NOUN VERB",
            "melisiniz me NOUN VERB", "meliyim me NOUN VERB", "meliyiz me NOUN VERB", "mem me NOUN VERB",
            "meme me NOUN VERB",
            "memiz me NOUN VERB", "memize me NOUN VERB", "memizi me NOUN VERB", "meniz me NOUN VERB",
            "menizde me NOUN VERB",
            "menizdir me NOUN VERB", "menizi me NOUN VERB", "menizle me NOUN VERB", "mesi me NOUN VERB",
            "mesidir me NOUN VERB",
            "mesin me NOUN VERB", "mesinde me NOUN VERB", "mesindeki me NOUN VERB", "mesine me NOUN VERB",
            "mesini me NOUN VERB",
            "mesiyle me NOUN VERB", "mişse miş NOUN VERB", "mını m NOUN NOUN", "mışsa mış NOUN VERB", "mız m NOUN NOUN",
            "na n NOUN NOUN", "ne n NOUN NOUN", "nin n NOUN NOUN", "niz n NOUN NOUN",
            "nın n NOUN NOUN", "nız n NOUN NOUN", "rdim r NOUN VERB", "rdım r NOUN VERB", "riz r NOUN VERB",
            "rız r NOUN VERB", "rken r NOUN VERB", "rken r NOUN VERB", "rsa r NOUN VERB", "rsak r NOUN VERB",
            "rsanız r NOUN VERB", "rse r NOUN VERB", "rsek r NOUN VERB", "rseniz r NOUN VERB", "rsiniz r NOUN VERB",
            "rsınız r NOUN VERB", "sa sa VERB ADJ", "se se VERB ADJ", "ulan u NOUN VERB", "un un VERB NOUN",
            "üne ün VERB NOUN", "unun un VERB NOUN", "ince i NOUN VERB", "unca u NOUN VERB", "ınca ı NOUN VERB", 
            "unca un NOUN VERB", "ilen ile VERB VERB"
        };

        /**
        * <summary>A constructor of {@link FsmParseList} class which takes an {@link List} fsmParses as an input. First it sorts
         * the items of the {@link List} then loops through it, if the current item's transitions equal to the next item's
        * transitions, it removes the latter item. At the end, it assigns this {@link List} to the fsmParses variable.</summary>
        *
        * <param name="fsmParses">{@link FsmParse} type{@link List} input.</param>
        */
        public FsmParseList(List<FsmParse> fsmParses)
        {
            fsmParses.Sort();
            for (int i = 0; i < fsmParses.Count - 1; i++)
            {
                if (fsmParses[i].TransitionList() == fsmParses[i + 1].TransitionList())
                {
                    fsmParses.RemoveAt(i + 1);
                    i--;
                }
            }

            this._fsmParses = fsmParses;
        }

        /**
         * <summary>The size method returns the size of fsmParses {@link List}.</summary>
         *
         * <returns>the size of fsmParses {@link List}.</returns>
         */
        public int Size()
        {
            return _fsmParses.Count;
        }

        /**
         * <summary>The getFsmParse method takes an integer index as an input and returns the item of fsmParses {@link List} at given index.</summary>
         *
         * <param name="index">Integer input.</param>
         * <returns>the item of fsmParses {@link List} at given index.</returns>
         */
        public FsmParse GetFsmParse(int index)
        {
            return _fsmParses[index];
        }

        /**
         * <summary>The rootWords method gets the first item's root of fsmParses {@link List} and uses it as currentRoot. Then loops through
         * the fsmParses, if the current item's root does not equal to the currentRoot, it then assigns it as the currentRoot and
         * accumulates root words in a {@link string} result.</summary>
         *
         * <returns>string result that has root words.</returns>
         */
        public string RootWords()
        {
            string result = _fsmParses[0].GetWord().GetName(), currentRoot = result;
            for (var i = 1; i < _fsmParses.Count; i++)
            {
                if (_fsmParses[i].GetWord().GetName() != currentRoot)
                {
                    currentRoot = _fsmParses[i].GetWord().GetName();
                    result = result + "$" + currentRoot;
                }
            }

            return result;
        }

        /**
         * <summary>The reduceToParsesWithSameRootAndPos method takes a {@link Word} currentWithPos as an input and loops i times till
         * i equals to the size of the fsmParses {@link List}. If the given currentWithPos does not equal to the ith item's
         * root and the MorphologicalTag of the first inflectional of fsmParses, it removes the ith item from the {@link List}.</summary>
         *
         * <param name="currentWithPos">{@link Word} input.</param>
         */
        public void ReduceToParsesWithSameRootAndPos(Word currentWithPos)
        {
            var i = 0;
            while (i < _fsmParses.Count)
            {
                if (!_fsmParses[i].GetWordWithPos().Equals(currentWithPos))
                {
                    _fsmParses.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        /**
         * <summary>The getParseWithLongestRootWord method returns the parse with the longest root word. If more than one parse has the
         * longest root word, the first parse with that root is returned. If the longest root word belongs to an exceptional
        * case, the parse with the next longest root word that does not, is returned.</summary>
         *
         * <returns>FsmParse Parse with the longest root word.</returns>
         */
        public FsmParse GetParseWithLongestRootWord()
        {
            var maxLength = -1;
            FsmParse bestParse = null;
            if (_fsmParses.Count > 0)
            {
                bestParse = _fsmParses[0];
            }
            foreach (var currentParse in _fsmParses)
            {
                if (currentParse.GetWord().GetName().Length > maxLength && !IsLongestRootException(currentParse))
                {
                    maxLength = currentParse.GetWord().GetName().Length;
                    bestParse = currentParse;
                }
            }

            return bestParse;
        }

        /**
        * <summary>The IsLongestRootException method returns true if the longest root word belongs to an exceptional
         * case, false otherwise.</summary>
        *
        * <param name="fsmParse">{@link FsmParse} input.</param>
        * <returns> true if the longest root belongs to an exceptional case, false otherwise. </returns>
        */
        private bool IsLongestRootException(FsmParse fsmParse)
        {
            var surfaceForm = fsmParse.GetSurfaceForm();
            var root = fsmParse.GetWord().GetName();

            foreach (var longestRootException in longestRootExceptions)
            {
                var exceptionItems = longestRootException.Split(" ");
                var surfaceFormEnding = exceptionItems[0];
                var longestRootEnding = exceptionItems[1];
                var longestRootPos = exceptionItems[2];
                var possibleRootPos = exceptionItems[3];
                var possibleRoot = surfaceForm.Replace(surfaceFormEnding, "");

                if (surfaceForm.EndsWith(surfaceFormEnding) && root.EndsWith(longestRootEnding)
                                                            && fsmParse.GetRootPos().Equals(longestRootPos))
                {
                    foreach (var currentParse in _fsmParses)
                    {
                        if (currentParse.GetWord().GetName().Equals(possibleRoot)
                            && currentParse.GetRootPos().Equals(possibleRootPos))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /**
         * <summary>The reduceToParsesWithSameRoot method takes a {@link string} currentWithPos as an input and loops i times till
         * i equals to the size of the fsmParses {@link List}. If the given currentRoot does not equal to the root of ith item of
         * fsmParses, it removes the ith item from the {@link List}.</summary>
         *
         * <param name="currentRoot">{@link string} input.</param>
         */
        public void ReduceToParsesWithSameRoot(string currentRoot)
        {
            var i = 0;
            while (i < _fsmParses.Count)
            {
                if (_fsmParses[i].GetWord().GetName() != currentRoot)
                {
                    _fsmParses.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        /**
         * <summary>The constructParseListForDifferentRootWithPos method initially creates a result {@link List} then loops through the
         * fsmParses {@link List}. For the first iteration, it creates new {@link List} as initial, then adds the
         * first item od fsmParses to initial and also add this initial {@link List} to the result {@link List}.
         * For the following iterations, it checks whether the current item's root with the MorphologicalTag of the first inflectional
         * equal to the previous item's  root with the MorphologicalTag of the first inflectional. If so, it adds that item
         * to the result {@link List}, if not it creates new {@link List} as initial and adds the first item od fsmParses
         * to initial and also add this initial {@link List} to the result {@link List}.</summary>
         *
         * <returns>result {@link List} type of {@link FsmParseList}.</returns>
         */
        public List<FsmParseList> ConstructParseListForDifferentRootWithPos()
        {
            var result = new List<FsmParseList>();
            var i = 0;
            while (i < _fsmParses.Count)
            {
                if (i == 0)
                {
                    var initial = new List<FsmParse>();
                    initial.Add(_fsmParses[i]);
                    result.Add(new FsmParseList(initial));
                }
                else
                {
                    if (_fsmParses[i].GetWordWithPos().Equals(_fsmParses[i - 1].GetWordWithPos()))
                    {
                        result[result.Count - 1]._fsmParses.Add(_fsmParses[i]);
                    }
                    else
                    {
                        var initial = new List<FsmParse> { _fsmParses[i] };
                        result.Add(new FsmParseList(initial));
                    }
                }

                i++;
            }

            return result;
        }

        /**
         * <summary>The parsesWithoutPrefixAndSuffix method first creates a {@link string} array named analyses with the size of fsmParses {@link List}'s size.
         * <p/>
         * If the size is just 1, it then returns the first item's transitionList, if it is greater than 1, loops through the fsmParses and
         * puts the transitionList of each item to the analyses array.
         * <p/>
         * If the removePrefix condition holds, it loops through the analyses array and takes each item's substring after the first + sign and updates that
         * item of analyses array with that substring.
         * <p/>
         * If the removeSuffix condition holds, it loops through the analyses array and takes each item's substring till the last + sign and updates that
         * item of analyses array with that substring.
         * <p/>
         * It then removes the duplicate items of analyses array and returns a result {@link string} that has the accumulated items of analyses array.</summary>
         *
         * <returns>result {@link string} that has the accumulated items of analyses array.</returns>
         */
        public string ParsesWithoutPrefixAndSuffix()
        {
            var analyses = new string[_fsmParses.Count];
            bool removePrefix = true, removeSuffix = true;
            if (_fsmParses.Count == 1)
            {
                return _fsmParses[0].TransitionList().Substring(_fsmParses[0].TransitionList().IndexOf("+") + 1);
            }

            for (var i = 0; i < _fsmParses.Count; i++)
            {
                analyses[i] = _fsmParses[i].TransitionList();
            }

            while (removePrefix)
            {
                for (var i = 0; i < _fsmParses.Count - 1; i++)
                {
                    if (!analyses[i].Contains("+") || !analyses[i + 1].Contains("+") ||
                        !analyses[i].Substring(0, analyses[i].IndexOf("+") + 1)
                            .Equals(analyses[i + 1].Substring(0, analyses[i + 1].IndexOf("+") + 1)))
                    {
                        removePrefix = false;
                        break;
                    }
                }

                if (removePrefix)
                {
                    for (var i = 0; i < _fsmParses.Count; i++)
                    {
                        analyses[i] = analyses[i].Substring(analyses[i].IndexOf("+") + 1);
                    }
                }
            }

            while (removeSuffix)
            {
                for (var i = 0; i < _fsmParses.Count - 1; i++)
                {
                    if (!analyses[i].Contains("+") || !analyses[i + 1].Contains("+") ||
                        !analyses[i].Substring(analyses[i].LastIndexOf("+"))
                            .Equals(analyses[i + 1].Substring(analyses[i + 1].LastIndexOf("+"))))
                    {
                        removeSuffix = false;
                        break;
                    }
                }

                if (removeSuffix)
                {
                    for (var i = 0; i < _fsmParses.Count; i++)
                    {
                        analyses[i] = analyses[i].Substring(0, analyses[i].LastIndexOf("+"));
                    }
                }
            }

            for (var i = 0; i < analyses.Length; i++)
            {
                for (var j = i + 1; j < analyses.Length; j++)
                {
                    if (analyses[i].CompareTo(analyses[j]) > 0)
                    {
                        var tmp = analyses[i];
                        analyses[i] = analyses[j];
                        analyses[j] = tmp;
                    }
                }
            }

            var result = analyses[0];
            for (var i = 1; i < analyses.Length; i++)
            {
                result = result + "$" + analyses[i];
            }

            return result;
        }

        /**
         * <summary>The overridden tostring method loops through the fsmParses {@link List} and accumulates the items to a result {@link string}.</summary>
         *
         * <returns>result {@link string} that has the items of fsmParses {@link List}.</returns>
         */
        public override string ToString()
        {
            var result = "";
            foreach (var t in _fsmParses)
            {
                result += t + "\n";
            }

            return result;
        }

        /**
         * <summary>The toJson method adds [\n to the beginning of the result {@link string} that has the items of fsmParses {@link List}
         * between double quotes and adds /n] to the ending.</summary>
         *
         * <returns>string output.</returns>
         */
        public string ToJson()
        {
            var json = "[\n";
            for (var i = 0; i < _fsmParses.Count; i++)
            {
                if (i == 0)
                {
                    json = json + "\"" + _fsmParses[i] + "\"";
                }
                else
                {
                    json = json + ",\n\"" + _fsmParses[i] + "\"";
                }
            }

            return json + "\n]";
        }
    }
}