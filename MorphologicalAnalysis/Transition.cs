using Dictionary.Dictionary;
using Dictionary.Language;

namespace MorphologicalAnalysis
{
    public class Transition
    {
        private readonly State _toState;
        private readonly string _with;
        private readonly string _withName;
        private string _formationToCheck;
        private readonly string _toPos;

        /**
         * <summary>A constructor of {@link Transition} class which takes  a {@link State}, and two {@link string}s as input. Then it
         * initializes toState, with and withName variables with given inputs.</summary>
         *
         * <param name="toState"> {@link State} input.</param>
         * <param name="with">    string input.</param>
         * <param name="withName">string input.</param>
         */
        public Transition(State toState, string with, string withName)
        {
            this._toState = toState;
            this._with = with;
            this._withName = withName;
            _toPos = null;
        }

        /**
         * <summary>Another constructor of {@link Transition} class which takes  a {@link State}, and three {@link string}s as input. Then it
         * initializes toState, with, withName and toPos variables with given inputs.</summary>
         *
         * <param name="toState"> {@link State} input.</param>
         * <param name="with">    string input.</param>
         * <param name="withName">string input.</param>
         * <param name="toPos">   string input.</param>
         */
        public Transition(State toState, string with, string withName, string toPos)
        {
            this._toState = toState;
            this._with = with;
            this._withName = withName;
            this._toPos = toPos;
        }

        /**
         * <summary>Another constructor of {@link Transition} class which only takes a {@link string}s as an input. Then it
         * initializes toState, withName and toPos variables as null and with variable with the given input.</summary>
         *
         * <param name="with">string input.</param>
         */
        public Transition(string with)
        {
            _toState = null;
            _withName = null;
            _toPos = null;
            this._with = with;
        }

        /**
         * <summary>Getter for the toState variable.</summary>
         *
         * <returns>toState variable.</returns>
         */
        public State ToState()
        {
            return _toState;
        }

        /**
         * <summary>Getter for the toPos variable.</summary>
         *
         * <returns>toPos variable.</returns>
         */
        public string ToPos()
        {
            return _toPos;
        }

        /**
         * <summary>The transitionPossible method takes two {@link string} as inputs; currentSurfaceForm and realSurfaceForm. If the
         * length of the given currentSurfaceForm is greater than the given realSurfaceForm, it directly returns true. If not,
         * it takes a substring from given realSurfaceForm with the size of currentSurfaceForm. Then checks for the characters of
         * with variable.
         * <p/>
         * If the character of with that makes transition is C, it returns true if the substring contains c or ç.
         * If the character of with that makes transition is D, it returns true if the substring contains d or t.
         * If the character of with that makes transition is A, it returns true if the substring contains a or e.
         * If the character of with that makes transition is K, it returns true if the substring contains k, g or ğ.
         * If the character of with that makes transition is other than the ones above, it returns true if the substring
         * contains the same character as with.</summary>
         *
         * <param name="currentSurfaceForm">{@link string} input.</param>
         * <param name="realSurfaceForm">   {@link string} input.</param>
         * <returns>true when the transition is possible according to Turkish grammar, false otherwise.</returns>
         */
        public bool TransitionPossible(string currentSurfaceForm, string realSurfaceForm)
        {
            if (currentSurfaceForm.Length == 0 || currentSurfaceForm.Length >= realSurfaceForm.Length)
            {
                return true;
            }

            string searchString = realSurfaceForm.Substring(currentSurfaceForm.Length);
            for (int i = 0; i < _with.Length; i++)
            {
                switch (_with[i])
                {
                    case 'C':
                        return searchString.Contains("c") || searchString.Contains("ç");
                    case 'D':
                        return searchString.Contains("d") || searchString.Contains("t");
                    case 'c':
                    case 'e':
                    case 'r':
                    case 'p':
                    case 'l':
                    case 'b':
                    case 'g':
                    case 'o':
                    case 'm':
                    case 'v':
                    case 'i':
                    case 'ü':
                    case 'z':
                        return searchString.Contains("" + _with[i]);
                    case 'A':
                        return searchString.Contains("a") || searchString.Contains("e");
                    case 'k':
                        return searchString.Contains("k") || searchString.Contains("g") || searchString.Contains("ğ");
                }
            }

            return true;
        }

