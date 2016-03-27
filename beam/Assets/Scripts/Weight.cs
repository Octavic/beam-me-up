using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public class Weight : MovableEntity
	{
		void Start()
		{
			this.IsToggledAndActive = true;
			int i = 0;
		}

		void FixedUpdate()
		{
			base._Update();
		}
	}
}
