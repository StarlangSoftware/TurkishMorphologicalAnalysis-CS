using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Corpus;
using DataStructure.Cache;
using Dictionary.Dictionary;
using Dictionary.Dictionary.Trie;

namespace MorphologicalAnalysis
{
    public class FsmMorphologicalAnalyzer
    {
        private readonly Trie _dictionaryTrie;
        private Trie _suffixTrie;
        private readonly FiniteStateMachine _finiteStateMachine;
        private static int MAX_DISTANCE = 2;
        private Dictionary<string, string> parsedSurfaceForms;
        private readonly TxtDictionary _dictionary;
        private readonly LRUCache<string, FsmParseList> _cache;
        private readonly Dictionary<string, Regex> _mostUsedPatterns = new Dictionary<string, Regex>();

        /**
         * <summary>First no-arg constructor of FsmMorphologicalAnalyzer class. It generates a new TxtDictionary type dictionary from
         * turkish_dictionary.txt with fixed cache size 10000000 and by using turkish_finite_state_machine.xml file.</summary>
         */
        public FsmMorphologicalAnalyzer() : this("turkish_finite_state_machine.xml", new TxtDictionary(), 10000000)
        {
        }

        /**
         * <summary>Another constructor of FsmMorphologicalAnalyzer class. It generates a new TxtDictionary type dictionary from
         * turkish_dictionary.txt with given input cacheSize and by using turkish_finite_state_machine.xml file.</summary>
         *
         * <param name="cacheSize">the size of the LRUCache.</param>
         */
        public FsmMorphologicalAnalyzer(int cacheSize) : this("turkish_finite_state_machine.xml", new TxtDictionary(),
            cacheSize)
        {
        }

        /**
         * <summary>Another constructor of FsmMorphologicalAnalyzer class. It generates a new TxtDictionary type dictionary from
         * given input dictionary file name and by using turkish_finite_state_machine.xml file.</summary>
         *
         * <param name="dictionaryFileName">the size of the LRUCache.</param>
         */
        public FsmMorphologicalAnalyzer(string dictionaryFileName) : this("turkish_finite_state_machine.xml",
            new TxtDictionary(dictionaryFileName, new TurkishWordComparator()),
            10000000)
        {
        }

        /**
         * <summary>Another constructor of FsmMorphologicalAnalyzer class. It generates a new TxtDictionary type dictionary from
         * given input dictionary file name and by using turkish_finite_state_machine.xml file.</summary>
         *
         * <param name="fileName">          the file to read the finite state machine.</param>
         * <param name="dictionaryFileName">the file to read the dictionary.</param>
         */
        public FsmMorphologicalAnalyzer(string fileName, string dictionaryFileName) : this(fileName,
            new TxtDictionary(dictionaryFileName, new TurkishWordComparator()), 10000000)
        {
        }

        /**
         * <summary>Another constructor of FsmMorphologicalAnalyzer class. It generates a new TxtDictionary type dictionary from
         * given input dictionary, with given inputs fileName and cacheSize.</summary>
         *
         * <param name="fileName">  the file to read the finite state machine.</param>
         * <param name="dictionary">the dictionary file that will be used to generate dictionaryTrie.</param>
         * <param name="cacheSize"> the size of the LRUCache.</param>
         */
        public FsmMorphologicalAnalyzer(string fileName, TxtDictionary dictionary, int cacheSize)
        {
            _dictionary = dictionary;
            _finiteStateMachine = new FiniteStateMachine(fileName);
            PrepareSuffixTrie();
            _dictionaryTrie = dictionary.PrepareTrie();
            if (cacheSize > 0)
            {
                _cache = new LRUCache<string, FsmParseList>(cacheSize);
            }
            else
            {
                _cache = null;
            }
        }

        /**
         * <summary>Another constructor of FsmMorphologicalAnalyzer class. It generates a new TxtDictionary type dictionary from
         * given input dictionary, with given input fileName and fixed size cacheSize = 10000000.</summary>
         *
         * <param name="fileName">  the file to read the finite state machine.</param>
         * <param name="dictionary">the dictionary file that will be used to generate dictionaryTrie.</param>
         */
        public FsmMorphologicalAnalyzer(string fileName, TxtDictionary dictionary) : this(fileName, dictionary,
            10000000)
        {
        }

        /**
         * <summary>Another constructor of FsmMorphologicalAnalyzer class. It generates a new TxtDictionary type dictionary from
         * given input dictionary, with given input fileName and fixed size cacheSize = 10000000.</summary>
         *
         * <param name="dictionary">the dictionary file that will be used to generate dictionaryTrie.</param>
         */
        public FsmMorphologicalAnalyzer(TxtDictionary dictionary) : this("turkish_finite_state_machine.xml", dictionary,
            10000000)
        {
        }
        
        /// <summary>
        /// Constructs and returns the reverse string of a given string.
        /// </summary>
        /// <param name="s">String to be reversed.</param>
        /// <returns>Reverse of a given string.</returns>
        private string ReverseString(string s)
        {
            var result = "";
            for (var i = s.Length - 1; i >= 0; i--)
            {
                result += s[i];
            }

            return result;
        }

        /// <summary>
        /// Constructs the suffix trie from the input file suffixes.txt. suffixes.txt contains the most frequent 6000
        /// suffixes that a verb or a noun can take. The suffix trie is a trie that stores these suffixes in reverse form,
        /// which can be then used to match a given word for its possible suffix content.
        /// </summary>
        private void PrepareSuffixTrie()
        {
            _suffixTrie = new Trie();
            var assembly = typeof(FiniteStateMachine).Assembly;
            var stream = assembly.GetManifestResourceStream("MorphologicalAnalysis.suffixes.txt");
            var streamReader = new StreamReader(stream);
            var suffix = streamReader.ReadLine();
            while (suffix != null)
            {
                var reverseSuffix = ReverseString(suffix);
                _suffixTrie.AddWord(reverseSuffix, new Word(reverseSuffix));
                suffix = streamReader.ReadLine();
            }
            streamReader.Close();
        }

        /// <summary>
        /// Reads the file for correct surface forms and their most frequent root forms, in other words, the surface forms
        /// which have at least one morphological analysis in Turkish.
        /// </summary>
        /// <param name="fileName">Input file containing analyzable surface forms and their root forms.</param>
        public void AddParsedSurfaceForms(string fileName)
        {
            parsedSurfaceForms = new Dictionary<string, string>();
            var assembly = typeof(FiniteStateMachine).Assembly;
            var stream = assembly.GetManifestResourceStream("MorphologicalAnalysis." + fileName);
            var streamReader = new StreamReader(stream);
            var line = streamReader.ReadLine();
            while (line != null)
            {
                var items = line.Split();
                parsedSurfaceForms[items[0]] = items[1];
                line = streamReader.ReadLine();
            }
            streamReader.Close();
        }

        /**
         * <summary>The getPossibleWords method takes {@link MorphologicalParse} and {@link MetamorphicParse} as input.
         * First it determines whether the given morphologicalParse is the root verb and whether it contains a verb tag.
         * Then it creates new transition with -mak and creates a new {@link HashSet} result.
         * <p/>
         * It takes the given {@link MetamorphicParse} input as currentWord and if there is a compound word starting with the
         * currentWord, it gets this compoundWord from dictionaryTrie. If there is a compoundWord and the difference of the
         * currentWord and compundWords is less than 3 than compoundWord is added to the result, otherwise currentWord is added.
         * <p/>
         * Then it gets the root from parse input as a currentRoot. If it is not null, and morphologicalParse input is verb,
         * it directly adds the verb to result after making transition to currentRoot with currentWord string. Else, it creates a new
         * transition with -lar and make this transition then adds to the result.</summary>
         *
         * <param name="morphologicalParse">{@link MorphologicalParse} type input.</param>
         * <param name="metamorphicParse">             {@link MetamorphicParse} type input.</param>
         * <returns>{@link HashSet} result.</returns>
         */
        public HashSet<string> GetPossibleWords(MorphologicalParse morphologicalParse,
            MetamorphicParse metamorphicParse)
        {
            bool isRootVerb = morphologicalParse.GetRootPos().Equals("VERB");
            bool containsVerb = morphologicalParse.ContainsTag(MorphologicalTag.VERB);
            var verbTransition = new Transition("mAk");
            var result = new HashSet<string>();
            if (metamorphicParse == null || metamorphicParse.GetWord() == null)
            {
                return result;
            }

            var currentWord = metamorphicParse.GetWord().GetName();
            var pluralIndex = -1;
            var compoundWord = _dictionaryTrie.GetCompoundWordStartingWith(currentWord);
            if (!isRootVerb)
            {
                if (compoundWord != null && compoundWord.GetName().Length - currentWord.Length < 3)
                {
                    result.Add(compoundWord.GetName());
                }

                result.Add(currentWord);
            }

            var currentRoot = (TxtWord)_dictionary.GetWord(metamorphicParse.GetWord().GetName());
            if (currentRoot == null && compoundWord != null)
            {
                currentRoot = compoundWord;
            }

            if (currentRoot != null)
            {
                string verbWord;
                if (isRootVerb)
                {
                    verbWord = verbTransition.MakeTransition(currentRoot, currentWord);
                    result.Add(verbWord);
                }

                string pluralWord = null;
                Transition transition;
                for (var i = 1; i < metamorphicParse.Size(); i++)
                {
                    transition = new Transition(null, metamorphicParse.GetMetaMorpheme(i), null);
                    if (metamorphicParse.GetMetaMorpheme(i).Equals("lAr"))
                    {
                        pluralWord = currentWord;
                        pluralIndex = i + 1;
                    }

                    currentWord = transition.MakeTransition(currentRoot, currentWord);
                    result.Add(currentWord);
                    if (containsVerb)
                    {
                        verbWord = verbTransition.MakeTransition(currentRoot, currentWord);
                        result.Add(verbWord);
                    }
                }

                if (pluralWord != null)
                {
                    currentWord = pluralWord;
                    for (var i = pluralIndex; i < metamorphicParse.Size(); i++)
                    {
                        transition = new Transition(null, metamorphicParse.GetMetaMorpheme(i), null);
                        currentWord = transition.MakeTransition(currentRoot, currentWord);
                        result.Add(currentWord);
                        if (containsVerb)
                        {
                            verbWord = verbTransition.MakeTransition(currentRoot, currentWord);
                            result.Add(verbWord);
                        }
                    }
                }
            }

            return result;
        }

