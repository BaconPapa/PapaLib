using PapaLib.Util.EnumAttribute;
using Xunit;

namespace PapaLib.Tests.Util
{
    public class EnumAttributeTests
    {
        private class IsNormalAttackAttribute : EnumAttribute {}
        private class AttackAttribute : EnumAttribute, IProperty<int>
        {
            public int Value { get; }
            public AttackAttribute(int value)
            {
                this.Value = value;
            }
        }
        private enum Skills
        {
            [IsNormalAttack]
            HeroCombo,
            Dodge,
            [Attack(10)]
            LaserBean,
        }

        [Fact]
        public void HeroCombo_Is_NormalAttack()
        {
            var isNormalAttack = Skills.HeroCombo.HasAttribute<Skills, IsNormalAttackAttribute>();
            Assert.True(isNormalAttack);
        }

        [Fact]
        public void Dodge_Is_Not_NormalAttack()
        {
            var isNormalAttack = Skills.Dodge.HasAttribute<Skills, IsNormalAttackAttribute>();
            Assert.False(isNormalAttack);
        }

        [Fact]
        public void Dodge_Attack_is_0()
        {
            var attack = Skills.Dodge.AttributeProperty<Skills, AttackAttribute, int>();
            Assert.Equal(0, attack);
        }

        [Fact]
        public void LaserBean_Attack_is_10()
        {
            var attack = Skills.LaserBean.AttributeProperty<Skills, AttackAttribute, int>();
            Assert.Equal(10, attack);
        }
    }
}