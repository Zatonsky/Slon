using System;
using System.Collections.Generic;
using System.Linq;

namespace Myxa
{
	/// <summary>
	/// Генетический алгоритм поиска цепочки слов
	/// </summary>
	public class Genetic
	{
		/// <summary>
		/// Маскимальное число итераций поиска
		/// </summary>
		private const int MaxSteps = 1000;

		/// <summary>
		/// Максимальное число особей популяции
		/// </summary>
		private const int MaxPopulation = 100;

		/// <summary>	
		/// Исxодное слово
		/// </summary>
		private readonly string _source;
		
		/// <summary>
		/// Конечное слово
		/// </summary>
		private readonly string _destination;

		/// <summary>
		/// Словарь слов
		/// </summary>
		private readonly List<string> _dictionary;

		/// <summary>
		/// Конструктор генетического алгоритма
		/// </summary>
		/// <param name="source">Исходное слово</param>
		/// <param name="destination">Конечное слово</param>
		/// <param name="dictionary">Словарь слов для поиска</param>
		public Genetic(string source, string destination, List<string> dictionary)
		{
			if (source.Length != destination.Length)
			{
				throw new ArgumentException("Исходное и конечное слово разной длины");
			}
			if (!dictionary.Contains(source))
			{
				throw new ArgumentException("Исходное слово не найдено в словаре");
			}
			if (!dictionary.Contains(destination))
			{
				throw new ArgumentException("Конечное слово не найдено в словаре");
			}
			_source = source;
			_destination = destination;
			_dictionary = dictionary;
		}

		/// <summary>
		/// Реализация генетического алгоритма
		/// </summary>
		/// <returns>Искомая цепочка слов</returns>
		public Chain Run()
		{
			// Инициализируем начальную популяцию
			var population = new List<Chain> {new Chain(_source)};
			// Главный цикл генетического алгоритма
			for (var i = 0; i < MaxSteps; ++i)
			{
				// Если цепочка найдена - возвращаем ее и заканчиваем поиск
				foreach (var chain in population.Where(chain => chain.Word == _destination))
				{
					return chain;
				}
				var nextPopulation = new List<Chain>();
				// Растим следующую популяцию: для каждой цепочки производим мутацию и добавляем ее в популяцию
				foreach (var chain in population)
				{
					nextPopulation.AddRange(Mutation(chain));
				}
				// Прредыдущее поколение больше не требуется
				population = nextPopulation;
				// Если ничего нового не появилось....
				if (population.Count == 0)
				{
					throw new AccessViolationException($"На шаге {i} (из максимально {MaxSteps}) закончились варианты. Поиск не увенчался успехом.");
				}
				// Сортируем по степени приспособленности
				population.Sort((x, y) => y.Score(_destination) - x.Score(_destination));
				// и оставляем самых лучших
				if (population.Count > MaxPopulation)
				{
					population.RemoveRange(MaxPopulation, population.Count - MaxPopulation);
				}
			}
			// Слишком глубокий поиск, но, к сожалению, ничего не найдено
			throw new AccessViolationException($"Пройдено максимально разрешённое число шагов ({MaxSteps}), но поиск не увенчался успехом.");

		}

		/// <summary>
		/// Получение мутации от данного слова
		/// </summary>
		/// <param name="chain">Слово вместе со всеми своими предками</param>
		/// <returns>Список мутированных слов</returns>
		private IEnumerable<Chain> Mutation(Chain chain)
		{

			if (chain.Word == null)
			{
				return null;
			}
			var mutation = new List<Chain>();

			for (var i = 0; i < chain.Word.Length; i++)
			{
				// Пропускаем исключённую букву (нет смысла менять ту же букву, что и на предыдущем шаге)
				if (i == chain.Mutated)
				{
					continue;
				}
				// Выделяем в словаре слова заданной длины, выкалываем у слов i-тую букву и ищем совпадение с выколотой i-той буквой шаблона
				var  words = _dictionary.FindAll(w => w.Length == chain.Word.Length).FindAll(w => w.Substring(0, i) + w.Substring(i + 1) == chain.Word.Substring(0, i) + chain.Word.Substring(i + 1));
				// Убираем встречавшиеся ранее слова, а остальное добавляем в мутацию
				mutation.AddRange(from word in words where !chain.Contains(word) select new Chain(word, chain, i));
			}
			return mutation;
		}
	}
}
