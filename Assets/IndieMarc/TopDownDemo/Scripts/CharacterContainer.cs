using IndieMarc.TopDown;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterContainer : MonoBehaviour
{
	public static CharacterContainer Instance { get { return instance; } }
	private static CharacterContainer instance;

	private List<Character> characters;

	private void Awake()
	{
		instance = this;
		characters = new List<Character>();
	}

	public int GetCharacterCount()
	{
		return characters.Count;
	}

	public List<Character> GetSortedCharacters()
	{
		return characters.OrderBy(c => c.score).ToList();
	}

	public void AddNewCharacter(Character newChar)
	{
		characters.Add(newChar);
	}

	public void DeleteCharacter(Character oldChar)
	{
		characters.Remove(oldChar);
	}
}
