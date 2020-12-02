using System.Collections.Generic;
using Dictionary.Dictionary;

namespace MorphologicalAnalysis
{
    public class FsmParseList
    {
        private readonly List<FsmParse> _fsmParses;

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
         * longest root word, the first parse with that root is returned.</summary>
         *
         * <returns>FsmParse Parse with the longest root word.</returns>
         */
        public FsmParse GetParseWithLongestRootWord()
        {
            var maxLength = -1;
            FsmParse bestParse = null;
            foreach (var currentParse in _fsmParses)
            {
                if (currentParse.GetWord().GetName().Length > maxLength)
                {
                    maxLength = currentParse.GetWord().GetName().Length;
                    bestParse = currentParse;
                }
            }

            return bestParse;
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
                        var initial = new List<FsmParse> {_fsmParses[i]};
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