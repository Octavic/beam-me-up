using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public abstract class MovableEntity : SolidEntity
	{
		// Gravity constant
		public float GravityConstant;

		// If the entity is affected by gravity
		public bool IsAffectedByGravity
		{
			get; set;
		}

		// The current velocity
		protected Vector2 _velocity;

		// Move the entity
		public void Move(Vector2 offset)
		{
			this.transform.Translate(offset);
		}
		public void MoveTo(Vector2 targetPosition)
		{
			this.transform.position = targetPosition;
		}

		// Update the velocity with offset
		public void UpdateVelocityX(float xOffset)
		{
			this._velocity.x += xOffset;
		}
		public void UpdateVelocityY(float yOffset)
		{
			this._velocity.y += yOffset;
		}
		public void UpdateVelocity(Vector2 offset)
		{
			this._velocity += offset;
		}
		public void ResetVelocityX(float target = 0)
		{
			this._velocity.x = target;
		}
		public void ResetVelocityY(float target = 0)
		{
			this._velocity.y = target;
		}
		// Get the current x or y velocity
		public float GetVelocityX()
		{
			return this._velocity.x;
		}
		public float GetVelocityY()
		{
			return this._velocity.y;
		}

		// Update
		protected void _Update()
		{
			if (this.IsAffectedByGravity)
			{
				this.UpdateVelocityY(GravityConstant);
			}
			this.transform.Translate(_velocity);

			if (this.transform.position.x < 0)
			{
				this.transform.position = new Vector2(0.001f, transform.position.y);
			}
			else if (this.transform.position.x > 6.08f)
			{
				this.transform.position = new Vector2(6.079f, transform.position.y);
			}
			this.IsAffectedByGravity = true;
        }
	}
}