        /**
         * <summary>The transitionPossible method takes a {@link FsmParse} currentFsmParse as an input. It then checks some special cases;</summary>
         *
         * <param name="currentFsmParse">Parse to be checked</param>
         * <returns>true if transition is possible false otherwise</returns>
         */
        public bool TransitionPossible(FsmParse currentFsmParse)
        {
            if (_with == "Ar" && currentFsmParse.GetSurfaceForm().EndsWith("l") &&
                currentFsmParse.GetWord().GetName() != currentFsmParse.GetSurfaceForm())
            {
                return false;
            }

            if (currentFsmParse.GetVerbAgreement() != null && currentFsmParse.GetPossessiveAgreement() != null &&
                _withName != null)
            {
                if (currentFsmParse.GetVerbAgreement() == "A3PL" && _withName == "^DB+VERB+ZERO+PRES+A1SG")
                {
                    return false;
                }

                if (currentFsmParse.GetVerbAgreement() == "A3SG" &&
                    (currentFsmParse.GetPossessiveAgreement() == "P1SG" ||
                     currentFsmParse.GetPossessiveAgreement() == "P2SG") &&
                    _withName == "^DB+VERB+ZERO+PRES+A1PL")
                {
                    return false;
                }
            }

            return true;
        }

        public bool TransitionPossible(TxtWord root, State fromState)
        {
            if (root.IsAdjective() && (root.IsNominal() && !root.IsExceptional() || root.IsPronoun()) &&
                _toState.GetName() == "NominalRoot(ADJ)" && _with == "0")
            {
                return false;
            }

            if (root.IsAdjective() && root.IsNominal() && _with == "^DB+VERB+ZERO+PRES+A3PL" &&
                fromState.GetName() == "AdjectiveRoot")
            {
                return false;
            }

            if (root.IsAdjective() && root.IsNominal() && _with == "SH" &&
                fromState.GetName() == "AdjectiveRoot")
            {
                return false;
            }

            if (_with == "ki")
            {
                return root.TakesRelativeSuffixKi();
            }

            if (_with == "kü")
            {
                return root.TakesRelativeSuffixKu();
            }

            if (_with == "dHr")
            {
                if (_toState.GetName() == "Adverb")
                {
                    return true;
                }

                return root.TakesSuffixDIRAsFactitive();
            }

            if (_with == "Hr" && (_toState.GetName() == "AdjectiveRoot(VERB)" ||
                                  _toState.GetName() == "OtherTense" ||
                                  _toState.GetName() == "OtherTense2"))
            {
                return root.TakesSuffixIRAsAorist();
            }

            return true;
        }

        /**
         * <summary>The BeforeLastVowel method takes a {@link string} stem as an input. It loops through the given stem and returns
         * the second last vowel.</summary>
         *
         * <param name="stem">string input.</param>
         * <returns>Vowel before the last vowel.</returns>
         */
        private char BeforeLastVowel(string stem)
        {
            int i, before = 1;
            var last = '0';
            for (i = stem.Length - 1; i >= 0; i--)
            {
                if (TurkishLanguage.IsVowel(stem[i]))
                {
                    if (before == 1)
                    {
                        last = stem[i];
                        before--;
                        continue;
                    }

                    return stem[i];
                }
            }

            return last;
        }

        /**
         * <summary>The LastVowel method takes a {@link string} stem as an input. It loops through the given stem and returns
         * the last vowel.</summary>
         *
         * <param name="stem">string input.</param>
         * <returns>the last vowel.</returns>
         */
        private char LastVowel(string stem)
        {
            int i;
            for (i = stem.Length - 1; i >= 0; i--)
            {
                if (TurkishLanguage.IsVowel(stem[i]))
                {
                    return stem[i];
                }
            }

            for (i = stem.Length - 1; i >= 0; i--)
            {
                if (stem[i] >= '0' && stem[i] <= '9')
                {
                    return stem[i];
                }
            }

            return '0';
        }

