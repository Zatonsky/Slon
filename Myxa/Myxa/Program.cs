using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Myxa
{
	internal static class Program
	{
		/// <summary>
		/// Точка входа в программу
		/// </summary>
		/// <param name="args">Список аргументов</param>
		private static void Main(string[] args)
		{
			// Должно быть два аргумента!
			if (args.Length != 2)
			{
				// Объяснение аргументов программы и выход
				Usage();
			}
			try
			{
				// Первый аргумент должен быть именем файла
				if (!File.Exists(args[0]))
				{
					throw new ArgumentException("Файл с начальным и конечным словом не найден.");
				}
				// Второй аргумент должен быть именем файла
				if (!File.Exists(args[1]))
				{
					throw new ArgumentException("Файл словаря не найден.");
				}

				string destination;
				string source;
				var dictionary = new List<string>();
				// Добавляем начальное и конечное слова
				using (var sr = File.OpenText(args[0]))
				{

					if ((source = sr.ReadLine()) == null)
					{
						throw new ArgumentException("Начальное слово не найдено.");
					}
					source = source.ToLower(CultureInfo.CurrentUICulture);
					if ((destination = sr.ReadLine()) == null)
					{
						throw new ArgumentException("Конечное слово не найдено.");
					}
					destination = destination.ToLower();
				}
				// Добавляем словарь
				using (var sr = File.OpenText(args[1]))
				{
					string s;
					while ((s = sr.ReadLine()) != null)
					{
						dictionary.Add(s.ToLower(CultureInfo.CurrentUICulture));
					}
				}
				// Создаем генетический алгоритм, выполняем его и печатаем результаты
				new Genetic(source, destination, dictionary).Run().Print();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		/// <summary>
		/// Объяснение аргументов программы и выход
		/// </summary>
		private static void Usage()
		{
			Console.WriteLine("Решение головоломки перобразования одного слова в другое, заменяя каждый раз одну букву");
			Console.WriteLine("Использование: {0} [путь к файлу с начальным и конечным словом] [путь к файлу со словарем]", Path.GetFileName(Environment.GetCommandLineArgs()[0]));
			Console.WriteLine();
			Console.WriteLine("[путь к файлу с начальным и конечным словом]\t\tПуть к текстовому файлу в кодировке UTF8, в котором указано начальное и конечное слово.");
			Console.WriteLine("\t\t\t\t\t\t\tНа первой строке указано начальное слово, на второй строке конечное");
			Console.WriteLine("[путь к файлу со словарем]\t\t\t\tПуть к текстовому файлу в кодировке UTF8, который содержит словарь. Слова в словаре указаны по одному на каждой строке.");
			Console.WriteLine("\t\t\t\t\t\t\tсловаре слова могут быть разной длины.");
			Environment.Exit(0);
		}
	}
}
