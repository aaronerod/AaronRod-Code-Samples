using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rod.Utilities.UI
{
    /// <summary>
    /// Utility para utilizar Safe Area de dispositivos móviles que lo posean
    /// </summary>
	[RequireComponent(typeof(RectTransform))]
	public class UISafeArea : MonoBehaviour
	{
        /// <summary>
        /// Rect transform objetivo a ser modificado
        /// </summary>
		RectTransform rectTransform;

        /// <summary>
        /// Canvas de referencia
        /// </summary>
		[SerializeField]
		Canvas canvas;
        /// <summary>
        /// Ajustar min horizontal
        /// </summary>
		[SerializeField]
		bool allowHorizontalMin = true;
        /// <summary>
        /// Ajustar max horizontal
        /// </summary>
		[SerializeField]
		bool allowHorizontalMax = true;
        /// <summary>
        /// Ajustar min vertical
        /// </summary>
		[SerializeField]
		bool allowVerticalMin = true;
        /// <summary>
        /// Ajustar max vertical
        /// </summary>
		[SerializeField]
		bool allowVerticalMax = true;
		void Start()
		{
            //Obtener el canvas si no ha sido asignado desde el principio
			Transform parent = transform.parent;
			while (!canvas)
			{
				canvas = parent.GetComponent<Canvas>();
				parent = parent.parent;
			}

			rectTransform = GetComponent<RectTransform>();

			Rect safeArea = Screen.safeArea;

			Vector2 anchorMin = safeArea.position;
			Vector2 anchorMax = safeArea.position + safeArea.size;

            //Ajusta los puntos de anclaje para que estén normalizados
			if (allowHorizontalMin)
				anchorMin.x /= canvas.pixelRect.width;
			else
				anchorMin.x = rectTransform.anchorMin.x;
			if (allowVerticalMin)
				anchorMin.y /= canvas.pixelRect.height;
			else
				anchorMin.y = rectTransform.anchorMin.y;
			if (allowHorizontalMax)
				anchorMax.x /= canvas.pixelRect.width;
			else
				anchorMax.x = rectTransform.anchorMax.x;
			if (allowVerticalMax)
				anchorMax.y /= canvas.pixelRect.height;
			else
				anchorMax.y = rectTransform.anchorMax.y;
			rectTransform.anchorMax = anchorMax;
			rectTransform.anchorMin = anchorMin;
		}
	}
}