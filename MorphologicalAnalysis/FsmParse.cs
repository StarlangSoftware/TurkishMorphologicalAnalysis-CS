using System;
using System.Collections.Generic;
using Dictionary.Dictionary;

namespace MorphologicalAnalysis
{
    public class FsmParse : MorphologicalParse, ICloneable, IComparable
    {
        private List<State> _suffixList;
        private List<string> _formList;
        private List<string> _transitionList;
        private List<string> _withList;
        private string _initialPos;
        private string _pos;
        private string _form;
        private string _verbAgreement;
        private string _possessiveAgreement;

        /**
         * <summary>A constructor of {@link FsmParse} class which takes a {@link Word} as an input and assigns it to root variable.</summary>
         *
         * <param name="root">{@link Word} input.</param>
         */
        public FsmParse(Word root)
        {
            this.root = root;
        }

        /**
         * <summary>Another constructor of {@link FsmParse} class which takes an {@link Integer} number and a {@link State} as inputs.
         * First, it creates a {@link TxtWord} with given number and adds flag to this number as IS_SAYI and initializes root variable with
         * number {@link TxtWord}. It also initializes form with root's name, pos and initialPos with given {@link State}'s POS, creates 4 new
         * {@link ArrayList} suffixList, formList, transitionList and withList and adds given {@link State} to suffixList, form to
         * formList.</summary>
         *
         * <param name="number">    {@link Integer} input.</param>
         * <param name="startState">{@link State} input.</param>
         */
        public FsmParse(int number, State startState)
        {
            var num = new TxtWord("" + number);
            num.AddFlag("IS_SAYI");
            this.root = num;
            this._form = root.GetName();
            this._pos = startState.GetPos();
            this._initialPos = startState.GetPos();
            _suffixList = new List<State> { startState };
            _formList = new List<string> { this._form };
            _transitionList = new List<string>();
            _withList = new List<string>();
        }

        /**
         * <summary>Another constructor of {@link FsmParse} class which takes a {@link Double} number and a {@link State} as inputs.
         * First, it creates a {@link TxtWord} with given number and adds flag to this number as IS_SAYI and initializes root variable with
         * number {@link TxtWord}. It also initializes form with root's name, pos and initialPos with given {@link State}'s POS, creates 4 new
         * {@link ArrayList} suffixList, formList, transitionList and withList and adds given {@link State} to suffixList, form to
         * formList.</summary>
         *
         * <param name="number">    {@link Double} input.</param>
         * <param name="startState">{@link State} input.</param>
         */
        public FsmParse(double number, State startState)
        {
            TxtWord num = new TxtWord("" + number);
            num.AddFlag("IS_SAYI");
            this.root = num;
            this._form = root.GetName();
            this._pos = startState.GetPos();
            this._initialPos = startState.GetPos();
            _suffixList = new List<State> { startState };
            _formList = new List<string> { this._form };
            _transitionList = new List<string>();
            _withList = new List<string>();
        }

        /**
         * <summary>Another constructor of {@link FsmParse} class which takes a {@link string} punctuation and a {@link State} as inputs.
         * First, it creates a {@link TxtWord} with given punctuation and initializes root variable with this {@link TxtWord}.
         * It also initializes form with root's name, pos and initialPos with given {@link State}'s POS, creates 4 new
         * {@link ArrayList} suffixList, formList, transitionList and withList and adds given {@link State} to suffixList, form to
         * formList.</summary>
         *
         * <param name="punctuation">{@link string} input.</param>
         * <param name="startState"> {@link State} input.</param>
         */
        public FsmParse(string punctuation, State startState)
        {
            this.root = new TxtWord(punctuation);
            this._form = root.GetName();
            this._pos = startState.GetPos();
            this._initialPos = startState.GetPos();
            _suffixList = new List<State> { startState };
            _formList = new List<string> { this._form };
            _transitionList = new List<string>();
            _withList = new List<string>();
        }

        /**
         * <summary>Another constructor of {@link FsmParse} class which takes a {@link TxtWord} root and a {@link State} as inputs.
         * First, initializes root variable with this {@link TxtWord}. It also initializes form with root's name, pos and
         * initialPos with given {@link State}'s POS, creates 4 new {@link ArrayList} suffixList, formList, transitionList
         * and withList and adds given {@link State} to suffixList, form to formList.</summary>
         *
         * <param name="root">      {@link TxtWord} input.</param>
         * <param name="startState">{@link State} input.</param>
         */
        public FsmParse(TxtWord root, State startState)
        {
            this.root = root;
            this._form = root.GetName();
            this._pos = startState.GetPos();
            this._initialPos = startState.GetPos();
            _suffixList = new List<State> { startState };
            _formList = new List<string> { this._form };
            _transitionList = new List<string>();
            _withList = new List<string>();
        }

