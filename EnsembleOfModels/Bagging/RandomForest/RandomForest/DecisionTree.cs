using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
// ************************************************************************************************************

namespace RandomForest
{
    class DecisionTree
    {
        private List<int> NumbOfFeature = new List<int>();  // Номера индексов признаков по которым будет создаваться предикат
        private List<List<double>> Bootstrap_Sample = new List<List<double>>();  // Считанная из файла бутстрэп выборка
        /*
         * 1 столбец - номер фичи с наилучшим предикатом
         * 2 столбец - значение по которому будет происходить разбиение (предикат)
         * 3 столбец - условие для значения по которому будет происходить разбиение
         * если 1, то >=
         * если 0, то <=
         * 4 столбец - значение которому равен целевой признак объекта по данному предикату
         */
        private List<List<double>> AllPredicates = new List<List<double>>();    // Результат работы класса - Созданные предикаты(узлы дерева)
        private int NumbOfModel;    // Номер модели в ансамбле
        // ------------------------------------------------------------------------------------------------------------
        public DecisionTree(int NumbOfModel)
        {
            this.NumbOfModel = NumbOfModel;
            List<List<double>> OnePredicate = new List<List<double>>();

            ReadBootstrapSample();

            // Цикл создания узлов дерева
            while (Bootstrap_Sample.Count > 1) // Условие создания новых узлов дерева
            {
                (Bootstrap_Sample, OnePredicate) = Training();

                for (int i = 0; i < OnePredicate.Count; i++)
                {
                    List<double> RowInAllPredicates = new List<double>();

                    for (int j = 0; j < OnePredicate[i].Count; j++)
                    {
                        RowInAllPredicates.Add(OnePredicate[i][j]);
                    }
                    AllPredicates.Add(RowInAllPredicates);
                }

                //Console.WriteLine("#######################################");
                //for (int i = 0; i < Bootstrap_Sample.Count; i++)
                //{
                //    for (int j = 0; j < Bootstrap_Sample[i].Count; j++)
                //    {
                //        Console.Write(Bootstrap_Sample[i][j] + "\t");
                //    }
                //    Console.WriteLine();
                //}

                //Console.WriteLine();

                //for (int i = 0; i < AllPredicates.Count; i++)
                //{
                //    for (int j = 0; j < AllPredicates[i].Count; j++)
                //    {
                //        Console.Write(AllPredicates[i][j] + "\t");
                //    }
                //    Console.WriteLine();
                //}
                //Console.WriteLine("#######################################");
            }
            Console.WriteLine("End Algorithm");

            WritePredicatesToFile();
            //ShowInfo();
        }
        // ------------------------------------------------------------------------------------------------------------
        private void ReadBootstrapSample()
        {
            // Функция чтения бутстрэп выборки из файла

            StreamReader Sr = new StreamReader($@"D:\Работа\EnsembleOfModels\Bagging\RandomForest\BootstrapSamples\BootstrapSample{NumbOfModel}.txt");

            int AmountOfRows = System.IO.File.ReadAllLines($@"D:\Работа\EnsembleOfModels\Bagging\RandomForest\BootstrapSamples\BootstrapSample{NumbOfModel}.txt").Length;

            // Построчное чтение всех данных из файла
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

                    List<double> RowInTraining_Sample = new List<double>();

                    for (int j = 0; j < Values.GetLength(0); j++)
                    {
                        RowInTraining_Sample.Add(Convert.ToDouble(Values[j]));
                    }
                    Bootstrap_Sample.Add(RowInTraining_Sample);
                }
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        private (List<List<double>>, List<List<double>>) Training()
        {
            // Функция обучения одного решающего дерева

            //for (int i = 0; i < Bootstrap_Sample.Count; i++)
            //{
            //    for (int j = 0; j < Bootstrap_Sample[i].Count; j++)
            //    {
            //        Console.Write(Bootstrap_Sample[i][j] + "\t");
            //    }
            //    Console.WriteLine();
            //}

            List<List<double>> AllDeltaOfGiniIndex = new List<List<double>>();  // Массив всех дельт индекса джини для разбиений на две части по каждой из фич
            List<List<double>> NewTraining_Sample = new List<List<double>>();  // Новая обучающая выборка, которую вернёт функция
            List<List<double>> Predicate = new List<List<double>>();  // Новый предикат, который вернёт функция

            for (int AmountOfFeature = 0; AmountOfFeature < Bootstrap_Sample[1].Count - 1; AmountOfFeature++)
            {
                // Алгоритм сортировки одной фичи обучающей выборки по возростанию методом сортировки выбором
                for (int i = 0; i < Bootstrap_Sample.Count; i++)
                {
                    int MinIndex = i;
                    List<double> TmpList = new List<double>();

                    for (int j = i + 1; j < Bootstrap_Sample.Count; j++)
                    {
                        if (Bootstrap_Sample[j][AmountOfFeature] < Bootstrap_Sample[MinIndex][AmountOfFeature])
                        {
                            MinIndex = j;
                        }
                    }
                    if (MinIndex != i)
                    {
                        for (int j = 0; j < Bootstrap_Sample[i].Count; j++)
                        {
                            TmpList.Add(Bootstrap_Sample[i][j]);
                        }

                        for (int j = 0; j < Bootstrap_Sample[i].Count; j++)
                        {
                            Bootstrap_Sample[i][j] = Bootstrap_Sample[MinIndex][j];
                        }

                        for (int j = 0; j < Bootstrap_Sample[i].Count; j++)
                        {
                            Bootstrap_Sample[MinIndex][j] = TmpList[j];
                        }
                    }
                }

                //Console.WriteLine($"--------------------------------Sorted by {AmountOfFeature} feature ----------------------------------------");
                //for (int i = 0; i < Bootstrap_Sample.Count; i++)
                //{
                //    for (int j = 0; j < Bootstrap_Sample[i].Count; j++)
                //    {
                //        Console.Write(Bootstrap_Sample[i][j] + "\t");
                //    }
                //    Console.WriteLine();
                //}

                // Выделение вектора целевого признака из обучающей выборки
                List<double> VectorOfTarget = new List<double>();

                for (int i = 0; i < Bootstrap_Sample.Count; i++)
                {
                    VectorOfTarget.Add(Bootstrap_Sample[i][Bootstrap_Sample[i].Count - 1]);
                }

                //for (int i = 0; i < VectorOfTarget.Count; i++)
                //{
                //    Console.WriteLine(VectorOfTarget[i]);
                //}
                //Console.WriteLine("-----------------------------------------------");

                int NumbOf0 = 0;
                int NumbOf1 = 0;

                for (int i = 0; i < VectorOfTarget.Count; i++)
                {
                    if (VectorOfTarget[i] == 0)
                    {
                        NumbOf0++;
                    }
                    else
                    {
                        NumbOf1++;
                    }
                }

                double GiniIndex = Math.Round((1 - Math.Pow((NumbOf0 / Convert.ToDouble(VectorOfTarget.Count)), 2) - Math.Pow((NumbOf1 / Convert.ToDouble(VectorOfTarget.Count)), 2)), 4);
                //Console.WriteLine("------ Gini index = " + GiniIndex);

                List<double> RowInAllDeltaOfGiniIndex = new List<double>(); // Список дельт индекса джини для каждой фичи
                int SampleSplitIndex = 0;   // Индекс на котором находится лучший предикат для разбиения
                bool FlagSampleSplitIndex = false;  // Переключается в true если наилучший предикат для разбиения находится не в первой фиче
                bool IsLeftSample = false;  // Переключается в true если индекс джини для левой части больше чем для правой
                bool IsLastNode = false;    // Переключается в true если индекс джини для левой и правой части равны

                for (int i = 0; i < VectorOfTarget.Count - 1; i++)
                {
                    if (VectorOfTarget[i] != VectorOfTarget[i + 1]) // Индекс джинни считается только в тех случаях, когда происходит изменение значения соседних целевых признаков
                    {
                        //Console.WriteLine("------------------------------------- New Iteration " + i + "-------------------------------------");

                        List<double> LeftSample = new List<double>();
                        List<double> RightSample = new List<double>();

                        for (int j = 0; j < VectorOfTarget.Count; j++)
                        {
                            if (j <= i)
                            {
                                LeftSample.Add(VectorOfTarget[j]);
                            }
                            else
                            {
                                RightSample.Add(VectorOfTarget[j]);
                            }
                        }

                        // Подсчёт кол-ва нулей и единиц в левой и правой выборке и нахождения индекса джини для них
                        int NumbOfLeft0 = 0;
                        int NumbOfLeft1 = 0;
                        int NumbOfRight0 = 0;
                        int NumbOfRight1 = 0;

                        for (int j = 0; j < LeftSample.Count; j++)
                        {
                            if (LeftSample[j] == 0)
                            {
                                NumbOfLeft0++;
                            }
                            else
                            {
                                NumbOfLeft1++;
                            }
                        }

                        for (int j = 0; j < RightSample.Count; j++)
                        {
                            if (RightSample[j] == 0)
                            {
                                NumbOfRight0++;
                            }
                            else
                            {
                                NumbOfRight1++;
                            }
                        }

                        double GiniIndexForLeftSample = Math.Round((1 - Math.Pow((NumbOfLeft0 / Convert.ToDouble(LeftSample.Count)), 2) - Math.Pow((NumbOfLeft1 / Convert.ToDouble(LeftSample.Count)), 2)), 4);
                        double GiniIndexForRightSample = Math.Round((1 - Math.Pow((NumbOfRight0 / Convert.ToDouble(RightSample.Count)), 2) - Math.Pow((NumbOfRight1 / Convert.ToDouble(RightSample.Count)), 2)), 4);
                        double DeltaOfGiniIndex = (GiniIndexForLeftSample + GiniIndexForRightSample) / 2;

                        RowInAllDeltaOfGiniIndex.Add(DeltaOfGiniIndex);

                        // Поиск наилучшего предиката для всего набора объектов по наименьшей дельте индексов джини
                        if (AllDeltaOfGiniIndex.Count == 0)
                        {
                            double MinOfAllDeltaOfGiniIndex = RowInAllDeltaOfGiniIndex.Min();

                            if (DeltaOfGiniIndex == MinOfAllDeltaOfGiniIndex)
                            {
                                SampleSplitIndex = i;

                                if (GiniIndexForLeftSample > GiniIndexForRightSample)
                                {
                                    IsLastNode = false;
                                    IsLeftSample = true;

                                    List<double> RowInPredicate = new List<double>();

                                    Predicate.Clear();
                                    RowInPredicate.Add(AmountOfFeature);
                                    RowInPredicate.Add((Bootstrap_Sample[i][AmountOfFeature] + Bootstrap_Sample[i + 1][AmountOfFeature]) / 2);
                                    RowInPredicate.Add(1);
                                    RowInPredicate.Add(Bootstrap_Sample[i + 1][Bootstrap_Sample[i].Count - 1]);

                                    Predicate.Add(RowInPredicate);
                                }
                                else if (GiniIndexForLeftSample < GiniIndexForRightSample)
                                {
                                    IsLastNode = false;
                                    List<double> RowInPredicate = new List<double>();

                                    Predicate.Clear();
                                    RowInPredicate.Add(AmountOfFeature);
                                    RowInPredicate.Add((Bootstrap_Sample[i][AmountOfFeature] + Bootstrap_Sample[i + 1][AmountOfFeature]) / 2);
                                    RowInPredicate.Add(0);
                                    RowInPredicate.Add(Bootstrap_Sample[i][Bootstrap_Sample[i].Count - 1]);

                                    Predicate.Add(RowInPredicate);
                                }
                                else if (GiniIndexForLeftSample == GiniIndexForRightSample)
                                {
                                    IsLastNode = true;
                                    Predicate.Clear();

                                    for (int k = 0; k < 2; k++)
                                    {
                                        List<double> RowInPredicate = new List<double>();

                                        if (k == 0)
                                        {
                                            RowInPredicate.Add(AmountOfFeature);
                                            RowInPredicate.Add((Bootstrap_Sample[i][AmountOfFeature] + Bootstrap_Sample[i + 1][AmountOfFeature]) / 2);
                                            RowInPredicate.Add(1);
                                            RowInPredicate.Add(Bootstrap_Sample[i + 1][Bootstrap_Sample[i].Count - 1]);

                                            Predicate.Add(RowInPredicate);
                                        }
                                        else
                                        {
                                            RowInPredicate.Add(AmountOfFeature);
                                            RowInPredicate.Add((Bootstrap_Sample[i][AmountOfFeature] + Bootstrap_Sample[i + 1][AmountOfFeature]) / 2);
                                            RowInPredicate.Add(0);
                                            RowInPredicate.Add(Bootstrap_Sample[i][Bootstrap_Sample[i].Count - 1]);

                                            Predicate.Add(RowInPredicate);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            double MinOfAllDeltaOfGiniIndex = RowInAllDeltaOfGiniIndex.Min();

                            for (int j = 0; j < AllDeltaOfGiniIndex.Count; j++)
                            {
                                if (DeltaOfGiniIndex > AllDeltaOfGiniIndex[j].Min())
                                {
                                    break;
                                }
                                else if (DeltaOfGiniIndex == MinOfAllDeltaOfGiniIndex)
                                {
                                    //Console.WriteLine("MinOfAllDeltaOfGiniIndex - " + MinOfAllDeltaOfGiniIndex + " < " + "AllDeltaOfGiniIndex[j].Min() - " + AllDeltaOfGiniIndex[j].Min());
                                    SampleSplitIndex = i;
                                    FlagSampleSplitIndex = true;

                                    if (GiniIndexForLeftSample > GiniIndexForRightSample)
                                    {
                                        IsLastNode = false;
                                        IsLeftSample = true;

                                        List<double> RowInPredicate = new List<double>();

                                        Predicate.Clear();
                                        RowInPredicate.Add(AmountOfFeature);
                                        RowInPredicate.Add((Bootstrap_Sample[i][AmountOfFeature] + Bootstrap_Sample[i + 1][AmountOfFeature]) / 2);
                                        RowInPredicate.Add(1);
                                        RowInPredicate.Add(Bootstrap_Sample[i + 1][Bootstrap_Sample[i].Count - 1]);

                                        Predicate.Add(RowInPredicate);
                                    }
                                    else if (GiniIndexForLeftSample < GiniIndexForRightSample)
                                    {
                                        IsLastNode = false;
                                        List<double> RowInPredicate = new List<double>();

                                        Predicate.Clear();
                                        RowInPredicate.Add(AmountOfFeature);
                                        RowInPredicate.Add((Bootstrap_Sample[i][AmountOfFeature] + Bootstrap_Sample[i + 1][AmountOfFeature]) / 2);
                                        RowInPredicate.Add(0);
                                        RowInPredicate.Add(Bootstrap_Sample[i][Bootstrap_Sample[i].Count - 1]);

                                        Predicate.Add(RowInPredicate);
                                    }
                                    else if (GiniIndexForLeftSample == GiniIndexForRightSample)
                                    {
                                        IsLastNode = true;
                                        Predicate.Clear();

                                        for (int k = 0; k < 2; k++)
                                        {
                                            List<double> RowInPredicate = new List<double>();

                                            if (k == 0)
                                            {
                                                RowInPredicate.Add(AmountOfFeature);
                                                RowInPredicate.Add((Bootstrap_Sample[i][AmountOfFeature] + Bootstrap_Sample[i + 1][AmountOfFeature]) / 2);
                                                RowInPredicate.Add(1);
                                                RowInPredicate.Add(Bootstrap_Sample[i + 1][Bootstrap_Sample[i].Count - 1]);

                                                Predicate.Add(RowInPredicate);
                                            }
                                            else
                                            {
                                                RowInPredicate.Add(AmountOfFeature);
                                                RowInPredicate.Add((Bootstrap_Sample[i][AmountOfFeature] + Bootstrap_Sample[i + 1][AmountOfFeature]) / 2);
                                                RowInPredicate.Add(0);
                                                RowInPredicate.Add(Bootstrap_Sample[i][Bootstrap_Sample[i].Count - 1]);

                                                Predicate.Add(RowInPredicate);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //for (int j = 0; j < LeftSample.Count; j++)
                        //{
                        //    Console.Write(LeftSample[j] + " ");
                        //}
                        //Console.WriteLine();
                        //for (int j = 0; j < RightSample.Count; j++)
                        //{
                        //    Console.Write(RightSample[j] + " ");
                        //}
                        //Console.WriteLine();
                        //Console.WriteLine("NumbOfLeft0 = " + NumbOfLeft0);
                        //Console.WriteLine("NumbOfLeft1 = " + NumbOfLeft1);
                        //Console.WriteLine("NumbORight0 = " + NumbOfRight0);
                        //Console.WriteLine("NumbOfRight1 = " + NumbOfRight1);
                        //Console.WriteLine("GiniIndexForLeftSample = " + GiniIndexForLeftSample);
                        //Console.WriteLine("GiniIndexForRightSample = " + GiniIndexForRightSample);
                        //Console.WriteLine("DeltaOfGiniIndex = " + DeltaOfGiniIndex);
                        //Console.WriteLine("Value in this index = " + Bootstrap_Sample[i][AmountOfFeature]);
                        //Console.WriteLine();
                    }
                }
                AllDeltaOfGiniIndex.Add(RowInAllDeltaOfGiniIndex);

                // Создание новой обучающей выборки если это не последний узел в дереве
                if (IsLastNode == false)
                {
                    if (AllDeltaOfGiniIndex.Count == 1 || FlagSampleSplitIndex == true)
                    {
                        NewTraining_Sample.Clear();

                        if (IsLeftSample == true)
                        {
                            for (int i = 0; i <= SampleSplitIndex; i++)
                            {
                                List<double> RowInNewTrainingSample = new List<double>();

                                for (int j = 0; j < Bootstrap_Sample[i].Count; j++)
                                {
                                    RowInNewTrainingSample.Add(Bootstrap_Sample[i][j]);
                                }
                                NewTraining_Sample.Add(RowInNewTrainingSample);
                            }
                        }
                        else
                        {
                            for (int i = SampleSplitIndex + 1; i < Bootstrap_Sample.Count; i++)
                            {
                                List<double> RowInNewTrainingSample = new List<double>();

                                for (int j = 0; j < Bootstrap_Sample[i].Count; j++)
                                {
                                    RowInNewTrainingSample.Add(Bootstrap_Sample[i][j]);
                                }
                                NewTraining_Sample.Add(RowInNewTrainingSample);
                            }
                        }
                    }
                }
                else
                {
                    NewTraining_Sample.Clear();
                }


                //Console.WriteLine("Sample split index [" + AmountOfFeature + "] = " + SampleSplitIndex);
                //Console.WriteLine();

                //Console.WriteLine("All delta of Gini index");
                //for (int i = 0; i < AllDeltaOfGiniIndex.Count; i++)
                //{
                //    for (int j = 0; j < AllDeltaOfGiniIndex[i].Count; j++)
                //    {
                //        Console.WriteLine(AllDeltaOfGiniIndex[i][j] + "\t");
                //    }
                //    Console.WriteLine();
                //}
            }

            return (NewTraining_Sample, Predicate);
        }
        // ------------------------------------------------------------------------------------------------------------
        private void WritePredicatesToFile()
        {
            // Запись всех предикатов(узлов дерева) в файл

            StreamWriter Sw = new StreamWriter($@"D:\Работа\EnsembleOfModels\Bagging\RandomForest\Predicates\Predicate{NumbOfModel}.txt", false);

            // Запись индексов параметров в файл
            for (int i = 0; i < NumbOfFeature.Count; i++)
            {
                Sw.Write(NumbOfFeature[i] + " ");
            }
            Sw.WriteLine();

            // Запись предикатов в файл
            for (int i = 0; i < AllPredicates.Count; i++)
            {
                for (int j = 0; j < AllPredicates[i].Count; j++)
                {
                    Sw.Write(AllPredicates[i][j] + "\t");
                }
                Sw.WriteLine();
            }

            Sw.Close();
        }
        // ------------------------------------------------------------------------------------------------------------
        private void ShowInfo()
        {
            for (int i = 0; i < NumbOfFeature.Count; i++)
            {
                Console.Write(NumbOfFeature[i] + "\t");
            }
            Console.WriteLine();

            for (int i = 0; i < Bootstrap_Sample.Count; i++)
            {
                for (int j = 0; j < Bootstrap_Sample[i].Count; j++)
                {
                    Console.Write(Bootstrap_Sample[i][j] + "\t");
                }
                Console.WriteLine();
            }
        }
        // ------------------------------------------------------------------------------------------------------------
    }
}
