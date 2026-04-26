using UnityEngine;
using UnityEngine.EventSystems;

namespace SimpleInputNamespace
{
	public static class SimpleInputUtils
	{
		public const int NON_EXISTING_TOUCH = -98765;

		private static float m_resolutionMultiplier;
		public static float ResolutionMultiplier
		{
			get
			{
				if( m_resolutionMultiplier <= 0f )
					m_resolutionMultiplier = 100f / ( Screen.width + Screen.height );

				return m_resolutionMultiplier;
			}
		}

		public static bool IsTouchInput( this PointerEventData eventData )
		{
			if( eventData.button != PointerEventData.InputButton.Left )
				return false;

			for( int i = ControlFreak2.CF2Input.touchCount - 1; i >= 0; i-- )
			{
				if( ControlFreak2.CF2Input.GetTouch( i ).fingerId == eventData.pointerId )
					return true;
			}

			return false;
		}

		public static bool IsValid( this PointerEventData eventData )
		{
			for( int i = ControlFreak2.CF2Input.touchCount - 1; i >= 0; i-- )
			{
				if( ControlFreak2.CF2Input.GetTouch( i ).fingerId == eventData.pointerId )
					return true;
			}

			return ControlFreak2.CF2Input.GetMouseButton( (int) eventData.button );
		}
	}
}