        /**
         * <summary>The lastPhoneme method takes a {@link string} stem as an input. It then returns the last phoneme of the given stem.</summary>
         *
         * <param name="stem">string input.</param>
         * <returns>the last phoneme.</returns>
         */
        private char LastPhoneme(string stem)
        {
            if (stem.Length == 0)
            {
                return ' ';
            }

            if (stem[stem.Length - 1] != '\'')
            {
                return stem[stem.Length - 1];
            }

            return stem[stem.Length - 2];
        }

        /**
         * <summary>The withFirstChar method returns the first character of the with variable.</summary>
         *
         * <returns>the first character of the with variable.</returns>
         */
        private char WithFirstChar()
        {
            if (_with.Length == 0)
            {
                return '$';
            }

            if (_with[0] != '\'')
            {
                return _with[0];
            }

            if (_with.Length == 1)
            {
                return _with[0];
            }

            return _with[1];
        }

        /**
         * <summary>The startWithVowelOrConsonantDrops method checks for some cases. If the first character of with variable is "nsy",
         * and with variable does not equal to one of the strings; "ylA, ysA, ymHs, yDH, yken", it returns true. If
         * <p/>
         * Or, if the first character of with variable is 'A, H: or any other vowels, it returns true.</summary>
         *
         * <returns>true if it starts with vowel or consonant drops, false otherwise.</returns>
         */
        private bool StartWithVowelOrConsonantDrops()
        {
            if (TurkishLanguage.IsConsonantDrop(WithFirstChar()) && _with != "ylA" &&
                _with != "ysA" && _with != "ymHs" && _with != "yDH" && _with != "yken")
            {
                return true;
            }

            if (WithFirstChar() == 'A' || WithFirstChar() == 'H' || TurkishLanguage.IsVowel(WithFirstChar()))
            {
                return true;
            }

            return false;
        }

        /**
         * <summary>The softenDuringSuffixation method takes a {@link TxtWord} root as an input. It checks two cases; first case returns
         * true if the given root is nominal or adjective and has one of the flags "IS_SD, IS_B_SD, IS_SDD" and with variable
         * equals o one of the strings "Hm, nDAn, ncA, nDA, yA, yHm, yHz, yH, nH, nA, nHn, H, sH, Hn, HnHz, HmHz".
         * <p/>
         * And the second case returns true if the given root is verb and has the "F_SD" flag, also with variable starts with
         * "Hyor" or equals one of the strings "yHs, yAn, yA, yAcAk, yAsH, yHncA, yHp, yAlH, yArAk, yAdur, yHver, yAgel, yAgor,
         * yAbil, yAyaz, yAkal, yAkoy, yAmA, yHcH, HCH, Hr, Hs, Hn, yHn", yHnHz, Ar, Hl").</summary>
         *
         * <param name="root">{@link TxtWord} input.</param>
         * <returns>true if there is softening during suffixation of the given root, false otherwise.</returns>
         */
        public bool SoftenDuringSuffixation(TxtWord root)
        {
            if ((root.IsNominal() || root.IsAdjective()) && root.NounSoftenDuringSuffixation() &&
                (_with == "Hm" || _with == "nDAn" || _with == "ncA" || _with == "nDA" || _with == "yA" ||
                 _with == "yHm" || _with == "yHz" || _with == "yH" || _with == "nH" ||
                 _with == "nA" || _with == "nHn" || _with == "H" || _with == "sH" || _with == "Hn" ||
                 _with == "HnHz" || _with == "HmHz"))
            {
                return true;
            }

            if (root.IsVerb() && root.VerbSoftenDuringSuffixation() && (_with.StartsWith("Hyor") || _with == "yHs" ||
                _with == "yAn" || _with == "yA" || _with.StartsWith("yAcAk") || _with == "yAsH" || _with == "yHncA" ||
                _with == "yHp" || _with == "yAlH" || _with == "yArAk" || _with == "yAdur" || _with == "yHver" ||
                _with == "yAgel" || _with == "yAgor" || _with == "yAbil" || _with == "yAyaz" || _with == "yAkal" ||
                _with == "yAkoy" || _with == "yAmA" || _with == "yHcH" || _with == "HCH" ||
                _with.StartsWith("Hr") || _with == "Hs" || _with == "Hn" || _with == "yHn" || _with == "yHnHz" ||
                _with.StartsWith("Ar") || _with == "Hl"))
            {
                return true;
            }

            return false;
        }

