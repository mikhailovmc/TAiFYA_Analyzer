using System;
using System.Collections.Generic;
using System.Text;

namespace TAiFYA_Analyzer
{
    class AnalyzerModel
    {
        private enum State { S, E, F,                                                                    //перечисление 
            A, B, C, D, E1, H, J1, K1, K2, K5, M1, M2, N2, O, J2, J3, K4, K3, L2, M4, M3, N3, EQ, BL, H1, H2,
            Z1, Z2, _J1, _K1, _K2, _K5, _M1, _M2, _N1, _O, _J2, _K3, _K4, _L2, _M3, _M4, _N3, Y,
            _Z1, _J11, _K22, _K12, _K13, _M12, _M22, _N12, _O2, _Q,
            _T1, _U21, _U22, _V21, _V22, _V23, _V24, _O3, _T2, _T12, _U11, _U12, _V11, _V12, _V13, _N13, _Z2
        }

        private readonly string[] reserved = { "for", "while", "break", "switch", "case", "const"};      //зарезервированные слова

        private int position;                                                                            //позиция курсора

        private string chain;                                                                            //цепочка

        private string errorMessage;                                                                     //сообщение об ошибке

        private int errorPosition;                                                                       //позиция ошибки

        private LinkedList<string> identifiersList;                                                      //список идентификаторов

        private LinkedList<string> constantList;                                                         //список констант

        public AnalyzerModel(string chain)                                                               //конструктор с параметром
        {
            this.chain = chain;
            position = 0;
            identifiersList = new LinkedList<string>();
            constantList = new LinkedList<string>();
        }

        public int GetErrorPosition { get { return errorPosition; } }                                    //свойство получения позиции ошибки
        public string GetErrorMessge { get { return errorMessage; } }                                    //свойство получения сообщения об ошибке
        public LinkedList<string> GetIdentifierList { get { return identifiersList; } }                  //свойство получения списка идентификаторов
        public LinkedList<string> GetConstantList { get { return constantList; } }                       //свойство получения списка констант

        private bool CheckId(string id)                                                                  //метод проверки идентификатора
        {
            return id.Length <= 12 && Array.IndexOf(reserved, id) == -1;
        }

        private bool CheckConst(string constant)                                                         //метод проверки константы
        {
            return short.TryParse(constant, out _);
        }

