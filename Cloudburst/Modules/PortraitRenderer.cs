using RoR2;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using Path = System.IO.Path; // RoR2 has a Path too

namespace Cloudburst.Modules
{
    // Dev tool, ported from Forces of Petrichor V: renders the Clay Guardsman with the game's own shaders and its idle pose.
    // Console: cloudburst_portraits -> <plugin folder>/PortraitRenders/CloudburstGuardsmanBody.png ("turntable" for 8 angles).
    // Copy the result to <plugin folder>/Portraits/ to use it in game.
    internal static class PortraitRenderer
    {
        const int Layer = 31, Size = 512;
        // yaw: 0 = straight from the front, positive swings the camera toward his right side
        // focus: height of the frame center as a fraction of the model (0 = feet, 1 = top); zoom: frame height as a fraction of the model
        static float yaw = 45f, focus = 0.68f, zoom = 0.55f;

        [ConCommand(commandName = "cloudburst_portraits", flags = ConVarFlags.None, helpText = "Renders the Clay Guardsman portrait into Cloudburst's PortraitRenders folder. Optional: [yaw] [focus] [zoom] (sticks until restart), or 'turntable' for 8 angles.")]
        static void Command(ConCommandArgs args)
        {
            if (!Enemies.Guardsman.Guardsman.bodyPrefab) { Log.Warning("cloudburst_portraits: Clay Guardsman is disabled in the config"); return; }
            bool turntable = args.Count > 0 && args[0] == "turntable";
            if (!turntable)
            {
                if (args.Count > 0) yaw = args.GetArgFloat(0);
                if (args.Count > 1) focus = args.GetArgFloat(1);
                if (args.Count > 2) zoom = args.GetArgFloat(2);
            }
            Log.Info($"cloudburst_portraits yaw {yaw} focus {focus} zoom {zoom}");
            Cloudburst.instance.StartCoroutine(Render(Enemies.Guardsman.Guardsman.bodyPrefab, turntable));
        }

        static IEnumerator Render(GameObject body, bool turntable)
        {
            var dir = Path.Combine(Path.GetDirectoryName(Cloudburst.instance.Info.Location), "PortraitRenders");
            Directory.CreateDirectory(dir);
            var stage = new GameObject("CloudburstPortraitStage");
            stage.transform.position = new Vector3(0f, -5000f, 0f); // out of sight of whatever scene is loaded
            var cam = new GameObject("Camera").AddComponent<Camera>();
            cam.transform.SetParent(stage.transform, false);
            cam.enabled = false;
            cam.orthographic = true;
            cam.cullingMask = 1 << Layer;
            cam.clearFlags = CameraClearFlags.SolidColor;
            var rt = new RenderTexture(Size, Size, 24, RenderTextureFormat.ARGB32) { antiAliasing = 8 };
            cam.targetTexture = rt;
            var key = Light(stage, 1.3f);
            key.shadows = LightShadows.Soft; // vanilla portraits have self-shadowing
            key.shadowStrength = 0.85f;
            var fill = Light(stage, 0.3f);

            // Under an inactive holder, so the model's RoR2 components (CharacterModel, AimAnimator...) never wake up without their body.
            var holder = new GameObject("holder");
            holder.SetActive(false);
            holder.transform.SetParent(stage.transform, false);
            var source = body.GetComponent<ModelLocator>().modelTransform;
            var model = Object.Instantiate(source.gameObject, holder.transform).transform;
            model.localPosition = Vector3.zero;
            // In play the facing comes from the transform CharacterDirection turns, so orient relative to that: +Z = where it walks.
            var direction = body.GetComponent<CharacterDirection>();
            var basis = direction && direction.targetTransform ? direction.targetTransform : body.transform;
            model.localRotation = Quaternion.Inverse(basis.rotation) * source.rotation;
            foreach (var b in model.GetComponentsInChildren<Behaviour>(true)) if (!(b is Animator)) b.enabled = false;
            foreach (var t in model.GetComponentsInChildren<Transform>(true)) t.gameObject.layer = Layer;
            var renderers = model.GetComponentsInChildren<Renderer>(true).Where(r => r is SkinnedMeshRenderer || r is MeshRenderer).ToArray();
            foreach (var r in model.GetComponentsInChildren<Renderer>(true).Except(renderers)) r.enabled = false; // particles, lines
            foreach (var r in renderers.OfType<SkinnedMeshRenderer>()) r.updateWhenOffscreen = true;
            holder.SetActive(true);
            var animator = model.GetComponentInChildren<Animator>();
            if (animator)
            {
                animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                if (animator.HasState(0, Animator.StringToHash("Idle"))) animator.Play("Idle", 0, 0.25f);
            }
            for (int i = 0; i < 3; i++) yield return null; // let the animator pose and the meshes skin

            var bounds = renderers.Where(r => r.enabled).Select(r => r.bounds).Aggregate((a, b) => { a.Encapsulate(b); return a; });
            byte[] Shot(float angle, bool closeUp)
            {
                cam.transform.rotation = Quaternion.Euler(10f, 180f + angle, 0f); // looks back toward the model's front, slightly from above
                var target = closeUp ? new Vector3(bounds.center.x, bounds.min.y + bounds.size.y * focus, bounds.center.z) : bounds.center;
                cam.transform.position = target - cam.transform.forward * bounds.size.magnitude * 2f;
                cam.nearClipPlane = 0.01f;
                cam.farClipPlane = bounds.size.magnitude * 5f;
                cam.orthographicSize = closeUp ? bounds.size.y * zoom * 0.5f : FitSize(cam, bounds) * 1.06f;
                key.transform.rotation = cam.transform.rotation * Quaternion.Euler(25f, -35f, 0f);
                fill.transform.rotation = cam.transform.rotation * Quaternion.Euler(-10f, 50f, 0f);
                return Cutout(cam, rt);
            }
            if (turntable)
                for (int angle = 0; angle < 360; angle += 45) File.WriteAllBytes(Path.Combine(dir, $"turn_{body.name}_{angle:000}.png"), Shot(angle, false));
            else
                File.WriteAllBytes(Path.Combine(dir, body.name + ".png"), Shot(yaw, true));

            Object.Destroy(stage);
            rt.Release();
            Log.Info($"portraits written to {dir}");
        }

