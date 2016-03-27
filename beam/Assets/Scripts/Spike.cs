using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public class Spike : SolidEntity
	{
		public override void Toggle()
		{
			throw new NotImplementedException();
		}

		void OnTriggerEnter2D(Collider2D sender)
		{
			// Get the tag of the sender
			var senderTag = sender.tag;
			if (senderTag == "Player" || senderTag == "Weight")
			{
				GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ResetLevel();
			}
		}
    }
}
