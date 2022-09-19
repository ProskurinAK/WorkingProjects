using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
// ************************************************************************************************************

namespace RandomForest
{
    class BootstrapSample
    {
        private List<List<double>> BootstrapSampleFeatures = new List<List<double>>();   // Матрица созданных объектов ДО ОБЪЕДИНЕНИЯ В МЕТОДЕ UNIT
        private List<double> BootstrapSampleTargets = new List<double>();    // Вектор ответов ДО ОБЪЕДИНЕНИЯ В МЕТОДЕ UNIT

        private List<int> NumbOfFeature = new List<int>();  // Номера индексов признаков выбранных для бутстрэп выборки методом случайных подпространств
        private List<int> IndicesOfObject = new List<int>();    // Номера индексов объектов взятых из обучающей в бутстрэп выборку
        private int NumbOfSample;   // Номер бутстрэп выборки

        public List<List<double>> Bootstrap_Sample = new List<List<double>>();   // Результат работы класса - Матрица объектов-признаков вместе с ответами для каждого объекта
        // ------------------------------------------------------------------------------------------------------------
        public BootstrapSample(int NumbOfSample)
        {
            this.NumbOfSample = NumbOfSample;

            MakeSample();
            Unit();
            WriteToFile();

            //ShowInfo();
        }
        // ------------------------------------------------------------------------------------------------------------
        private void MakeSample()
        {
            // Функция создания бутстрэп выборки

            Random Rnd = new Random();

            int FeaturesAmount = 12; // Количество признаков в бутстрэп выборке (равно квадратному корню из количества признаков исходной выборки)

            // Генерация номеров признаков для обучающей выборки
            for (int i = 0; i < FeaturesAmount;)
            {
                bool AlreadyThere = false;
                int Value = Rnd.Next(145);    // 145 - количество признаков в исходном датасете

                for (int j = 0; j < NumbOfFeature.Count; j++)
                {
                    if (Value == NumbOfFeature[j])
                    {
                        AlreadyThere = true;
                        break;
                    }
                }
                if (!AlreadyThere)
                {
                    NumbOfFeature.Add(Value);
                    i++;
                }
            }
            NumbOfFeature.Sort();

            int BootstrapSampleSize = 200;    // Количество объектов в бутстрэп выборке

            // Создание матрицы объектов-признаков на основе бутстрепа и метода случайных подпространств для бутстрэп выборки
            for (int k = 0; k < BootstrapSampleSize; k++)
            {
                int NewObjectIndex = Rnd.Next(RandomForestClassifier.DataSetFeaturesForTraining.Count);
                IndicesOfObject.Add(NewObjectIndex);

                List<double> BootstrapSampleFeaturesRow = new List<double>();

                for (int i = 0; i < RandomForestClassifier.DataSetFeaturesForTraining.Count; i++)
                {
                    if (NewObjectIndex == i)
                    {
                        for (int n = 0; n < NumbOfFeature.Count; n++)
                        {
                            for (int j = 0; j < RandomForestClassifier.DataSetFeaturesForTraining[i].Count; j++)
                            {
                                if (j == NumbOfFeature[n])
                                {
                                    BootstrapSampleFeaturesRow.Add(RandomForestClassifier.DataSetFeaturesForTraining[i][j]);
                                }
                            }
                        }
                    }
                }

                BootstrapSampleFeatures.Add(BootstrapSampleFeaturesRow);

                // Создание вектора ответов на основе бутстрепа и метода случайных подпространств для бутстрэп выборки
                for (int i = 0; i < RandomForestClassifier.DataSetTargetsForTraining.Count; i++)
                {
                    if (NewObjectIndex == i)
                    {
                        BootstrapSampleTargets.Add(RandomForestClassifier.DataSetTargetsForTraining[i]);
                    }
                }
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        private void Unit()
        {
            // Функция объединения матрицы объектов-признаков и вектора ответов в одну матрицу

            for (int i = 0; i < BootstrapSampleFeatures.Count; i++)
            {
                List<double> RowInBootstrap_Sample = new List<double>();

                for (int j = 0; j < BootstrapSampleFeatures[i].Count; j++)
                {
                    RowInBootstrap_Sample.Add(BootstrapSampleFeatures[i][j]);
                }

                RowInBootstrap_Sample.Add(BootstrapSampleTargets[i]);

                Bootstrap_Sample.Add(RowInBootstrap_Sample);
            }

            BootstrapSampleFeatures.Clear();
            BootstrapSampleTargets.Clear();
        }
        // ------------------------------------------------------------------------------------------------------------
        private void WriteToFile()
        {
            // Функция записи бутстрэп выборки в файл

            StreamWriter Sw = new StreamWriter($@"D:\Работа\EnsembleOfModels\Bagging\RandomForest\BootstrapSamples\BootstrapSample{NumbOfSample}.txt", false);

            // Запись индексов параметров из датасета по которым построилась обучающая выборка с помощью метода случайных подпространств
            for (int i = 0; i < NumbOfFeature.Count; i++)
            {
                Sw.Write(NumbOfFeature[i] + " ");
            }
            Sw.WriteLine();

            // Запись индексов объектов из датасета в файл
            //for (int i = 0; i < IndicesOfObject.Count; i++)
            //{
            //    Sw.Write(IndicesOfObject[i] + " ");
            //}
            //Sw.WriteLine();

            // Запись обучающей выборки в файл
            for (int i = 0; i < Bootstrap_Sample.Count; i++)
            {
                for (int j = 0; j < Bootstrap_Sample[i].Count; j++)
                {
                    Sw.Write(Bootstrap_Sample[i][j] + "\t");
                }
                Sw.WriteLine();
            }

            Sw.Close();
        }
        // ------------------------------------------------------------------------------------------------------------
        private void ShowInfo()
        {
            Console.WriteLine("Numb of feature");
            for (int i = 0; i < NumbOfFeature.Count; i++)
            {
                Console.WriteLine(NumbOfFeature[i]);
            }
            Console.WriteLine("--------------------");

            Console.WriteLine("Indices of object");
            for (int i = 0; i < IndicesOfObject.Count; i++)
            {
                Console.WriteLine(IndicesOfObject[i]);
            }
            Console.WriteLine("--------------------");

            Console.WriteLine("Bootstrap_Sample");
            for (int i = 0; i < Bootstrap_Sample.Count; i++)
            {
                for (int j = 0; j < Bootstrap_Sample[i].Count; j++)
                {
                    Console.Write(Bootstrap_Sample[i][j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("--------------------");

            Console.WriteLine("Bootstrap_Sample.Count = " + Bootstrap_Sample.Count);
            Console.WriteLine("BootstrapSampleFeatures.Count = " + BootstrapSampleFeatures.Count);
            Console.WriteLine("BootstrapSampleTargets.Count = " + BootstrapSampleTargets.Count);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Console.WriteLine("Bootstrap sample features");
            //for (int i = 0; i < BootstrapSampleFeatures.Count; i++)
            //{
            //    for (int j = 0; j < BootstrapSampleFeatures[i].Count; j++)
            //    {
            //        Console.Write(BootstrapSampleFeatures[i][j] + "\t");
            //    }
            //    Console.WriteLine();
            //}
            //Console.WriteLine("--------------------");

            //Console.WriteLine("Bootstrap sample targets");
            //for (int i = 0; i < BootstrapSampleTargets.Count; i++)
            //{
            //    Console.WriteLine(BootstrapSampleTargets[i]);
            //}
            //Console.WriteLine("--------------------");
        }
    }
}