        /**
         * <summary>The getDictionary method is used to get TxtDictionary.</summary>
         *
         * <returns>TxtDictionary type dictionary.</returns>
         */
        public TxtDictionary GetDictionary()
        {
            return _dictionary;
        }

        /**
         * <summary>The getFiniteStateMachine method is used to get FiniteStateMachine.</summary>
         *
         * <returns>FiniteStateMachine type finiteStateMachine.</returns>
         */
        public FiniteStateMachine GetFiniteStateMachine()
        {
            return _finiteStateMachine;
        }

        /**
         * <summary><p>The isPossibleSubstring method first checks whether given short and long strings are equal to root word.
         * Then, compares both short and long strings' chars till the last two chars of short string. In the presence of mismatch,
         * false is returned. On the other hand, it counts the distance between two strings until it becomes greater than 2,
         * which is the MAX_DISTANCE also finds the index of the last char.
         * </p><p>
         * If the substring is a rootWord and.Equals to 'ben', which is a special case or root holds the lastIdropsDuringSuffixation or
         * lastIdropsDuringPassiveSuffixation conditions, then it returns true if distance is not greater than MAX_DISTANCE.
         * </p><p>
         * On the other hand, if the shortStrong ends with one of these chars 'e, a, p, ç, t, k' and 't 's a rootWord with
         * the conditions of rootSoftenDuringSuffixation, vowelEChangesToIDuringYSuffixation, vowelAChangesToIDuringYSuffixation
         * or endingKChangesIntoG then it returns true if the last index is not equal to 2 and distance is not greater than
         * MAX_DISTANCE and false otherwise.
         * </p></summary>
         *
         * <param name="shortString">the possible substring.</param>
         * <param name="longString"> the long string to compare with substring.</param>
         * <param name="root">       the root of the long string.</param>
         * <returns>true if given substring is the actual substring of the longstring, false otherwise.</returns>
         */
        private bool IsPossibleSubstring(string shortString, string longString, TxtWord root)
        {
            var rootWord = shortString == root.GetName() || longString == root.GetName();
            int distance = 0, j, last = 1;
            for (j = 0; j < shortString.Length; j++)
            {
                if (shortString[j] != longString[j])
                {
                    if (j < shortString.Length - 2)
                    {
                        return false;
                    }

                    last = shortString.Length - j;
                    distance++;
                    if (distance > MAX_DISTANCE)
                    {
                        break;
                    }
                }
            }

            if (rootWord && (root.GetName().Equals("ben") || root.GetName().Equals("sen") ||
                             root.LastIdropsDuringSuffixation() ||
                             root.LastIdropsDuringPassiveSuffixation()))
            {
                return distance <= MAX_DISTANCE;
            }

            if (shortString.EndsWith("e") || shortString.EndsWith("a") || shortString.EndsWith("p") ||
                shortString.EndsWith("ç") || shortString.EndsWith("t") || shortString.EndsWith("k") ||
                (rootWord && (root.RootSoftenDuringSuffixation() || root.VowelEChangesToIDuringYSuffixation() ||
                              root.VowelAChangesToIDuringYSuffixation() || root.EndingKChangesIntoG())))
            {
                return last != 2 && distance <= MAX_DISTANCE - 1;
            }

            return distance <= MAX_DISTANCE - 2;
        }

