using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public class Player : MovableEntity
	{
		// List of possible player states
		public enum PlayerState
		{
			Idle,
			Opening,
			OpenedIdle,
			Closing
		}

		// Forces
		public float VerticalForce;
		public float HorizontalForce;
		// Max movement speed
		public float MaxHorizontalMovementSpeed;
		public float MaxVerticalMovementSpeed;
		// Max jump frames
		public int MaxJumpFrames;
		// Friction
		public float HorizontalFriction;
		public float VerticalFriction;

		// Beam base
		public GameObject BeamBase;

        // Firing
        public bool firing = false;

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

        private Animator animator;

		// Initialize
		void Start()
		{
			this._beamList = new List<BeamSegment>();
			this.IsInEndZone = false;
			this.IsAffectedByGravity = true;
            animator = this.GetComponent<Animator>();
		}

		// Reset the jump frames
		public void ResetJumpFrames()
		{
			this._jumpFramesHeld = 0;
		}

		// Update
		void FixedUpdate()
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
			var currentVelocityy = GetVelocityY();
			if (Math.Abs(currentVelocityy) > MaxVerticalMovementSpeed)
			{
				this.ResetVelocityY(currentVelocityy > 0 ? MaxVerticalMovementSpeed : -MaxVerticalMovementSpeed);
			}
			// Apply friction
			if (Math.Abs(currentVelocityx) >= HorizontalFriction)
			{
				this.UpdateVelocityX(currentVelocityx > 0 ? -HorizontalFriction : HorizontalFriction);
			}
			else
			{
				this.UpdateVelocityX(currentVelocityx);
            }
			// Apply friction
			if (Math.Abs(currentVelocityy) >= VerticalFriction)
			{
				this.UpdateVelocityX(currentVelocityy > 0 ? -VerticalFriction : VerticalFriction);
			}
			else
			{
				this.UpdateVelocityY(currentVelocityy);
			}


			// Update the velocity onto the object
			base._Update();
		}

		void Update()
		{
			// Emit the beam if the key is pressed
			if (Input.GetKeyDown(Fire))
			{
                animator.SetBool("Firing", !firing);
                firing = !firing;
                if (this._beamList.Count == 0)
				{
					// Add the upwards beam
					var newBeam = Instantiate(BeamBase);
					newBeam.AddComponent<BeamSegment>();
					var newBeamClass = newBeam.GetComponent<BeamSegment>();
					newBeamClass.InitializeBeam(BeamSegment.Direction.Up, this.transform.position);
					this._beamList.Add(newBeamClass);

					// Add the downwards beam
					newBeam = Instantiate(BeamBase);
					newBeam.AddComponent<BeamSegment>();
					newBeamClass = newBeam.GetComponent<BeamSegment>();
					newBeamClass.InitializeBeam(BeamSegment.Direction.Down, this.transform.position);
					this._beamList.Add(newBeamClass);

					// Add the left beam
					newBeam = Instantiate(BeamBase);
					newBeam.AddComponent<BeamSegment>();
					newBeamClass = newBeam.GetComponent<BeamSegment>();
					newBeamClass.InitializeBeam(BeamSegment.Direction.Left, this.transform.position);
					this._beamList.Add(newBeamClass);

					// Add the right beam
					newBeam = Instantiate(BeamBase);
					newBeam.AddComponent<BeamSegment>();
					newBeamClass = newBeam.GetComponent<BeamSegment>();
					newBeamClass.InitializeBeam(BeamSegment.Direction.Right, this.transform.position);
					this._beamList.Add(newBeamClass);
				}
				else
				{
					foreach (var beam in this._beamList)
					{
						Destroy(beam.gameObject);
					}
					this._beamList = new List<BeamSegment>();
				}
			}

			foreach (var beam in this._beamList)
			{
				beam.UpdateBeam(this.transform.position);
			}
		}
	}
}
