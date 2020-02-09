using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
			if (EventSystem.current.IsPointerOverGameObject()) return;

			move = Vector2.zero;
			action_hold = false;
			action_press = false;

#if UNITY_ANDROID && !UNITY_EDITOR
			if (Input.touchCount > 0)
			{
				if (startPoint == Vector3.zero)
					startPoint = Input.GetTouch(0).position;

				move.x = Input.GetTouch(0).position.x - startPoint.x;
				move.y = Input.GetTouch(0).position.y - startPoint.y;
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
					startPoint = Input.mousePosition;
				}
				move.x = Input.mousePosition.x - startPoint.x;
				move.y = Input.mousePosition.y - startPoint.y;
			}
			else
			{
				startPoint = Vector3.zero;
			}
#endif

			float maxDiff = Mathf.Abs(move.x)>Mathf.Abs(move.y) ? Mathf.Abs(move.x):Mathf.Abs(move.y);
			move/=maxDiff;

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