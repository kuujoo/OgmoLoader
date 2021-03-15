using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace kuujoo.Pixel
{
	public class GamepadState
	{
		public Vector2 LeftStick { get; private set; }
		public Vector2 RightStick { get; private set; }
		public bool Used { get; private set; }
		public GamePadDeadZone DeadZone = GamePadDeadZone.IndependentAxes;
		PlayerIndex _playerIndex;
		GamePadState _previousState;
		GamePadState _currentState;
		float _rumbleTime;
		internal GamepadState(PlayerIndex playerIndex)
		{
			_playerIndex = playerIndex;
			_previousState = new GamePadState();
			_currentState = GamePad.GetState(_playerIndex);
			_rumbleTime = 0.0f;
			Used = false;
		}
		public void Update()
		{
			_previousState = _currentState;
			_currentState = GamePad.GetState(_playerIndex, DeadZone);
			Used = (_currentState.Buttons != _previousState.Buttons || _currentState.DPad != _previousState.DPad);
			if (_rumbleTime > 0f)
			{
				_rumbleTime = Calc.Approach(_rumbleTime, 0.0f, Time.UnscaledDeltaTime);
				if (_rumbleTime <= 0f)
				{
					GamePad.SetVibration(_playerIndex, 0, 0);
				}
			}
			LeftStick = _currentState.ThumbSticks.Left;
			RightStick = _currentState.ThumbSticks.Right;
		}
		public void SetVibration(float left, float right, float duration)
		{
			_rumbleTime = duration;
			GamePad.SetVibration(_playerIndex, left, right);
		}
		public void StopVibration()
		{
			GamePad.SetVibration(_playerIndex, 0, 0);
			_rumbleTime = 0f;
		}
		public bool IsConnected()
		{
			return _currentState.IsConnected;
		}
		public bool IsButtonPressed(Buttons button)
		{
			return _currentState.IsButtonDown(button) && !_previousState.IsButtonDown(button);
		}
		public bool IsButtonDown(Buttons button)
		{
			return _currentState.IsButtonDown(button);
		}
		public bool IsButtonReleased(Buttons button)
		{
			return !_currentState.IsButtonDown(button) && _previousState.IsButtonDown(button);
		}
	}
}
