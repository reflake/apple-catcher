using Game.Enum;
using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(CharacterController))]
	public class Player : MonoBehaviour
	{
		[SerializeField] private Input input = default;
		[Space]
		[SerializeField] private float playerSpeed = 4f;
		[SerializeField] private float jumpStrength = 3f;
		[SerializeField] private float jumpDuration = 0.6f;
		[SerializeField] private float jumpGravityAmplification = 0.3f;
		[SerializeField] private float gravityScale = 2.5f;

		private CharacterController _characterController;
		private Vector3 _velocity = Vector3.zero;
		private float _jumpTime = -1;
		private bool _holdJump = false;

		private void Awake()
		{
			_characterController = GetComponent<CharacterController>();
			
			Debug.Assert(_characterController != null);
		}

		private void Update()
		{
			var cmd = input.GetCmd();

			switch (cmd.SideMove)
			{
				case Side.Left:
					Walk(-playerSpeed);
					break;
				case Side.Right:
					Walk(playerSpeed);
					break;
			}

			var grounded = _characterController.isGrounded;

			if (cmd.PressingJump)
			{
				// Started jumping
				if (grounded)
				{
					_velocity = Vector3.up * jumpStrength;

					_jumpTime = Time.time + jumpDuration;
					_holdJump = true;
					grounded = false;
				}
				else if (_jumpTime < Time.time)
				{
					_holdJump = false;
				}
			} else if (_holdJump)
			{
				_holdJump = false;
			}

			// Apply gravity to player
			if (!grounded)
			{
				var gravityStrength = _holdJump ? jumpGravityAmplification : 1f;
				var gravityAcceleration = Physics.gravity * gravityScale * Time.deltaTime * gravityStrength;

				_velocity += gravityAcceleration * 0.5f;

				_characterController.Move(_velocity * Time.deltaTime);

				_velocity += gravityAcceleration * 0.5f;
			}
			else
			{
				_velocity = Vector3.zero;
			}
		}

		private void Walk(float velocity)
		{
			var offset = transform.right * velocity;
			
			if (_characterController.isGrounded)
				
				_characterController.Move(offset * Time.deltaTime + Vector3.down);
			else
			
				_characterController.Move(offset * Time.deltaTime);
		}
	}
}