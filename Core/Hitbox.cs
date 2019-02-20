using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core
{
    /*
    public enum HitBoxes
    {
        HITBOX_PELVIS,
        HITBOX_L_THIGH,
        HITBOX_L_CALF,
        HITBOX_L_FOOT,
        HITBOX_R_THIGH,
        HITBOX_R_CALF,
        HITBOX_R_FOOT,
        HITBOX_SPINE1,
        HITBOX_SPINE2,
        HITBOX_SPINE3,
        HITBOX_NECK,
        HITBOX_HEAD,
        HITBOX_L_UPPERARM,
        HITBOX_L_FOREARM,
        HITBOX_L_HAND,
        HITBOX_R_UPPERARM,
        HITBOX_R_FOREARM,
        HITBOX_R_HAND,
        HITBOX_L_CLAVICLE,
        HITBOX_R_CLAVICLE,
        HITBOX_SPINE4,
        HITBOX_MAX,
    };

    public enum Bone
    {
        BONE_PELVIS = 0,
        BONE_SPINE1,
        BONE_SPINE2,
        BONE_SPINE3,
        BONE_SPINE4,
        BONE_NECK,
        BONE_L_CLAVICLE,
        BONE_L_UPPER_ARM,
        BONE_L_FOREARM,
        BONE_L_HAND,
        BONE_HEAD,
        BONE_FORWARD,
        BONE_R_CLAVICLE,
        BONE_R_UPPER_ARM,
        BONE_R_FOREARM,
        BONE_R_HAND,
        BONE_WEAPON,
        BONE_WEAPON_SLIDE,
        BONE_WEAPON_R_HAND,
        BONE_WEAPON_L_HAND,
        BONE_WEAPON_CLIP1,
        BONE_WEAPON_CLIP2,
        BONE_SILENCER,
        BONE_R_THIGH,
        BONE_R_CALF,
        BONE_R_FOOT,
        BONE_L_THIGH,
        BONE_L_CALF,
        BONE_L_FOOT,
        BONE_L_WEAPON_HAND,
        BONE_R_WEAPON_HAND,
        BONE_L_FORETWIST,
        BONE_L_CALFTWIST,
        BONE_R_CALFTWIST,
        BONE_L_THIGHTWIST,
        BONE_R_THIGHTWIST,
        BONE_L_UPARMTWIST,
        BONE_R_UPARMTWIST,
        BONE_R_FORETWIST,
        BONE_R_TOE,
        BONE_L_TOE,
        BONE_R_FINGER01,
        BONE_R_FINGER02,
        BONE_R_FINGER03,
        BONE_R_FINGER04,
        BONE_R_FINGER05,
        BONE_R_FINGER06,
        BONE_R_FINGER07,
        BONE_R_FINGER08,
        BONE_R_FINGER09,
        BONE_R_FINGER10,
        BONE_R_FINGER11,
        BONE_R_FINGER12,
        BONE_R_FINGER13,
        BONE_R_FINGER14,
        BONE_L_FINGER01,
        BONE_L_FINGER02,
        BONE_L_FINGER03,
        BONE_L_FINGER04,
        BONE_L_FINGER05,
        BONE_L_FINGER06,
        BONE_L_FINGER07,
        BONE_L_FINGER08,
        BONE_L_FINGER09,
        BONE_L_FINGER10,
        BONE_L_FINGER11,
        BONE_L_FINGER12,
        BONE_L_FINGER13,
        BONE_L_FINGER14,
        BONE_L_FINGER15,
        BONE_R_FINGER15,
        BONE_MAX
    };

    public struct Hitbox
    {
        public Bone bone;
        public Vector3 min;
        public Vector3 max;

        public Hitbox(Bone bone, Vector3 min, Vector3 max)
        {
            this.bone = bone;
            this.min = min;
            this.max = max;
        }

        public static Hitbox[] hitboxes = new Hitbox[21]
        {
            new Hitbox(Bone.BONE_PELVIS, new Vector3( -6.42f, -5.7459f, -6.8587f), new Vector3( 4.5796f,    4.5796f,   6.8373f ) ), // Torso
            new Hitbox(Bone.BONE_L_THIGH, new Vector3(  1.819f,  -3.959f,  -2.14f   ), new Vector3( 22.149002f, 3.424f,    4.5796f ) ),
            new Hitbox(Bone.BONE_L_CALF, new Vector3(  2.0758f, -3.21f,   -2.1507f ), new Vector3( 19.26f,     2.675f,    3.0495f ) ),
            new Hitbox(Bone.BONE_L_FOOT, new Vector3(  1.8725f, -2.675f,  -2.4075f ), new Vector3( 5.6175f,    9.694201f, 2.4075f ) ),
            new Hitbox(Bone.BONE_R_THIGH, new Vector3(  1.819f,  -3.7557f, -4.5796f ), new Vector3( 22.149002f, 3.424f,    2.14f   ) ),
            new Hitbox(Bone.BONE_R_CALF, new Vector3(  2.0758f, -3.21f,   -2.8462f ), new Vector3( 19.26f,     2.675f,    2.247f  ) ),
            new Hitbox(Bone.BONE_R_FOOT, new Vector3(  1.8725f, -2.675f,  -2.4075f ), new Vector3( 5.6175f,    9.694201f,  2.4075f ) ),
            new Hitbox(Bone.BONE_SPINE2, new Vector3( -4.28f,   -4.5796f, -6.3879f ), new Vector3( 3.21f,      5.885f,    6.2809f ) ), // Torso
		    new Hitbox(Bone.BONE_SPINE3, new Vector3( -4.28f,   -5.029f,  -6.0883f ), new Vector3( 3.21f,      5.885f,    5.9813f ) ), // Torso
		    new Hitbox(Bone.BONE_SPINE4, new Vector3( -4.28f,   -5.35f,   -5.885f  ), new Vector3( 2.9211f,    5.1467f,   5.885f  ) ), // Chest
		    new Hitbox(Bone.BONE_NECK, new Vector3(  0.3317f, -3.0174f, -2.4503f ), new Vector3( 3.4026f,    2.4182f,   2.354f  ) ), // Chest
		    new Hitbox(Bone.BONE_HEAD, new Vector3( -2.7713f, -2.8783f, -3.103f  ), new Vector3( 6.955f,     3.5203f,   3.0067f ) ),
            new Hitbox(Bone.BONE_L_UPPER_ARM, new Vector3( -2.675f,  -3.21f,   -2.14f   ), new Vector3( 12.84f,     3.21f,     2.14f   ) ),
            new Hitbox(Bone.BONE_L_FOREARM, new Vector3( -0f,     -2.14f,   -2.14f   ), new Vector3( 9.63f,      2.14f,     2.14f   ) ),
            new Hitbox(Bone.BONE_L_HAND, new Vector3( -1.7227f, -1.2198f, -1.3803f ), new Vector3( 4.4726f,    1.2198f,   1.3803f ) ),
            new Hitbox(Bone.BONE_R_UPPER_ARM, new Vector3( -2.675f,  -3.21f,   -2.14f   ), new Vector3( 12.84f,     3.21f,     2.14f   ) ),
            new Hitbox(Bone.BONE_R_FOREARM, new Vector3( -0f,     -2.14f,   -2.14f   ), new Vector3( 9.63f,      2.14f,     2.14f   ) ),
            new Hitbox(Bone.BONE_R_HAND, new Vector3( -1.7227f, -1.2198f, -1.3803f ), new Vector3( 4.4726f,    1.2198f,   1.3803f ) ),
            new Hitbox(Bone.BONE_L_CLAVICLE, new Vector3( -0f,     -3.21f,   -5.35f   ), new Vector3( 7.49f,      4.28f,     3.21f   ) ), // Chest
		    new Hitbox(Bone.BONE_R_CLAVICLE, new Vector3( -0f,     -3.21f,   -3.21f   ), new Vector3( 7.49f,      4.28f,     5.35f   ) ), // Chest
            new Hitbox(Bone.BONE_SPINE4, new Vector3( -0.2996f, -6.0027f, -4.9969f ), new Vector3( 5.4998f, 2.5038f, 5.1039f ) )
        };
    }
    */
}
