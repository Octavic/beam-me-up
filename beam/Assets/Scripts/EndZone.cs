using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public class EndZone : Collidable
	{
		// Constructor
		public EndZone()
		{

		}
		// Bool to keep track of the finished state
		public bool IsFinished {  get; private set; }

		// On collision enter, if it's a palyer, then it's finished
		void OnTriggerEnter2D(Collider2D sender)
		{
			if (sender.transform.tag == "Player")
			{
				this.IsFinished = true;
			}
		}
		// On collision leave, if it's a palyer, then it's not finished
		void OnTriggerExit2D(Collider2D sender)
		{
			if (sender.transform.tag == "Player")
			{
				this.IsFinished = false;
			}
		}
	}
}
