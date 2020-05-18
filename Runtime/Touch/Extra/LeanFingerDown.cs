using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Lean.Touch
{
	/// <summary>This component invokes events when a finger touches the screen that satisfies the specified conditions.</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanFingerDown")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Finger Down")]
	public class LeanFingerDown : MonoBehaviour
	{
		[System.Serializable] public class LeanFingerEvent : UnityEvent<LeanFinger> {}
		[System.Serializable] public class Vector3Event : UnityEvent<Vector3> {}

		/// <summary>Ignore fingers with StartedOverGui?</summary>
		[Tooltip("Ignore fingers with StartedOverGui?")]
		public bool IgnoreStartedOverGui = true;

		/// <summary>Do nothing if this LeanSelectable isn't selected?</summary>
		[Tooltip("Do nothing if this LeanSelectable isn't selected?")]
		public LeanSelectable RequiredSelectable;

		/// <summary>Called on the first frame the conditions are met.</summary>
		public LeanFingerEvent OnFinger { get { if (onFinger == null) onFinger = new LeanFingerEvent(); return onFinger; } } [FormerlySerializedAs("onDown")] [FormerlySerializedAs("OnDown")] [SerializeField] private LeanFingerEvent onFinger;
		
		/// <summary>The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more information.</summary>
		public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

		/// <summary>Called on the first frame the conditions are met.
		/// Vector3 = Start point based on the ScreenDepth settings.</summary>
		public Vector3Event OnPosition { get { if (onPosition == null) onPosition = new Vector3Event(); return onPosition; } } [SerializeField] private Vector3Event onPosition;
#if UNITY_EDITOR
		protected virtual void Reset()
		{
			RequiredSelectable = GetComponentInParent<LeanSelectable>();
		}
#endif
		protected virtual void Awake()
		{
			if (RequiredSelectable == null)
			{
				RequiredSelectable = GetComponentInParent<LeanSelectable>();
			}
		}

		protected virtual void OnEnable()
		{
			LeanTouch.OnFingerDown += HandleOnFingerDown;
		}

		protected virtual void OnDisable()
		{
			LeanTouch.OnFingerDown -= HandleOnFingerDown;
		}

		private void HandleOnFingerDown(LeanFinger finger)
		{
			if (IgnoreStartedOverGui == true && finger.IsOverGui == true)
			{
				return;
			}

			if (RequiredSelectable != null && RequiredSelectable.IsSelected == false)
			{
				return;
			}

			if (onFinger != null)
			{
				onFinger.Invoke(finger);
			}

			if (onPosition != null)
			{
				var position = ScreenDepth.Convert(finger.StartScreenPosition, gameObject);

				onPosition.Invoke(position);
			}
		}
	}
}
