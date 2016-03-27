using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public class Player : MovableEntity
	{
		// Forces
		public float VerticalForce;
		public float HorizontalForce;
		// Max movement speed
		public float MaxHorizontalMovementSpeed;
		// Max jump frames
		public int MaxJumpFrames;
		// Friction
		public float HorizontalFriction;

		// Beam base
		public GameObject BeamBase;

		// The control scheme for the player
		public KeyCode Up;
		public KeyCode Down;
		public KeyCode Left;
		public KeyCode Right;
		public KeyCode Fire;

		[HideInInspector]public bool IsInEndZone {
			get; set;
		}

		// List of beams shot out from the player
		private IList<BeamSegment> _beamList;

		// How many frames have passed in the previous jump
		private int _jumpFramesHeld;

		// Initialize
		void Start()
		{
			this._beamList = new List<BeamSegment>();
			this.IsInEndZone = false;
			this.IsAffectedByGravity = true;
		}

		// Reset the jump frames
		public void ResetJumpFrames()
		{
			this._jumpFramesHeld = 0;
		}

		// Update
		void Update()
		{
			if (Input.GetKey(Up))
			{
				this._jumpFramesHeld++;
				if (this._jumpFramesHeld < MaxJumpFrames)
				{
					UpdateVelocityY(VerticalForce);
				}
			}
			else
			{
				if (this._jumpFramesHeld > 0)
				{
					this._jumpFramesHeld += MaxJumpFrames;
				}
			}
			if (Input.GetKey(Down))
			{
				UpdateVelocityY(-VerticalForce);
			}
			if (Input.GetKey(Right))
			{
				UpdateVelocityX(HorizontalForce);
			}
			if (Input.GetKey(Left))
			{
				UpdateVelocityX(-HorizontalForce);
			}
			// limit the velocity
			var currentVelocityx = GetVelocityX();
			if (Math.Abs(currentVelocityx) > MaxHorizontalMovementSpeed)
			{
				this.ResetVelocityX(currentVelocityx > 0 ? MaxHorizontalMovementSpeed : -MaxHorizontalMovementSpeed);
			}
			this.UpdateVelocityX(currentVelocityx > 0 ? -HorizontalFriction : HorizontalFriction);
			base._Update();

			// Emit the beam if the key is pressed
			if (Input.GetKey(Fire))
			{
				// Add the upwards beam
				var newBeam = Instantiate(BeamBase);
				newBeam.AddComponent<BeamSegment>();
				var newBeamClass = newBeam.GetComponent<BeamSegment>();
				newBeamClass.InitializeBeam(BeamSegment.Direction.Up);
				this._beamList.Add(newBeamClass);

				// Add the downwards beam
				newBeam = Instantiate(BeamBase);
				newBeam.AddComponent<BeamSegment>();
				newBeamClass = newBeam.GetComponent<BeamSegment>();
				newBeamClass.InitializeBeam(BeamSegment.Direction.Down);
				this._beamList.Add(newBeamClass);

                // Add the left beam
                newBeam = Instantiate(BeamBase);
				newBeam.AddComponent<BeamSegment>();
				newBeamClass = newBeam.GetComponent<BeamSegment>();
				newBeamClass.InitializeBeam(BeamSegment.Direction.Left);
				this._beamList.Add(newBeamClass);

                // Add the right beam
                newBeam = Instantiate(BeamBase);
				newBeam.AddComponent<BeamSegment>();
				newBeamClass = newBeam.GetComponent<BeamSegment>();
				newBeamClass.InitializeBeam(BeamSegment.Direction.Right);
				this._beamList.Add(newBeamClass);

            }

			foreach (var beam in this._beamList)
			{
				beam.UpdateBeam(this.transform.position);
			}
		}

		public override void Toggle()
		{
			throw new NotImplementedException();
		}
	}
}
