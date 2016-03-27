using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public abstract class SolidEntity : ScreenEntity
	{
		// If the entity is toggled on or off
		[HideInInspector]
		public bool IsToggledAndActive
		{
			get; set;
		}

		// Set the toggle state
		public void SetToggleState(bool state)
		{
			this.IsToggledAndActive = state;
		}
		// Toggle the toggle state
		public abstract void Toggle();

		// When an item collides against it
		void OnTriggerEnter2D(Collider2D sender)
		{
			// If inactive, don't return anything
			//if (!IsToggledAndActive)
			//{
			//	return;
			//}
			// Get the tag of the sender
			var senderTag = sender.tag;
			// Get the corresponding sender class
			MovableEntity senderClass = null;
			if (senderTag == "Player")
			{
				senderClass = sender.GetComponent<Player>();
			}
			else if (senderTag == "Weight")
			{
				senderClass = sender.GetComponent<Weight>();
			}
			else
			{
				return;
			}

			senderClass.IsAffectedByGravity = false;

			// Get the position of sender
			var senderX = sender.transform.position.x;
			var senderY = sender.transform.position.y;
			// Get the offset
			var offsetX = senderX - this.transform.position.x;
			var offsetY = senderY - this.transform.position.y;

			if (Math.Abs(offsetX) -0.02 < Math.Abs(offsetY))
			{
				// Vertical collision
				senderClass.ResetVelocityY();
				senderClass.MoveTo(new Vector2(senderX, this.transform.position.y + (offsetY > 0 ? 0.31f : -0.31f)));
				if (senderClass is Player)
				{
					Player p = (Player)senderClass;
					p.ResetJumpFrames();
				}
			}
			else
			{
				senderClass.ResetVelocityX();
				senderClass.MoveTo(new Vector2(this.transform.position.x + (offsetX > 0 ? 0.31f : -0.31f), senderY));
			}
		}
		// When the collider stays
		void OnTriggerStay2D(Collider2D sender)
		{
			this.OnTriggerEnter2D(sender);
		}
		// When the collider leaves
		void OnTriggerExit2D(Collider2D sender)
		{
			// Get the tag of the sender
			var senderTag = sender.tag;
			// Get the corresponding sender class
			MovableEntity senderClass = null;
			if (senderTag == "Player")
			{
				senderClass = sender.GetComponent<Player>();
			}
			else if (senderTag == "Weight")
			{
				senderClass = sender.GetComponent<Weight>();
			}
			else
			{
				return;
			}

			senderClass.IsAffectedByGravity = true;
		}
	}
}