        /**
         * <summary>The makeTransition method takes a {@link TxtWord} root and s {@link string} stem as inputs. If given root is a verb,
         * it makes transition with given root and stem with the verbal root state. If given root is not verb, it makes transition
         * with given root and stem and the nominal root state.</summary>
         *
         * <param name="root">{@link TxtWord} input.</param>
         * <param name="stem">string input.</param>
         * <returns>string type output that has the transition.</returns>
         */
        public string MakeTransition(TxtWord root, string stem)
        {
            if (root.IsVerb())
            {
                return MakeTransition(root, stem, new State("VerbalRoot", true, false));
            }

            return MakeTransition(root, stem, new State("NominalRoot", true, false));
        }

        public string MakeTransition(TxtWord root, string stem, State startState)
        {
            var rootWord = root.GetName() == stem || root.GetName() + "'" == stem;
            var formation = stem;
            var i = 0;
            if (_with == "0")
            {
                return stem;
            }

            if ((stem.Equals("bu") || stem.Equals("şu") || stem.Equals("o")) && rootWord &&
                _with == "ylA")
            {
                return stem + "nunla";
            }

            if (_with == "yA")
            {
                if (stem.Equals("ben"))
                {
                    return "bana";
                }
            }

            _formationToCheck = stem;
            //---vowelEChangesToIDuringYSuffixation---
            //de->d(i)yor, ye->y(i)yor
            if (rootWord && WithFirstChar() == 'y' && root.VowelEChangesToIDuringYSuffixation() &&
                _with[1] != 'H')
            {
                formation = stem.Substring(0, stem.Length - 1) + 'i';
                _formationToCheck = formation;
            }
            else
            {
                //---lastIdropsDuringPassiveSuffixation---
                // yoğur->yoğrul, ayır->ayrıl, buyur->buyrul, çağır->çağrıl, çevir->çevril, devir->devril,
                // kavur->kavrul, kayır->kayrıl, kıvır->kıvrıl, savur->savrul, sıyır->sıyrıl, yoğur->yoğrul
                if (rootWord && (_with == "Hl" || _with == "Hn") && root.LastIdropsDuringPassiveSuffixation())
                {
                    formation = stem.Substring(0, stem.Length - 2) + stem[stem.Length - 1];
                    _formationToCheck = stem;
                }
                else
                {
                    //---showsSuRegularities---
                    //karasu->karasuyu, özsu->özsuyu, ağırsu->ağırsuyu, akarsu->akarsuyu, bengisu->bengisuyu
                    if (rootWord && root.ShowsSuRegularities() && StartWithVowelOrConsonantDrops() &&
                        !_with.StartsWith("y"))
                    {
                        formation = stem + 'y';
                        _formationToCheck = formation;
                    }
                    else
                    {
                        if (rootWord && root.DuplicatesDuringSuffixation() &&
                            TurkishLanguage.IsConsonantDrop(_with[0]))
                        {
                            //---duplicatesDuringSuffixation---
                            if (SoftenDuringSuffixation(root))
                            {
                                //--extra softenDuringSuffixation
                                switch (LastPhoneme(stem))
                                {
                                    case 'p':
                                        //tıp->tıbbı
                                        formation = stem.Substring(0, stem.Length - 1) + "bb";
                                        break;
                                    case 't':
                                        //cet->ceddi, met->meddi, ret->reddi, serhat->serhaddi, zıt->zıddı, şet->şeddi
                                        formation = stem.Substring(0, stem.Length - 1) + "dd";
                                        break;
                                }
                            }
                            else
                            {
                                //cer->cerri, emrihak->emrihakkı, fek->fekki, fen->fenni, had->haddi, hat->hattı,
                                // haz->hazzı, his->hissi
                                formation = stem + stem[stem.Length - 1];
                            }

                            _formationToCheck = formation;
                        }
                        else
                        {
                            if (rootWord && root.LastIdropsDuringSuffixation() &&
                                !startState.GetName().StartsWith("VerbalRoot") &&
                                !startState.GetName().StartsWith("ProperRoot") && StartWithVowelOrConsonantDrops())
                            {
                                //---lastIdropsDuringSuffixation---
                                if (SoftenDuringSuffixation(root))
                                {
                                    //---softenDuringSuffixation---
                                    switch (LastPhoneme(stem))
                                    {
                                        case 'p':
                                            //hizip->hizbi, kayıp->kaybı, kayıt->kaydı, kutup->kutbu
                                            formation = stem.Substring(0, stem.Length - 2) + 'b';
                                            break;
                                        case 't':
                                            //akit->akdi, ahit->ahdi, lahit->lahdi, nakit->nakdi, vecit->vecdi
                                            formation = stem.Substring(0, stem.Length - 2) + 'd';
                                            break;
                                        case 'ç':
                                            //eviç->evci, nesiç->nesci
                                            formation = stem.Substring(0, stem.Length - 2) + 'c';
                                            break;
                                    }
                                }
                                else
                                {
                                    //sarıağız->sarıağzı, zehir->zehri, zikir->zikri, nutuk->nutku, omuz->omzu, ömür->ömrü
                                    //lütuf->lütfu, metin->metni, kavim->kavmi, kasıt->kastı
                                    formation = stem.Substring(0, stem.Length - 2) + stem[stem.Length - 1];
                                }

                                _formationToCheck = stem;
                            }
                            else
                            {
                                switch (LastPhoneme(stem))
                                {
                                    //---nounSoftenDuringSuffixation or verbSoftenDuringSuffixation
                                    case 'p':
                                        //adap->adabı, amip->amibi, azap->azabı, gazap->gazabı
                                        if (StartWithVowelOrConsonantDrops() && rootWord &&
                                            SoftenDuringSuffixation(root))
                                        {
                                            formation = stem.Substring(0, stem.Length - 1) + 'b';
                                        }

                                        break;
                                    case 't':
                                        //abat->abadı, adet->adedi, akort->akordu, armut->armudu
                                        //affet->affedi, yoket->yokedi, sabret->sabredi, rakset->raksedi
                                        if (StartWithVowelOrConsonantDrops() && rootWord &&
                                            SoftenDuringSuffixation(root))
                                        {
                                            formation = stem.Substring(0, stem.Length - 1) + 'd';
                                        }

                                        break;
                                    case 'ç':
                                        //ağaç->ağacı, almaç->almacı, akaç->akacı, avuç->avucu
                                        if (StartWithVowelOrConsonantDrops() && rootWord &&
                                            SoftenDuringSuffixation(root))
                                        {
                                            formation = stem.Substring(0, stem.Length - 1) + 'c';
                                        }

                                        break;
                                    case 'g':
                                        //arkeolog->arkeoloğu, filolog->filoloğu, minerolog->mineroloğu
                                        if (StartWithVowelOrConsonantDrops() && rootWord &&
                                            SoftenDuringSuffixation(root))
                                        {
                                            formation = stem.Substring(0, stem.Length - 1) + 'ğ';
                                        }

                                        break;
                                    case 'k':
                                        //ahenk->ahengi, künk->küngü, renk->rengi, pelesenk->pelesengi
                                        if (StartWithVowelOrConsonantDrops() && rootWord &&
                                            root.EndingKChangesIntoG() && !root.IsProperNoun())
                                        {
                                            formation = stem.Substring(0, stem.Length - 1) + 'g';
                                        }
                                        else
                                        {
                                            //ablak->ablağı, küllük->küllüğü, kitaplık->kitaplığı, evcilik->evciliği
                                            if (StartWithVowelOrConsonantDrops() &&
                                                (!rootWord ||
                                                 (SoftenDuringSuffixation(root) &&
                                                  (!root.IsProperNoun() ||
                                                   !startState.ToString().Equals("ProperRoot")))))
                                            {
                                                formation = stem.Substring(0, stem.Length - 1) + 'ğ';
                                            }
                                        }

                                        break;
                                }

                                _formationToCheck = formation;
                            }
                        }
                    }
                }
            }

            if (TurkishLanguage.IsConsonantDrop(WithFirstChar()) &&
                !TurkishLanguage.IsVowel(stem[stem.Length - 1]) &&
                (root.IsNumeral() || root.IsReal() || root.IsFraction() || root.IsTime() || root.IsDate() ||
                 root.IsPercent() || root.IsRange()) && (root.GetName().EndsWith("1") || root.GetName().EndsWith("3") ||
                                                         root.GetName().EndsWith("4") || root.GetName().EndsWith("5") ||
                                                         root.GetName().EndsWith("8") || root.GetName().EndsWith("9") ||
                                                         root.GetName().EndsWith("10") ||
                                                         root.GetName().EndsWith("30") ||
                                                         root.GetName().EndsWith("40") ||
                                                         root.GetName().EndsWith("60") ||
                                                         root.GetName().EndsWith("70") ||
                                                         root.GetName().EndsWith("80") ||
                                                         root.GetName().EndsWith("90") ||
                                                         root.GetName().EndsWith("00")))
            {
                if (_with[0] == '\'')
                {
                    formation += '\'';
                    i = 2;
                }
                else
                {
                    i = 1;
                }
            }
            else
            {
                if ((TurkishLanguage.IsConsonantDrop(WithFirstChar()) &&
                     TurkishLanguage.IsConsonant(LastPhoneme(stem))) ||
                    (rootWord && root.ConsonantSMayInsertedDuringPossesiveSuffixation()))
                {
                    if (_with[0] == '\'')
                    {
                        formation += '\'';
                        if (root.IsAbbreviation())
                            i = 1;
                        else
                            i = 2;
                    }
                    else
                    {
                        i = 1;
                    }
                }
            }

            for (; i < _with.Length; i++)
            {
                switch (_with[i])
                {
                    case 'D':
                        formation = ResolveD(root, formation);
                        break;
                    case 'A':
                        formation = ResolveA(root, formation, rootWord);
                        break;
                    case 'H':
                        if (_with[0] != '\'')
                        {
                            formation = ResolveH(root, formation, i == 0, _with.StartsWith("Hyor"), rootWord);
                        }
                        else
                        {
                            formation = ResolveH(root, formation, i == 1, false, rootWord);
                        }

                        break;
                    case 'C':
                        formation = ResolveC(formation);
                        break;
                    case 'S':
                        formation = ResolveS(formation);
                        break;
                    case 'Ş':
                        formation = ResolveSh(formation);
                        break;
                    default:
                        if (i == _with.Length - 1 && _with[i] == 's')
                        {
                            formation += 'ş';
                        }
                        else
                        {
                            formation += _with[i];
                        }

                        break;
                }

                _formationToCheck = formation;
            }

            return formation;
        }

