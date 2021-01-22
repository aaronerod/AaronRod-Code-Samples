using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rod.Utilities.UI
{
    /// <summary>
    /// Componente encargado de comunicar con UIController
    /// </summary>
    public class UINavigator : MonoBehaviour
    {
        /// <summary>
        /// Referencia al connector de UIController
        /// </summary>
        [SerializeField]
        UIConnector uiConnector;
        /// <summary>
        /// Referencia a la pantalla que conectarña
        /// </summary>
        [SerializeField]
        UIScreen targetScreen;
        /// <summary>
        /// Abrir pantalla al iniciar
        /// </summary>
        [SerializeField]
        bool openOnStart;
        /// <summary>
        /// Referencia al UIController
        /// </summary>
        UIController uiController;
        private void Start()
        {
            //suscribirse al evento que notifica que el controlador ha sido asignado
            uiConnector.ControllerAssigned += OnControllerAssigned;
        }
        /// <summary>
        /// Receptor del evento que notifica que el controlador ha sido asignado
        /// </summary>
        /// <param name="uiController"></param>
        void OnControllerAssigned(UIController uiController)
        {
            this.uiController = uiController;
            if (openOnStart)
                OpenScreen();
        }
        /// <summary>
        /// Abrir la ventana
        /// </summary>
        public void OpenScreen()
        {
            uiConnector.Controller.OpenScreen(targetScreen);
        }
        /// <summary>
        /// Cerrar la ventana
        /// </summary>
        public void CloseScreen()
        {
            uiConnector.Controller.CloseScreen(targetScreen);
        }
        /// <summary>
        /// Esconder la ventana
        /// </summary>
        public void HideScreen()
        {
            targetScreen.Hide();
        }
        /// <summary>
        /// Mostrar la ventana
        /// </summary>
        public void ShowScreen()
        {
            targetScreen.Show();

        }
        /// <summary>
        /// Mostrar todas las ventanas abiertas
        /// </summary>
        public void ShowFullUI()
        {
            uiConnector.Controller.ShowFullUI();
        }
        /// <summary>
        /// Ocultar todas la ventanas abiertas
        /// </summary>
        public void HideFullUI()
        {
            uiConnector.Controller.HideFullUI();
        }
    }
}