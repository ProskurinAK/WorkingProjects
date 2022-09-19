using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Diagnostics;
// ************************************************************************************************************

namespace RandomForest
{
    class RandomForestClassifier
    {
        private static int AmountOfSamplesAndTrees = 3;    // Количество созданных бутстрэп выборок и деревьев

        private static List<List<double>> DataSetFeatures = new List<List<double>>();
        private static List<double> DataSetTargets = new List<double>();

        public static List<List<double>> DataSetFeaturesForTraining = new List<List<double>>();
        public static List<double> DataSetTargetsForTraining = new List<double>();

        private static List<List<double>> DataSetFeaturesForTest = new List<List<double>>();
        private static List<double> DataSetTargetsForTest = new List<double>();

        private static List<List<double>> PredictionResult = new List<List<double>>();  // Список ответов для тестовой выборки на основании предикатов для каждой модели дерева
        private static List<double> FinalPredict = new List<double>();  // Финальное предсказание для каждого объекта на основе всех простых моделей дерева

        // Структура хранения частей данных для записи в DataSetFeature (используется при распараллеливании чтения данных)
        struct ThreadRange
        {
            public int Start;
            public int Stop;
            public List<List<double>> DataInRange;
            public string Path;
        }
        // ------------------------------------------------------------------------------------------------------------
        static void Main(string[] args)
        {
            Stopwatch StopW = new Stopwatch();
            StopW.Start();

            MakeTrainingSet();
            //ShowInfo();

            for (int i = 0; i < AmountOfSamplesAndTrees; i++)
            {
                Console.WriteLine("--------------------------------------- Create bootstrap sample ---------------------------------------");
                BootstrapSample NewSample = new BootstrapSample(i);
            }

            DataSetFeaturesForTraining.Clear();
            DataSetTargetsForTraining.Clear();

            for (int i = 0; i < AmountOfSamplesAndTrees; i++)
            {
                Console.WriteLine("--------------------------------------- Decision tree start ---------------------------------------");
                DecisionTree NewTree = new DecisionTree(i);
            }

            MakeTestSet();
            //ShowInfo();
            Compute();
            PredictionAccuarcy();

            StopW.Stop();
            Console.WriteLine("Time = " + StopW.Elapsed);
        }
        // ------------------------------------------------------------------------------------------------------------
        private static void MakeDataSetFeatures(int AmountOfThread, int[,] Borders, string Path)
        {
            // AmountOfThread - Количество создаваемых потоков
            // Borders - Номера файлов из всего массива (границы) считываемых из каталога с данными
            // Path - Путь до каталога с данными

            List<ThreadRange> ListThreadRange = new List<ThreadRange>();    // Список структур с данными в заданных границах

            // Цикл создания структур с данными в заданных гнраницах и добавление их в список
            for (int i = 0; i < AmountOfThread; i++)
            {
                List<List<double>> Data = new List<List<double>>();

                ThreadRange NewTreadRange = new ThreadRange();
                NewTreadRange.Start = Borders[i, 0];
                NewTreadRange.Stop = Borders[i, 1];
                NewTreadRange.DataInRange = Data;
                NewTreadRange.Path = Path;

                ListThreadRange.Add(NewTreadRange);
            }

            Thread[] Threads = new Thread[AmountOfThread];  // Массив создаваемых потоков

            // Цикл создания новых потоков и добавления их в массив
            for (int i = 0; i < AmountOfThread; i++)
            {
                Thread NewThread = new Thread(ReadDataSetFeatures);
                Threads[i] = NewThread;
                NewThread.Start(ListThreadRange[i]);
            }

            // Цикл ожидания отрабатывания всех потоков
            for (int i = 0; i < AmountOfThread; i++)
            {
                Threads[i].Join();
            }

            // Цикл добавления данных их всех структур в одну
            for (int i = 0; i < ListThreadRange.Count - 1; i++)
            {
                ListThreadRange[0].DataInRange.AddRange(ListThreadRange[i + 1].DataInRange);
            }

            // Добавление всех считанных параллельно данных в один список
            DataSetFeatures.AddRange(ListThreadRange[0].DataInRange);
        }
        // ------------------------------------------------------------------------------------------------------------
        private static void ReadDataSetFeatures(object Obj)
        {
            if (Obj is ThreadRange NewThreadRange)
            {
                string[] FilesName = Directory.GetFiles(NewThreadRange.Path);

                //for (int i = 0; i < FilesName.GetLength(0); i++)
                //{
                //    Console.WriteLine(FilesName[i]);
                //}
                //Console.WriteLine(FilesName.Length);

                int FirstRow = 157; // номер первой строки с данными

                for (int i = NewThreadRange.Start; i < NewThreadRange.Stop; i++)
                {
                    StreamReader Sr = new StreamReader(FilesName[i]);

                    int AmountOfLines = File.ReadAllLines(FilesName[i]).Length; // количество строк в файле

                    for (int j = 0; j < AmountOfLines; j++)
                    {
                        string Line = Sr.ReadLine();

                        if (j >= FirstRow)
                        {
                            string[] Values = Line.Split(new char[] { ' ', ':', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                            if (Values.GetLength(0) == 0)   // пропуск пыстых строк в файле
                            {
                                continue;
                            }

                            List<double> RowInDataSetFeatures = new List<double>();
                            for (int k = 3; k < Values.GetLength(0); k++)   // k = 3 чтобы отбросить время из файла(первые 3 значения)
                            {
                                try    // проверка на то есть ли значение в ячейке признака, если нет то заполняется нулём
                                {
                                    RowInDataSetFeatures.Add(Convert.ToDouble(Values[k]));
                                }
                                catch
                                {
                                    RowInDataSetFeatures.Add(0);
                                }
                            }

                            NewThreadRange.DataInRange.Add(RowInDataSetFeatures);
                        }
                    }
                }
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        private static void MakeDataSetTargets(int AmountOfZeroTargets)
        {
            for (int i = 0; i < DataSetFeatures.Count; i++)
            {
                if (i < AmountOfZeroTargets)
                {
                    DataSetTargets.Add(0);
                }
                else
                {
                    DataSetTargets.Add(1);
                }
                
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        private static void MakeTrainingSet()
        {
            // Функция создание тренировочной выборки

            int AmountOfThread1 = 6;
            int[,] Borders1 = { { 0, 3 }, { 3, 6 }, { 6, 9 }, { 9, 12 }, { 12, 15 }, { 15, 18 } };
            string Path1 = @"D:\Работа\EnsembleOfModels\Bagging\RandomForest\Data\2020";
            MakeDataSetFeatures(AmountOfThread1, Borders1, Path1);

            int AmountOfZeroTargets = DataSetFeatures.Count;

            int AmountOfThread2 = 6;
            int[,] Borders2 = { { 0, 3 }, { 3, 6 }, { 6, 9 }, { 9, 12 }, { 12, 15 }, { 15, 18 } };
            string Path2 = @"D:\Работа\EnsembleOfModels\Bagging\RandomForest\Data\2021";
            MakeDataSetFeatures(AmountOfThread2, Borders2, Path2);

            MakeDataSetTargets(AmountOfZeroTargets);

            DataSetFeaturesForTraining.AddRange(DataSetFeatures);
            DataSetFeatures.Clear();
            DataSetTargetsForTraining.AddRange(DataSetTargets);
            DataSetTargets.Clear();
        }
        // ------------------------------------------------------------------------------------------------------------ 
        private static void MakeTestSet()
        {
            // Функция создание тестовой выборки

            int AmountOfThread1 = 6;
            int[,] Borders1 = { { 18, 21 }, { 21, 24 }, { 24, 27 }, { 27, 30 }, { 30, 33 }, { 33, 35 } };
            string Path1 = @"D:\Работа\EnsembleOfModels\Bagging\RandomForest\Data\2020";
            MakeDataSetFeatures(AmountOfThread1, Borders1, Path1);

            int AmountOfZeroTargets = DataSetFeatures.Count;

            int AmountOfThread2 = 6;
            int[,] Borders2 = { { 18, 21 }, { 21, 24 }, { 24, 27 }, { 27, 30 }, { 30, 33 }, { 33, 36 } };
            string Path2 = @"D:\Работа\EnsembleOfModels\Bagging\RandomForest\Data\2021";
            MakeDataSetFeatures(AmountOfThread2, Borders2, Path2);

            MakeDataSetTargets(AmountOfZeroTargets);

            DataSetFeaturesForTest.AddRange(DataSetFeatures);
            DataSetFeatures.Clear();
            DataSetTargetsForTest.AddRange(DataSetTargets);
            DataSetTargets.Clear();
        }
        // ------------------------------------------------------------------------------------------------------------
        private static void Compute()
        {
            // Функция предсказания ответа на основании полученных предикатов

            int AmountOfAllPredicates = AmountOfSamplesAndTrees;

            for (int NumbOfPredicate = 0; NumbOfPredicate < AmountOfAllPredicates; NumbOfPredicate++)
            {
                List<List<double>> Predicates = new List<List<double>>();   // Матрица предикатов
                List<int> NumbOfFeature = new List<int>();  // Номера фич из исходного датасета

                List<double> ListOfPrediction = new List<double>();

                StreamReader Sr = new StreamReader($@"D:\Работа\EnsembleOfModels\Bagging\RandomForest\Predicates\Predicate{NumbOfPredicate}.txt");

                int AmountOfRows = System.IO.File.ReadAllLines($@"D:\Работа\EnsembleOfModels\Bagging\RandomForest\Predicates\Predicate{NumbOfPredicate}.txt").Length;

                // Чтение предикатов и номеров признаков из файла
                for (int i = 0; i < AmountOfRows; i++)
                {
                    if (i == 0)
                    {
                        string Line = Sr.ReadLine();

                        string[] Values = Line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        for (int j = 0; j < Values.GetLength(0); j++)
                        {
                            NumbOfFeature.Add(Convert.ToInt32(Values[j]));
                        }
                    }
                    else
                    {
                        string Line = Sr.ReadLine();

                        string[] Values = Line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        List<double> RowInPredicates = new List<double>();

                        for (int j = 0; j < Values.GetLength(0); j++)
                        {
                            RowInPredicates.Add(Convert.ToDouble(Values[j]));
                        }
                        Predicates.Add(RowInPredicates);
                    }
                }

                //for (int i = 0; i < NumbOfFeature.Count; i++)
                //{
                //    Console.Write(NumbOfFeature[i] + " ");
                //}
                //Console.WriteLine();

                //for (int i = 0; i < Predicates.Count; i++)
                //{
                //    for (int j = 0; j < Predicates[i].Count; j++)
                //    {
                //        Console.Write(Predicates[i][j] + "\t");
                //    }
                //    Console.WriteLine();
                //}

                // Замена индексов в первом столбце на индексы соответсвтующие реальному датасету
                for (int i = 0; i < Predicates.Count; i++)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        for (int k = 0; k < NumbOfFeature.Count; k++)
                        {
                            if (Predicates[i][j] == k)
                            {
                                Predicates[i][j] = NumbOfFeature[k];
                                break;
                            }
                        }
                    }
                }

                //Console.WriteLine("-------------------------------");
                //for (int i = 0; i < Predicates.Count; i++)
                //{
                //    for (int j = 0; j < Predicates[i].Count; j++)
                //    {
                //        Console.Write(Predicates[i][j] + "\t");
                //    }
                //    Console.WriteLine();
                //}

                int AmountOfAllObjects = DataSetFeaturesForTest.Count;   // Количество всех объектов в тестовой выборке

                for (int NumbOfObject = 0; NumbOfObject < AmountOfAllObjects; NumbOfObject++)
                {
                    double Prediction = -1;  // Полученное предсказание

                    for (int i = 0; i < Predicates.Count; i++)
                    {
                        for (int k = 0; k < DataSetFeaturesForTest[NumbOfObject].Count; k++)
                        {
                            if (Predicates[i][0] == k)
                            {
                                if (Predicates[i][2] == 1)
                                {
                                    if (DataSetFeaturesForTest[NumbOfObject][k] >= Predicates[i][1])
                                    {
                                        Prediction = Predicates[i][3];
                                        break;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else if (Predicates[i][2] == 0)
                                {
                                    if (DataSetFeaturesForTest[NumbOfObject][k] <= Predicates[i][1])
                                    {
                                        Prediction = Predicates[i][3];
                                        break;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        if (Prediction >= 0)
                        {
                            break;
                        }
                    }
                    ListOfPrediction.Add(Prediction);
                }
                //Console.Write("List of prediction - ");
                //for (int i = 0; i < ListOfPrediction.Count; i++)
                //{
                //    Console.Write(ListOfPrediction[i] + " ");
                //}
                //Console.WriteLine();

                PredictionResult.Add(ListOfPrediction);
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        private static void PredictionAccuarcy()
        {
            // подсчёт количества совпадений и общий процент правильных предсказаний для каждой из моделей

            for (int i = 0; i < PredictionResult.Count; i++)
            {
                int Counter = 0;
                double Percent = 0;

                for (int j = 0; j < PredictionResult[i].Count; j++)
                {
                    if (PredictionResult[i][j] == DataSetTargetsForTest[j])
                    {
                        Counter++;
                    }
                }
                Percent = (double)Counter / PredictionResult[i].Count * 100;

                Console.WriteLine($"Counter for {i} model = " + Counter);
                Console.WriteLine($"Percent of {i} model = " + Percent + "%");
            }
            Console.WriteLine("-----------------------------------------------------------");

            // выбор лучшего предсказания из всех моделей
            for (int j = 0; j < PredictionResult[0].Count; j++)
            {
                int NumbOf0 = 0;
                int NumbOf1 = 0;

                for (int i = 0; i < PredictionResult.Count; i++)
                {
                    if (PredictionResult[i][j] == 0)
                    {
                        NumbOf0++;
                    }
                    else if (PredictionResult[i][j] == 1)
                    {
                        NumbOf1++;
                    }
                }

                if (NumbOf0 > NumbOf1)
                {
                    FinalPredict.Add(0);
                }
                else if (NumbOf0 < NumbOf1)
                {
                    FinalPredict.Add(1);
                }
            }

            //for (int i = 0; i < FinalPredict.Count; i++)
            //{
            //    Console.Write(FinalPredict[i]);
            //}
            //Console.WriteLine();

            // подсчёт количества совпадений и общий процент правильных предсказаний для всех моделей
            int FinalCounter = 0;
            double FinalPercent = 0;

            for (int i = 0; i < FinalPredict.Count; i++)
            {
                if (FinalPredict[i] == DataSetTargetsForTest[i])
                {
                    FinalCounter++;
                }
            }

            FinalPercent = (double)FinalCounter / FinalPredict.Count * 100;

            Console.WriteLine("Final Counter = " + FinalCounter);
            Console.WriteLine("Final Percent = " + FinalPercent + "%");
        }
        // ------------------------------------------------------------------------------------------------------------
        private static void ShowInfo()
        {
            Console.WriteLine("DataSetFeatures.Count = " + DataSetFeatures.Count);
            Console.WriteLine("DataSetTargets.Count = " + DataSetTargets.Count);

            for (int i = 0; i < DataSetFeaturesForTest.Count; i++)
            {
                if (i == 0 || i == DataSetFeaturesForTest.Count - 1)
                {
                    for (int j = 0; j < DataSetFeaturesForTest[i].Count; j++)
                    {
                        Console.Write(DataSetFeaturesForTest[i][j] + "\t");
                    }
                    Console.WriteLine();
                }
            }

            Console.WriteLine("DataSetFeaturesForTest row count = " + DataSetFeaturesForTest.Count);

            Console.WriteLine("DataSetTargetsForTest[0] = " + DataSetTargetsForTest[0]);
            Console.WriteLine("DataSetTargetsForTest[352042] = " + DataSetTargetsForTest[352042]);
            Console.WriteLine("DataSetTargetsForTest[352043] = " + DataSetTargetsForTest[352043]);
            Console.WriteLine("DataSetTargetsForTest[352044] = " + DataSetTargetsForTest[352044]);
            Console.WriteLine("DataSetTargetsForTest[DataSetTest.Count - 1] = " + DataSetTargetsForTest[DataSetTargetsForTest.Count - 1]);

            // ------------------------------------------------------------------------------------------------------------

            //Console.WriteLine("DataSetFeatures.Count = " + DataSetFeatures.Count);
            //Console.WriteLine("DataSetTargets.Count = " + DataSetTargets.Count);

            //for (int i = 0; i < DataSetFeaturesForTraining.Count; i++)
            //{
            //    if (i == 0 || i == DataSetFeaturesForTraining.Count - 1)
            //    {
            //        for (int j = 0; j < DataSetFeaturesForTraining[i].Count; j++)
            //        {
            //            Console.Write(DataSetFeaturesForTraining[i][j] + "\t");
            //        }
            //        Console.WriteLine();
            //    }
            //}

            //Console.WriteLine("DataSetFeaturesForTraining row count = " + DataSetFeaturesForTraining.Count);

            //Console.WriteLine("DataSetTargetsForTraining[0] = " + DataSetTargetsForTraining[0]);
            //Console.WriteLine("DataSetTargetsForTraining[445120] = " + DataSetTargetsForTraining[445120]);
            //Console.WriteLine("DataSetTargetsForTraining[445121] = " + DataSetTargetsForTraining[445121]);
            //Console.WriteLine("DataSetTargetsForTraining[445122] = " + DataSetTargetsForTraining[445122]);
            //Console.WriteLine("DataSetTargetsForTraining[DataSetTargets.Count - 1] = " + DataSetTargetsForTraining[DataSetTargetsForTraining.Count - 1]);

            // ------------------------------------------------------------------------------------------------------------

            //for (int i = 0; i < DataSetFeatures.Count; i++)
            //{
            //    if (i == 0 || i == DataSetFeatures.Count - 1)
            //    {
            //        for (int j = 0; j < DataSetFeatures[i].Count; j++)
            //        {
            //            Console.Write(DataSetFeatures[i][j] + "\t");
            //        }
            //        Console.WriteLine();
            //    }
            //}

            //Console.WriteLine("DataSetFeatures row count = " + DataSetFeatures.Count);

            //Console.WriteLine("DataSetTargets[0] = " + DataSetTargets[0]);
            //Console.WriteLine("DataSetTargets[797163] = " + DataSetTargets[797163]);
            //Console.WriteLine("DataSetTargets[797164] = " + DataSetTargets[797164]);
            //Console.WriteLine("DataSetTargets[797165] = " + DataSetTargets[797165]);
            //Console.WriteLine("DataSetTargets[DataSetTargets.Count - 1] = " + DataSetTargets[DataSetTargets.Count - 1]);
        }
        // ------------------------------------------------------------------------------------------------------------
    }
}
