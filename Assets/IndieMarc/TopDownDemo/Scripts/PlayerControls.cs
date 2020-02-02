using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player controls for platformer demo
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// </summary>

namespace IndieMarc.TopDown
{

	public class PlayerControls : MonoBehaviour
	{
		public int player_id;
		public KeyCode left_key;
		public KeyCode right_key;
		public KeyCode up_key;
		public KeyCode down_key;
		public KeyCode action_key;

		public float touchYCenter;

		private Vector2 move = Vector2.zero;
		private bool action_press = false;
		private bool action_hold = false;

		private static Dictionary<int, PlayerControls> controls = new Dictionary<int, PlayerControls>();

		private Vector3 startPoint;

		public bool IsStay { get { return move == Vector2.zero; } }

		void Awake()
		{
			controls[player_id] = this;
		}

		void OnDestroy()
		{
			controls.Remove(player_id);
		}

		void Update()
		{
			move = Vector2.zero;
			action_hold = false;
			action_press = false;

#if UNITY_ANDROID && !UNITY_EDITOR
			if (Input.touchCount > 0)
			{
				if (startPoint == Vector3.zero)
					startPoint = Input.GetTouch(0).position;

				if (Input.GetTouch(0).position.x - startPoint.x > 0)
					move += Vector2.right;
				if (Input.GetTouch(0).position.x - startPoint.x < 0)
					move += -Vector2.right;
				if (Input.GetTouch(0).position.y - startPoint.y > 0)
					move += Vector2.up;
				if (Input.GetTouch(0).position.y - startPoint.y < 0)
					move += -Vector2.up;
			}
			else
			{
				startPoint = Vector3.zero;
			}
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
			if (Input.GetMouseButton(0))
			{
				if (startPoint == Vector3.zero)
				{
					Debug.Log("L");
					startPoint = Input.mousePosition;
				}
				if (Input.mousePosition.x - startPoint.x > 0)
					move += Vector2.right;
				if (Input.mousePosition.x - startPoint.x < 0)
					move += -Vector2.right;
				if (Input.mousePosition.y - startPoint.y > 0)
					move += Vector2.up;
				if (Input.mousePosition.y - startPoint.y < 0)
					move += -Vector2.up;
			}
			else
			{
				startPoint = Vector3.zero;
			}
#endif

			/*if (Input.GetKey(left_key))
				move += -Vector2.right;
			if (Input.GetKey(right_key))
				move += Vector2.right;
			if (Input.GetKey(up_key))
				move += Vector2.up;
			if (Input.GetKey(down_key))
				move += -Vector2.up;
			if (Input.GetKey(action_key))
				action_hold = true;
			if (Input.GetKeyDown(action_key))
				action_press = true;*/

			float move_length = Mathf.Min(move.magnitude, 1f);
			move = move.normalized * move_length;

			if (move != Vector2.zero)
				AudioManager.Instance.PlayLoopSound("Walk");
			else
				AudioManager.Instance.StopLoopSound("Walk");
		}


		//------ These functions should be called from the Update function, not FixedUpdate
		public Vector2 GetMove()
		{
			return move;
		}

		public bool GetActionDown()
		{
			return action_press;
		}

		public bool GetActionHold()
		{
			return action_hold;
		}

		//-----------

		public static PlayerControls Get(int player_id)
		{
			foreach (PlayerControls control in GetAll())
			{
				if (control.player_id == player_id)
				{
					return control;
				}
			}
			return null;
		}

		public static PlayerControls[] GetAll()
		{
			PlayerControls[] list = new PlayerControls[controls.Count];
			controls.Values.CopyTo(list, 0);
			return list;
		}

	}

}