        /**
         * <summary>The initializeParseList method initializes the given given fsm ArrayList with given root words by parsing them.
         * <p>
         * It checks many conditions;
         * isPlural; if root holds the condition then it gets the state with the name of NominalRootPlural, then
         * creates a new parsing and adds this to the input fsmParse Arraylist.
         * Ex : Açıktohumlular
         * </p><p>
         * !isPlural and isPortmanteauEndingWithSI, if root holds the conditions then it gets the state with the
         * name of NominalRootNoPossesive.
         * Ex : Balarısı
         * </p><p>
         * !isPlural and isPortmanteau, if root holds the conditions then it gets the state with the name of
         * CompoundNounRoot.
         * Ex : Aslanağızı
         * </p><p>
         * !isPlural, !isPortmanteau and isHeader, if root holds the conditions then it gets the state with the
         * name of HeaderRoot.
         * Ex : 
         * </p><p>
         * !isPlural, !isPortmanteau and isInterjection, if root holds the conditions then it gets the state
         * with the name of InterjectionRoot.
         * Ex : Hey, Aa
         * </p><p>
         * !isPlural, !isPortmanteau and isDuplicate, if root holds the conditions then it gets the state
         * with the name of DuplicateRoot.
         * Ex : Allak,
         * </p><p>
         * !isPlural, !isPortmanteau and isCode, if root holds the conditions then it gets the state
         * with the name of CodeRoot.
         * Ex : 9400f,
         * </p><p>
         * !isPlural, !isPortmanteau and isMetric, if root holds the conditions then it gets the state
         * with the name of MetricRoot.
         * Ex : 11x8x12,
         * </p><p>
         * !isPlural, !isPortmanteau and isNumeral, if root holds the conditions then it gets the state
         * with the name of CardinalRoot.
         * Ex : Yüz, bin
         * </p><p>
         * !isPlural, !isPortmanteau and isReal, if root holds the conditions then it gets the state
         * with the name of RealRoot.
         * Ex : 1.2
         * </p><p>
         * !isPlural, !isPortmanteau and isFraction, if root holds the conditions then it gets the state
         * with the name of FractionRoot.
         * Ex : 1/2
         * </p><p>
         * !isPlural, !isPortmanteau and isDate, if root holds the conditions then it gets the state
         * with the name of DateRoot.
         * Ex : 11/06/2018
         * </p><p>
         * !isPlural, !isPortmanteau and isPercent, if root holds the conditions then it gets the state
         * with the name of PercentRoot.
         * Ex : %12.5
         * </p><p>
         * !isPlural, !isPortmanteau and isRange, if root holds the conditions then it gets the state
         * with the name of RangeRoot.
         * Ex : 3-5
         * </p><p>
         * !isPlural, !isPortmanteau and isTime, if root holds the conditions then it gets the state
         * with the name of TimeRoot.
         * Ex : 13:16:08
         * </p><p>
         * !isPlural, !isPortmanteau and isOrdinal, if root holds the conditions then it gets the state
         * with the name of OrdinalRoot.
         * Ex : Altıncı
         * </p><p>
         * !isPlural, !isPortmanteau, and isVerb if root holds the conditions then it gets the state
         * with the name of VerbalRoot. Or isPassive, then it gets the state with the name of PassiveHn.
         * Ex : Anla (!isPAssive)
         * Ex : Çağrıl (isPassive)
         * </p><p>
         * !isPlural, !isPortmanteau and isPronoun, if root holds the conditions then it gets the state
         * with the name of PronounRoot. There are 6 different Pronoun state names, REFLEX, QUANT, QUANTPLURAL, DEMONS, PERS, QUES.
         * REFLEX = Reflexive Pronouns Ex : kendi
         * QUANT = Quantitative Pronouns Ex : öbür, hep, kimse, hiçbiri, bazı, kimi, biri
         * QUANTPLURAL = Quantitative Plural Pronouns Ex : tümü, çoğu, hepsi
         * DEMONS = Demonstrative Pronouns Ex : o, bu, şu
         * PERS = Personal Pronouns Ex : ben, sen, o, biz, siz, onlar
         * QUES = Interrogatıve Pronouns Ex : nere, ne, kim, hangi
         * </p><p>
         * !isPlural, !isPortmanteau and isAdjective, if root holds the conditions then it gets the state
         * with the name of AdjectiveRoot.
         * Ex : Absürt, Abes
         * </p><p>
         * !isPlural, !isPortmanteau and isPureAdjective, if root holds the conditions then it gets the state
         * with the name of Adjective.
         * Ex : Geçmiş, Cam
         * </p><p>
         * !isPlural, !isPortmanteau and isNominal, if root holds the conditions then it gets the state
         * with the name of NominalRoot.
         * Ex : Görüş
         * </p><p>
         * !isPlural, !isPortmanteau and isProper, if root holds the conditions then it gets the state
         * with the name of ProperRoot.
         * Ex : Abdi
         * </p><p>
         * !isPlural, !isPortmanteau and isQuestion, if root holds the conditions then it gets the state
         * with the name of QuestionRoot.
         * Ex : Mi, mü
         * </p><p>
         * !isPlural, !isPortmanteau and isDeterminer, if root holds the conditions then it gets the state
         * with the name of DeterminerRoot.
         * Ex : Çok, bir
         * </p><p>
         * !isPlural, !isPortmanteau and isConjunction, if root holds the conditions then it gets the state
         * with the name of ConjunctionRoot.
         * Ex : Ama , ancak
         * </p><p>
         * !isPlural, !isPortmanteau and isPostP, if root holds the conditions then it gets the state
         * with the name of PostP.
         * Ex : Ait, dair
         * </p><p>
         * !isPlural, !isPortmanteau and isAdverb, if root holds the conditions then it gets the state
         * with the name of AdverbRoot.
         * Ex : Acilen
         * </p></summary>
         *
         * <param name="fsmParse">ArrayList to initialize.</param>
         * <param name="root">    word to check properties and add to fsmParse according to them.</param>
         * <param name="isProper">is used to check a word is proper or not.</param>
         */
        private void InitializeParseList(List<FsmParse> fsmParse, TxtWord root, bool isProper)
        {
            FsmParse currentFsmParse;
            if (root.IsPlural())
            {
                currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("NominalRootPlural"));
                fsmParse.Add(currentFsmParse);
            }
            else
            {
                if (root.IsPortmanteauEndingWithSI())
                {
                    currentFsmParse = new FsmParse(root.GetName().Substring(0, root.GetName().Length - 2),
                        _finiteStateMachine.GetState("CompoundNounRoot"));
                    fsmParse.Add(currentFsmParse);
                    currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("NominalRootNoPossesive"));
                    fsmParse.Add(currentFsmParse);
                }
                else
                {
                    if (root.IsPortmanteau())
                    {
                        if (root.IsPortmanteauFacedVowelEllipsis())
                        {
                            currentFsmParse = new FsmParse(root,
                                _finiteStateMachine.GetState("NominalRootNoPossesive"));
                            fsmParse.Add(currentFsmParse);
                            currentFsmParse = new FsmParse(
                                root.GetName().Substring(0, root.GetName().Length - 2) +
                                root.GetName()[root.GetName().Length - 1] +
                                root.GetName()[root.GetName().Length - 2],
                                _finiteStateMachine.GetState("CompoundNounRoot"));
                        }
                        else
                        {
                            if (root.IsPortmanteauFacedSoftening())
                            {
                                currentFsmParse = new FsmParse(root,
                                    _finiteStateMachine.GetState("NominalRootNoPossesive"));
                                fsmParse.Add(currentFsmParse);
                                switch (root.GetName()[root.GetName().Length - 2])
                                {
                                    case 'b':
                                        currentFsmParse =
                                            new FsmParse(root.GetName().Substring(0, root.GetName().Length - 2) + 'p',
                                                _finiteStateMachine.GetState("CompoundNounRoot"));
                                        break;
                                    case 'c':
                                        currentFsmParse =
                                            new FsmParse(root.GetName().Substring(0, root.GetName().Length - 2) + 'ç',
                                                _finiteStateMachine.GetState("CompoundNounRoot"));
                                        break;
                                    case 'd':
                                        currentFsmParse =
                                            new FsmParse(root.GetName().Substring(0, root.GetName().Length - 2) + 't',
                                                _finiteStateMachine.GetState("CompoundNounRoot"));
                                        break;
                                    case 'ğ':
                                        currentFsmParse =
                                            new FsmParse(root.GetName().Substring(0, root.GetName().Length - 2) + 'k',
                                                _finiteStateMachine.GetState("CompoundNounRoot"));
                                        break;
                                    default:
                                        currentFsmParse =
                                            new FsmParse(root.GetName().Substring(0, root.GetName().Length - 1),
                                                _finiteStateMachine.GetState("CompoundNounRoot"));
                                        break;
                                }
                            }
                            else
                            {
                                currentFsmParse = new FsmParse(root.GetName().Substring(0, root.GetName().Length - 1),
                                    _finiteStateMachine.GetState("CompoundNounRoot"));
                            }
                        }

                        fsmParse.Add(currentFsmParse);
                    }
                    else
                    {
                        if (root.IsHeader())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("HeaderRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsInterjection())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("InterjectionRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsDuplicate())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("DuplicateRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsCode())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("CodeRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsMetric())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("MetricRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsNumeral())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("CardinalRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsReal())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("RealRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsFraction())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("FractionRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsDate())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("DateRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsPercent())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("PercentRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsRange())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("RangeRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsTime())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("TimeRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsOrdinal())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("OrdinalRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsVerb() || root.IsPassive())
                        {
                            if (!root.VerbType().Equals(""))
                            {
                                currentFsmParse = new FsmParse(root,
                                    _finiteStateMachine.GetState("VerbalRoot(" + root.VerbType() + ")"));
                            }
                            else
                            {
                                if (!root.IsPassive())
                                {
                                    currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("VerbalRoot"));
                                }
                                else
                                {
                                    currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("PassiveHn"));
                                }
                            }

                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsPronoun())
                        {
                            if (root.GetName().Equals("kendi"))
                            {
                                currentFsmParse = new FsmParse(root,
                                    _finiteStateMachine.GetState("PronounRoot(REFLEX)"));
                                fsmParse.Add(currentFsmParse);
                            }

                            if (root.GetName().Equals("öbür") || root.GetName().Equals("öteki") ||
                                root.GetName().Equals("hep") || root.GetName().Equals("kimse") ||
                                root.GetName().Equals("diğeri") ||
                                root.GetName().Equals("hiçbiri") ||
                                root.GetName().Equals("böylesi") ||
                                root.GetName().Equals("birbiri") ||
                                root.GetName().Equals("birbirleri") ||
                                root.GetName().Equals("biri") || root.GetName().Equals("başkası") ||
                                root.GetName().Equals("bazı") || root.GetName().Equals("kimi"))
                            {
                                currentFsmParse = new FsmParse(root,
                                    _finiteStateMachine.GetState("PronounRoot(QUANT)"));
                                fsmParse.Add(currentFsmParse);
                            }

                            if (root.GetName().Equals("tümü") || root.GetName().Equals("topu") ||
                                root.GetName().Equals("herkes") ||
                                root.GetName().Equals("cümlesi") || root.GetName().Equals("çoğu") ||
                                root.GetName().Equals("birçoğu") ||
                                root.GetName().Equals("birkaçı") ||
                                root.GetName().Equals("birçokları") ||
                                root.GetName().Equals("hepsi"))
                            {
                                currentFsmParse = new FsmParse(root,
                                    _finiteStateMachine.GetState("PronounRoot(QUANTPLURAL)"));
                                fsmParse.Add(currentFsmParse);
                            }

                            if (root.GetName().Equals("o") || root.GetName().Equals("bu") ||
                                root.GetName().Equals("şu"))
                            {
                                currentFsmParse = new FsmParse(root,
                                    _finiteStateMachine.GetState("PronounRoot(DEMONS)"));
                                fsmParse.Add(currentFsmParse);
                            }

                            if (root.GetName().Equals("ben") || root.GetName().Equals("sen") ||
                                root.GetName().Equals("o") || root.GetName().Equals("biz") ||
                                root.GetName().Equals("siz") || root.GetName().Equals("onlar"))
                            {
                                currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("PronounRoot(PERS)"));
                                fsmParse.Add(currentFsmParse);
                            }

                            if (root.GetName().Equals("nere") || root.GetName().Equals("ne") ||
                                root.GetName().Equals("kaçı") || root.GetName().Equals("kim") ||
                                root.GetName().Equals("hangi"))
                            {
                                currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("PronounRoot(QUES)"));
                                fsmParse.Add(currentFsmParse);
                            }
                        }

