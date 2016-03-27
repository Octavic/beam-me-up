using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public abstract class Switch : ScreenEntity
	{
		// The on and off sprites
		public Sprite OnSprite;
		public Sprite OffSprite;

		// A list of child objects to be toggled
		protected IList<ScreenEntity> _childToggleObjectList;

		// The toggle state
		public bool IsActivated
		{
			get; set;
		}

		void Start()
		{
			
		}

		// Add to the child objects
		public void AddToChildToggleObjectList(ScreenEntity entity)
		{
			if (this._childToggleObjectList == null)
			{
				this._childToggleObjectList = new List<ScreenEntity>();
			}
			this._childToggleObjectList.Add(entity);
		}

		// On collision enter and exit
		void OnTriggerEnter2D(Collider2D sender)
		{
			var senderTag = sender.tag;
			if (senderTag == "Player" || senderTag == "Weight")
            {
				this.IsActivated = true;
			}
			foreach (var child in this._childToggleObjectList)
			{
				child.Toggle();
			}
		}
		void OnTriggerExit2D(Collider2D sender)
		{
			var senderTag = sender.tag;
			if (senderTag == "Player" || senderTag == "Weight")
			{
				this.IsActivated = false;
			}
			foreach (var child in this._childToggleObjectList)
			{
				child.Toggle();
			}
		}

		// Update
		void Update()
		{
			if (this.IsActivated)
			{
				this.GetComponent<SpriteRenderer>().sprite = OnSprite;
			}
			else
			{
				this.GetComponent<SpriteRenderer>().sprite = OffSprite;
			}
		}
	}
}
