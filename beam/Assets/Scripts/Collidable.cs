using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public abstract class Collidable : MonoBehaviour
	{
		public void Initialize(Vector2 tilePosition, Sprite sprite)
		{
			this.transform.position = TileCoordinate.TranslateToUnity(tilePosition);
			this.GetComponent<SpriteRenderer>().sprite = sprite;
		}
	}
}