        /**
         * <summary>The constructInflectionalGroups method initially calls the transitionList method and assigns the resulting {@link string}
         * to the parse variable and creates a new {@link ArrayList} as iGs. If parse {@link string} contains a derivational boundary
         * it adds the substring starting from the 0 to the index of derivational boundary to the iGs. If it does not contain a DB,
         * it directly adds parse to the iGs. Then, creates and initializes new {@link ArrayList} as inflectionalGroups and fills with
         * the items of iGs.</summary>
         */
        public void ConstructInflectionalGroups()
        {
            var parse = TransitionList();
            int i;
            var iGs = new List<string>();
            while (parse.Contains("^DB+"))
            {
                iGs.Add(parse.Substring(0, parse.IndexOf("^DB+")));
                parse = parse.Substring(parse.IndexOf("^DB+") + 4);
            }

            iGs.Add(parse);
            inflectionalGroups = new List<InflectionalGroup>
            {
                new InflectionalGroup(iGs[0].Substring(iGs[0].IndexOf('+') + 1))
            };
            for (i = 1; i < iGs.Count; i++)
            {
                inflectionalGroups.Add(new InflectionalGroup(iGs[i]));
            }
        }

        /**
         * <summary>Getter for the verbAgreement variable.</summary>
         *
         * <returns>the verbAgreement variable.</returns>
         */
        public string GetVerbAgreement()
        {
            return _verbAgreement;
        }

        /**
         * <summary>Getter for the getPossessiveAgreement variable.</summary>
         *
         * <returns>the possessiveAgreement variable.</returns>
         */
        public string GetPossessiveAgreement()
        {
            return _possessiveAgreement;
        }

        /**
         * <summary>The setAgreement method takes a {@link string} transitionName as an input and if it is one of the A1SG, A2SG, A3SG,
         * A1PL, A2PL or A3PL it assigns transitionName input to the verbAgreement variable. Or if it is ine of the PNON, P1SG, P2SG,P3SG,
         * P1PL, P2PL or P3PL it assigns transitionName input to the possessiveAgreement variable.</summary>
         *
         * <param name="transitionName">{@link string} input.</param>
         */
        public void SetAgreement(string transitionName)
        {
            if (transitionName != null && (transitionName == "A1SG" ||
                                           transitionName == "A2SG" ||
                                           transitionName == "A3SG" ||
                                           transitionName == "A1PL" ||
                                           transitionName == "A2PL" ||
                                           transitionName == "A3PL"))
            {
                this._verbAgreement = transitionName;
            }

            if (transitionName != null && (transitionName == "PNON" ||
                                           transitionName == "P1SG" ||
                                           transitionName == "P2SG" ||
                                           transitionName == "P3SG" ||
                                           transitionName == "P1PL" ||
                                           transitionName == "P2PL" ||
                                           transitionName == "P3PL"))
            {
                this._possessiveAgreement = transitionName;
            }
        }

        /**
         * <summary>The getLastLemmaWithTag method takes a string input pos as an input. If given pos is an initial pos then it assigns
         * root to the lemma, and assign null otherwise.  Then, it loops i times where i ranges from 1 to size of the formList,
         * if the item at i-1 of transitionList is not null and contains a derivational boundary with pos but not with ZERO,
         * it assigns the ith item of formList to lemma.</summary>
         *
         * <param name="pos">{@link string} input.</param>
         * <returns>string output lemma.</returns>
         */
        public string GetLastLemmaWithTag(string pos)
        {
            string lemma;
            if (_initialPos != null && _initialPos == pos)
            {
                lemma = root.GetName();
            }
            else
            {
                lemma = null;
            }

            for (var i = 1; i < _formList.Count; i++)
            {
                if (_transitionList[i - 1] != null && _transitionList[i - 1].Contains("^DB+" + pos) &&
                    !_transitionList[i - 1].Contains("^DB+" + pos + "+ZERO"))
                {
                    lemma = _formList[i];
                }
            }

            return lemma;
        }