        public bool AnalyzeChain()                                                                       //метод проверки принадлежности цепочки языку
        {
            State state = State.S;
            string identifiyer = "";
            string constant = "";
            int simplexprcount = 0;
            while (state != State.E && state != State.F)
            {
                if (position < chain.Length)
                {
                    char ch = char.ToLower(chain[position]);
                    switch (state)
                    {
                        case State.S:
                            {
                                if (ch == 'w')
                                    state = State.A;
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \"w\" или пробел!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.A:
                            {
                                if (ch == 'h')
                                    state = State.B;
                                else
                                {
                                    errorMessage = "Ожидалось \"h\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.B:
                            {
                                if (ch == 'i')
                                    state = State.C;
                                else
                                {
                                    errorMessage = "Ожидалось \"i\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.C:
                            {
                                if (ch == 'l')
                                    state = State.D;
                                else
                                {
                                    errorMessage = "Ожидалось \"l\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.D:
                            {
                                if (ch == 'e')
                                    state = State.E1;
                                else
                                {
                                    errorMessage = "Ожидалось \"e\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.E1:
                            {
                                if (ch == '(')
                                    state = State.H;
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \"(\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.H:
                            {
                                //simplexprcount++;
                                if (simplexprcount < 4)
                                {
                                    if (char.IsDigit(ch))
                                    {
                                        state = State.J2;
                                        constant = ch.ToString();
                                        simplexprcount++;
                                    }
                                    else if (ch == '+' || ch == '-')
                                    {
                                        state = State.J3;
                                        constant = ch.ToString();
                                        simplexprcount++;
                                    }
                                    else if (ch == '_' || char.IsLetter(ch))
                                    {
                                        state = State.J1;
                                        identifiyer = ch.ToString();
                                        simplexprcount++;
                                    }
                                    else if (ch != ' ')
                                    {
                                        errorMessage = "Ожидалось \" \", \"+\", \"-\",\"_\", буква или цифра!";
                                        state = State.E;
                                    }
                                }
                                else 
                                {
                                    errorMessage = "Количество simple-expr в expr не должно превышать 4-х!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.J1:
                            {
                                if (ch == ' ')
                                {
                                    state = State.K2;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else 
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '[')
                                {
                                    state = State.K1;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '!' || ch == '=')
                                {
                                    state = State.EQ;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '<' || ch == '>')
                                {
                                    state = State.BL;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '&')
                                {
                                    state = State.H1;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '|')
                                {
                                    state = State.H2;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == ')')
                                {
                                    state = State.Y;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                //
                                //
                                //
                                else if (ch == '_' || char.IsLetter(ch) || char.IsDigit(ch))
                                    identifiyer += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \"[\", \"!\", \"=\", \"<\", \">\", \"&\", \"|\", \")\",\"_\" или буква!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.K2:
                            {
                                if (ch == '[')
                                    state = State.K1;
                                else if (ch == '!' || ch == '=')
                                    state = State.EQ;
                                else if (ch == '<' || ch == '>')
                                    state = State.BL;
                                else if (ch == '&')
                                    state = State.H1;
                                else if (ch == '|')
                                    state = State.H2;
                                else if (ch == ')')
                                    state = State.Y;
                                //
                                //
                                //
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \", \"[\", \"!\", \"=\", \"<\", \">\", \"&\", \"|\", \")\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.K1:
                            {
                                if (ch == '_' || char.IsLetter(ch))
                                {
                                    state = State.M1;
                                    identifiyer = ch.ToString();
                                }
                                else if (ch == '+' || ch == '-')
                                {
                                    state = State.K5;
                                    constant = ch.ToString();
                                }
                                else if (char.IsDigit(ch))
                                {
                                    state = State.M2;
                                    constant = ch.ToString();
                                }
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \", \"+\", \"-\",\"_\", буква или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.M1:
                            {
                                if (ch == ' ')
                                {
                                    state = State.N2;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == ']')
                                {
                                    state = State.O;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '_' || char.IsLetter(ch) || char.IsDigit(ch))
                                    identifiyer += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \"]\",\"_\", буква или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.K5:
                            {
                                if (char.IsDigit(ch))
                                {
                                    state = State.M2;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалась цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.M2:
                            {
                                if (ch == ' ')
                                {
                                    state = State.N2;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == ']')
                                {
                                    state = State.O;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (char.IsDigit(ch))
                                    constant += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \"]\", или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.N2:
                            {
                                if (ch == ']')
                                    state = State.O;
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \" или \"]\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.O:
                            {
                                if (ch == '!' || ch == '=')
                                    state = State.EQ;
                                else if (ch == '<' || ch == '>')
                                    state = State.BL;
                                else if (ch == '&')
                                    state = State.H1;
                                else if (ch == '|')
                                    state = State.H2;
                                else if (ch == ')')
                                    state = State.Y;
                                //
                                //
                                //
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \"!\", \"=\", \"<\", \">\", \"&\", \"|\", \")\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.J2:
                            {
                                if (ch == '.')
                                {
                                    state = State.K4;
                                    constant += ch;
                                }
                                else if (ch == ' ')
                                {
                                    state = State.N3;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '!' || ch == '=')
                                {
                                    state = State.EQ;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '<' || ch == '>')
                                {
                                    state = State.BL;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '&')
                                {
                                    state = State.H1;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '|')
                                {
                                    state = State.H2;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == ')')
                                {
                                    state = State.Y;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == 'e')
                                {
                                    state = State.L2;
                                    constant += ch;
                                }
                                //
                                //
                                //
                                else if (char.IsDigit(ch))
                                    constant += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \".\", \"!\", \"=\", \"<\", \">\", \"&\", \"|\", \")\" или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                                
                        case State.J3:
                            {
                                if (char.IsDigit(ch))
                                {
                                    state = State.J2;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалась цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.K4:
                            {
                                if (char.IsDigit(ch))
                                {
                                    state = State.K3;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалась цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.K3:
                            {
                                if (ch == 'e')
                                {
                                    state = State.L2;
                                    constant += ch;
                                }
                                else if (ch == ' ')
                                {
                                    state = State.N3;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == '!' || ch == '=')
                                {
                                    state = State.EQ;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == '<' || ch == '>')
                                {
                                    state = State.BL;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == '&')
                                {
                                    state = State.H1;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == '|')
                                {
                                    state = State.H2;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == ')')
                                {
                                    state = State.Y;
                                    constantList.AddLast(constant);
                                }
                                //
                                //
                                //
                                else if (char.IsDigit(ch))
                                    constant += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \"e\", \" \", \"!\", \"=\", \"<\", \">\", \"&\", \"|\", \")\" или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.L2:
                            {
                                if (ch == '+' || ch == '-')
                                {
                                    state = State.M4;
                                    constant += ch;
                                }
                                else if (char.IsDigit(ch))
                                {
                                    state = State.M3;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалось \"+\", \"-\" или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.M4:
                            {
                                if (char.IsDigit(ch))
                                {
                                    state = State.M3;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалaсь цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.M3:
                            {
                                if (ch == ' ')
                                {
                                    state = State.N3;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == '!' || ch == '=')
                                {
                                    state = State.EQ;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == '<' || ch == '>')
                                {
                                    state = State.BL;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == '&')
                                {
                                    state = State.H1;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == '|')
                                {
                                    state = State.H2;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == ')')
                                {
                                    state = State.Y;
                                    constantList.AddLast(constant);
                                }
                                //
                                //
                                //
                                else if (char.IsDigit(ch))
                                    constant += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \"!\", \"=\", \"<\", \">\", \"&\", \"|\", \")\" или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.N3:
                            {
                                if (ch == '!' || ch == '=')
                                    state = State.EQ;
                                else if (ch == '<' || ch == '>')
                                    state = State.BL;
                                else if (ch == '&')
                                    state = State.H1;
                                else if (ch == '|')
                                    state = State.H2;
                                else if (ch == ')')
                                    state = State.Y;
                                //
                                //
                                //
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \", \"!\", \"=\", \"<\", \">\", \"&\", \"|\", \")\" или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.H1:
                            {
                                if (ch == '&')
                                    state = State.H;
                                else 
                                {
                                    errorMessage = "Ожидалось \"&\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.H2:
                            {
                                if (ch == '|')
                                    state = State.H;
                                else 
                                {
                                    errorMessage = "Ожидалось \"|\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.EQ:
                            {
                                if (ch == '=')
                                    state = State.Z1;
                                else
                                {
                                    errorMessage = "Ожидалось \"=\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.BL:
                            {
                                if (char.IsDigit(ch))
                                {
                                    state = State._J2;
                                    constant = ch.ToString();
                                }
                                else if (char.IsLetter(ch) || ch == '_')
                                {
                                    state = State._J1;
                                    identifiyer = ch.ToString();
                                }
                                else if (ch == '+' || ch == '-')
                                {
                                    state = State.Z2;
                                    constant = ch.ToString();
                                }
                                else if (ch == '=' || ch == ' ')
                                {
                                    state = State.Z1;
                                }
                                else
                                {
                                    errorMessage = "Ожидалось \"=\", \"+\", \"-\", \"_\", \" \" буква или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.Z1:
                            {
                                if (ch == '+' || ch == '-')
                                {
                                    state = State.Z2;
                                    constant = ch.ToString();
                                }
                                else if (char.IsDigit(ch))
                                {
                                    state = State._J2;
                                    constant = ch.ToString();
                                }
                                else if (char.IsLetter(ch) || ch == '_')
                                {
                                    state = State._J1;
                                    identifiyer = ch.ToString();
                                }
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \"=\", \"+\", \"-\", \"_\", \" \" буква или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.Z2:
                            {
                                if (char.IsDigit(ch))
                                {
                                    state = State._J2;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалась цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._J1:
                            {
                                if (ch == ' ')
                                {
                                    state = State._K2;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '[')
                                {
                                    state = State._K1;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '&')
                                {
                                    state = State.H1;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '|')
                                {
                                    state = State.H2;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == ')')
                                {
                                    state = State.Y;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                //
                                //
                                //
                                else if (ch == '_' || char.IsLetter(ch) || char.IsDigit(ch))
                                    identifiyer += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \"[\", \"&\", \"|\", \")\",\"_\" или буква!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._K2:
                            {
                                if (ch == '[')
                                    state = State._K1;
                                else if (ch == '&')
                                    state = State.H1;
                                else if (ch == '|')
                                    state = State.H2;
                                else if (ch == ')')
                                    state = State.Y;
                                //
                                //
                                //
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \", \"[\", \"&\", \"|\", \")\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._K1:
                            {
                                if (ch == '_' || char.IsLetter(ch))
                                {
                                    state = State._M1;
                                    identifiyer = ch.ToString();
                                }
                                else if (ch == '+' || ch == '-')
                                {
                                    state = State._K5;
                                    constant = ch.ToString();
                                }
                                else if (char.IsDigit(ch))
                                {
                                    state = State._M2;
                                    constant = ch.ToString();
                                }
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \", \"+\", \"-\",\"_\", буква или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._M1:
                            {
                                if (ch == ' ')
                                {
                                    state = State._N1;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == ']')
                                {
                                    state = State._O;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '_' || char.IsLetter(ch) || char.IsDigit(ch))
                                    identifiyer += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \"]\",\"_\", буква или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._K5:
                            {
                                if (char.IsDigit(ch))
                                {
                                    state = State._M2;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалась цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._M2:
                            {
                                if (ch == ' ')
                                {
                                    state = State._N1;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == ']')
                                {
                                    state = State._O;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (char.IsDigit(ch))
                                    constant += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \"]\", или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._N1:
                            {
                                if (ch == ']')
                                    state = State._O;
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \" или \"]\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._O:
                            {
                                if (ch == '&')
                                    state = State.H1;
                                else if (ch == '|')
                                    state = State.H2;
                                else if (ch == ')')
                                    state = State.Y;
                                //
                                //
                                //
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \"!\", \"=\", \"<\", \">\", \"&\", \"|\", \")\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._J2:
                            {
                                if (ch == '.')
                                {
                                    state = State._K3;
                                    constant += ch;
                                }
                                else if (ch == ' ')
                                {
                                    state = State._N3;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '&')
                                {
                                    state = State.H1;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '|')
                                {
                                    state = State.H2;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == ')')
                                {
                                    state = State.Y;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == 'e')
                                {
                                    state = State._L2;
                                    constant += ch;
                                }
                                //
                                //
                                //
                                else if (char.IsDigit(ch))
                                    constant += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \".\", \"&\", \"|\", \")\" или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._K3:
                            {
                                if (char.IsDigit(ch))
                                {
                                    state = State._K4;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалась цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._K4:
                            {
                                if (ch == 'e')
                                {
                                    state = State._L2;
                                    constant += ch;
                                }
                                else if (ch == ' ')
                                {
                                    state = State._N3;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == '&')
                                {
                                    state = State.H1;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == '|')
                                {
                                    state = State.H2;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == ')')
                                {
                                    state = State.Y;
                                    constantList.AddLast(constant);
                                }
                                //
                                //
                                //
                                else if (char.IsDigit(ch))
                                    constant += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \"e\", \" \", \"&\", \"|\", \")\" или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._L2:
                            {
                                if (ch == '+' || ch == '-')
                                {
                                    state = State._M4;
                                    constant += ch;
                                }
                                else if (char.IsDigit(ch))
                                {
                                    state = State._M3;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалось \"+\", \"-\" или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._M4:
                            {
                                if (char.IsDigit(ch))
                                {
                                    state = State._M3;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалaсь цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._M3:
                            {
                                if (ch == ' ')
                                {
                                    state = State._N3;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == '&')
                                {
                                    state = State.H1;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == '|')
                                {
                                    state = State.H2;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == ')')
                                {
                                    state = State.Y;
                                    constantList.AddLast(constant);
                                }
                                //
                                //
                                //
                                else if (char.IsDigit(ch))
                                    constant += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \"&\", \"|\", \")\" или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._N3:
                            {
                                if (ch == '&')
                                    state = State.H1;
                                else if (ch == '|')
                                    state = State.H2;
                                else if (ch == ')')
                                    state = State.Y;
                                //
                                //
                                //
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \", \"&\", \"|\", \")\" или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State.Y:
                            {
                                if (ch == '{')
                                    state = State._Z1;
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \" или \"{\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._Z1:
                            {
                                if (ch == '_' || char.IsLetter(ch))
                                {
                                    state = State._J11;
                                    identifiyer = ch.ToString();
                                }
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \"_\" или буква!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._J11:
                            {
                                if (ch == ' ')
                                {
                                    state = State._K22;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '[')
                                {
                                    state = State._K12;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '=')
                                {
                                    state = State._Q;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                //
                                //
                                //
                                else if (ch == '_' || char.IsLetter(ch) || char.IsDigit(ch))
                                    identifiyer += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \"[\", \"&\", \"|\", \")\",\"_\" или буква!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._K22:
                            {
                                if (ch == '[')
                                    state = State._K12;
                                else if (ch == '=')
                                    state = State._Q;
                                //
                                //
                                //
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \", \"=\", \"[\" !";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._K12:
                            {
                                if (ch == '_' || char.IsLetter(ch))
                                {
                                    state = State._M12;
                                    identifiyer = ch.ToString();
                                }
                                else if (ch == '+' || ch == '-')
                                {
                                    state = State._K13;
                                    constant = ch.ToString();
                                }
                                else if (char.IsDigit(ch))
                                {
                                    state = State._M22;
                                    constant = ch.ToString();
                                }
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \", \"+\", \"-\",\"_\", буква или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._M12:
                            {
                                if (ch == ' ')
                                {
                                    state = State._N12;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == ']')
                                {
                                    state = State._O2;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '_' || char.IsLetter(ch) || char.IsDigit(ch))
                                    identifiyer += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \"]\",\"_\", буква или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._K13:
                            {
                                if (char.IsDigit(ch))
                                {
                                    state = State._M22;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалась цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._M22:
                            {
                                if (ch == ' ')
                                {
                                    state = State._N12;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == ']')
                                {
                                    state = State._O2;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (char.IsDigit(ch))
                                    constant += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \"]\", или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._N12:
                            {
                                if (ch == ']')
                                    state = State._O2;
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \" или \"]\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._O2:
                            {
                                if (ch == '=')
                                    state = State._Q;
                                //
                                //
                                //
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \"=\" !";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._Q:
                            {
                                if (ch == '_' || char.IsLetter(ch))
                                {
                                    state = State._T1;
                                    identifiyer = ch.ToString();
                                }
                                else if (ch == '+' || ch == '-')
                                {
                                    state = State._T2;
                                    constant = ch.ToString();
                                }
                                else if (char.IsDigit(ch))
                                {
                                    state = State._T12;
                                    constant = ch.ToString();
                                }
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \" \"_\", \"+\", \"-\" буква или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._T1:
                            {
                                if (ch == ' ')
                                {
                                    state = State._U22;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '[')
                                {
                                    state = State._U21;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '%')
                                {
                                    state = State._Q;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == ';')
                                {
                                    state = State._Z2;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                //
                                //
                                //
                                else if (ch == '_' || char.IsLetter(ch) || char.IsDigit(ch))
                                    identifiyer += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \"[\", \"+\", \"-\", \"*\", \"/\", \"%\", \"_\", \";\" или буква!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._U22:
                            {
                                if (ch == '[')
                                    state = State._U21;
                                else if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '%')
                                    state = State._Q;
                                else if (ch == ';')
                                    state = State._Z2;
                                //
                                //
                                //
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \", \"[\", \"+\", \"-\", \"*\", \"/\", \"%\", \"_\", \";\" или буква!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._U21:
                            {
                                if (ch == '_' || char.IsLetter(ch))
                                {
                                    state = State._V21;
                                    identifiyer = ch.ToString();
                                }
                                else if (ch == '+' || ch == '-')
                                {
                                    state = State._V22;
                                    constant = ch.ToString();
                                }
                                else if (char.IsDigit(ch))
                                {
                                    state = State._V23;
                                    constant = ch.ToString();
                                }
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \", \"+\", \"-\",\"_\", буква или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._V21:
                            {
                                if (ch == ' ')
                                {
                                    state = State._V24;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == ']')
                                {
                                    state = State._O3;
                                    if (CheckId(identifiyer))
                                        identifiersList.AddLast(identifiyer);
                                    else
                                    {
                                        errorMessage = "Некорректный идентификатор!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '_' || char.IsLetter(ch) || char.IsDigit(ch))
                                    identifiyer += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \"]\",\"_\", буква или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._V22:
                            {
                                if (char.IsDigit(ch))
                                {
                                    state = State._V23;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалась цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._V23:
                            {
                                if (ch == ' ')
                                {
                                    state = State._V24;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == ']')
                                {
                                    state = State._O3;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (char.IsDigit(ch))
                                    constant += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \"]\", или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._V24:
                            {
                                if (ch == ']')
                                    state = State._O3;
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \" или \"]\"!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._O3:
                            {
                                if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '%')
                                    state = State._Q;
                                else if (ch == ';')
                                    state = State._Z2;
                                //
                                //
                                //
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \"+\", \"-\", \"*\", \"/\", \"%\", \";\", \" \" !";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._T2:
                            {
                                if (char.IsDigit(ch))
                                {
                                    state = State._T12;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалась цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._T12:
                            {
                                if (ch == '.')
                                {
                                    state = State._U11;
                                    constant += ch;
                                }
                                else if (ch == ' ')
                                {
                                    state = State._N13;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '%')
                                {
                                    state = State._Q;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == ';')
                                {
                                    state = State._Z2;
                                    if (CheckConst(constant))
                                        constantList.AddLast(constant);
                                    else
                                    {
                                        errorMessage = "Некорректная константа!";
                                        state = State.E;
                                    }
                                }
                                else if (ch == 'e')
                                {
                                    state = State._V11;
                                    constant += ch;
                                }
                                //
                                //
                                //
                                else if (char.IsDigit(ch))
                                    constant += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \".\", \"+\", \"-\", \"*\", \"/\", \"%\", \";\" или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._U11:
                            {
                                if (char.IsDigit(ch))
                                {
                                    state = State._U12;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалась цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._U12:
                            {
                                if (ch == 'e')
                                {
                                    state = State._V11;
                                    constant += ch;
                                }
                                else if (ch == ' ')
                                {
                                    state = State._N13;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '%')
                                {
                                    state = State._Q;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == ';')
                                {
                                    state = State._Z2;
                                    constantList.AddLast(constant);
                                }
                                //
                                //
                                //
                                else if (char.IsDigit(ch))
                                    constant += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \"e\", \" \", \"+\", \"-\", \"*\", \"/\", \"%\", \";\" или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._V11:
                            {
                                if (ch == '+' || ch == '-')
                                {
                                    state = State._V12;
                                    constant += ch;
                                }
                                else if (char.IsDigit(ch))
                                {
                                    state = State._V13;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалось \"+\", \"-\" или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._V12:
                            {
                                if (char.IsDigit(ch))
                                {
                                    state = State._V13;
                                    constant += ch;
                                }
                                else
                                {
                                    errorMessage = "Ожидалaсь цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._V13:
                            {
                                if (ch == ' ')
                                {
                                    state = State._N13;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '%')
                                {
                                    state = State._Q;
                                    constantList.AddLast(constant);
                                }
                                else if (ch == ';')
                                {
                                    state = State._Z2;
                                    constantList.AddLast(constant);
                                }
                                //
                                //
                                //
                                else if (char.IsDigit(ch))
                                    constant += ch;
                                else
                                {
                                    errorMessage = "Ожидалось \" \", \"+\", \"-\", \"*\", \"/\", \"%\", \";\" или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._N13:
                            {
                                if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '%')
                                    state = State._Q;
                                else if (ch == ';')
                                    state = State._Z2;
                                //
                                //
                                //
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \", \"+\", \"-\", \"*\", \"/\", \"%\", \";\" или цифра!";
                                    state = State.E;
                                }
                                break;
                            }
                        case State._Z2:
                            {
                                if (ch == '}')
                                    state = State.F;
                                else if (ch == '_' || char.IsLetter(ch))
                                {
                                    state = State._J11;
                                    identifiyer = ch.ToString();
                                }
                                else if (ch != ' ')
                                {
                                    errorMessage = "Ожидалось \" \", \"}\" !";
                                    state = State.E;
                                }
                                break;
                            }
                        default:
                            {
                                errorMessage = "Неизвестная ошибка!";
                                state = State.E;
                                break;
                            }
                    }
                    //position++;
                }
                else 
                {
                    errorMessage = "Курсор вышел за пределы строки!";
                    state = State.E;
                }
                position++;
            }
            if (state == State.E)
            {
                errorPosition = position--;
            }
            return state == State.F;
        }
    }
}
