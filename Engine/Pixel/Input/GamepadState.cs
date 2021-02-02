using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace kuujoo.Pixel
{
	public class GamepadState
	{
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
		}
		public void Update()
		{
			_previousState = _currentState;
			_currentState = GamePad.GetState(_playerIndex, DeadZone);
			if (_rumbleTime > 0f)
			{
				_rumbleTime = MathExt.Approach(_rumbleTime, 0.0f, Time.UnscaledDeltaTime);
				if (_rumbleTime <= 0f)
				{
					GamePad.SetVibration(_playerIndex, 0, 0);
				}
			}
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