        /**
         * <summary>The getLastLemma method initially assigns root as lemma. Then, it loops i times where i ranges from 1 to size of the formList,
         * if the item at i-1 of transitionList is not null and contains a derivational boundary, it assigns the ith item of formList to lemma.</summary>
         *
         * <returns>string output lemma.</returns>
         */
        public string GetLastLemma()
        {
            var lemma = root.GetName();
            for (var i = 1; i < _formList.Count; i++)
            {
                if (_transitionList[i - 1] != null && _transitionList[i - 1].Contains("^DB+"))
                {
                    lemma = _formList[i];
                }
            }

            return lemma;
        }

        /**
         * <summary>The addSuffix method takes 5 different inputs; {@link State} suffix, {@link string} form, transition, with and toPos.
         * If the pos of given input suffix is not null, it then assigns it to the pos variable. If the pos of the given suffix
         * is null but given toPos is not null than it assigns toPos to pos variable. At the end, it adds suffix to the suffixList,
         * form to the formList, transition to the transitionList and if given with is not 0, it is also added to withList.</summary>
         *
         * <param name="suffix">    {@link State} input.</param>
         * <param name="form">      {@link string} input.</param>
         * <param name="transition">{@link string} input.</param>
         * <param name="with">      {@link string} input.</param>
         * <param name="toPos">     {@link string} input.</param>
         */
        public void AddSuffix(State suffix, string form, string transition, string with, string toPos)
        {
            if (suffix.GetPos() != null)
            {
                _pos = suffix.GetPos();
            }
            else
            {
                if (toPos != null)
                {
                    _pos = toPos;
                }
            }

            _suffixList.Add(suffix);
            _formList.Add(form);
            _transitionList.Add(transition);
            if (with != "0")
            {
                _withList.Add(with);
            }

            this._form = form;
        }

        /**
         * <summary>Getter for the form variable.</summary>
         *
         * <returns>the form variable.</returns>
         */
        public string GetSurfaceForm()
        {
            return _form;
        }

        /**
         * <summary>The getStartState method returns the first item of suffixList {@link ArrayList}.</summary>
         *
         * <returns>the first item of suffixList {@link ArrayList}.</returns>
         */
        public State GetStartState()
        {
            return _suffixList[0];
        }

        /**
         * <summary>Getter for the pos variable.</summary>
         *
         * <returns>the pos variable.</returns>
         */
        public string GetFinalPos()
        {
            return _pos;
        }

        /**
         * <summary>Getter for the initialPos variable.</summary>
         *
         * <returns>the initialPos variable.</returns>
         */
        public string GetInitialPos()
        {
            return _initialPos;
        }

        /**
         * <summary>The setForm method takes a {@link string} name as an input and assigns it to the form variable, then it removes the first item
         * of formList {@link ArrayList} and adds the given name to the formList.</summary>
         *
         * <param name="name">string input to set form.</param>
         */
        public void SetForm(string name)
        {
            _form = name;
            _formList.RemoveAt(0);
            _formList.Add(name);
        }

        /**
         * <summary>The getFinalSuffix method returns the last item of suffixList {@link ArrayList}.</summary>
         *
         * <returns>the last item of suffixList {@link ArrayList}.</returns>
         */
        public State GetFinalSuffix()
        {
            return _suffixList[_suffixList.Count - 1];
        }

        /**
         * <summary>The overridden clone method creates a new {@link FsmParse} abject with root variable and initializes variables form, pos,
         * initialPos, verbAgreement, possessiveAgreement, and also the {@link ArrayList}s suffixList, formList, transitionList and withList.
         * Then returns newly created and cloned {@link FsmParse} object.</summary>
         *
         * <returns>FsmParse object.</returns>
         */
        public object Clone()
        {
            int i;
            var p = new FsmParse(root)
            {
                _form = _form,
                _pos = _pos,
                _initialPos = _initialPos,
                _verbAgreement = _verbAgreement,
                _possessiveAgreement = _possessiveAgreement,
                _suffixList = new List<State>()
            };
            for (i = 0; i < _suffixList.Count; i++)
            {
                p._suffixList.Add(_suffixList[i]);
            }

            p._formList = new List<string>();
            for (i = 0; i < _formList.Count; i++)
            {
                p._formList.Add(_formList[i]);
            }

            p._transitionList = new List<string>();
            for (i = 0; i < _transitionList.Count; i++)
            {
                p._transitionList.Add(_transitionList[i]);
            }

            p._withList = new List<string>();
            for (i = 0; i < _withList.Count; i++)
            {
                p._withList.Add(_withList[i]);
            }

            return p;
        }

