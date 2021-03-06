﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public class BeamSegment : MonoBehaviour
	{
        // How powerfully the beam pulls you towards itself.
        private static float _gravityForce;
        private static float _verticalForce;
        private static float _horizontalForce;

		// power factor
		private static float _powerFactor = 0.8f;
		
        // A list of entities trapped in the beam
        private IList<MovableEntity> _trappedEntityList;

		// The position in the previous frame
		private Vector2 _previousPosition;

		// The direction
		private Direction _direction;
		private Vector2 _vectorDirection;


		public enum Direction
		{
			Up,
			Right,
			Down,
			Left
		}
		
		// Initialize the beam
		public void InitializeBeam(Direction d, Vector2 playerPosition)
		{
            _gravityForce = GameObject.FindWithTag("Player").GetComponent<Player>().GravityConstant;
            _verticalForce = GameObject.FindWithTag("Player").GetComponent<Player>().VerticalForce * _powerFactor;
            _horizontalForce = GameObject.FindWithTag("Player").GetComponent<Player>().HorizontalForce * _powerFactor;
            this._previousPosition = playerPosition;
			this._trappedEntityList = new List<MovableEntity>();
			this._direction = d;
			_vectorDirection = new Vector2(-1,0);
			int angle = 90;
			switch (d)
			{
				case Direction.Up:
					{
						_vectorDirection = new Vector2(0, 1);
						angle = 0;
						break;
                    }
				case Direction.Right:
					{
						_vectorDirection = new Vector2(1, 0);
						angle = 90;
						break;
					}
				case Direction.Down:
					{
						_vectorDirection = new Vector2(0, -1);
						angle = 0;
						break;
					}
			}
			this.transform.Rotate(Vector3.forward,angle);
        }
		
		// Update the beam to match the correct length and player
		public void UpdateBeam(Vector2 playerPosition)
		{
			this._trappedEntityList = new List<MovableEntity>();
            var positionOffset = playerPosition - _previousPosition;
			var hits = Physics2D.RaycastAll(playerPosition, _vectorDirection);
			this.transform.position = playerPosition;
			float length = 23;

			bool isFirstPlayerSkipped = false;

			for (int index = 1; index < hits.Length; index++)
			{
				var hit = hits[index];
				var hitTag = hit.transform.tag;
				// If it hits glass, spikes or another beak
				if (hitTag == "Transparent" || hitTag == "Beam")
				{
					continue;
				}
				if (hitTag == "Player")
				{
					if (!isFirstPlayerSkipped)
					{
						isFirstPlayerSkipped = true;
						continue;
					}
					this._trappedEntityList.Add(hit.transform.GetComponent<Player>());
				}
				else if (hitTag == "Weight")
				{
					this._trappedEntityList.Add(hit.transform.GetComponent<Weight>());
				}
				else if (hitTag == "Solid")
				{
					float distance;
					if (_direction == Direction.Up || _direction == Direction.Down)
					{
						distance = Math.Abs(hit.transform.position.y - this.transform.position.y);
					}
					else
					{
						distance = Math.Abs(hit.transform.position.x - this.transform.position.x);
					}
					distance /= 0.32f;
					length = distance;
					break;
				}
			}
            if (_direction == Direction.Up)
            {
                this.transform.Translate(new Vector3(0, length * 0.16f, 0));
                foreach (var trapped in _trappedEntityList)
                {
                    trapped.UpdateVelocity(new Vector2(Mathf.Sign(this.transform.position.x - trapped.transform.position.x) * _horizontalForce, -1 * _gravityForce));
                    trapped.transform.Translate(new Vector3(0, positionOffset.y, 0));
                }
			}
			else if (_direction == Direction.Down)
			{
				this.transform.Translate(new Vector3(0, -length * 0.16f, 0));
                foreach (var trapped in _trappedEntityList)
                {
                    trapped.UpdateVelocity(new Vector2(Mathf.Sign(this.transform.position.x - trapped.transform.position.x) * _horizontalForce, -1 * _gravityForce));
                    trapped.transform.Translate(new Vector3(0, positionOffset.y, 0));
                }
            }
			else if (_direction == Direction.Left)
			{
				this.transform.Translate(new Vector3(0, length * 0.16f, 0));
                foreach (var trapped in _trappedEntityList)
                {
                    if(this.transform.position.y - trapped.transform.position.y <= 0)
                    {
                        trapped.UpdateVelocity(new Vector2(0 , -1 * _gravityForce + _verticalForce));
                    }
                    else
                    {
                        trapped.UpdateVelocity(new Vector2(0, 0));
                    }
                    trapped.transform.Translate(new Vector3(0, positionOffset.x, 0));
                }
            }
			else // Direction.Right
			{
				this.transform.Translate(new Vector3(0, -length * 0.16f, 0));
                foreach (var trapped in _trappedEntityList)
                {
                    if (this.transform.position.y - trapped.transform.position.y <= 0)
                    {
                        trapped.UpdateVelocity(new Vector2(0, -1 * _gravityForce + _verticalForce));
                    }
                    else
                    {
                        trapped.UpdateVelocity(new Vector2(0, 0));
                    }
                    trapped.transform.Translate(new Vector3(0, positionOffset.x, 0));
                }
            }

			this.transform.localScale = new Vector3(transform.localScale.x, length, transform.localScale.z);
            // Update beam's previous position
            _previousPosition = playerPosition;
        }
    }
}
