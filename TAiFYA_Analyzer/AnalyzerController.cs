using System;
using System.Collections.Generic;
using System.Text;

namespace TAiFYA_Analyzer
{
    class AnalyzerController
    {
        private AnalyzerModel analyzerModel;                                                       //поле с объектом класса analyzermodel

        public AnalyzerController(string chain)                                                    //конструктор с параметром
        {
            analyzerModel = new AnalyzerModel(chain);
        }

        public int GetErrorPosition { get; set; }                                                  //свойство получения позиции ошибки
        public string GetErrorMessage { get; set; }                                                //свойство получения сообщения об ошибке
        public LinkedList<string> GetIdentifierList { get; set; }                                  //свойство получения списка идентификаторов
        public LinkedList<string> GetConstantList { get; set; }                                    //свойство получения списка констант

        public string Analyze()                                                                    //метод анализа принадлежности цепочки языку
        {
            if (analyzerModel.AnalyzeChain())
            {
                GetConstantList = analyzerModel.GetConstantList;
                GetIdentifierList = analyzerModel.GetIdentifierList;
                return "Ошибок не обнаружено";
            }
            else
            {
                GetErrorMessage = analyzerModel.GetErrorMessge;
                GetErrorPosition = analyzerModel.GetErrorPosition;
                return "Ошибка: " + GetErrorMessage + ", позиция " + GetErrorPosition;
            } 
        }


    }
}
