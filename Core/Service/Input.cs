using Chronos.Core.Context;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Service
{
    public class Input : IService
    {

        #region Constants

        private const double MaxClickDuration_s = 0.2;

        private static MouseButton[] mouseEventButtons =
            new MouseButton[] { MouseButton.Left, MouseButton.Right };

        #endregion

        #region Events

        public event EventHandler MouseMoved;

        public event EventHandler<MouseEventArgs> MouseDown;

        public event EventHandler<MouseEventArgs> MouseUp;

        #endregion

        #region Public properties

        public bool SuppressAll { get; set; }

        public Vector2 MouseLocation
        {
            get { return currentState.MouseLocation; }
        }

        public Vector2 MouseLocationDelta
        {
            get
            {
                return currentState.MouseLocation
                    - lastState.MouseLocation;
            }
        }

        public bool CursorVisible
        {
            get { return window.CursorVisible; }
            set { window.CursorVisible = value; }
        }

        #endregion

        #region Private members

        private NativeWindow window;

        private MouseDevice mouseDevice;

        private KeyboardDevice keyboardDevice;

        private Clock clock;

        private InputState lastState = new InputState();

        private InputState currentState = new InputState();

        #endregion

        #region Constructors

        public Input(
            NativeWindow window,
            KeyboardDevice keyboardDevice,
            MouseDevice mouseDevice)
        {
            this.window = window;
            this.keyboardDevice = keyboardDevice;
            this.mouseDevice = mouseDevice;
        }

        #endregion

        #region Public methods

        public void Initialise(IContext context)
        {
            clock = context.GetService<Clock>();
            clock.VariableTick += Update;
        }

        public bool KeyPressed(Key key)
        {
            return currentState.KeyState[(int)key].Pressed
                && !lastState.KeyState[(int)key].Pressed
                && !SuppressAll;
        }

        public bool KeyReleased(Key key)
        {
            return !currentState.KeyState[(int)key].Pressed
                && lastState.KeyState[(int)key].Pressed
                && !SuppressAll;
        }

        public bool KeyDown(Key key)
        {
            return currentState.KeyState[(int)key].Pressed
                && !SuppressAll;
        }

        public bool HasMouseMoved()
        {
            return (currentState.MouseLocation.X != lastState.MouseLocation.X)
                || (currentState.MouseLocation.Y != lastState.MouseLocation.Y);
        }

        public bool ButtonPressed(MouseButton button)
        {
            return currentState.ButtonState[(int)button].Pressed
                && !lastState.ButtonState[(int)button].Pressed
                && !SuppressAll;
        }

        public bool ButtonReleased(MouseButton button)
        {
            return !currentState.ButtonState[(int)button].Pressed
                && lastState.ButtonState[(int)button].Pressed
                && !SuppressAll;
        }

        public bool ButtonDown(MouseButton button)
        {
            return currentState.ButtonState[(int)button].Pressed
                && !SuppressAll;
        }

        public bool ButtonClicked(MouseButton button)
        {
            bool clicked = false;
            if (ButtonReleased(button))
            {
                double duration = currentState.ButtonState[(int)button].Time
                    - lastState.ButtonState[(int)button].Time;
                clicked = duration.CompareTo(MaxClickDuration_s) < 0;
            }
            return clicked && !SuppressAll;
        }

        public IList<Key> KeysPressed()
        {

            IList<Key> pressedKeys = new List<Key>();

            for (int k = 0; k < lastState.KeyState.Length; ++k)
            {
                KeyState last = lastState.KeyState[k];
                KeyState current = currentState.KeyState[k];

                if (!last.Pressed && current.Pressed)
                {
                    pressedKeys.Add((Key)k);
                }

            }

            return pressedKeys;

        }

        #endregion

        #region Private methods

        private void Update(object sender, EventArgs e)
        {

            // Reset suppress flag:
            SuppressAll = false;

            // Switch last and current objects:
            InputState oldLast = lastState;
            lastState = currentState;
            currentState = oldLast;

            // Refresh the current state:            
            currentState.RefreshState(
                keyboardDevice,
                mouseDevice);

            // Check for keyboard state changes:
            for (int i = 0; i < currentState.KeyState.Length; ++i)
            {
                if (lastState.KeyState[i].Pressed != currentState.KeyState[i].Pressed)
                {
                    currentState.KeyState[i].Time = clock.Runtime;
                }
                else
                {
                    currentState.KeyState[i].Time = lastState.KeyState[i].Time;
                }
            }

            // Check for mouse state changes:
            for (int i = 0; i < currentState.ButtonState.Length; ++i)
            {
                if (lastState.ButtonState[i].Pressed != currentState.ButtonState[i].Pressed)
                {
                    currentState.ButtonState[i].Time = clock.Runtime;
                }
                else
                {
                    currentState.ButtonState[i].Time = lastState.ButtonState[i].Time;
                }
            }

            FireEvents();

        }

        private void FireEvents()
        {
            if (HasMouseMoved())
            {
                var evt = MouseMoved;
                if (evt != null)
                {
                    evt(this, null);
                }
            }

            foreach (MouseButton mouseButton in mouseEventButtons)
            {

                if (ButtonDown(mouseButton))
                {
                    var evt = MouseDown;
                    if (evt != null)
                    {
                        evt(this, new MouseEventArgs(mouseButton));
                    }
                }
                if (ButtonReleased(mouseButton))
                {
                    var evt = MouseUp;
                    if (evt != null)
                    {
                        evt(this, new MouseEventArgs(mouseButton));
                    }
                }

            }

        }

        #endregion

        #region Private classes

        private class InputState
        {

            private static int[] InvalidButtonCodes = new int[] { 8 };

            public Vector2 MouseLocation { get; private set; }

            public KeyState[] KeyState { get; private set; }

            public ButtonState[] ButtonState { get; private set; }

            public InputState()
            {

                MouseLocation = Vector2.Zero;

                int kl = Enum.GetValues(typeof(Key)).Length;

                IList<int> keyCodes = new List<int>();
                foreach (Key key in Enum.GetValues(typeof(Key)))
                {
                    int keyCode = (int)key;
                    if (!keyCodes.Contains(keyCode))
                    {
                        keyCodes.Add(keyCode);
                    }

                }

                int maxKeyCode = keyCodes.Max();
                KeyState = new KeyState[maxKeyCode];
                for (int i = 0; i < maxKeyCode; ++i)
                {
                    KeyState[i] = new KeyState();
                    KeyState[i].ValidKey = keyCodes.Contains(i);

                }

                IList<int> buttonCodes = new List<int>();
                foreach (MouseButton button in Enum.GetValues(typeof(MouseButton)))
                {
                    int buttonCode = (int)button;
                    if (!buttonCodes.Contains(buttonCode)
                        && !InvalidButtonCodes.Contains(buttonCode))
                    {
                        buttonCodes.Add(buttonCode);
                    }
                }

                int maxButtonCode = buttonCodes.Max();
                ButtonState = new ButtonState[maxButtonCode];
                for (int i = 0; i < maxButtonCode; ++i)
                {
                    ButtonState[i] = new ButtonState();
                    ButtonState[i].ValidButton = buttonCodes.Contains(i);
                }

            }

            public void RefreshState(
                KeyboardDevice keyboardDevice,
                MouseDevice mouseDevice)
            {

                // Refresh keyboard state:

                for (int i = 0; i < KeyState.Length; ++i)
                {
                    Key key = (Key)i;
                    KeyState keyState = KeyState[i];
                    if (keyState.ValidKey)
                    {
                        keyState.Pressed = keyboardDevice[key];
                    }
                }

                // Refresh mouse state:
                MouseLocation = new Vector2(mouseDevice.X, mouseDevice.Y);

                for (int i = 0; i < ButtonState.Length; ++i)
                {
                    MouseButton mouseButton = (MouseButton)i;
                    ButtonState buttonState = ButtonState[i];
                    if (buttonState.ValidButton)
                    {
                        buttonState.Pressed = mouseDevice[mouseButton];
                    }
                }

            }

        }

        private class KeyState
        {

            public bool ValidKey { get; set; }

            public bool Pressed { get; set; }

            public double Time { get; set; }

            public KeyState()
            {
                ValidKey = true;
            }

        }

        private class ButtonState
        {

            public bool ValidButton { get; set; }

            public bool Pressed { get; set; }

            public double Time { get; set; }

            public ButtonState()
            {
                ValidButton = true;
            }

        }

        #endregion

    }
}
