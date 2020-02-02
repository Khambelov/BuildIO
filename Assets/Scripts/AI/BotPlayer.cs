using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPlayer : IndieMarc.TopDown.Character
{
	Pathfinding.AIDestinationSetter setter;
	Transform currentTarget;

	// Start is called before the first frame update
	void Start()
	{
		// setter = transform.parent.GetComponent<Pathfinding.AIDestinationSetter>();
		setter = GetComponent<Pathfinding.AIDestinationSetter>();
	}

	private void FixedUpdate()
	{

	}
	Transform lastTarget;
	bool humanMode = false;

	// Update is called once per frame
	void Update()
	{
		if (!UIManager.Instance.isStartGame) return;

		if (humanMode && currentTarget)
		{
			float dist = Vector2.Distance(transform.position, currentTarget.position);
			Human human = currentTarget.GetComponent<Human>();
			if (human.used)
			{
				lastTarget = currentTarget;
				currentTarget = null;
				humanMode = false;
			}

			if (currentTarget && dist < 1 && !human.used && human.gameObject.activeSelf)
			{
				human.used = true;
				EmployeeAdd(currentTarget.position);
				PoolingManager.Instance.release(human);
				humanMode = false;
			}
			return;
		}

		if (currentTarget != null)
		{
			House house = currentTarget.GetComponent<House>();
			if (house && house.isBuilded())
			{
				currentTarget = null;
			}
		}

		if (currentTarget == null)
		{
			currentTarget = BotTargetSelector.Instance.findTarget(this, lastTarget, out humanMode);
			setter.target = currentTarget;
			return;
		}

		if (Vector2.Distance(currentTarget.position, transform.position) < 2.5f)
		{
			currentTarget = null;
		}
		else
		{
			// Debug.Log(getEmployeesCount());
		}

	}
}