        private string ResolveD(TxtWord root, string formation)
        {
            if (root.IsAbbreviation())
            {
                return formation + 'd';
            }

            if (LastPhoneme(_formationToCheck) >= '0' && LastPhoneme(_formationToCheck) <= '9')
            {
                switch (LastPhoneme(_formationToCheck))
                {
                    case '3':
                    case '4':
                    case '5':
                        //3->3'tü, 5->5'ti, 4->4'tü
                        return formation + 't';
                    case '0':
                        if (root.GetName().EndsWith("40") || root.GetName().EndsWith("60") ||
                            root.GetName().EndsWith("70"))
                            //40->40'tı, 60->60'tı, 70->70'ti
                            return formation + 't';
                        else
                            //30->30'du, 50->50'ydi, 80->80'di
                            return formation + 'd';
                    default:
                        return formation + 'd';
                }
            }

            if (TurkishLanguage.IsSertSessiz(LastPhoneme(_formationToCheck)))
            {
                //yap+DH->yaptı
                return formation + 't';
            }

            //sar+DH->sardı
            return formation + 'd';
        }

        private string ResolveA(TxtWord root, string formation, bool rootWord)
        {
            if (root.IsAbbreviation())
            {
                return formation + 'e';
            }

            if (LastVowel(_formationToCheck) >= '0' && LastVowel(_formationToCheck) <= '9')
            {
                switch (LastVowel(_formationToCheck))
                {
                    case '6':
                    case '9':
                        //6'ya, 9'a
                        return formation + 'a';
                    case '0':
                        if (root.GetName().EndsWith("10") || root.GetName().EndsWith("30") ||
                            root.GetName().EndsWith("40") || root.GetName().EndsWith("60") ||
                            root.GetName().EndsWith("90"))
                            //10'a, 30'a, 40'a, 60'a, 90'a
                            return formation + 'a';
                        else
                            //20'ye, 50'ye, 80'e, 70'e
                            return formation + 'e';
                    default:
                        //3'e, 8'e, 4'e, 2'ye
                        return formation + 'e';
                }
            }

            if (TurkishLanguage.IsBackVowel(LastVowel(_formationToCheck)))
            {
                if (root.NotObeysVowelHarmonyDuringAgglutination() && rootWord)
                {
                    //alkole, anormale, ampule, tümamirali, spirali, sosyali
                    return formation + 'e';
                }

                //sakala, kabala, eve, kediye
                return formation + 'a';
            }

            if (TurkishLanguage.IsFrontVowel(LastVowel(_formationToCheck)))
            {
                if (root.NotObeysVowelHarmonyDuringAgglutination() && rootWord)
                {
                    //sakala, kabala, eve, kediye
                    return formation + 'a';
                }

                //alkole, anormale, ampule, tümamirali, spirali, sosyali
                return formation + 'e';
            }

            if (root.IsNumeral() || root.IsFraction() || root.IsReal())
            {
                if (root.GetName().EndsWith("6") || root.GetName().EndsWith("9") || root.GetName().EndsWith("10") ||
                    root.GetName().EndsWith("30") || root.GetName().EndsWith("40") || root.GetName().EndsWith("60") ||
                    root.GetName().EndsWith("90"))
                {
                    return formation + 'a';
                }

                return formation + 'e';
            }

            return formation;
        }

