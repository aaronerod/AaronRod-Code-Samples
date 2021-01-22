using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rod.Utilities.Execution;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Rod.Utilities.UI
{

    /// <summary>
    /// Sistema encargado de manejar la jeraquía de UI
    /// </summary>
    public class UIController : MonoBehaviour, IPreInitializable
    {
        /// <summary>
        /// Referencia al connector
        /// </summary>
        [SerializeField]
        UIConnector uiConnector;
        List<UIScreen> openScreens = new List<UIScreen>();
        /// <summary>
        /// Lista de pantallas que se encuentran abiertas
        /// </summary>
        public List<UIScreen> OpenScreens => openScreens;
        /// <summary>
        /// Usar el backbutton para cerrar pantalla 
        /// </summary>
        [SerializeField] bool _allowBackButton;

        /// <summary>
        /// Preinicializar sistema
        /// </summary>
        public IEnumerator PreInitialize()
        {
            if (uiConnector.Controller == null)
            {
                uiConnector.Controller = this;
                transform.SetParent(null);
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            yield return null;
        }

        void Update()
        {
            if (_allowBackButton)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
                {
                    CloseLastScreen();
                }
            }
        }

        /// <summary>
        /// Abrir una pantalla
        /// </summary>
        /// <param name="screen"></param>
        public void OpenScreen(UIScreen screen)
        {
            if (screen != null)
            {
                if (!openScreens.Contains(screen))
                {
                    UIScreen lastOpenedScreen = GetLastOpenScreen();
                    bool canCloseLastScreen = lastOpenedScreen ? lastOpenedScreen.IsPermanent || lastOpenedScreen.CanBeClosed() : true;
                    if (canCloseLastScreen && screen.CanBeOpened())
                    {
                        screen.gameObject.SetActive(true);
                        bool openResult = screen.Open();
                        if (openResult)
                        {
                            screen.ReparentTarget.SetAsLastSibling();
                            if (!screen.IsPopUp && lastOpenedScreen)
                            {
                                lastOpenedScreen.ScreenOutFocus();
                            }
                            openScreens.Add(screen);
                        }
                    }
                }
                else
                {
                    if (screen != GetLastOpenScreen())
                        FocusOpenScreen(screen);
                }
            }
        }
        /// <summary>
        /// Manda pantalla al frente de todas
        /// </summary>
        /// <param name="screen"></param>
        void FocusOpenScreen(UIScreen screen)
        {
            if (screen != null)
            {
                if (openScreens.Contains(screen))
                {
                    openScreens.Remove(screen);
                    if (!screen.IsShowing)
                    {
                        screen.gameObject.SetActive(true);
                        screen.ScreenOnFocus();
                    }
                    screen.ReparentTarget.SetAsLastSibling();
                    openScreens.Add(screen);
                }
            }
        }

        /// <summary>
        /// Cerrar pantalla
        /// </summary>
        /// <param name="screen"></param>
        public void CloseScreen(UIScreen screen)
        {
            if (screen != null)
            {
                bool closeResult = screen.Close();
                if (closeResult)
                {
                    openScreens.Remove(screen);
                }

                UIScreen lastOpenedScreen = GetLastOpenScreen();
                if (lastOpenedScreen)
                {
                    lastOpenedScreen.gameObject.SetActive(true);
                    lastOpenedScreen.ScreenOnFocus();
                }
            }
        }

        /// <summary>
        /// Cierra la última pantalla abierta
        /// </summary>
        public void CloseLastScreen()
        {
            UIScreen screen = GetLastOpenScreen();
            CloseScreen(screen);
        }

        /// <summary>
        /// Obtener la última pantalla abierta
        /// </summary>
        /// <returns></returns>
        UIScreen GetLastOpenScreen()
        {
            UIScreen lastOpenedScreen = null;
            if (openScreens.Count > 0)
            {
                lastOpenedScreen = openScreens[openScreens.Count - 1];
            }
            return lastOpenedScreen;
        }

        /// <summary>
        /// Esconder todas las pantallas
        /// </summary>
        public void HideFullUI()
        {
            foreach (UIScreen screen in openScreens)
            {
                screen.Hide();
            }
        }
        /// <summary>
        /// Mostrar todas las pantallas
        /// </summary>
        public void ShowFullUI()
        {
            foreach (UIScreen screen in openScreens)
            {
                screen.gameObject.SetActive(true);
                screen.Show();
            }
        }
        /// <summary>
        /// Esconder la última pantalla
        /// </summary>
        public void HideLastScreen()
        {
            UIScreen lastOpenScreen = GetLastOpenScreen();
            if (lastOpenScreen != null)
                lastOpenScreen.Hide();
        }
        /// <summary>
        /// Mostrar la última pantalla
        /// </summary>
        public void ShowLastScreen()
        {
            UIScreen lastOpenScreen = GetLastOpenScreen();
            if (lastOpenScreen != null)
                lastOpenScreen.Show();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UIController))]
    public class UIControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UIController controller = (UIController)target;
            GUILayout.Label("Screens:");
            for (int i = 0; i < controller.OpenScreens.Count; i++)
            {
                GUILayout.Label(controller.OpenScreens[i].gameObject.name);
            }
        }
    }
#endif
}