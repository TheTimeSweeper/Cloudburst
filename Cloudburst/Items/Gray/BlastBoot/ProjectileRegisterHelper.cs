using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UnityEngine;

namespace Cloudburst.Items.Gray.BlastBoot
{
	class ProjectileRegisterHelper
	{
		public class Projectiles
		{
			public static void RegisterProjectile(GameObject projectile)
			{
				if (ProjectileDefinitions.Contains(projectile) || !projectile || !projectile.GetComponent<ProjectileController>())
				{
					string text = ((projectile != null) ? projectile.ToString() : null) + " has already been registered, please do not register the same projectile twice.";
					if (!projectile.GetComponent<ProjectileController>())
					{
						text += " And/Or, the projectile does not have a projectile controller component.";
					}
				}
				ProjectileDefinitions.Add(projectile);
			}

			internal static GameObject[] DumpContent()
			{
				List<GameObject> list = new List<GameObject>();
				foreach (GameObject item in ProjectileDefinitions)
				{
					list.Add(item);
				}
				return list.ToArray();
			}

			internal static ObservableCollection<GameObject> ProjectileDefinitions = new ObservableCollection<GameObject>();
		}
	}
}
