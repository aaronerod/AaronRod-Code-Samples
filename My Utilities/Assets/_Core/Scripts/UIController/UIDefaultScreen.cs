
namespace Rod.Utilities.UI
{
    /// <summary>
    /// Implementación de una pantalla default
    /// </summary>
    public class UIDefaultScreen : UIScreen
    {
        public override bool CanBeClosed()
        {
            bool result = false;
            if (!IsPermanent)
            {
                if (longestTweener)
                {
                    if (IsTweenerAvailable())
                        result = true;
                }
                else
                    result = true;
            }
            else
                result = false;
            return result;
        }

        public override bool CanBeOpened()
        {
            bool result = false;
            if (longestTweener)
            {
                if (IsTweenerAvailable())
                    result = true;
            }
            else
                result = true;
            return result;
        }

        public override bool Close()
        {
            bool result = false;
            if (CanBeClosed())
            {
                Hide();
                result = true;
                ScreenClosed?.Invoke();
            }
            return result;
        }

        public override bool Open()
        {
            bool result = false;
            if (CanBeOpened())
            {
                Show();
                result = true;
                ScreenOpened?.Invoke();
            }
            return result;
        }

        public override void ScreenOnFocus()
        {
            if (!IsShowing)
            {
                Show();
            }
            ScreenFocused?.Invoke();
        }

        public override void ScreenOutFocus()
        {
            switch (BehindBehaviour)
            {
                case ScreenBehindBehaviour.none:
                    break;
                case ScreenBehindBehaviour.hide:
                    Hide();
                    break;
                case ScreenBehindBehaviour.close:
                    Navigator.CloseScreen();
                    break;
            }
            ScreenOutFocused?.Invoke();
        }
    }
}
