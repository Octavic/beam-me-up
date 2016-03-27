using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public abstract class ScreenEntity : Collidable
	{
		// The toggle state
		public bool IsToggledAndActive { get; set; }

		// The sprite
		protected Sprite _sprite;

		void Start()
		{
			this.IsToggledAndActive = true;
			this._sprite = this.GetComponent<SpriteRenderer>().sprite;
		}

		// Toggle the entity
		public void Toggle()
		{
			this.IsToggledAndActive = !this.IsToggledAndActive;
			if (this.IsToggledAndActive)
			{
				this.GetComponent<SpriteRenderer>().sprite = _sprite;
			}
			else
			{
				this.GetComponent<SpriteRenderer>().sprite = null;
			}
		}
	}
}