        static Light Light(GameObject stage, float intensity)
        {
            var light = new GameObject("Light").AddComponent<Light>();
            light.transform.SetParent(stage.transform, false);
            light.type = LightType.Directional;
            light.intensity = intensity;
            light.cullingMask = 1 << Layer;
            return light;
        }

        // Half the larger extent of the bounds' corners as seen by the camera, so the whole model fits.
        static float FitSize(Camera cam, Bounds b)
        {
            float max = 0f;
            for (int i = 0; i < 8; i++)
            {
                var corner = b.center + Vector3.Scale(b.extents, new Vector3((i & 1) * 2 - 1, (i & 2) - 1, (i & 4) / 2 - 1));
                var local = cam.transform.InverseTransformVector(corner - b.center);
                max = Mathf.Max(max, Mathf.Abs(local.x), Mathf.Abs(local.y));
            }
            return max;
        }

        // Renders on black and on white: alpha = 1 - (white - black), color = black / alpha. The game's shaders don't write usable alpha.
        static byte[] Cutout(Camera cam, RenderTexture rt)
        {
            var fog = RenderSettings.fog;
            var (ambientMode, ambient) = (RenderSettings.ambientMode, RenderSettings.ambientLight);
            var (shadows, shadowDistance) = (QualitySettings.shadows, QualitySettings.shadowDistance);
            RenderSettings.fog = false;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.32f, 0.32f, 0.36f);
            QualitySettings.shadows = ShadowQuality.All; // regardless of the player's shadow setting
            QualitySettings.shadowDistance = Mathf.Max(shadowDistance, cam.farClipPlane);
            Color[] Grab(Color background)
            {
                cam.backgroundColor = background;
                cam.Render();
                var previous = RenderTexture.active;
                RenderTexture.active = rt;
                var tex = new Texture2D(Size, Size, TextureFormat.RGBA32, false);
                tex.ReadPixels(new Rect(0, 0, Size, Size), 0, 0);
                RenderTexture.active = previous;
                var pixels = tex.GetPixels();
                Object.Destroy(tex);
                return pixels;
            }
            var black = Grab(Color.black);
            var white = Grab(Color.white);
            RenderSettings.fog = fog;
            (RenderSettings.ambientMode, RenderSettings.ambientLight) = (ambientMode, ambient);
            (QualitySettings.shadows, QualitySettings.shadowDistance) = (shadows, shadowDistance);

            var result = new Texture2D(Size, Size, TextureFormat.RGBA32, false);
            var output = new Color[black.Length];
            for (int i = 0; i < black.Length; i++)
            {
                var b = black[i];
                var w = white[i];
                var a = Mathf.Clamp01(1f - ((w.r - b.r) + (w.g - b.g) + (w.b - b.b)) / 3f);
                output[i] = a > 0.001f ? new Color(Mathf.Clamp01(b.r / a), Mathf.Clamp01(b.g / a), Mathf.Clamp01(b.b / a), a) : Color.clear;
            }
            result.SetPixels(output);
            var png = result.EncodeToPNG();
            Object.Destroy(result);
            return png;
        }
    }
}
