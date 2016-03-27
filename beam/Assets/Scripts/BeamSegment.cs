using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public class BeamSegment : MonoBehaviour
	{
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
		public void InitializeBeam(Direction d)
		{
			this._direction = d;
			_vectorDirection = new Vector2(-1,0);
			int angle = -90;
			switch (d)
			{
				case Direction.Up:
					{
						_vectorDirection = new Vector2(0, -1);
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
						_vectorDirection = new Vector2(0, 1);
						angle = 180;
						break;
					}
			}
			this.transform.Rotate(new Vector3(0, 0, 1), angle);
        }
		
		// Update the beam to match the correct length and player
		public void UpdateBeam(Vector2 playerPosition)
		{
			var hits = Physics.RaycastAll(playerPosition, _vectorDirection);
			float length = 23;
			foreach (var hit in hits)
			{
				var hitTag = hit.transform.tag;
				// If it hits glass or spikes
				if (hitTag == "Transparent")
				{
					continue;
				}
				if (hitTag == "Player")
				{
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
					if (_direction == Direction.Up)
					{
						this.transform.Translate(new Vector3(0, -distance / 2, 0));

					}
					else if (_direction == Direction.Down)
					{
						this.transform.Translate(new Vector3(0, distance / 2, 0));
                        this.transform.Rotate(new Vector3(0, 0, 90));
                    }
					else if (_direction == Direction.Left)
					{
						this.transform.Translate(new Vector3(-distance / 2, 0, 0));
					}
					else
					{
						this.transform.Translate(new Vector3(distance / 2, 0, 0));
                        this.transform.Rotate(new Vector3(0, 0, 90));
                    }

					this.transform.localScale = new Vector3(transform.localScale.x, distance, transform.localScale.z);
				}
			}
		}
	}
}
