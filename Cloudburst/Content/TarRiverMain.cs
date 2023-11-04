using Cloudburst.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cloudburst.Content
{
    public class TarRiverMain
    {
        public TarRiverMain()
        {
            if (!Config.BindAndOptions<bool>("Misc", "Enable Tar River", true, "Toggles the Tar River on Goolake").Value) return;

            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            if(arg1.name == "goolake")
            {
                GameObject holder = GameObject.Find("GooPlane, High");
                if(!holder) return;

                holder.transform.Find("GooPlane, High")?.gameObject.SetActive(true);
            }
        }
    }
}