        /**
         * <summary>The headerTransition method gets the first item of formList and checks for cases;
         * <p>
         * If it is &lt;DOC&gt;, it returns &lt;DOC&gt;+BDTAG which indicates the beginning of a document.
         * If it is &lt;/DOC&gt;, it returns &lt;/DOC&gt;+EDTAG which indicates the ending of a document.
         * If it is &lt;TITLE&gt;, it returns &lt;TITLE&gt;+BTTAG which indicates the beginning of a title.
         * If it is &lt;/TITLE&gt;, it returns &lt;/TITLE&gt;+ETTAG which indicates the ending of a title.
         * If it is &lt;S&gt;, it returns &lt;S&gt;+BSTAG which indicates the beginning of a sentence.
         * If it is &lt;/S&gt;, it returns &lt;/S&gt;+ESTAG which indicates the ending of a sentence.</p></summary>
         *
         * <returns>corresponding tags of the headers and an empty {@link string} if any case does not match.</returns>
         */
        public string HeaderTransition()
        {
            switch (_formList[0])
            {
                case "<DOC>":
                    return "<DOC>+BDTAG";
                case "</DOC>":
                    return "</DOC>+EDTAG";
                case "<TITLE>":
                    return "<TITLE>+BTTAG";
                case "</TITLE>":
                    return "</TITLE>+ETTAG";
                case "<S>":
                    return "<S>+BSTAG";
                case "</S>":
                    return "</S>+ESTAG";
                default:
                    return "";
            }
        }

        /**
         * <summary>The pronounTransition method gets the first item of formList and checks for cases;
         * <p>
         * If it is "kendi", it returns kendi+PRON+REFLEXP which indicates a reflexive pronoun.
         * If it is one of the "hep, öbür, topu, öteki, kimse, hiçbiri, tümü, çoğu, hepsi, herkes, başkası, birçoğu, birçokları, biri, birbirleri, birbiri, birkaçı, böylesi, diğeri, cümlesi, bazı, kimi", it returns
         * +PRON+QUANTP which indicates a quantitative pronoun.
         * If it is one of the "o, bu, şu" and if it is "o" it also checks the first item of suffixList and if it is a PronounRoot(DEMONS),
         * it returns +PRON+DEMONSP which indicates a demonstrative pronoun.
         * If it is "ben", it returns +PRON+PERS+A1SG+PNON which indicates a 1st person singular agreement.
         * If it is "sen", it returns +PRON+PERS+A2SG+PNON which indicates a 2nd person singular agreement.
         * If it is "o" and the first item of suffixList, if it is a PronounRoot(PERS), it returns +PRON+PERS+A3SG+PNON which
         * indicates a 3rd person singular agreement.
         * If it is "biz", it returns +PRON+PERS+A1PL+PNON which indicates a 1st person plural agreement.
         * If it is "siz", it returns +PRON+PERS+A2PL+PNON which indicates a 2nd person plural agreement.
         * If it is "onlar" and the first item of suffixList, if it is a PronounRoot(PERS), it returns o+PRON+PERS+A3PL+PNON which
         * indicates a 3rd person plural agreement.
         * If it is one of the "nere, ne, kim, hangi", it returns +PRON+QUESP which indicates a question pronoun.</p></summary>
         *
         * <returns>corresponding transitions of pronouns and an empty {@link string} if any case does not match.</returns>
         */
        public string PronounTransition()
        {
            switch (_formList[0])
            {
                case "kendi":
                    return "kendi+PRON+REFLEXP";
                case "hep":
                case "öbür":
                case "topu":
                case "öteki":
                case "kimse":
                case "hiçbiri":
                case "tümü":
                case "çoğu":
                case "hepsi":
                case "herkes":
                case "başkası":
                case "birçoğu":
                case "birçokları":
                case "birbiri":
                case "birbirleri":
                case "biri":
                case "birkaçı":
                case "böylesi":
                case "diğeri":
                case "cümlesi":
                case "bazı":
                case "kimi":
                    return _formList[0] + "+PRON+QUANTP";
                case "biz":
                    return _formList[0] + "+PRON+PERS+A1PL+PNON";
                case "siz":
                    return _formList[0] + "+PRON+PERS+A2PL+PNON";
                case "onlar":
                    return "o+PRON+PERS+A3PL+PNON";
                case "nere":
                case "ne":
                case "kaçı":
                case "kim":
                case "hangi":
                    return _formList[0] + "+PRON+QUESP";
                case "ben":
                    return _formList[0] + "+PRON+PERS+A1SG+PNON";
                case "sen":
                    return _formList[0] + "+PRON+PERS+A2SG+PNON";
                case "o":
                    if (_suffixList[0].GetName() == "PronounRoot(PERS)")
                    {
                        return _formList[0] + "+PRON+PERS+A3SG+PNON";
                    }
                    else
                    {
                        if (_suffixList[0].GetName() == "PronounRoot(DEMONS)")
                        {
                            return _formList[0] + "+PRON+DEMONSP";
                        }

                        return "";
                    }
                case "bu":
                case "şu":
                    return _formList[0] + "+PRON+DEMONSP";
                default:
                    return "";
            }
        }

