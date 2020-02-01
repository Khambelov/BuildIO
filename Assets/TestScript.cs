//using IndieMarc.TopDown;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TestScript : MonoBehaviour
//{
//	public SpriteRenderer rend;


//	private void Start()
//	{
//		rend = GetComponent<SpriteRenderer>();
//	}

//	private void OnTriggerEnter2D(Collider2D collision)
//	{
//		if (collision.gameObject.GetComponent<PlayerCharacter>() != null)
//		{
//			rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, rend.color.a / 2);
//		}
//	}

//	private void OnTriggerExit2D(Collider2D collision)
//	{
//		if (collision.gameObject.GetComponent<PlayerCharacter>() != null)
//		{
//			rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 1f);
//		}
//	}
//}
