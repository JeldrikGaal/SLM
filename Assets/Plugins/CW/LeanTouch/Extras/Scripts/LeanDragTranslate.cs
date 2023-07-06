using UnityEngine;
using CW.Common;

namespace Lean.Touch
{
	/// <summary>This component allows you to translate the current GameObject relative to the camera using the finger drag gesture.</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanDragTranslate")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Drag Translate")]
	public class LeanDragTranslate : MonoBehaviour
	{
		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter Use = new LeanFingerFilter(true);

		/// <summary>The camera the translation will be calculated using.
		/// None/null = MainCamera.</summary>
		public Camera Camera { set { _camera = value; } get { return _camera; } } [SerializeField] private Camera _camera;

		/// <summary>The movement speed will be multiplied by this.
		/// -1 = Inverted Controls.</summary>
		public float Sensitivity { set { sensitivity = value; } get { return sensitivity; } } [SerializeField] private float sensitivity = 1.0f;

		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		public float Damping { set { damping = value; } get { return damping; } } [SerializeField] protected float damping = -1.0f;

		/// <summary>This allows you to control how much momentum is retained when the dragging fingers are all released.
		/// NOTE: This requires <b>Dampening</b> to be above 0.</summary>
		public float Inertia { set { inertia = value; } get { return inertia; } } [SerializeField] [Range(0.0f, 1.0f)] private float inertia;

		[SerializeField]
		private Vector3 remainingTranslation;

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually add a finger.</summary>
		public void AddFinger(LeanFinger finger)
		{
			Use.AddFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove a finger.</summary>
		public void RemoveFinger(LeanFinger finger)
		{
			Use.RemoveFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove all fingers.</summary>
		public void RemoveAllFingers()
		{
			Use.RemoveAllFingers();
		}

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}
#endif

		protected virtual void Awake()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}

		private void LimitPositionToScreenBounds()
		{
			RectTransform rectTransform = GetComponent<RectTransform>();
			if (rectTransform != null)
			{
				Canvas canvas = GetComponentInParent<Canvas>();
				if (canvas != null)
				{
					Vector3[] corners = new Vector3[4];
					rectTransform.GetWorldCorners(corners);
					Rect canvasRect = canvas.pixelRect;

					float margin = 0.2f; // 20% margin
					float marginWidth = canvasRect.width * margin;
					float marginHeight = canvasRect.height * margin;

					Vector3 bottomLeft = RectTransformUtility.WorldToScreenPoint(Camera.main, corners[0]);
					Vector3 topRight = RectTransformUtility.WorldToScreenPoint(Camera.main, corners[2]);

					Vector2 localPoint;
					RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, bottomLeft, null, out localPoint);

					// Update margins
					canvasRect.min = new Vector2(canvasRect.min.x + marginWidth, canvasRect.min.y + marginHeight);
					canvasRect.max = new Vector2(canvasRect.max.x - marginWidth, canvasRect.max.y - marginHeight);

					// Update rect transform
					Vector2 clampedLocalPos = Vector2.Min(Vector2.Max(canvasRect.min, localPoint), canvasRect.max - rectTransform.rect.size);
					rectTransform.anchoredPosition = clampedLocalPos;
				}
			}
		}




		protected virtual void Update()
		{
			// Store
			var oldPosition = transform.localPosition;

			// Get the fingers we want to use
			var fingers = Use.UpdateAndGetFingers();

			// Calculate the screenDelta value based on these fingers
			var screenDelta = LeanGesture.GetScreenDelta(fingers);
			

			if (screenDelta != Vector2.zero)
			{
				// Perform the translation
				if (transform is RectTransform)
				{
					TranslateUI(screenDelta);
				}
				else
				{
					Translate(screenDelta);
				}
			}

			// Increment
			remainingTranslation += transform.localPosition - oldPosition;
			// Get t value
			var factor = CwHelper.DampenFactor(Damping, Time.deltaTime);

			// Dampen remainingDelta
			var newRemainingTranslation = Vector3.Lerp(remainingTranslation, Vector3.zero, factor);

			// Shift this transform by the change in delta
			transform.localPosition = oldPosition + remainingTranslation - newRemainingTranslation;
			transform.localPosition = new Vector2(Mathf.Clamp(transform.localPosition.x, -700f, 700f), Mathf.Clamp(transform.localPosition.y, -400f, 400f));

			

			if (fingers.Count == 0 && inertia > 0.0f && Damping > 0.0f)
			{
				newRemainingTranslation = Vector3.Lerp(newRemainingTranslation, remainingTranslation, inertia);
			}

			// Update remainingDelta with the dampened value
			remainingTranslation = newRemainingTranslation;
			// remainingTranslation = new Vector2(Mathf.Clamp(newRemainingTranslation.x, 0f, 1f * Screen.width), Mathf.Clamp(newRemainingTranslation.y, 0f, 1f * Screen.height));

			//LimitPositionToScreenBounds();
			

		}

		private void TranslateUI(Vector2 screenDelta)
		{
			var finalCamera = _camera;

			if (finalCamera == null)
			{
				var canvas = transform.GetComponentInParent<Canvas>();

				if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
				{
					finalCamera = canvas.worldCamera;
				}
			}

			// Screen position of the transform
			var screenPoint = RectTransformUtility.WorldToScreenPoint(finalCamera, transform.position);

			// Add the deltaPosition
			screenPoint += screenDelta * Sensitivity;
			
			// ONLY FOR EFFECT TO NOT CLAMP SO HARSH
			float _marginClampEffectX = 0.0f;
			float _marginClampEffectY = 0.0f;
			screenPoint = new Vector2(Mathf.Clamp(screenPoint.x, _marginClampEffectX * Screen.width, (1-_marginClampEffectX) * Screen.width), Mathf.Clamp(screenPoint.y, _marginClampEffectY * Screen.height, (1-_marginClampEffectY) * Screen.height));


			// Convert back to world space
			var worldPoint = default(Vector3);

			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.parent as RectTransform, screenPoint, finalCamera, out worldPoint) == true)
			{
				transform.position = worldPoint;
				//transform.position = new Vector2(Mathf.Clamp(worldPoint.x, 0f, 1f * Screen.width), Mathf.Clamp(worldPoint.y, 0f, 1f * Screen.height));
			}
		}

		private void Translate(Vector2 screenDelta)
		{
			// Make sure the camera exists
			var camera = CwHelper.GetCamera(this._camera, gameObject);

			if (camera != null)
			{
				// Screen position of the transform
				var screenPoint = camera.WorldToScreenPoint(transform.position);

				// Add the deltaPosition
				screenPoint += (Vector3)screenDelta * Sensitivity;

				// Convert back to world space
				transform.position = camera.ScreenToWorldPoint(screenPoint);
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your camera as MainCamera, or set one in this component.", this);
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Editor
{
	using UnityEditor;
	using TARGET = LeanDragTranslate;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET), true)]
	public class LeanDragTranslate_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("Use");
			Draw("_camera", "The camera the translation will be calculated using.\n\nNone/null = MainCamera.");
			Draw("sensitivity", "The movement speed will be multiplied by this.\n\n-1 = Inverted Controls.");
			Draw("damping", "If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.");
			Draw("inertia", "This allows you to control how much momentum is retained when the dragging fingers are all released.\n\nNOTE: This requires <b>Damping</b> to be above 0.");
		}
	}
}
#endif