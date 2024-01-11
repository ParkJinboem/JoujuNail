using System.Reflection;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.U2D.Animation;

namespace OnDot.Util
{
    public class AnimationUtil
    {
        /// <summary>
        /// 애니메이터 Rebind
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="writeDefaultValues"></param>
        public static void AnimatorRebind(Animator animator, bool writeDefaultValues)
        {
            if (animator == null)
            {
                return;
            }

            var rebindMethod = typeof(Animator).GetMethod("Rebind", BindingFlags.NonPublic | BindingFlags.Instance);
            rebindMethod.Invoke(animator, new object[] { writeDefaultValues });
        }

        /// <summary>
        /// SpriteSkin Rebind
        /// </summary>
        /// <param name="spriteSkin"></param>
        /// <param name="spriteRenderer"></param>
        public static void SpriteSkinRebind(SpriteSkin spriteSkin, SpriteRenderer spriteRenderer)
        {
            var spriteBones = spriteRenderer.sprite.GetBones();
            var boneTransforms = spriteSkin.boneTransforms;
            for (int i = 0; i < boneTransforms.Length; ++i)
            {
                var boneTransform = boneTransforms[i];
                if (i < spriteBones.Length)
                {
                    var spriteBone = spriteBones[i];
                    if (spriteBone.parentId != -1)
                    {
                        boneTransform.localPosition = spriteBone.position;
                        boneTransform.localRotation = spriteBone.rotation;
                        boneTransform.localScale = Vector3.one;
                    }
                }
            }
        }
    }
}