        /**
         * <summary>The transitionList method first creates an empty {@link string} result, then gets the first item of suffixList and checks for cases;
         * <p>
         * If it is one of the "NominalRoot, NominalRootNoPossessive, CompoundNounRoot, NominalRootPlural", it assigns concatenation of first
         * item of formList and +NOUN to the result string.
         * Ex : Birincilik
         * </p><p>
         * If it is one of the "VerbalRoot, PassiveHn", it assigns concatenation of first item of formList and +VERB to the result string.
         * Ex : Başkalaştı
         * </p><p>
         * If it is "CardinalRoot", it assigns concatenation of first item of formList and +NUM+CARD to the result string.
         * Ex : Onuncu
         * </p><p>
         * If it is "FractionRoot", it assigns concatenation of first item of formList and NUM+FRACTION to the result string.
         * Ex : 1/2
         * </p><p>
         * If it is "TimeRoot", it assigns concatenation of first item of formList and +TIME to the result string.
         * Ex : 14:28
         * </p><p>
         * If it is "RealRoot", it assigns concatenation of first item of formList and +NUM+REAL to the result string.
         * Ex : 1.2
         * </p><p>
         * If it is "Punctuation", it assigns concatenation of first item of formList and +PUNC to the result string.
         * Ex : ,
         * </p><p>
         * If it is "Hashtag", it assigns concatenation of first item of formList and +HASHTAG to the result string.
         * Ex : #
         * </p><p>
         * If it is "DateRoot", it assigns concatenation of first item of formList and +DATE to the result string.
         * Ex : 11/06/2018
         * </p><p>
         * If it is "RangeRoot", it assigns concatenation of first item of formList and +RANGE to the result string.
         * Ex : 3-5
         * </p><p>
         * If it is "Email", it assigns concatenation of first item of formList and +EMAIL to the result string.
         * Ex : abc@
         * </p><p>
         * If it is "PercentRoot", it assigns concatenation of first item of formList and +PERCENT to the result string.
         * Ex : %12.5
         * </p><p>
         * If it is "DeterminerRoot", it assigns concatenation of first item of formList and +DET to the result string.
         * Ex : Birtakım
         * </p><p>
         * If it is "ConjunctionRoot", it assigns concatenation of first item of formList and +CONJ to the result string.
         * Ex : Ama
         * </p><p>
         * If it is "AdverbRoot", it assigns concatenation of first item of formList and +ADV to the result string.
         * Ex : Acilen
         * </p><p>
         * If it is "ProperRoot", it assigns concatenation of first item of formList and +NOUN+PROP to the result string.
         * Ex : Ahmet
         * </p><p>
         * If it is "HeaderRoot", it assigns the result of the headerTransition method to the result string.
         * Ex : &lt;DOC&gt;
         * </p><p>
         * If it is "InterjectionRoot", it assigns concatenation of first item of formList and +INTERJ to the result string.
         * Ex : Hey
         * </p><p>
         * If it is "DuplicateRoot", it assigns concatenation of first item of formList and +DUP to the result string.
         * Ex : Allak
         * </p><p>
         * If it is "CodeRoot", it assigns concatenation of first item of formList and +CODE to the result String.
         * Ex : 5000-WX
         * </p><p>
         * If it is "MetricRoot", it assigns concatenation of first item of formList and +METRIC to the result String.
         * Ex : 6cmx12cm
         * </p><p>
         * If it is "QuestionRoot", it assigns concatenation of first item of formList and +QUES to the result string.
         * Ex : Mı
         * </p><p>
         * If it is "PostP", and the first item of formList is one of the "karşı, ilişkin, göre, kadar, ait, yönelik, rağmen, değin,
         * dek, doğru, karşın, dair, atfen, binaen, hitaben, istinaden, mahsuben, mukabil, nazaran", it assigns concatenation of first
         * item of formList and +POSTP+PCDAT to the result string.
         * Ex : İlişkin
         * </p><p>
         * If it is "PostP", and the first item of formList is one of the "sonra, önce, beri, fazla, dolayı, itibaren, başka,
         * çok, evvel, ötürü, yana, öte, aşağı, yukarı, dışarı, az, gayrı", it assigns concatenation of first
         * item of formList and +POSTP+PCABL to the result string.
         * Ex : Başka
         * </p><p>
         * If it is "PostP", and the first item of formList is "yanısıra", it assigns concatenation of first
         * item of formList and +POSTP+PCGEN to the result string.
         * Ex : Yanısıra
         * </p><p>
         * If it is "PostP", and the first item of formList is one of the "birlikte, beraber", it assigns concatenation of first
         * item of formList and +PPOSTP+PCINS to the result string.
         * Ex : Birlikte
         * </p><p>
         * If it is "PostP", and the first item of formList is one of the "aşkın, takiben", it assigns concatenation of first
         * item of formList and +POSTP+PCACC to the result string.
         * Ex : Takiben
         * </p><p>
         * If it is "PostP", it assigns concatenation of first item of formList and +POSTP+PCNOM to the result string.
         * </p><p>
         * If it is "PronounRoot", it assigns result of the pronounTransition method to the result string.
         * Ex : Ben
         * </p><p>
         * If it is "OrdinalRoot", it assigns concatenation of first item of formList and +NUM+ORD to the result string.
         * Ex : Altıncı
         * </p><p>
         * If it starts with "Adjective", it assigns concatenation of first item of formList and +ADJ to the result string.
         * Ex : Güzel
         * </p>
         * At the end, it loops through the formList and concatenates each item with result {@link string}.</summary>
         *
         * <returns>string result accumulated with items of formList.</returns>
         */
        public string TransitionList()
        {
            var result = "";
            if (_suffixList[0].GetName() == "NominalRoot" ||
                _suffixList[0].GetName() == "NominalRootNoPossessive" ||
                _suffixList[0].GetName() == "CompoundNounRoot" ||
                _suffixList[0].GetName() == "NominalRootPlural")
            {
                result = _formList[0] + "+NOUN";
            }
            else
            {
                if (_suffixList[0].GetName().StartsWith("VerbalRoot") ||
                    _suffixList[0].GetName() == "PassiveHn")
                {
                    result = _formList[0] + "+VERB";
                }
                else
                {
                    if (_suffixList[0].GetName() == "CardinalRoot")
                    {
                        result = _formList[0] + "+NUM+CARD";
                    }
                    else
                    {
                        if (_suffixList[0].GetName() == "FractionRoot")
                        {
                            result = _formList[0] + "+NUM+FRACTION";
                        }
                        else
                        {
                            if (_suffixList[0].GetName() == "TimeRoot")
                            {
                                result = _formList[0] + "+TIME";
                            }
                            else
                            {
                                if (_suffixList[0].GetName() == "RealRoot")
                                {
                                    result = _formList[0] + "+NUM+REAL";
                                }
                                else
                                {
                                    if (_suffixList[0].GetName() == "Punctuation")
                                    {
                                        result = _formList[0] + "+PUNC";
                                    }
                                    else
                                    {
                                        if (_suffixList[0].GetName() == "Hashtag")
                                        {
                                            result = _formList[0] + "+HASHTAG";
                                        }
                                        else
                                        {
                                            if (_suffixList[0].GetName() == "DateRoot")
                                            {
                                                result = _formList[0] + "+DATE";
                                            }
                                            else
                                            {
                                                if (_suffixList[0].GetName() == "RangeRoot")
                                                {
                                                    result = _formList[0] + "+RANGE";
                                                }
                                                else
                                                {
                                                    if (_suffixList[0].GetName() == "Email")
                                                    {
                                                        result = _formList[0] + "+EMAIL";
                                                    }
                                                    else
                                                    {
                                                        if (_suffixList[0].GetName() == "PercentRoot")
                                                        {
                                                            result = _formList[0] + "+PERCENT";
                                                        }
                                                        else
                                                        {
                                                            if (_suffixList[0].GetName() == "DeterminerRoot")
                                                            {
                                                                result = _formList[0] + "+DET";
                                                            }
                                                            else
                                                            {
                                                                if (_suffixList[0].GetName() == "ConjunctionRoot")
                                                                {
                                                                    result = _formList[0] + "+CONJ";
                                                                }
                                                                else
                                                                {
                                                                    if (_suffixList[0].GetName() == "AdverbRoot")
                                                                    {
                                                                        result = _formList[0] + "+ADV";
                                                                    }
                                                                    else
                                                                    {
                                                                        if (_suffixList[0].GetName() == "ProperRoot")
                                                                        {
                                                                            result = _formList[0] + "+NOUN+PROP";
                                                                        }
                                                                        else
                                                                        {
                                                                            if (_suffixList[0].GetName() ==
                                                                                "HeaderRoot")
                                                                            {
                                                                                result = HeaderTransition();
                                                                            }
                                                                            else
                                                                            {
                                                                                if (_suffixList[0].GetName() ==
                                                                                 "InterjectionRoot")
                                                                                {
                                                                                    result = _formList[0] + "+INTERJ";
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (_suffixList[0].GetName() ==
                                                                                     "DuplicateRoot")
                                                                                    {
                                                                                        result = _formList[0] + "+DUP";
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (_suffixList[0].GetName() ==
                                                                                         "CodeRoot")
                                                                                        {
                                                                                            result = _formList[0] +
                                                                                                "+CODE";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if (_suffixList[0]
                                                                                                 .GetName() ==
                                                                                             "MetricRoot")
                                                                                            {
                                                                                                result = _formList[0] +
                                                                                                    "+METRIC";
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                if (_suffixList[0]
                                                                                                     .GetName() ==
                                                                                                 "QuestionRoot")
                                                                                                {
                                                                                                    result = "mi+QUES";
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    if (_suffixList[0]
                                                                                                         .GetName() ==
                                                                                                     "PostP")
                                                                                                    {
                                                                                                        switch
                                                                                                            (_formList[
                                                                                                                0])
                                                                                                        {
                                                                                                            case "karşı"
                                                                                                                :
                                                                                                            case
                                                                                                                "ilişkin"
                                                                                                                :
                                                                                                            case "göre":
                                                                                                            case "kadar"
                                                                                                                :
                                                                                                            case "ait":
                                                                                                            case
                                                                                                                "yönelik"
                                                                                                                :
                                                                                                            case
                                                                                                                "rağmen"
                                                                                                                :
                                                                                                            case "değin"
                                                                                                                :
                                                                                                            case "dek":
                                                                                                            case "doğru"
                                                                                                                :
                                                                                                            case
                                                                                                                "karşın"
                                                                                                                :
                                                                                                            case "dair":
                                                                                                            case "atfen"
                                                                                                                :
                                                                                                            case
                                                                                                                "binaen"
                                                                                                                :
                                                                                                            case
                                                                                                                "hitaben"
                                                                                                                :
                                                                                                            case
                                                                                                                "istinaden"
                                                                                                                :
                                                                                                            case
                                                                                                                "mahsuben"
                                                                                                                :
                                                                                                            case
                                                                                                                "mukabil"
                                                                                                                :
                                                                                                            case
                                                                                                                "nazaran"
                                                                                                                :
                                                                                                                result =
                                                                                                                    _formList
                                                                                                                        [0] +
                                                                                                                    "+POSTP+PCDAT";
                                                                                                                break;
                                                                                                            case "sonra"
                                                                                                                :
                                                                                                            case "önce":
                                                                                                            case "beri":
                                                                                                            case "fazla"
                                                                                                                :
                                                                                                            case
                                                                                                                "dolayı"
                                                                                                                :
                                                                                                            case
                                                                                                                "itibaren"
                                                                                                                :
                                                                                                            case "başka"
                                                                                                                :
                                                                                                            case "çok":
                                                                                                            case "evvel"
                                                                                                                :
                                                                                                            case "ötürü"
                                                                                                                :
                                                                                                            case "yana":
                                                                                                            case "öte":
                                                                                                            case "aşağı"
                                                                                                                :
                                                                                                            case
                                                                                                                "yukarı"
                                                                                                                :
                                                                                                            case
                                                                                                                "dışarı"
                                                                                                                :
                                                                                                            case "az":
                                                                                                            case "gayrı"
                                                                                                                :
                                                                                                                result =
                                                                                                                    _formList
                                                                                                                        [0] +
                                                                                                                    "+POSTP+PCABL";
                                                                                                                break;
                                                                                                            case
                                                                                                                "yanısıra"
                                                                                                                :
                                                                                                                result =
                                                                                                                    _formList
                                                                                                                        [0] +
                                                                                                                    "+POSTP+PCGEN";
                                                                                                                break;
                                                                                                            case
                                                                                                                "birlikte"
                                                                                                                :
                                                                                                            case
                                                                                                                "beraber"
                                                                                                                :
                                                                                                                result =
                                                                                                                    _formList
                                                                                                                        [0] +
                                                                                                                    "+POSTP+PCINS";
                                                                                                                break;
                                                                                                            case "aşkın"
                                                                                                                :
                                                                                                            case
                                                                                                                "takiben"
                                                                                                                :
                                                                                                                result =
                                                                                                                    _formList
                                                                                                                        [0] +
                                                                                                                    "+POSTP+PCACC";
                                                                                                                break;
                                                                                                            default:
                                                                                                                result =
                                                                                                                    _formList
                                                                                                                        [0] +
                                                                                                                    "+POSTP+PCNOM";
                                                                                                                break;
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        if (_suffixList[
                                                                                                             0]
                                                                                                         .GetName()
                                                                                                         .StartsWith(
                                                                                                             "PronounRoot"))
                                                                                                        {
                                                                                                            result =
                                                                                                                PronounTransition();
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            if
                                                                                                                (_suffixList
                                                                                                                         [0]
                                                                                                                     .GetName() ==
                                                                                                                 "OrdinalRoot")
                                                                                                            {
                                                                                                                result =
                                                                                                                    _formList
                                                                                                                        [0] +
                                                                                                                    "+NUM+ORD";
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                if
                                                                                                                    (_suffixList
                                                                                                                         [0]
                                                                                                                     .GetName()
                                                                                                                     .StartsWith(
                                                                                                                         "Adjective"))
                                                                                                                {
                                                                                                                    result =
                                                                                                                        _formList
                                                                                                                            [0] +
                                                                                                                        "+ADJ";
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
                }
            }

            foreach (var transition in _transitionList)
            {
                if (transition != null)
                {
                    if (!transition.StartsWith("^"))
                    {
                        result = result + "+" + transition;
                    }
                    else
                    {
                        result += transition;
                    }
                }
            }

            return result;
        }

        /**
         * <summary>The suffixList method gets the first items of suffixList and formList and concatenates them with parenthesis and
         * assigns a string result. Then, loops through the formList and it the current ith item is not equal to previous
         * item it accumulates ith items of formList and suffixList to the result {@link string}.</summary>
         *
         * <returns>result {@link string} accumulated with the items of formList and suffixList.</returns>
         */
        public string SuffixList()
        {
            var result = _suffixList[0].GetName() + '(' + _formList[0] + ')';
            for (var i = 1; i < _formList.Count; i++)
            {
                if (_formList[i] != _formList[i - 1])
                {
                    result = result + "+" + _suffixList[i].GetName() + '(' + _formList[i] + ')';
                }
            }

            return result;
        }

        /**
         * <summary>The withList method gets the root as a result {@link string} then loops through the withList and concatenates each item
         * with result {@link string}.</summary>
         *
         * <returns>result {@link string} accumulated with items of withList.</returns>
         */
        public string WithList()
        {
            var result = root.GetName();
            foreach (var aWith in _withList)
            {
                result = result + "+" + aWith;
            }

            return result;
        }

        /**
        * <summary>Replace root word of the current parse with the new root word and returns the new word.</summary>
        * <param name="newRoot"> Replaced root word</param>
        * <returns> Root word of the parse will be replaced with the newRoot and the resulting surface form is returned.</returns>
        */
        public string ReplaceRootWord(TxtWord newRoot)
        {
            var result = newRoot.GetName();
            foreach (var aWith in _withList)
            {
                var transition = new Transition(null, aWith, null);
                result = transition.MakeTransition(newRoot, result);
            }

            return result;
        }


        /**
         * <summary>The overridden ToString method which returns transitionList method.</summary>
         *
         * <returns>returns transitionList method.</returns>
         */
        public override string ToString()
        {
            return TransitionList();
        }

        /**
         * <summary>The overridden compareTo method takes an {@link Object} as an input and if it is an instance of the {@link FsmParse}
         * class it returns the result of comparison of the items of transitionList with input {@link Object}.</summary>
         *
         * <param name="o">{@link Object} input to compare.</param>
         * <returns>comparison of the items of transitionList with input {@link Object}, and returns 0 if input is not an</returns>
         * instance of {@link FsmParse} class.
         */
        public int CompareTo(object o)
        {
            return TransitionList().CompareTo(((FsmParse)o).TransitionList());
        }
    }
}