        private string ResolveH(TxtWord root, string formation, bool beginningOfSuffix,
            bool specialCaseTenseSuffix, bool rootWord)
        {
            if (root.IsAbbreviation())
                return formation + 'i';
            if (beginningOfSuffix && TurkishLanguage.IsVowel(LastPhoneme(_formationToCheck)) && !specialCaseTenseSuffix)
            {
                return formation;
            }

            if (specialCaseTenseSuffix)
            {
                //eğer ek Hyor eki ise,
                if (rootWord)
                {
                    if (root.VowelAChangesToIDuringYSuffixation())
                    {
                        if (TurkishLanguage.IsFrontRoundedVowel(BeforeLastVowel(_formationToCheck)))
                        {
                            //büyülüyor, bölümlüyor, çözümlüyor, döşüyor
                            return formation.Substring(0, formation.Length - 1) + 'ü';
                        }

                        if (TurkishLanguage.IsFrontUnroundedVowel(BeforeLastVowel(_formationToCheck)))
                        {
                            //adresliyor, alevliyor, ateşliyor, bekliyor
                            return formation.Substring(0, formation.Length - 1) + 'i';
                        }

                        if (TurkishLanguage.IsBackRoundedVowel(BeforeLastVowel(_formationToCheck)))
                        {
                            //buğuluyor, bulguluyor, çamurluyor, aforozluyor
                            return formation.Substring(0, formation.Length - 1) + 'u';
                        }

                        if (TurkishLanguage.IsBackUnroundedVowel(BeforeLastVowel(_formationToCheck)))
                        {
                            //açıklıyor, çalkalıyor, gazlıyor, gıcırdıyor
                            return formation.Substring(0, formation.Length - 1) + 'ı';
                        }
                    }
                }

                if (TurkishLanguage.IsVowel(LastPhoneme(_formationToCheck)))
                {
                    if (TurkishLanguage.IsFrontRoundedVowel(BeforeLastVowel(_formationToCheck)))
                    {
                        return formation.Substring(0, formation.Length - 1) + 'ü';
                    }

                    if (TurkishLanguage.IsFrontUnroundedVowel(BeforeLastVowel(_formationToCheck)))
                    {
                        return formation.Substring(0, formation.Length - 1) + 'i';
                    }

                    if (TurkishLanguage.IsBackRoundedVowel(BeforeLastVowel(_formationToCheck)))
                    {
                        return formation.Substring(0, formation.Length - 1) + 'u';
                    }

                    if (TurkishLanguage.IsBackUnroundedVowel(BeforeLastVowel(_formationToCheck)))
                    {
                        return formation.Substring(0, formation.Length - 1) + 'ı';
                    }
                }
            }

            if (TurkishLanguage.IsFrontRoundedVowel(LastVowel(_formationToCheck)) ||
                (TurkishLanguage.IsBackRoundedVowel(LastVowel(_formationToCheck)) &&
                 root.NotObeysVowelHarmonyDuringAgglutination()))
            {
                return formation + 'ü';
            }

            if (TurkishLanguage.IsFrontUnroundedVowel(LastVowel(_formationToCheck)) ||
                (LastVowel(_formationToCheck) == 'a' && root.NotObeysVowelHarmonyDuringAgglutination()))
            {
                return formation + 'i';
            }

            if (TurkishLanguage.IsBackRoundedVowel(LastVowel(_formationToCheck)))
            {
                return formation + 'u';
            }

            if (TurkishLanguage.IsBackUnroundedVowel(LastVowel(_formationToCheck)))
            {
                return formation + 'ı';
            }

            if (root.IsNumeral() || root.IsFraction() || root.IsReal())
            {
                if (root.GetName().EndsWith("6") || root.GetName().EndsWith("40") || root.GetName().EndsWith("60") ||
                    root.GetName().EndsWith("90"))
                {
                    //6'yı, 40'ı, 60'ı
                    return formation + 'ı';
                }

                if (root.GetName().EndsWith("3") || root.GetName().EndsWith("4") || root.GetName().EndsWith("00"))
                {
                    //3'ü, 4'ü, 100'ü
                    return formation + 'ü';
                }

                if (root.GetName().EndsWith("9") || root.GetName().EndsWith("10") ||
                    root.GetName().EndsWith("30"))
                {
                    //9'u, 10'u, 30'u
                    return formation + 'u';
                }

                //2'yi, 5'i, 8'i
                return formation + 'i';
            }

            return formation;
        }

