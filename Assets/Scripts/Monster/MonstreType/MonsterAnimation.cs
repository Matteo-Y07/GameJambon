using UnityEngine;

namespace GameJambon.Monster
{
    // Mirror of PlayerAnimation but for monsters
    [RequireComponent(typeof(Animator))]
    public class MonsterAnimation : MonoBehaviour
    {
        private Animator _animator;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int HitHash = Animator.StringToHash("Hit");
        private static readonly int DeadHash = Animator.StringToHash("Dead");
        private static readonly int HorizontalHash = Animator.StringToHash("Horizontal");
        private static readonly int VerticalHash = Animator.StringToHash("Vertical");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetMove(float speed)
        {
            _animator.SetFloat(SpeedHash, Mathf.Abs(speed));
        }

        public void SetDirection(Vector2 direction)
        {
            _animator.SetFloat(HorizontalHash, direction.x);
            _animator.SetFloat(VerticalHash, direction.y);
        }

        public void Attack()
        {
            _animator.SetTrigger(AttackHash);
        }

        public void Hit()
        {
            _animator.SetTrigger(HitHash);
        }

        public void Die()
        {
            _animator.SetBool(DeadHash, true);
        }
    }
}