                        if (root.IsAdjective())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("AdjectiveRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsPureAdjective())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("Adjective"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsNominal())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("NominalRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsAbbreviation())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("NominalRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsProperNoun() && isProper)
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("ProperRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsQuestion())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("QuestionRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsDeterminer())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("DeterminerRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsConjunction())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("ConjunctionRoot"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsPostP())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("PostP"));
                            fsmParse.Add(currentFsmParse);
                        }

                        if (root.IsAdverb())
                        {
                            currentFsmParse = new FsmParse(root, _finiteStateMachine.GetState("AdverbRoot"));
                            fsmParse.Add(currentFsmParse);
                        }
                    }
                }
            }
        }

        /**
         * <summary>The initializeParseListFromRoot method is used to create an {@link ArrayList} which consists of initial fsm parsings. First, traverses
         * this HashSet and uses each word as a root and calls initializeParseList method with this root and ArrayList.
         * </summary>
         *
         * <param name="parseList">ArrayList to initialize.</param>
         * <param name="root">the root form to generate initial parse list.</param>
         * <param name="isProper">   is used to check a word is proper or not.</param>
         */
        private void InitializeParseListFromRoot(List<FsmParse> parseList, TxtWord root, bool isProper)
        {
            InitializeParseList(parseList, root, isProper);
            if (root.ObeysAndNotObeysVowelHarmonyDuringAgglutination())
            {
                var newRoot = root.Clone();
                newRoot.RemoveFlag("IS_UU");
                newRoot.RemoveFlag("IS_UUU");
                InitializeParseList(parseList, newRoot, isProper);
            }

            if (root.RootSoftenAndNotSoftenDuringSuffixation())
            {
                var newRoot = root.Clone();
                newRoot.RemoveFlag("IS_SD");
                newRoot.RemoveFlag("IS_SDD");
                InitializeParseList(parseList, newRoot, isProper);
            }

            if (root.LastIDropsAndNotDropDuringSuffixation())
            {
                var newRoot = root.Clone();
                newRoot.RemoveFlag("IS_UD");
                newRoot.RemoveFlag("IS_UDD");
                InitializeParseList(parseList, newRoot, isProper);
            }

            if (root.DuplicatesAndNotDuplicatesDuringSuffixation())
            {
                var newRoot = root.Clone();
                newRoot.RemoveFlag("IS_ST");
                newRoot.RemoveFlag("IS_STT");
                InitializeParseList(parseList, newRoot, isProper);
            }

            if (root.EndingKChangesIntoG() && root.ContainsFlag("IS_OA"))
            {
                var newRoot = root.Clone();
                newRoot.RemoveFlag("IS_OA");
                InitializeParseList(parseList, newRoot, isProper);
            }
        }

        /**
         * <summary>The initializeParseListFromSurfaceForm method is used to create an {@link ArrayList} which consists of initial fsm parsings. First,
         * it calls.GetWordsWithPrefix methods by using input string surfaceForm and generates a {@link HashSet}. Then, traverses
         * this HashSet and uses each word as a root and calls initializeParseListFromRoot method with this root and ArrayList.
         * </summary>
         *
         * <param name="surfaceForm">the string used to generate a HashSet of words.</param>
         * <param name="isProper">   is used to check a word is proper or not.</param>
         * <returns>initialFsmParse ArrayList.</returns>
         */
        private List<FsmParse> InitializeParseListFromSurfaceForm(string surfaceForm, bool isProper)
        {
            var initialFsmParse = new List<FsmParse>();
            if (surfaceForm.Length == 0)
            {
                return initialFsmParse;
            }

            var words = _dictionaryTrie.GetWordsWithPrefix(surfaceForm);
            foreach (var word in words)
            {
                var root = (TxtWord)word;
                InitializeParseListFromRoot(initialFsmParse, root, isProper);
            }

            return initialFsmParse;
        }

        /**
         * <summary>The addNewParsesFromCurrentParse method initially gets the final suffixes from input currentFsmParse called as currentState,
         * and by using the currentState information it gets the new analysis. Then loops through each currentState's transition.
         * If the currentTransition is possible, it makes the transition.</summary>
         *
         * <param name="currentFsmParse">FsmParse type input.</param>
         * <param name="fsmParse">       an ArrayList of FsmParse.</param>
         * <param name="maxLength">    Maximum.Length of the parse.</param>
         * <param name="root">           TxtWord used to make transition.</param>
         */
        private void AddNewParsesFromCurrentParse(FsmParse currentFsmParse, List<FsmParse> fsmParse, int maxLength,
            TxtWord root)
        {
            var currentState = currentFsmParse.GetFinalSuffix();
            var currentSurfaceForm = currentFsmParse.GetSurfaceForm();
            foreach (var currentTransition in _finiteStateMachine.GetTransitions(currentState))
            {
                if (currentTransition.TransitionPossible(currentFsmParse) &&
                    (!string.Equals(currentSurfaceForm, root.GetName(), StringComparison.Ordinal) ||
                     (string.Equals(currentSurfaceForm, root.GetName(), StringComparison.Ordinal) &&
                      currentTransition.TransitionPossible(root, currentState))))
                {
                    var tmp =
                        currentTransition.MakeTransition(root, currentSurfaceForm, currentFsmParse.GetStartState());
                    if (tmp.Length <= maxLength)
                    {
                        var newFsmParse = (FsmParse)currentFsmParse.Clone();
                        newFsmParse.AddSuffix(currentTransition.ToState(), tmp, currentTransition.With(),
                            currentTransition.ToString(), currentTransition.ToPos());
                        newFsmParse.SetAgreement(currentTransition.With());
                        fsmParse.Add(newFsmParse);
                    }
                }
            }
        }

        /**
         * <summary>The addNewParsesFromCurrentParse method initially gets the final suffixes from input currentFsmParse called as currentState,
         * and by using the currentState information it gets the currentSurfaceForm. Then loops through each currentState's transition.
         * If the currentTransition is possible, it makes the transition</summary>
         *
         * <param name="currentFsmParse">FsmParse type input.</param>
         * <param name="fsmParse">       an ArrayList of FsmParse.</param>
         * <param name="surfaceForm">    string to use during transition.</param>
         * <param name="root">           TxtWord used to make transition.</param>
         */
        private void AddNewParsesFromCurrentParse(FsmParse currentFsmParse, List<FsmParse> fsmParse,
            string surfaceForm, TxtWord root)
        {
            var currentState = currentFsmParse.GetFinalSuffix();
            var currentSurfaceForm = currentFsmParse.GetSurfaceForm();
            foreach (var currentTransition in _finiteStateMachine.GetTransitions(currentState))
            {
                if (currentTransition.TransitionPossible(currentFsmParse.GetSurfaceForm(), surfaceForm) &&
                    currentTransition.TransitionPossible(currentFsmParse) &&
                    (!string.Equals(currentSurfaceForm, root.GetName(), StringComparison.Ordinal) ||
                     (string.Equals(currentSurfaceForm, root.GetName(), StringComparison.Ordinal) &&
                      currentTransition.TransitionPossible(root, currentState))))
                {
                    var tmp =
                        currentTransition.MakeTransition(root, currentSurfaceForm, currentFsmParse.GetStartState());
                    if ((tmp.Length < surfaceForm.Length && IsPossibleSubstring(tmp, surfaceForm, root)) ||
                        (tmp.Length == surfaceForm.Length &&
                         (root.LastIdropsDuringSuffixation() || (tmp.Equals(surfaceForm)))))
                    {
                        var newFsmParse = (FsmParse)currentFsmParse.Clone();
                        newFsmParse.AddSuffix(currentTransition.ToState(), tmp, currentTransition.With(),
                            currentTransition.ToString(), currentTransition.ToPos());
                        newFsmParse.SetAgreement(currentTransition.With());
                        fsmParse.Add(newFsmParse);
                    }
                }
            }
        }

        /**
         * <summary>The parseExists method is used to check the existence of the parse.</summary>
         *
         * <param name="fsmParse">   an ArrayList of FsmParse</param>
         * <param name="surfaceForm">string to use during transition.</param>
         * <returns>true when the currentState is end state and input surfaceForm id equal to currentSurfaceForm, otherwise false.</returns>
         */
        private bool ParseExists(List<FsmParse> fsmParse, string surfaceForm)
        {
            while (fsmParse.Count > 0)
            {
                var currentFsmParse = fsmParse[0];
                fsmParse.RemoveAt(0);
                var root = (TxtWord)currentFsmParse.GetWord();
                var currentState = currentFsmParse.GetFinalSuffix();
                var currentSurfaceForm = currentFsmParse.GetSurfaceForm();
                if (currentState.IsEndState() &&
                    string.Equals(currentSurfaceForm, surfaceForm, StringComparison.Ordinal))
                {
                    return true;
                }

                AddNewParsesFromCurrentParse(currentFsmParse, fsmParse, surfaceForm, root);
            }

            return false;
        }

        /**
         * <summary>The parseWord method is used to parse a given fsmParse. It simply adds new parses to the current parse by
         * using addNewParsesFromCurrentParse method.</summary>
         *
         * <param name="fsmParse">   an ArrayList of FsmParse</param>
         * <param name="maxLength">maximum.Length of the surfaceform.</param>
         * <returns>result {@link ArrayList} which has the currentFsmParse.</returns>
         */
        private List<FsmParse> ParseWord(List<FsmParse> fsmParse, int maxLength)
        {
            var result = new List<FsmParse>();
            var resultSuffixList = new List<string>();
            while (fsmParse.Count > 0)
            {
                var currentFsmParse = fsmParse[0];
                fsmParse.RemoveAt(0);
                var root = (TxtWord)currentFsmParse.GetWord();
                var currentState = currentFsmParse.GetFinalSuffix();
                var currentSurfaceForm = currentFsmParse.GetSurfaceForm();
                if (currentState.IsEndState() && currentSurfaceForm.Length <= maxLength)
                {
                    var currentSuffixList = currentFsmParse.SuffixList();

                    if (!resultSuffixList.Contains(currentSuffixList))
                    {
                        result.Add(currentFsmParse);
                        currentFsmParse.ConstructInflectionalGroups();
                        resultSuffixList.Add(currentSuffixList);
                    }
                }

                AddNewParsesFromCurrentParse(currentFsmParse, fsmParse, maxLength, root);
            }

            return result;
        }

        /**
         * <summary>The parseWord method is used to parse a given fsmParse. It simply adds new parses to the current parse by
         * using addNewParsesFromCurrentParse method.</summary>
         *
         * <param name="fsmParse">   an ArrayList of FsmParse</param>
         * <param name="surfaceForm">string to use during transition.</param>
         * <returns>result {@link ArrayList} which has the currentFsmParse.</returns>
         */
        private List<FsmParse> ParseWord(List<FsmParse> fsmParse, string surfaceForm)
        {
            var result = new List<FsmParse>();
            var resultSuffixList = new List<string>();
            while (fsmParse.Count > 0)
            {
                var currentFsmParse = fsmParse[0];
                fsmParse.RemoveAt(0);
                var root = (TxtWord)currentFsmParse.GetWord();
                var currentState = currentFsmParse.GetFinalSuffix();
                var currentSurfaceForm = currentFsmParse.GetSurfaceForm();
                if (currentState.IsEndState() &&
                    string.Equals(currentSurfaceForm, surfaceForm, StringComparison.Ordinal))
                {
                    var currentSuffixList = currentFsmParse.SuffixList();
                    if (!resultSuffixList.Contains(currentSuffixList))
                    {
                        result.Add(currentFsmParse);
                        currentFsmParse.ConstructInflectionalGroups();
                        resultSuffixList.Add(currentSuffixList);
                    }
                }

                AddNewParsesFromCurrentParse(currentFsmParse, fsmParse, surfaceForm, root);
            }

            return result;
        }

        /**
         * <summary>The morphologicalAnalysis with 3 inputs is used to initialize an {@link ArrayList} and add a new FsmParse
         * with given root and state.</summary>
         *
         * <param name="root">       TxtWord input.</param>
         * <param name="surfaceForm">string input to use for parsing.</param>
         * <param name="state">      string input.</param>
         * <returns>parseWord method with newly populated FsmParse ArrayList and input surfaceForm.</returns>
         */
        public List<FsmParse> MorphologicalAnalysis(TxtWord root, string surfaceForm, string state)
        {
            var initialFsmParse = new List<FsmParse> { new FsmParse(root, _finiteStateMachine.GetState(state)) };
            return ParseWord(initialFsmParse, surfaceForm);
        }

        /**
         * <summary>The generateAllParses with 2 inputs is used to generate all parses with given root. Then it calls initializeParseListFromRoot method to initialize list with newly created ArrayList, input root,
         * and maximum.Length.</summary>
         *
         * <param name="root">       TxtWord input.</param>
         * <param name="maxLength">Maximum.Length of the surface form.</param>
         * <returns>parseWord method with newly populated FsmParse ArrayList and maximum.Length.</returns>
         */
        public List<FsmParse> GenerateAllParses(TxtWord root, int maxLength)
        {
            var initialFsmParse = new List<FsmParse>();
            if (root.IsProperNoun())
            {
                InitializeParseListFromRoot(initialFsmParse, root, true);
            }

            InitializeParseListFromRoot(initialFsmParse, root, false);
            return ParseWord(initialFsmParse, maxLength);
        }

        /**
         * <summary>The morphologicalAnalysis with 2 inputs is used to initialize an {@link ArrayList} and add a new FsmParse
         * with given root. Then it calls initializeParseListFromRoot method to initialize list with newly created ArrayList, input root,
         * and input surfaceForm.</summary>
         *
         * <param name="root">       TxtWord input.</param>
         * <param name="surfaceForm">string input to use for parsing.</param>
         * <returns>parseWord method with newly populated FsmParse ArrayList and input surfaceForm.</returns>
         */
        public List<FsmParse> MorphologicalAnalysis(TxtWord root, string surfaceForm)
        {
            var initialFsmParse = new List<FsmParse>();
            InitializeParseListFromRoot(initialFsmParse, root, IsProperNoun(surfaceForm));
            return ParseWord(initialFsmParse, surfaceForm);
        }

        /**
        * <summary>Replaces previous lemma in the sentence with the new lemma. Both lemma can contain multiple
         * words.</summary>
        * <param name="original"> Original sentence to be replaced with.</param>
        * <param name="previousWord"> Root word in the original sentence</param>
        * <param name="newWord"> New word to be replaced.</param>
        * <returns>Newly generated sentence by replacing the previous word in the original sentence with the new word.</returns>
        */
        public Sentence ReplaceWord(Sentence original, String previousWord, String newWord)
        {
            int i;
            string[] previousWordSplitted = null, newWordSplitted = null;
            var result = new Sentence();
            string replacedWord = null, lastWord, newRootWord;
            var previousWordMultiple = previousWord.Contains(" ");
            var newWordMultiple = newWord.Contains(" ");
            if (previousWordMultiple)
            {
                previousWordSplitted = previousWord.Split(" ");
                lastWord = previousWordSplitted[previousWordSplitted.Length - 1];
            }
            else
            {
                lastWord = previousWord;
            }

            if (newWordMultiple)
            {
                newWordSplitted = newWord.Split(" ");
                newRootWord = newWordSplitted[newWordSplitted.Length - 1];
            }
            else
            {
                newRootWord = newWord;
            }

            var newRootTxtWord = (TxtWord)_dictionary.GetWord(newRootWord);
            var parseList = MorphologicalAnalysis(original);
            for (i = 0; i < parseList.Length; i++)
            {
                var replaced = false;
                for (var j = 0; j < parseList[i].Size(); j++)
                {
                    if (parseList[i].GetFsmParse(j).GetWord().GetName() == lastWord && newRootTxtWord != null)
                    {
                        replaced = true;
                        replacedWord = parseList[i].GetFsmParse(j).ReplaceRootWord(newRootTxtWord);
                    }
                }

                if (replaced && replacedWord != null)
                {
                    if (previousWordMultiple)
                    {
                        for (int k = 0; k < i - previousWordSplitted.Length + 1; k++)
                        {
                            result.AddWord(original.GetWord(k));
                        }
                    }

                    if (newWordMultiple)
                    {
                        for (var k = 0; k < newWordSplitted.Length - 1; k++)
                        {
                            if (result.WordCount() == 0)
                            {
                                result.AddWord(new Word((newWordSplitted[k][0] + "").ToUpper(new CultureInfo("tr")) +
                                                        newWordSplitted[k].Substring(1)));
                            }
                            else
                            {
                                result.AddWord(new Word(newWordSplitted[k]));
                            }
                        }
                    }

                    if (result.WordCount() == 0)
                    {
                        replacedWord = (replacedWord[0] + "").ToUpper(new CultureInfo("tr")) +
                                       replacedWord.Substring(1);
                    }

                    result.AddWord(new Word(replacedWord));
                    if (previousWordMultiple)
                    {
                        i++;
                        break;
                    }
                }
                else
                {
                    if (!previousWordMultiple)
                    {
                        result.AddWord(original.GetWord(i));
                    }
                }
            }

            if (previousWordMultiple)
            {
                for (; i < parseList.Length; i++)
                {
                    result.AddWord(original.GetWord(i));
                }
            }

            return result;
        }

        /**
         * <summary>The analysisExists method checks several cases. If the given surfaceForm is a punctuation or double then it
         * returns true. If it is not a root word, then it initializes the parse list and returns the parseExists method with
         * this newly initialized list and surfaceForm.</summary>
         *
         * <param name="rootWord">   TxtWord root.</param>
         * <param name="surfaceForm">string input.</param>
         * <param name="isProper">   bool variable indicates a word is proper or not.</param>
         * <returns>true if surfaceForm is punctuation or double, otherwise returns parseExist method with given surfaceForm.</returns>
         */
        private bool AnalysisExists(TxtWord rootWord, string surfaceForm, bool isProper)
        {
            List<FsmParse> initialFsmParse;
            if (Word.IsPunctuation(surfaceForm))
            {
                return true;
            }

            if (IsDouble(surfaceForm))
            {
                return true;
            }

            if (rootWord != null)
            {
                initialFsmParse = new List<FsmParse>();
                InitializeParseListFromRoot(initialFsmParse, rootWord, isProper);
            }
            else
            {
                initialFsmParse = InitializeParseListFromSurfaceForm(surfaceForm, isProper);
            }

            return ParseExists(initialFsmParse, surfaceForm);
        }

        /**
         * <summary>The analysis method is used by the morphologicalAnalysis method. It gets string surfaceForm as an input and checks
         * its type such as punctuation, number or compares with the regex for date, fraction, percent, time, range, hashtag,
         * and mail or checks its variable type as integer or double. After finding the right case for given surfaceForm, it calls
         * constructInflectionalGroups method which creates sub-word units.</summary>
         *
         * <param name="surfaceForm">string to analyse.</param>
         * <param name="isProper">   is used to indicate the proper words.</param>
         * <returns>ArrayList type initialFsmParse which holds the analyses.</returns>
         */
        private List<FsmParse> Analysis(string surfaceForm, bool isProper)
        {
            List<FsmParse> initialFsmParse;
            FsmParse fsmParse;
            if (Word.IsPunctuation(surfaceForm) && !surfaceForm.Equals("%"))
            {
                initialFsmParse = new List<FsmParse>(1);
                fsmParse = new FsmParse(surfaceForm, new State(("Punctuation"), true, true));
                fsmParse.ConstructInflectionalGroups();
                initialFsmParse.Add(fsmParse);
                return initialFsmParse;
            }

            if (IsNumber(surfaceForm))
            {
                initialFsmParse = new List<FsmParse>(1);
                fsmParse = new FsmParse(surfaceForm, new State(("CardinalRoot"), true, true));
                fsmParse.ConstructInflectionalGroups();
                initialFsmParse.Add(fsmParse);
                return initialFsmParse;
            }

            if (PatternMatches("\\d+/\\d+", surfaceForm))
            {
                initialFsmParse = new List<FsmParse>(1);
                fsmParse = new FsmParse(surfaceForm, new State(("FractionRoot"), true, true));
                fsmParse.ConstructInflectionalGroups();
                initialFsmParse.Add(fsmParse);
                fsmParse = new FsmParse(surfaceForm, new State(("DateRoot"), true, true));
                fsmParse.ConstructInflectionalGroups();
                initialFsmParse.Add(fsmParse);
                return initialFsmParse;
            }

            if (IsDate(surfaceForm))
            {
                initialFsmParse = new List<FsmParse>(1);
                fsmParse = new FsmParse(surfaceForm, new State(("DateRoot"), true, true));
                fsmParse.ConstructInflectionalGroups();
                initialFsmParse.Add(fsmParse);
                return initialFsmParse;
            }

            if (PatternMatches("\\d+\\\\/\\d+", surfaceForm))
            {
                initialFsmParse = new List<FsmParse>(1);
                fsmParse = new FsmParse(surfaceForm, new State(("FractionRoot"), true, true));
                fsmParse.ConstructInflectionalGroups();
                initialFsmParse.Add(fsmParse);
                return initialFsmParse;
            }

            if (surfaceForm.Equals("%") || IsPercent(surfaceForm))
            {
                initialFsmParse = new List<FsmParse>(1);
                fsmParse = new FsmParse(surfaceForm, new State(("PercentRoot"), true, true));
                fsmParse.ConstructInflectionalGroups();
                initialFsmParse.Add(fsmParse);
                return initialFsmParse;
            }

            if (IsTime(surfaceForm))
            {
                initialFsmParse = new List<FsmParse>(1);
                fsmParse = new FsmParse(surfaceForm, new State(("TimeRoot"), true, true));
                fsmParse.ConstructInflectionalGroups();
                initialFsmParse.Add(fsmParse);
                return initialFsmParse;
            }

            if (IsRange(surfaceForm))
            {
                initialFsmParse = new List<FsmParse>(1);
                fsmParse = new FsmParse(surfaceForm, new State(("RangeRoot"), true, true));
                fsmParse.ConstructInflectionalGroups();
                initialFsmParse.Add(fsmParse);
                return initialFsmParse;
            }

            if (surfaceForm.StartsWith("#"))
            {
                initialFsmParse = new List<FsmParse>(1);
                fsmParse = new FsmParse(surfaceForm, new State(("Hashtag"), true, true));
                fsmParse.ConstructInflectionalGroups();
                initialFsmParse.Add(fsmParse);
                return initialFsmParse;
            }

            if (surfaceForm.Contains("@"))
            {
                initialFsmParse = new List<FsmParse>(1);
                fsmParse = new FsmParse(surfaceForm, new State(("Email"), true, true));
                fsmParse.ConstructInflectionalGroups();
                initialFsmParse.Add(fsmParse);
                return initialFsmParse;
            }

            if (surfaceForm.EndsWith(".") && IsInteger(surfaceForm.Substring(0, surfaceForm.Length - 1)))
            {
                initialFsmParse = new List<FsmParse>(1);
                fsmParse = new FsmParse(int.Parse(surfaceForm.Substring(0, surfaceForm.Length - 1)),
                    _finiteStateMachine.GetState("OrdinalRoot"));
                fsmParse.ConstructInflectionalGroups();
                initialFsmParse.Add(fsmParse);
                return initialFsmParse;
            }

            if (IsInteger(surfaceForm))
            {
                initialFsmParse = new List<FsmParse>(1);
                fsmParse = new FsmParse(int.Parse(surfaceForm), _finiteStateMachine.GetState("CardinalRoot"));
                fsmParse.ConstructInflectionalGroups();
                initialFsmParse.Add(fsmParse);
                return initialFsmParse;
            }

            if (IsDouble(surfaceForm))
            {
                initialFsmParse = new List<FsmParse>(1);
                fsmParse = new FsmParse(Double.Parse(surfaceForm), _finiteStateMachine.GetState("RealRoot"));
                fsmParse.ConstructInflectionalGroups();
                initialFsmParse.Add(fsmParse);
                return initialFsmParse;
            }

            initialFsmParse = InitializeParseListFromSurfaceForm(surfaceForm, isProper);
            return ParseWord(initialFsmParse, surfaceForm);
        }

        /// <summary>
        /// This method uses cache idea to speed up pattern matching in Fsm. mostUsedPatterns stores the compiled forms of
        /// the previously used patterns. When Fsm tries to match a string to a pattern, first we check if it exists in
        /// mostUsedPatterns. If it exists, we directly use the compiled pattern to match the string. Otherwise, new pattern
        /// is compiled and put in the mostUsedPatterns.
        /// </summary>
        /// <param name="expr">Pattern to check</param>
        /// <param name="value">String to match the pattern</param>
        /// <returns>True if the string matches the pattern, false otherwise.</returns>
        private bool PatternMatches(string expr, string value)
        {
            Regex p;
            if (_mostUsedPatterns.ContainsKey(expr))
            {
                p = _mostUsedPatterns[expr];
            }
            else
            {
                p = new Regex("^" + expr + "$");
                _mostUsedPatterns[expr] = p;
            }

            return p.IsMatch(value);
        }

        /**
         * <summary>The isProperNoun method takes surfaceForm string as input and checks its each char whether they are in the range
         * of letters between A to Z or one of the Turkish letters such as İ, Ü, Ğ, Ş, Ç, and Ö.</summary>
         *
         * <param name="surfaceForm">string to check for proper noun.</param>
         * <returns>false if surfaceForm is null or.Length of 0, return true if it is a letter.</returns>
         */
        public bool IsProperNoun(string surfaceForm)
        {
            if (string.IsNullOrEmpty(surfaceForm))
            {
                return false;
            }

            return (surfaceForm[0] >= 'A' && surfaceForm[0] <= 'Z') ||
                   surfaceForm[0] == '\u0130' || surfaceForm[0] == '\u00dc' ||
                   surfaceForm[0] == '\u011e' || surfaceForm[0] == '\u015e' ||
                   surfaceForm[0] == '\u00c7' || surfaceForm[0] == '\u00d6'; // İ, Ü, Ğ, Ş, Ç, Ö
        }

        /**
        * <summary>The isCode method takes surfaceForm string as input and checks if it consists of both letters and numbers.</summary>
        *
        * <param name="surfaceForm"> string to check for code-like word.</param>
        * <returns> true if it is a code-like word, return false otherwise.</returns>
        */
        public bool IsCode(string surfaceForm) {
            if (string.IsNullOrEmpty(surfaceForm)) {
                return false;
            }
            return PatternMatches(".*[0-9].*", surfaceForm) && PatternMatches(".*[a-zA-ZçöğüşıÇÖĞÜŞİ].*", surfaceForm);
        }
        
        /// <summary>
        /// Identifies a possible new root word for a given surface form. It also adds the new root form to the dictionary
        /// for further usage. The method first searches the suffix trie for the reverse string of the surface form. This
        /// way, it can identify if the word has a suffix that is in the most frequently used suffix list. Since a word can
        /// have multiple possible suffixes, the method identifies the longest suffix and returns the substring of the
        /// surface form tht does not contain the suffix. Let say the word is 'googlelaştırdık', it will identify 'tık' as
        /// a suffix and will return 'googlelaştır' as a possible root form. Another example will be 'homelesslerimizle', it
        /// will identify 'lerimizle' as suffix and will return 'homeless' as a possible root form. If the root word ends
        /// with 'ğ', it is replacesd with 'k'. 'morfolojikliğini' will return 'morfolojikliğ' then which will be replaced
        /// with 'morfolojiklik'.
        /// </summary>
        /// <param name="surfaceForm">Surface form for which we will identify a possible new root form.</param>
        /// <returns>Possible new root form.</returns>
        private List<TxtWord> RootOfPossiblyNewWord(string surfaceForm){
            var words = _suffixTrie.GetWordsWithPrefix(ReverseString(surfaceForm));
            var candidateList = new List<TxtWord>();
            foreach (var word in words){
                var candidateWord = surfaceForm.Substring(0, surfaceForm.Length - word.GetName().Length);
                TxtWord newWord;
                if (candidateWord.EndsWith("ğ")){
                    candidateWord = candidateWord.Substring(0, candidateWord.Length - 1) + "k";
                    newWord = new TxtWord(candidateWord, "CL_ISIM");
                    newWord.AddFlag("IS_SD");
                } else {
                    newWord = new TxtWord(candidateWord, "CL_ISIM");
                    newWord.AddFlag("CL_FIIL");
                }
                candidateList.Add(newWord);
                _dictionaryTrie.AddWord(candidateWord, newWord);
            }
            return candidateList;
        } 

        /**
         * <summary>The robustMorphologicalAnalysis is used to analyse surfaceForm string. First it gets the currentParse of the surfaceForm
         * then, if the size of the currentParse is 0, and given surfaceForm is a proper noun, it adds the surfaceForm
         * whose state name is ProperRoot to an {@link ArrayList}, if it is a code-like word, it adds the surfaceForm
         * whose state name is CodeRoot to the {@link ArrayList} and if it is neither, it adds the surfaceForm
         * whose state name is NominalRoot to the {@link ArrayList}.</summary>
         *
         * <param name="surfaceForm">string to analyse.</param>
         * <returns>FsmParseList type currentParse which holds morphological analysis of the surfaceForm.</returns>
         */
        public FsmParseList RobustMorphologicalAnalysis(string surfaceForm)
        {
            if (string.IsNullOrEmpty(surfaceForm))
            {
                return new FsmParseList(new List<FsmParse>());
            }

            var currentParse = MorphologicalAnalysis(surfaceForm);
            if (currentParse.Size() == 0)
            {
                var fsmParse = new List<FsmParse>(1);
                if (IsProperNoun(surfaceForm))
                {
                    fsmParse.Add(new FsmParse(surfaceForm, _finiteStateMachine.GetState("ProperRoot")));
                }
                if (IsCode(surfaceForm))
                {
                    fsmParse.Add(new FsmParse(surfaceForm, _finiteStateMachine.GetState("CodeRoot")));
                }
                var newCandidateList = RootOfPossiblyNewWord(surfaceForm);
                if (newCandidateList.Count != 0)
                {
                    foreach (var word in newCandidateList)
                    {
                        fsmParse.Add(new FsmParse(word, _finiteStateMachine.GetState("VerbalRoot")));
                        fsmParse.Add(new FsmParse(word, _finiteStateMachine.GetState("NominalRoot")));
                    }
                }
                fsmParse.Add(new FsmParse(surfaceForm, _finiteStateMachine.GetState("NominalRoot")));
                return new FsmParseList(ParseWord(fsmParse, surfaceForm));
            }
            else
            {
                return currentParse;
            }
        }

        /**
         * <summary>The morphologicalAnalysis is used for debug purposes.</summary>
         *
         * <param name="sentence"> to get word from.</param>
         * <returns>FsmParseList type result.</returns>
         */
        public FsmParseList[] MorphologicalAnalysis(Sentence sentence)
        {
            var result = new FsmParseList[sentence.WordCount()];
            for (var i = 0; i < sentence.WordCount(); i++)
            {
                var originalForm = sentence.GetWord(i).GetName();
                var spellCorrectedForm = _dictionary.GetCorrectForm(originalForm);
                if (spellCorrectedForm == null)
                {
                    spellCorrectedForm = originalForm;
                }

                var wordFsmParseList = MorphologicalAnalysis(spellCorrectedForm);
                result[i] = wordFsmParseList;
            }

            return result;
        }


        /**
         * <summary>The robustMorphologicalAnalysis method takes just one argument as an input. It gets the name of the words from
         * input sentence then calls robustMorphologicalAnalysis with surfaceForm.</summary>
         *
         * <param name="sentence">Sentence type input used to get surfaceForm.</param>
         * <returns>FsmParseList array which holds the result of the analysis.</returns>
         */
        public FsmParseList[] RobustMorphologicalAnalysis(Sentence sentence)
        {
            var result = new FsmParseList[sentence.WordCount()];
            for (var i = 0; i < sentence.WordCount(); i++)
            {
                var originalForm = sentence.GetWord(i).GetName();
                var spellCorrectedForm = _dictionary.GetCorrectForm(originalForm);
                if (spellCorrectedForm == null)
                {
                    spellCorrectedForm = originalForm;
                }

                var fsmParseList = RobustMorphologicalAnalysis(spellCorrectedForm);
                result[i] = fsmParseList;
            }

            return result;
        }

        /**
         * <summary>The isInteger method compares input surfaceForm with regex [-+]?\d+ and returns the result.
         * Supports positive integer checks only.</summary>
         *
         * <param name="surfaceForm">string to check.</param>
         * <returns>true if surfaceForm matches with the regex.</returns>
         */
        private bool IsInteger(string surfaceForm)
        {
            if (!PatternMatches("[-+]?\\d+", surfaceForm))
                return false;
            var len = surfaceForm.Length;
            if (len < 10)
            {
                return true;
            }

            if (len > 10)
            {
                return false;
            }

            return surfaceForm.CompareTo("2147483647") <= 0;
        }

        /**
         * <summary>The isDouble method compares input surfaceForm with regex ([-+]?\\d+\\.\\d+)|(\\d*\\.\\d+) and returns the result.</summary>
         *
         * <param name="surfaceForm">string to check.</param>
         * <returns>true if surfaceForm matches with the regex.</returns>
         */
        private bool IsDouble(string surfaceForm)
        {
            return PatternMatches("([-+]?\\d+\\.\\d+)|(\\d*\\.\\d+)", surfaceForm);
        }

        /**
         * <summary>The isNumber method compares input surfaceForm with the array of written numbers and returns the result.</summary>
         *
         * <param name="surfaceForm">string to check.</param>
         * <returns>true if surfaceForm matches with the regex.</returns>
         */
        private bool IsNumber(string surfaceForm)
        {
            var count = 0;
            string[] numbers =
            {
                "bir", "iki", "üç", "dört", "beş", "altı", "yedi", "sekiz", "dokuz",
                "on", "yirmi", "otuz", "kırk", "elli", "altmış", "yetmiş", "seksen", "doksan",
                "yüz", "bin", "milyon", "milyar", "trilyon", "katrilyon"
            };
            string word = surfaceForm;
            while (word != "")
            {
                var found = false;
                foreach (var number in numbers)
                {
                    if (word.StartsWith(number))
                    {
                        found = true;
                        count++;
                        word = word.Substring(number.Length);
                        break;
                    }
                }

                if (!found)
                {
                    break;
                }
            }

            return word == "" && count > 1;
        }

        /// <summary>
        /// Checks if a given surface form matches to a percent value. It should be something like %4, %45, %4.3 or %56.786
        /// </summary>
        /// <param name="surfaceForm">Surface form to be checked.</param>
        /// <returns>True if the surface form is in percent form</returns>
        private bool IsPercent(string surfaceForm)
        {
            return PatternMatches("%(\\d\\d|\\d)", surfaceForm) || PatternMatches("%(\\d\\d|\\d)\\.\\d+", surfaceForm);
        }

        /// <summary>
        /// Checks if a given surface form matches to a time form. It should be something like 3:34, 12:56 etc.
        /// </summary>
        /// <param name="surfaceForm">Surface form to be checked.</param>
        /// <returns>True if the surface form is in time form</returns>
        private bool IsTime(string surfaceForm)
        {
            return PatternMatches("(\\d\\d|\\d):(\\d\\d|\\d):(\\d\\d|\\d)", surfaceForm) ||
                   PatternMatches("(\\d\\d|\\d):(\\d\\d|\\d)", surfaceForm);
        }

        /// <summary>
        /// Checks if a given surface form matches to a range form. It should be something like 123-1400 or 12:34-15:78 or
        /// 3.45-4.67.
        /// </summary>
        /// <param name="surfaceForm">Surface form to be checked.</param>
        /// <returns>True if the surface form is in range form</returns>
        private bool IsRange(string surfaceForm)
        {
            return PatternMatches("\\d+-\\d+", surfaceForm) ||
                   PatternMatches("(\\d\\d|\\d):(\\d\\d|\\d)-(\\d\\d|\\d):(\\d\\d|\\d)", surfaceForm) ||
                   PatternMatches("(\\d\\d|\\d)\\.(\\d\\d|\\d)-(\\d\\d|\\d)\\.(\\d\\d|\\d)", surfaceForm);
        }

        /// <summary>
        /// Checks if a given surface form matches to a date form. It should be something like 3/10/2023 or 2.3.2012
        /// </summary>
        /// <param name="surfaceForm">Surface form to be checked.</param>
        /// <returns>True if the surface form is in date form</returns>
        private bool IsDate(string surfaceForm)
        {
            return PatternMatches("(\\d\\d|\\d)/(\\d\\d|\\d)/\\d+", surfaceForm) ||
                   PatternMatches("(\\d\\d|\\d)\\.(\\d\\d|\\d)\\.\\d+", surfaceForm);
        }

        /**
         * <summary>The morphologicalAnalysis method is used to analyse a FsmParseList by comparing with the regex.
         * It creates an {@link ArrayList} fsmParse to hold the result of the analysis method. For each surfaceForm input,
         * it gets a substring and considers it as a possibleRoot. Then compares with the regex.
         * <p/>
         * If the surfaceForm input string matches with Turkish chars like Ç, Ş, İ, Ü, Ö, it adds the surfaceForm to Trie with IS_OA tag.
         * If the possibleRoot contains /, then it is added to the Trie with IS_KESIR tag.
         * If the possibleRoot contains \d\d|\d)/(\d\d|\d)/\d+, then it is added to the Trie with IS_DATE tag.
         * If the possibleRoot contains \\d\d|\d, then it is added to the Trie with IS_PERCENT tag.
         * If the possibleRoot contains \d\d|\d):(\d\d|\d):(\d\d|\d), then it is added to the Trie with IS_ZAMAN tag.
         * If the possibleRoot contains \d+-\d+, then it is added to the Trie with IS_RANGE tag.
         * If the possibleRoot is an Integer, then it is added to the Trie with IS_SAYI tag.
         * If the possibleRoot is a Double, then it is added to the Trie with IS_REELSAYI tag.</summary>
         *
         * <param name="surfaceForm">string to analyse.</param>
         * <returns>fsmParseList which holds the analysis.</returns>
         */
        public FsmParseList MorphologicalAnalysis(string surfaceForm)
        {
            FsmParseList fsmParseList;
            var lowerCased = surfaceForm.ToLower(new CultureInfo("tr"));
            if (parsedSurfaceForms != null &&
                parsedSurfaceForms.ContainsKey(lowerCased) && !IsInteger(surfaceForm) &&
                !IsDouble(surfaceForm) && !IsPercent(surfaceForm) && !IsTime(surfaceForm) && !IsRange(surfaceForm) &&
                !IsDate(surfaceForm))
            {
                var parses = new List<FsmParse>();
                parses.Add(new FsmParse(new Word(lowerCased)));
                return new FsmParseList(parses);
            }

            if (_cache != null && _cache.Contains(surfaceForm))
            {
                return _cache.Get(surfaceForm);
            }

            if (PatternMatches("(\\w|Ç|Ş|İ|Ü|Ö)\\.", surfaceForm))
            {
                _dictionaryTrie.AddWord(surfaceForm.ToLower(new CultureInfo("tr")),
                    new TxtWord(surfaceForm.ToLower(new CultureInfo("tr")), "IS_OA"));
            }

            var defaultFsmParse =
                Analysis(surfaceForm.ToLower(new CultureInfo("tr")), IsProperNoun(surfaceForm));
            if (defaultFsmParse.Count > 0)
            {
                fsmParseList = new FsmParseList(defaultFsmParse);
                _cache?.Add(surfaceForm, fsmParseList);

                return fsmParseList;
            }

            var fsmParse = new List<FsmParse>();
            if (surfaceForm.Contains("'"))
            {
                var possibleRoot = surfaceForm.Substring(0, surfaceForm.IndexOf('\''));
                if (possibleRoot != "")
                {
                    if (possibleRoot.Contains("/") || possibleRoot.Contains("\\/"))
                    {
                        _dictionaryTrie.AddWord(possibleRoot, new TxtWord(possibleRoot, "IS_KESIR"));
                        fsmParse = Analysis(surfaceForm.ToLower(new CultureInfo("tr")), IsProperNoun(surfaceForm));
                    }
                    else
                    {
                        if (IsDate(possibleRoot))
                        {
                            _dictionaryTrie.AddWord(possibleRoot, new TxtWord(possibleRoot, "IS_DATE"));
                            fsmParse = Analysis(surfaceForm.ToLower(new CultureInfo("tr")), IsProperNoun(surfaceForm));
                        }
                        else
                        {
                            if (PatternMatches("\\d+/\\d+", possibleRoot))
                            {
                                _dictionaryTrie.AddWord(possibleRoot, new TxtWord(possibleRoot, "IS_KESIR"));
                                fsmParse = Analysis(surfaceForm.ToLower(new CultureInfo("tr")),
                                    IsProperNoun(surfaceForm));
                            }
                            else
                            {
                                if (IsPercent(possibleRoot))
                                {
                                    _dictionaryTrie.AddWord(possibleRoot, new TxtWord(possibleRoot, "IS_PERCENT"));
                                    fsmParse = Analysis(surfaceForm.ToLower(new CultureInfo("tr")),
                                        IsProperNoun(surfaceForm));
                                }
                                else
                                {
                                    if (IsTime(surfaceForm))
                                    {
                                        _dictionaryTrie.AddWord(possibleRoot, new TxtWord(possibleRoot, "IS_ZAMAN"));
                                        fsmParse = Analysis(surfaceForm.ToLower(new CultureInfo("tr")),
                                            IsProperNoun(surfaceForm));
                                    }
                                    else
                                    {
                                        if (IsRange(surfaceForm))
                                        {
                                            _dictionaryTrie.AddWord(possibleRoot,
                                                new TxtWord(possibleRoot, "IS_RANGE"));
                                            fsmParse = Analysis(surfaceForm.ToLower(new CultureInfo("tr")),
                                                IsProperNoun(surfaceForm));
                                        }
                                        else
                                        {
                                            if (IsInteger(possibleRoot))
                                            {
                                                _dictionaryTrie.AddWord(possibleRoot,
                                                    new TxtWord(possibleRoot, "IS_SAYI"));
                                                fsmParse = Analysis(surfaceForm.ToLower(new CultureInfo("tr")),
                                                    IsProperNoun(surfaceForm));
                                            }
                                            else
                                            {
                                                if (IsDouble(possibleRoot))
                                                {
                                                    _dictionaryTrie.AddWord(possibleRoot,
                                                        new TxtWord(possibleRoot, "IS_REELSAYI"));
                                                    fsmParse = Analysis(surfaceForm.ToLower(new CultureInfo("tr")),
                                                        IsProperNoun(surfaceForm));
                                                }
                                                else
                                                {
                                                    if (Word.IsCapital(possibleRoot))
                                                    {
                                                        TxtWord newWord = null;
                                                        if (_dictionary.GetWord(
                                                                possibleRoot.ToLower(new CultureInfo("tr"))) != null)
                                                        {
                                                            ((TxtWord)_dictionary.GetWord(
                                                                    possibleRoot.ToLower(new CultureInfo("tr"))))
                                                                .AddFlag("IS_OA");
                                                        }
                                                        else
                                                        {
                                                            newWord = new TxtWord(
                                                                possibleRoot.ToLower(new CultureInfo("tr")), "IS_OA");
                                                            _dictionaryTrie.AddWord(
                                                                possibleRoot.ToLower(new CultureInfo("tr")), newWord);
                                                        }

                                                        fsmParse = Analysis(surfaceForm.ToLower(new CultureInfo("tr")),
                                                            IsProperNoun(surfaceForm));
                                                        if (fsmParse.Count == 0 && newWord != null)
                                                        {
                                                            newWord.AddFlag("IS_KIS");
                                                            fsmParse = Analysis(
                                                                surfaceForm.ToLower(new CultureInfo("tr")),
                                                                IsProperNoun(surfaceForm));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            fsmParseList = new FsmParseList(fsmParse);
            if (fsmParseList.Size() > 0)
            {
                _cache?.Add(surfaceForm, fsmParseList);
            }

            return fsmParseList;
        }

        /**
         * <summary>The morphologicalAnalysisExists method calls analysisExists to check the existence of the analysis with given
         * root and surfaceForm.</summary>
         *
         * <param name="surfaceForm">string to check.</param>
         * <param name="rootWord">   TxtWord input root.</param>
         * <returns>true an analysis exists, otherwise return false.</returns>
         */
        public bool MorphologicalAnalysisExists(TxtWord rootWord, string surfaceForm)
        {
            return AnalysisExists(rootWord, surfaceForm.ToLower(new CultureInfo("tr")), true);
        }
    }
}