        /**
         * <summary>The resolveC method takes a {@link string} formation as an input. If the last phoneme is on of the "çfhkpsşt", it
         * concatenates given formation with 'ç', if not it concatenates given formation with 'c'.</summary>
         *
         * <param name="formation">{@link string} input.</param>
         * <returns>resolved string.</returns>
         */
        private string ResolveC(string formation)
        {
            if (TurkishLanguage.IsSertSessiz(LastPhoneme(_formationToCheck)))
            {
                return formation + 'ç';
            }

            return formation + 'c';
        }

        /**
         * <summary>The resolveS method takes a {@link string} formation as an input. It then concatenates given formation with 's'.</summary>
         *
         * <param name="formation">{@link string} input.</param>
         * <returns>resolved string.</returns>
         */
        private string ResolveS(string formation)
        {
            return formation + 's';
        }

        /**
         * <summary>The resolveSh method takes a {@link string} formation as an input. If the last character is a vowel, it concatenates
         * given formation with ş, if the last character is not a vowel, and not 't' it directly returns given formation, but if it
         * is equal to 't', it transforms it to 'd'.</summary>
         *
         * <param name="formation">{@link string} input.</param>
         * <returns>resolved string.</returns>
         */
        private string ResolveSh(string formation)
        {
            if (TurkishLanguage.IsVowel(formation[formation.Length - 1]))
            {
                return formation + 'ş';
            }

            if (formation[formation.Length - 1] != 't')
                return formation;
            return formation.Substring(0, formation.Length - 1) + 'd';
        }

        /**
         * <summary>An overridden ToString method which returns the with variable.</summary>
         *
         * <returns>with variable.</returns>
         */
        public override string ToString()
        {
            return _with;
        }

        /**
         * <summary>The with method returns the withName variable.</summary>
         *
         * <returns>the withName variable.</returns>
         */
        public string With()
        {
            return _withName;
        }
    }
}