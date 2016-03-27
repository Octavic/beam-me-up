using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public abstract class SolidEntity : ScreenEntity
	{
		public SolidEntity()
		{

		}
		void OnCollisionEnter2D(Collision2D sender)
		{
			var senderTag = sender.transform.tag;
			// If the sender is a beam segment, stop it
			if (senderTag == "Beam")
			{
				sender.transform.GetComponent<BeamSegment>().StopBeam(this.transform.position);
				return;
			}
			MovableEntity movableEntity = null;

			// If the sender is movable, bounce it back to original position;
			if (senderTag == "Player")
			{
				movableEntity = sender.transform.GetComponent<Player>();
			}
			else if (senderTag == "Weight")
			{
				movableEntity = sender.transform.GetComponent<Weight>();
			}

			// Get the coordiantes for both entities
			var movableEntityX = movableEntity.transform.position.x;
			var movableEntityY = movableEntity.transform.position.y;
			var thisX = this.transform.position.x;
			var thisY = this.transform.position.y;
			// Get how much is differed
			var xDiff = movableEntityX - thisX;
			var yDiff = movableEntityY - thisY;

			// If the y different is bigger, reset it to the corresponding Y position
			if (Math.Abs(yDiff) >= Math.Abs(xDiff))
			{
				movableEntity.ResetYVelocity();
				if (yDiff > 0)
				{
					movableEntity.MoveTo(new Vector2(movableEntityX, thisY + 0.32f));
				}
				else
				{
					movableEntity.MoveTo(new Vector2(movableEntityX, thisY - 0.32f));
				}
			}
			else
			{
				movableEntity.ResetXVelocity();
				if (xDiff > 0)
				{
					movableEntity.MoveTo(new Vector2(thisX + 0.32f, movableEntityY));
				}
				else
				{
					movableEntity.MoveTo(new Vector2(thisX - 0.32f, movableEntityY));
				}
			}
        }
    }
}
