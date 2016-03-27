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
		private static float GravityConstant = 0.1f;

		// Total states of the entity
		public enum EntityState
		{
			OnGround,
			Airborne,
			InBeam
		}

		// The current state of the entity
		public EntityState CurrentState{get; set;}

		// The combined force that's currently affecting the object
		private Vector2 _force;
		// The current velocity of the object
		private Vector2 _velocity;

		// Reset the current x and y velocity
		public void ResetXVelocity()
		{
			this._velocity.x = 0;
		}
		public void ResetYVelocity()
		{
			this._velocity.y = 0;
		}

		// Move the entity
		public void Move(Vector2 offset)
		{
			this.transform.Translate(offset);
		}
		public void MoveTo(Vector2 targetPosition)
		{
			this.Move(targetPosition - new Vector2(this.transform.position.x, this.transform.position.y));
		}

		// When updating, move the block based on the velocity
		void Update()
		{
			// Reset the force
			this._force = new Vector2(0, 0);
			// If the object is in air, add gravity
			if (this.CurrentState == EntityState.Airborne)
			{
				this._force += new Vector2(0, GravityConstant);
            }

			this._velocity += _force;
			this.transform.Translate(_velocity);
		}
	}
}
