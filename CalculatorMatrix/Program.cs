using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace CalculatorMatrix
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Приветики!)Добро пожаловать в 'Калькулятор матриц'!");
                PrintMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Метод вывода меню на экран.
        /// </summary>
        private static void PrintMenu()
        {
            int[,] array1 = null;
            int[,] array2 = null;
            do
            {
                Console.WriteLine("Вам доступны такие операции с матрицами:");
                Console.WriteLine("1. нахождение следа матрицы(квадратная матрица);");
                Console.WriteLine("2. транспонирование матрицы(квадратная матрица);");
                Console.WriteLine("3. сумма двух матриц(размерность матриц должна быть одинаковой);");
                Console.WriteLine("4. разность двух матриц(размерность матриц должна быть одинаковой);");
                Console.WriteLine("5. произведение двух матриц;");
                Console.WriteLine("6. умножение матрицы на число;");
                Console.WriteLine("7. нахождение определителя матрицы(квадратная матрица).");
                Console.WriteLine("Выберите операцию(введите число от 1 до 7):");
                Choice(NumberCheck(Console.ReadLine(), 1, 7), ref array1, ref array2);
                array1 = null;
                array2 = null;
                Console.WriteLine("Нажмите Esc для завершения работы, в противном случае нажмите Enter ДВА РАЗА!");
                if (Console.ReadKey().Key == ConsoleKey.Enter)
                    Console.Clear();
            } while (Console.ReadKey().Key != ConsoleKey.Escape);

        }

        /// <summary>
        /// Проверяет правильность введенного числа.
        /// </summary>
        /// <param name="input">Входные данные.</param>
        /// <param name="begin">Начало диапазона.</param>
        /// <param name="end">Конец диапазона.</param>
        /// <returns></returns>
        public static int NumberCheck(string input, int begin, int end)
        {
            int number;
            while (!int.TryParse(input, out number) || (number < begin || number > end))
            {
                Console.WriteLine("Входные данные введены некорректно.Введите число снова.");
                input = Console.ReadLine();
            }
            return number;
        }

        /// <summary>
        /// Вывод матрицы.
        /// </summary>
        /// <param name="array"></param>
        public static void MatrixOutput(int[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(array[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Метод для выбора операции.
        /// </summary>
        /// <param name="n">Выбор операции.</param>
        /// <param name="array1">Массив 1.</param>
        /// <param name="array2">Массив 2.</param>
        static void Choice(int n, ref int[,] array1, ref int[,] array2)
        {
            InputMatrix(n, ref array1, ref array2);
            switch (n)
            {
                case 1:
                    Console.WriteLine(MatrixTrace(ref array1));
                    break;
                case 2:
                    MatrixTransposition(ref array1);
                    Console.WriteLine("Результат операции:");
                    MatrixOutput(array1);
                    break;
                case 3:
                    Console.WriteLine("Результат операции:");
                    MatrixOutput(MatrixSumDif(ref array1, ref array2, true));
                    break;
                case 4:
                    Console.WriteLine("Результат операции:");
                    MatrixOutput(MatrixSumDif(ref array1, ref array2, false));
                    break;
                case 5:
                    Console.WriteLine("Результат операции:");
                    MatrixOutput(MatrixMultiply(ref array1, ref array2));
                    break;
                case 6:
                    Console.WriteLine("Введите число, на которое вы хотите умножить(от -1000 до 1000):");
                    int number = NumberCheck(Console.ReadLine(), -1000, 1000);
                    Console.WriteLine("Результат операции:");
                    MatrixOutput(MatrixNumber(ref array1, number));
                    break;
                case 7:
                    Console.WriteLine("Результат операции:");
                    Console.WriteLine(MatrixDet(array1, 0));
                    break;
            }
        }

        /// <summary>
        /// Метод спрашивает у пользователя каким способом он хочет ввести матрицу.
        /// </summary>
        /// <param name="n">Выбранная операция.</param>
        /// <param name="array1">Массив 1.</param>
        /// <param name="array2">Массив 2.</param>
        private static void InputMatrix(int n, ref int[,] array1, ref int[,] array2)
        {
            Console.WriteLine("Каким из способов вы хотите ввести матрицу(матрицы)?");
            Console.WriteLine("1.Вывод из консоли.");
            Console.WriteLine("2.Вывод из файла.");
            Console.WriteLine("3.Генерация случайным образом.");
            switch (NumberCheck(Console.ReadLine(), 1, 3))
            {
                case 1:
                    ConsoleInput(n, ref array1, ref array2);
                    break;
                case 2:
                    array1 = FileInput(n);
                    //В случае, если нужны две матрицы.
                    if (n == 3 || n == 4 || n == 5)
                        array2 = FileInput(n);
                    //При выборе операции умножения.
                    if (n == 5)
                        if (array2.GetLength(0) != array1.GetLength(1))
                        {
                            Console.WriteLine("Входные данные введены некорректно." +
                                "Кол-во столбцов 1 матрицы не равно кол-ву строк 2 матрицы.Попробуйте еще раз.");
                            array1 = null;
                            array2 = null;
                            InputMatrix(n, ref array1, ref array2);
                        }
                    //При выборе операции суммы или разности матриц
                    if (n == 3 || n == 4)
                    {
                        if (array1.GetLength(1) != array2.GetLength(1) || array1.GetLength(0) != array2.GetLength(0))
                        {
                            Console.WriteLine("Входные данные введены некорректно." +
                                "Размерность матриц не совпадает.");
                            array1 = null;
                            array2 = null;
                            InputMatrix(n, ref array1, ref array2);
                        }
                    }
                    break;
                case 3:
                    array1 = RandomInput(n, ref array1, ref array2);
                    if (n == 3 || n == 4 || n == 5)
                        array2 = RandomInput(n, ref array1, ref array2);
                    break;
            }
        }

        /// <summary>
        /// Случайная генерация матрицы.
        /// </summary>
        /// <param name="n">Выбранная операция.</param>
        /// <param name="array1">Массив 1.</param>
        /// <param name="array2">Массив 2.</param>
        /// <returns>полученная матрица</returns>
        private static int[,] RandomInput(int n, ref int[,] array1, ref int[,] array2)
        {
            int[,] array = null;
            var rnd = new Random();
            Console.WriteLine("Введите параметры для случайной генерации:");
            Console.WriteLine("Введите конец диапазона для генерации размера матрицы(максимум 10*10):");
            Console.WriteLine("Для строк:");
            int lines = rnd.Next(NumberCheck(Console.ReadLine(), 0, 10));
            Console.WriteLine("Для столбцов:");
            int columns = rnd.Next(NumberCheck(Console.ReadLine(), 0, 10));
            if (n == 1 || n == 2 || n == 7)
                columns = lines;
            if ((n == 3 || n == 4) && array1 != null)
            {
                lines = array1.GetLength(0);
                columns = array1.GetLength(1);
            }
            if (n == 5 && array1 != null)
            {
                lines = array1.GetLength(0);
            }
            Console.WriteLine("Введите числа нужного диапазона для генерации элементов матрицы(от -1000 до 1000):");
            Console.WriteLine("Введите начало диапазона");
            int begin = NumberCheck(Console.ReadLine(), -1000, 1000);
            Console.WriteLine("Введите конец диапазона");
            int end = NumberCheck(Console.ReadLine(), -1000, 1000);
            array = new int[lines, columns];
            //Генерируем матрицу.
            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    array[i, j] = rnd.Next(begin, end);
                }
            }
            Console.WriteLine("Получившаяся матрица:");
            MatrixOutput(array);
            return array;
        }

        /// <summary>
        /// Ввод матрицы из файла.
        /// </summary>
        /// <param name="n">Выбранная операция.</param>
        /// <returns></returns>
        private static int[,] FileInput(int n)
        {
            int[,] array = null;
            Console.WriteLine("Введите путь к файлу, где хранится матрица.В первой строчке файла " +
                "должно быть кол-во строк, во второй кол-во столбцов.");
            string path = Console.ReadLine();
            if (File.Exists(path))
            {
                int lines = NumberCheck(File.ReadAllLines(path)[0], 0, 10);
                int columns = NumberCheck(File.ReadAllLines(path)[0], 0, 10);
                if ((n == 1 || n == 2 || n == 7) && lines != columns)
                {
                    Console.WriteLine("Входные данные некорректны.Матрица не квадратная.Перезапишите файл.");
                    FileInput(n);
                }
                array = new int[lines, columns];
                //Считываем строки из файла и записываем в массив.
                for (int i = 0; i < lines; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        array[i, j] = NumberCheck(File.ReadAllLines(path)[i + 2].Split(' ')[j], -1000, 1000);
                        if (array.GetLength(1) != columns)
                        {
                            Console.WriteLine("Данные введены некорpектно.Попробуйте снова.");
                            FileInput(n);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Файл не существует.Попробуйте ввести путь снова.");
                FileInput(n);
            }
            return array;
        }

        /// <summary>
        /// Вывод матрицы через консоль.
        /// </summary>
        /// <param name="n">Выбранная операция.</param>
        /// <param name="array1">Массив 1.</param>
        /// <param name="array2">Массив 2.</param>
        private static void ConsoleInput(int n, ref int[,] array1, ref int[,] array2)
        {
            //Для квадратной матрицы.
            if (n == 1 || n == 2 || n == 7)
            {
                Console.WriteLine("Введите размер квадратной матрицы(максимум 10, формат ввода - одно число, типа int):");
                int number = NumberCheck(Console.ReadLine(), 0, 10);
                while (array1 == null)
                    array1 = MatrixConsoleInput(n, number, number, ref array1, ref array2);
            }
            //Для матрицы любого размера.
            else if (n == 6)
            {
                Console.WriteLine("Введите размер матрицы(максимум 10*10):");
                Console.WriteLine("Количество строк:");
                int lines = NumberCheck(Console.ReadLine(), 0, 10);
                Console.WriteLine("Количество столбцов:");
                int columns = NumberCheck(Console.ReadLine(), 0, 10);
                while (array1 == null)
                    array1 = MatrixConsoleInput(n, lines, columns, ref array1, ref array2);
            }
            //Для двух матриц.
            else
            {
                Console.WriteLine("Введите размеры для двух матриц(максимум 10*10):");
                if (n == 5)
                    Console.WriteLine("кол - во столбцов 1 матрицы должно быть равно кол - ву строк во 2 матрице");
                if (n == 3 || n == 4)
                    Console.WriteLine("Размерность матриц должна быть одинаковой.");
                Console.WriteLine("Количество строк:");
                int lines1 = NumberCheck(Console.ReadLine(), 0, 10);
                Console.WriteLine("Количество столбцов:");
                int columns1 = NumberCheck(Console.ReadLine(), 0, 10);
                array1 = MatrixConsoleInput(n, lines1, columns1, ref array1, ref array2);
                Console.WriteLine("Количество строк 2 матрицы:");
                int lines2 = NumberCheck(Console.ReadLine(), 0, 10);
                Console.WriteLine("Количество столбцов 2 матрицы:");
                int columns2 = NumberCheck(Console.ReadLine(), 0, 10);
                if ((n == 5 && columns2 != lines1) || ((n == 3 || n == 4) && (lines1 != lines2 || columns1 != columns2)))
                {
                    Console.WriteLine("Данные введены некоректно.Проверьте размерность матрицы.Попробуйте снова.");
                    array1 = null;
                    array2 = null;
                    ConsoleInput(n, ref array1, ref array2);
                }
                while (array2 == null)
                    array2 = MatrixConsoleInput(n, lines2, columns2, ref array1, ref array2);
            }
        }

        /// <summary>
        /// Метод для считывания матрицы с консоли.
        /// </summary>
        /// <param name="n">выбранная операция.</param>
        /// <param name="lines">Кол-во строк.</param>
        /// <param name="columns">Кол-во столбцов.</param>
        /// <param name="array1">Массив 1.</param>
        /// <param name="array2">Массив 2.</param>
        /// <returns>Полученная матрица.</returns>
        private static int[,] MatrixConsoleInput(int n, int lines, int columns, ref int[,] array1, ref int[,] array2)
        {
            int[,] array = new int[lines, columns];
            Console.WriteLine("Вводте элементы матрицы(числа типа int, от -1000 до 1000) в строку через пробел." +
                "Для перехода на новую строку нажмите Enter:");
            for (int i = 0; i < lines; i++)
            {
                string[] line = Console.ReadLine().Split(' ');
                for (int j = 0; j < columns; j++)
                {
                    array[i, j] = NumberCheck(line[j], -1000, 1000);
                    if (line.Length != columns)
                    {
                        Console.WriteLine("Данные введены некорректно. Попробуйте снова.");
                        array1 = null;
                        array2 = null;
                        return null;
                    }
                }
            }
            return array;
        }

        /// <summary>
        /// Нахождение следа матрицы.
        /// </summary>
        /// <param name="array1">Массив 1.</param>
        /// <returns>След матрицы.</returns>
        private static int MatrixTrace(ref int[,] array1)
        {
            int res = 1;
            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    //Умножаем числа, стоящие на главной диагонали.
                    if (i == j)
                        res *= array1[i, j];
                }
            }
            return res;
        }

        /// <summary>
        /// Транспонирование матрицы.
        /// </summary>
        /// <param name="array1">Массив 1.</param>
        private static void MatrixTransposition(ref int[,] array1)
        {
            int[,] new_array = new int[array1.GetLength(0), array1.GetLength(1)];
            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    //Добавляем в новый массив значение элемента на его новое расположение.
                    new_array[i, j] = array1[j, i];
                }
            }
            array1 = new_array;
        }

        /// <summary>
        /// Сумма и разность матриц.
        /// </summary>
        /// <param name="array1">Массив 1.</param>
        /// <param name="array2">Массив 2.</param>
        /// <param name="flag">Булевая переменная для определенной операции(разность, сумма).</param>
        /// <returns>Полученный массив.</returns>
        private static int[,] MatrixSumDif(ref int[,] array1, ref int[,] array2, bool flag)
        {
            int[,] array = new int[array1.GetLength(0), array1.GetLength(1)];
            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    //В зависимости от операции складываем или отнимаем элементы.
                    if (flag)
                        array[i, j] = array1[i, j] + array2[i, j];
                    else
                        array[i, j] = array1[i, j] - array2[i, j];
                }
            }
            return array;
        }

        /// <summary>
        /// Умножает матрицы.
        /// </summary>
        /// <param name="array1">Массив 1.</param>
        /// <param name="array2">Массив 2.</param>
        /// <returns>Полученный массив.</returns>
        private static int[,] MatrixMultiply(ref int[,] array1, ref int[,] array2)
        {
            int[,] array = new int[array1.GetLength(0), array2.GetLength(1)];
            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array2.GetLength(1); j++)
                {
                    array[i, j] = 0;
                    for (int k = 0; k < array1.GetLength(1); k++)
                    {
                        //В новый массив складываем полученный элемент.
                        array[i, j] += array1[i, k] * array2[k, j];
                    }
                }
            }
            return array;
        }

        /// <summary>
        /// Умножает матрицу на число.
        /// </summary>
        /// <param name="array1">Массив 1.</param>
        /// <param name="number">Число.</param>
        /// <returns>Полученный массив.</returns>
        private static int[,] MatrixNumber(ref int[,] array1, int number)
        {
            int[,] array = new int[array1.GetLength(0), array1.GetLength(1)];
            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    //В новый массив записываем элемент, умноженный на данное число.
                    array[i, j] = number * array1[i, j];
                }
            }
            return array;
        }

        /// <summary>
        /// Нахождение определителя.
        /// </summary>
        /// <param name="matrix">Массив 1.</param>
        /// <param name="det">Определитель.</param>
        /// <returns>Определитель.</returns>
        private static int MatrixDet(int[,] matrix, int det)
        {
            try
            {
                int size = matrix.GetUpperBound(0) + 1;
                det = 0;
                if (size == 1)
                    det = matrix[0, 0];
                if (size == 2)
                    det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
                int[,] otherMatrix = new int[size - 1, size - 1];
                if (size > 2)
                {
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            Decomposition(matrix, out otherMatrix, i, j);
                            if (j % 2 == 0)
                                det += matrix[0, j] * MatrixDet(otherMatrix, det);
                            else
                                det -= matrix[0, j] * MatrixDet(otherMatrix, det);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return det;
        }

        /// <summary>
        /// Исключает из матрицы i-ую строку и j-ый столбец.
        /// </summary>
        /// <param name="matrix">Массив.</param>
        /// <param name="otherMatrix">Полученная матрица.</param>
        /// <param name="i">Номер строки.</param>
        /// <param name="j">Номер столбца.</param>
        public static void Decomposition(int[,] matrix, out int[,] otherMatrix, int i, int j)
        {
            int i1, j1, iPlus, jPlus;
            iPlus = 0;
            int newSize = matrix.GetUpperBound(0);
            otherMatrix = new int[newSize, newSize];
            for (i1 = 0; i1 < matrix.GetUpperBound(0); i1++)
            {
                if (i1 == i)
                    iPlus = 1;
                jPlus = 0;
                for (j1 = 0; j1 < matrix.GetUpperBound(0); j1++)
                {
                    if (j1 == j)
                        jPlus = 1;
                    otherMatrix[i1, j1] = matrix[i1 + iPlus, j1 + jPlus];
                }
            }
        }
    }
}
