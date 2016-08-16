using System;

namespace Myxa
{
	/// <summary>
	/// Цепочка слов
	/// </summary>
	public class Chain
	{
		/// <summary>
		/// Текущее слово
		/// </summary>
		public string Word { get; }

		/// <summary>
		/// Предыдущее слово
		/// </summary>
		private Chain Parent { get; }

		/// <summary>
		/// Номер измененной буквы (ее не будем меня в следующий раз)
		/// </summary>
		public int Mutated { get; }

		/// <summary>
		/// Инициализация цепочки
		/// </summary>
		/// <param name="word">Начальное слово</param>
		public Chain(string word)
		{
			Word = word;
			Parent = null;
			Mutated = -1;
		}

		/// <summary>
		/// Добавление нового слова в цепочку
		/// </summary>
		/// <param name="word">Добавляемое слово</param>
		/// <param name="parent">Цепочка, к которой добавляется слово</param>
		/// <param name="mutated">Номер изменненной буквы</param>
		public Chain(string word, Chain parent, int mutated)
		{
			Word = word;
			Parent = parent;
			Mutated = mutated;
		}

		/// <summary>
		/// Проверяет, содержится ли в цепочке искомое слово
		/// </summary>
		/// <param name="pattern">Искомое слово</param>
		/// <returns>true - если содержится, false - в противном случае</returns>
		public bool Contains(string pattern)
		{
			var chain = this;
			while (chain != null)
			{
				if (chain.Word == pattern)
				{
					return true;
				}
				chain = chain.Parent;
			}
			return false;
		}

		/// <summary>
		/// Функция оценки похожести слова
		/// </summary>
		/// <param name="pattern">Эталонное слово</param>
		/// <returns>Оценка похожести</returns>
		public int Score(string pattern)
		{
			const string vowels = "аеёиоуыэюя";
			var score = 0;
			for (var i = 0; i < Word.Length; ++i)
			{
				// Полностью совпадающим символам максимальная оценка = 3
				if (Word[i] == pattern[i])
				{
					score += 3;
					continue;
				}
				if (vowels.IndexOf(Word[i]) == -1)
				{
					continue;
				}
				if (vowels.IndexOf(pattern[i]) != -1)       // Совпадение позиции гласной буквы = 2
				{
					score += 2;
				}
				else
				{
					score += 1;                             // Наличие гласной буквы = 1
				}
			}
			return score;
		}

		/// <summary>
		/// Печатаем цепочку в обратном порядке
		/// </summary>
		public void Print()
		{
			if (Parent == null)
			{
				Console.WriteLine(Word);
			}
			else
			{
				Parent.Print();
				Console.WriteLine(Word);
			}
		}
	}
}
